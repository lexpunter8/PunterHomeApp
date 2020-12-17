using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;

namespace PunterHomeDomain.Interfaces
{
    public interface IRecipeService
    {
        void CreateRecipe(string recipeName);
        IEnumerable<RecipeApiModel> GetAllRecipes();
        void DeleteRecipeById(Guid id);
        void UpdateRecipe(Guid id, string newName);
        Task<RecipeDetailsApiModel> GetRecipeSummaryDetails(Guid recipeId);
        Task<List<ApiIngredientModel>> GetIngredientsDetailsForRecipe(Guid recipeId, int numberOfPersons = 1);
        void AddStep(RecipeStep step, Guid recipeId);
        void RemoveStep(Guid step);
        void AddRecipeIngredientsToShoppingList(Guid recipeId, int numberOfPersons, Guid shoppingListId);
    }
}
