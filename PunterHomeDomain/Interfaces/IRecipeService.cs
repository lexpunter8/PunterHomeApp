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
    }
}
