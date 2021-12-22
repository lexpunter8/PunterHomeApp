using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeApp.Services;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Models;

namespace PunterHomeDomain
{
    public interface IRecipeDataAdapter
    {
        Task<IEnumerable<RecipeApiModel>> GetAllRecipes();
        void SaveRecipe(string name, ERecipeType type);
        RecipeApiModel GetRecipeById(Guid recipeId);
        Task<bool> AddIngredient(Guid recipeId, IIngredient ingredient);
        void UpdateRecipe(Guid id, string newName);
        void DeleteById(Guid id);
        void AddStep(string text, int order, Guid recipeId);
        void RemoveStep(Guid stepId);
        void UpdateStep(Guid stepId, string text = null, int order = 0);
        IEnumerable<RecipeStep> GetStepForRecipe(Guid guid);


        //ddd
        RecipeStepAggregate GetRecipeStep(Guid id);
    }
}
