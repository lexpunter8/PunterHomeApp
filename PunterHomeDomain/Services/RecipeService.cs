using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataModels.Measurements;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Conversions;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using static Enums;

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
            var recipe = recipeAdapter.GetRecipeById(recipeId);

            foreach (var ingredient in recipe.Ingredients)
            {
                var request = new AddProductToShoppingListRequest
                {
                    MeasurementAmount = ingredient.UnitQuantity * numberOfPersons,
                    MeasurementType = ingredient.UnitQuantityType,
                    NrOfPersons = numberOfPersons,
                    ProductId = ingredient.ProductId,
                    Reason = EShoppingListReason.Recipe,
                    RecipeId = recipeId
                };

                myShoppingListService.AddProductToShoppingList(shoppingListId, request);
            }
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

        public async Task<IEnumerable<RecipeApiModel>> Search(SearchRecipeParameters parameters)
        {
            var all = await recipeAdapter.GetAllRecipes();

            BaseRecipeFilter filter = new RecipeNameFilter(parameters.Name);
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
        public EUnitMeasurementType UnitQuantityType { get; set; }

    }

    public interface IFilter<T>
    {
        IEnumerable<T> Filter(IEnumerable<T> items);
    }

    public class RecipeNameFilter : BaseRecipeFilter
    {
        private readonly string filterName;

        public RecipeNameFilter(string filterName, BaseRecipeFilter previous = null) : base(previous)
        {
            this.filterName = filterName;
        }
        protected override IEnumerable<RecipeApiModel> FilterAction(IEnumerable<RecipeApiModel> items)
        {
            return items.Where(i => i.Name.Contains(filterName));
        }
    }

    public class RecipeTypeFilter : BaseRecipeFilter
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

    public abstract class BaseRecipeFilter : IFilter<RecipeApiModel>
    {
        private readonly IFilter<RecipeApiModel> previousFilter;

        public BaseRecipeFilter(IFilter<RecipeApiModel> previousFilter)
        {
            this.previousFilter = previousFilter;
        }


        protected abstract IEnumerable<RecipeApiModel> FilterAction(IEnumerable<RecipeApiModel> items);

        public IEnumerable<RecipeApiModel> Filter(IEnumerable<RecipeApiModel> items)
        {
            var filtered = previousFilter?.Filter(items) ?? items;
            return FilterAction(filtered);
        }
    }
}
