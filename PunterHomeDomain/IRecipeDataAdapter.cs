using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeApp.Services;
using PunterHomeDomain.Models;

namespace PunterHomeApp.Interfaces
{
    public interface IRecipeDataAdapter
    {
        Task<IEnumerable<Recipe>> GetAllRecipes();
        void SaveRecipe(Recipe recipe);
        IRecipe GetRecipeById(Guid recipeId);
        Task<bool> AddIngredient(Guid recipeId, IIngredient ingredient);
        void UpdateRecipe(Guid id, string newName);
        void DeleteById(Guid id);
    }
}
