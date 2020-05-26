using System;
using System.Collections.Generic;
using PunterHomeApp.Services;

namespace PunterHomeDomain.Interfaces
{
    public interface IRecipeService
    {
        void CreateRecipe(string recipeName, List<string> steps, List<Ingredient> ingredients);
        IEnumerable<IRecipe> GetAllRecipes();
    }
}
