using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Models;

namespace PunterHomeDomain.Interfaces
{
    public interface IRecipeService
    {
        void CreateRecipe(string recipeName, ERecipeType type);
        IEnumerable<RecipeApiModel> GetAllRecipes();
        void DeleteRecipeById(Guid id);
        void UpdateRecipe(Guid id, string newName);
        Task<RecipeDetailsApiModel> GetRecipeSummaryDetails(Guid recipeId);
        Task<List<ApiIngredientModel>> GetIngredientsDetailsForRecipe(Guid recipeId, int numberOfPersons = 1);
        void AddStep(RecipeStep step, Guid recipeId);
        void RemoveStep(Guid step);
        void AddRecipeIngredientsToShoppingList(Guid recipeId, int numberOfPersons, Guid shoppingListId, bool onlyUnavailableItems);
        Task<IEnumerable<RecipeApiModel>> Search(SearchRecipeParameters parameters);
        void UpdateStep(RecipeStep step);

        /// ddd

        //RecipeStepAggregate AddIngredientToStep(Guid stepId, Guid ingredientId);
    }

    
}
