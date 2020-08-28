using System;
using System.Collections.Generic;
using PunterHomeApp.Services;
using PunterHomeDomain.Models;

namespace PunterHomeDomain.Interfaces
{
    public interface IRecipeService
    {
        void CreateRecipe(string recipeName);
        IEnumerable<Recipe> GetAllRecipes();
        void DeleteRecipeById(Guid id);
        void UpdateRecipe(Guid id, string newName);
    }
}
