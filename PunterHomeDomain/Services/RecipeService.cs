using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Conversions;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using PunterHomeDomain.Shared;
using static Enums;
using EUnitMeasurementType = PunterHomeDomain.Enums.EUnitMeasurementType;

namespace PunterHomeApp.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeDataAdapter recipeAdapter;
        private readonly IProductDataAdapter productDataAdapter;
        private readonly IShoppingListService myShoppingListService;

        public RecipeService(IRecipeDataAdapter recipeAdapter, IProductDataAdapter productDataAdapter, IShoppingListService shoppingListService)
        {
            this.recipeAdapter = recipeAdapter;
            this.productDataAdapter = productDataAdapter;
            this.myShoppingListService = shoppingListService;
        }

        public bool AddIngredient(Guid recipeId, IIngredient ingredient)
        {
            recipeAdapter.AddIngredient(recipeId, ingredient);
            return true;
        }

        public IEnumerable<RecipeApiModel> GetAllRecipes()
        {
            var recipes = recipeAdapter.GetAllRecipes().Result.ToList();
            recipes.ForEach(r => r.IsAvailable = r.Ingredients.ToList().All(i => IsIngedientAvailable(i, productDataAdapter.GetProductById(i.ProductId))));
            return recipes;
        }

        public void CreateRecipe(string recipeName, ERecipeType type)
        {
            recipeAdapter.SaveRecipe(recipeName, type);
        }

        public void DeleteRecipeById(Guid id)
        {
            recipeAdapter.DeleteById(id);
        }

        public void UpdateRecipe(Guid id, string newName)
        {
            recipeAdapter.UpdateRecipe(id, newName);
        }

        public async Task<RecipeDetailsApiModel> GetRecipeSummaryDetails(Guid recipeId)
        {
            var recipe = recipeAdapter.GetRecipeById(recipeId);
            var products = await productDataAdapter.GetAllProductDetails();

            var recipeDetails = new RecipeDetailsApiModel
            {
                Id = recipe.Id,
                Ingredients = recipe.Ingredients.Select(s => {
                        var r = IngredientApiModelIngredientConversion.Convert(s);
                        r.IsAvaliable = IsIngedientAvailable(s, products.FirstOrDefault(i => i.Id == s.ProductId));
                        return r;
                    }).ToList(),
                Name = recipe.Name,
                Steps = recipe.Steps.OrderBy(o => o.Order).ToList(),
            };
            return recipeDetails;
        }

        public async Task<List<ApiIngredientModel>> GetIngredientsDetailsForRecipe(Guid recipeId, int numberOfPersons = 1)
        {
            var recipe = recipeAdapter.GetRecipeById(recipeId);
            var products = await productDataAdapter.GetAllProductDetails();

            IEnumerable<ApiIngredientModel> ingredients = recipe.Ingredients.Select(s =>
            {
                var r = IngredientApiModelIngredientConversion.Convert(s);
                r.IsAvaliable = IsIngedientAvailable(s, products.FirstOrDefault(i => i.Id == s.ProductId), numberOfPersons);
                return r;
            });
            return ingredients.ToList();
        }

        public void AddRecipeIngredientsToShoppingList(Guid recipeId, int numberOfPersons, Guid shoppingListId, bool onlyUnavailable)
        {
            throw new NotImplementedException();
            //var recipe = recipeAdapter.GetRecipeById(recipeId);

            //var request = new AddProductToShoppingListRequest
            //{
            //    NrOfPersons = numberOfPersons,
            //    RecipeId = recipeId,
            //    RecipeOnlyAvailable = onlyUnavailable
            //};

            //myShoppingListService.AddProductToShoppingList(shoppingListId, request);
        }

        public bool IsIngedientAvailable(Ingredient i, ProductDetails p, int numberOfPersons = 1)
        {
            if (numberOfPersons < 1)
            {
                numberOfPersons = 1;
            }

            bool productHasSameMeasurementClass = p.ProductQuantities.Any(pq => Measurements.GetMeasurementClassForEUnitQuantityType(pq.MeasurementType) == Measurements.GetMeasurementClassForEUnitQuantityType(i.UnitQuantityType));

            if (!productHasSameMeasurementClass || p.MeasurementAmounts == null)
            {
                return false;
            }
            var totalAmountOfProduct = p.MeasurementAmounts.GetTotalAmount(i.UnitQuantityType);
            return totalAmountOfProduct >= i.UnitQuantity * numberOfPersons; 
        }

        private double GetTotalProductQuantitiesToMeasuremenType(IEnumerable<BaseMeasurement> productQuantities, EUnitMeasurementType type)
        {
            var total = 0.0;
            foreach(var pq in productQuantities)
            {
                total += pq.ConvertTo(type);
            }
            return total;
        }

        public void AddStep(RecipeStep step, Guid recipeId)
        {
            recipeAdapter.AddStep(step.Text, step.Order, recipeId);
        }

        public void RemoveStep(Guid stepId)
        {
            recipeAdapter.RemoveStep(stepId);
        }

        public void UpdateStep(RecipeStep step)
        {
            var allStepForRecipe = recipeAdapter.GetStepForRecipe(step.RecipeId).ToList();
            var recipeToUpdate = allStepForRecipe.FirstOrDefault(r => r.Id == step.Id);

            bool isTextChanged = !string.IsNullOrEmpty(step.Text) && step.Text != recipeToUpdate.Text;
            if ( isTextChanged && step.Order == recipeToUpdate.Order)
            {
                recipeAdapter.UpdateStep(step.Id, step.Text);
                return;
            }

            allStepForRecipe.OrderBy(s => s.Order);
            allStepForRecipe.Remove(recipeToUpdate);
            allStepForRecipe.Insert(step.Order - 1, recipeToUpdate);

            int order = 1;
            foreach (var item in allStepForRecipe)
            {
                item.Order = order++;
                if (item.Id == recipeToUpdate.Id)
                {
                    recipeAdapter.UpdateStep(step.Id, isTextChanged ? step.Text : null, step.Order);
                    continue;
                }
                recipeAdapter.UpdateStep(item.Id, order: item.Order);
            }

        }

        public void UpdateStep1(RecipeStep step)
        {
            var allStepForRecipe = recipeAdapter.GetStepForRecipe(step.RecipeId);
            var recipeToUpdate = allStepForRecipe.FirstOrDefault(r => r.Id == step.Id);

            if (step.Order < recipeToUpdate.Order)
            {

                var stepsBetween = allStepForRecipe.Where(r => r.Order >= step.Order && r.Order < recipeToUpdate.Order);
                foreach (var item in stepsBetween)
                {
                    item.Order++;
                    recipeAdapter.UpdateStep(item.Id, item.Text, item.Order);
                }

            }
            else
            {
                var stepsBetween = allStepForRecipe.Where(r => r.Order <= step.Order && r.Order > recipeToUpdate.Order);
                foreach (var item in stepsBetween)
                {
                    item.Order--;
                    recipeAdapter.UpdateStep(item.Id, item.Text, item.Order);
                }
            }

            recipeAdapter.UpdateStep(step.Id, step.Text, step.Order);

        }
        public async Task<IEnumerable<RecipeApiModel>> Search(SearchRecipeParameters parameters)
        {
            var all = await recipeAdapter.GetAllRecipes();

            BaseFilter<RecipeApiModel> filter = new NameFilter<RecipeApiModel>(parameters.Name);
            if(parameters.Type != ERecipeType.None)
            {
                filter = new RecipeTypeFilter(parameters.Type, filter);
            }

            return filter.Filter(all);
        }

    }

    public interface IRecipe
    {
        string Name { get; set; }
        IEnumerable<RecipeStep> Steps { get; set; }
        IEnumerable<Ingredient> Ingredients { get; set; }
        Guid Id { get; set; }
    }

    public interface IIngredient
    {
        public Guid ProductId { get; set; }
        public Guid RecipeId { get; set; }
        public string ProductName { get; set; }
        public int UnitQuantity { get; set; }
        public Enums.EUnitMeasurementType UnitQuantityType { get; set; }

    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items);
    }

    public class NameFilter<T> : BaseFilter<T> where T : IName
    {
        private readonly string filterName;

        public NameFilter(string filterName, BaseFilter<T> previous = null) : base(previous)
        {
            this.filterName = filterName;
        }
        protected override IEnumerable<T> FilterAction(IEnumerable<T> items)
        {
            return items.Where(i => i.Name.ToLower().Contains(filterName.ToLower()));
        }
    }
    public interface IName
    {
        public string Name { get; set; }
    }

    public class RecipeTypeFilter : BaseFilter<RecipeApiModel> 
    {
        private readonly ERecipeType typeFilter;

        public RecipeTypeFilter(ERecipeType typeFilter, IFilter<RecipeApiModel> previousFilter = null) : base(previousFilter)
        {
            this.typeFilter = typeFilter;
        }

        protected override IEnumerable<RecipeApiModel> FilterAction(IEnumerable<RecipeApiModel> items)
        {
            return items.Where(i => i.Type == typeFilter);
        }
    }

    public abstract class BaseFilter<T> : IFilter<T>
    {
        private readonly IFilter<T> previousFilter;

        public BaseFilter(IFilter<T> previousFilter)
        {
            this.previousFilter = previousFilter;
        }


        protected abstract IEnumerable<T> FilterAction(IEnumerable<T> items);

        public IEnumerable<T> Filter(IEnumerable<T> items)
        {
            var filtered = previousFilter?.Filter(items) ?? items;
            return FilterAction(filtered);
        }
    }
}
