using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeApp.Services;

namespace PunterHomeApp.Interfaces
{
    public interface IRecipeDataAdapter
    {
        IEnumerable<IRecipe> GetAllRecipes();
        void SaveRecipe(IRecipe recipe);
        IRecipe GetRecipeById(Guid recipeId);
        Task<bool> AddIngredient(Guid recipeId, IIngredient ingredient);
    }
}
