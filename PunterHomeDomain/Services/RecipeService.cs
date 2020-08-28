using System;
using System.Collections.Generic;
using PunterHomeApp.Interfaces;
using PunterHomeDomain.Interfaces;
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

        public bool AddIngredient(Guid recipeId, IIngredient ingredient)
        {
            recipeAdapter.AddIngredient(recipeId, ingredient);
            return true;
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return recipeAdapter.GetAllRecipes().Result;
        }

        public void CreateRecipe(string recipeName)
        {
            recipeAdapter.SaveRecipe(new Recipe
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
    }

    public interface IRecipe
    {
        string Name { get; set; }
        IEnumerable<RecipeStep> Steps { get; set; }
        IEnumerable<Ingredient> Ingredients { get; set; }
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
