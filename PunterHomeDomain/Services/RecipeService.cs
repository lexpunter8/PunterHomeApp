using System;
using System.Collections.Generic;
using PunterHomeAdapters.Models;
using PunterHomeApp.Interfaces;
using PunterHomeDomain.Models;
using static Enums;

namespace PunterHomeApp.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeDataAdapter recipeAdapter;

        public RecipeService(IRecipeDataAdapter recipeAdapter)
        {
            this.recipeAdapter = recipeAdapter;
        }

        public void CreateRecipe(string recipeName, List<string> steps, List<Ingredient> ingredients)
        {
            recipeAdapter.SaveRecipe(new Recipe
            {
                Id = Guid.NewGuid(),
                Name = recipeName,
                Steps = steps,
                Ingredients = ingredients
            }); ;
        }

        public bool AddIngredient(Guid recipeId, IIngredient ingredient)
        {
            recipeAdapter.AddIngredient(recipeId, ingredient);
            return true;
        }

        public IEnumerable<IRecipe> GetAllRecipes()
        {
            return recipeAdapter.GetAllRecipes();
        }

    }

    public interface IRecipe
    {
        string Name { get; set; }
        IEnumerable<string> Steps { get; set; }
        IEnumerable<IIngredient> Ingredients { get; set; }
        Guid Id { get; set; }
    }

    public class Ingredient : IIngredient
    {
        public IProduct Product { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitQuantityType UnitQuantityType { get; set; }
    }

    public interface IIngredient
    {
        IProduct Product { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitQuantityType UnitQuantityType { get; set; }

    }
}
