using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeApp.Services;
using PunterHomeDomain.Models;

namespace PunterHomeDomain
{
    public interface IRecipeDataAdapter
    {
        Task<IEnumerable<RecipeApiModel>> GetAllRecipes();
        void SaveRecipe(RecipeApiModel recipe);
        RecipeApiModel GetRecipeById(Guid recipeId);
        Task<bool> AddIngredient(Guid recipeId, IIngredient ingredient);
        void UpdateRecipe(Guid id, string newName);
        void DeleteById(Guid id);
    }
}
