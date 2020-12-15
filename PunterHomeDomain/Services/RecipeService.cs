using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataModels.Measurements;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Conversions;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using static Enums;

namespace PunterHomeApp.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeDataAdapter recipeAdapter;
        private readonly IProductDataAdapter productDataAdapter;

        public RecipeService(IRecipeDataAdapter recipeAdapter, IProductDataAdapter productDataAdapter)
        {
            this.recipeAdapter = recipeAdapter;
            this.productDataAdapter = productDataAdapter;
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

        public void CreateRecipe(string recipeName)
        {
            recipeAdapter.SaveRecipe(new RecipeApiModel
            {
                Name = recipeName
            });
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

        public bool IsIngedientAvailable(Ingredient i, ProductDetails p)
        {
            bool productHasSameMeasurementClass = p.ProductQuantities.Any(pq => Measurements.GetMeasurementClassForEUnitQuantityType(pq.MeasurementType) == Measurements.GetMeasurementClassForEUnitQuantityType(i.UnitQuantityType));

            if (!productHasSameMeasurementClass || p.MeasurementAmounts == null)
            {
                return false;
            }

            return p.MeasurementAmounts.GetTotalAmount(i.UnitQuantityType) >= i.UnitQuantity;
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
}
