using PunterHomeApp.Services;
using System;

namespace PunterHomeDomain.Interfaces
{
    public interface IIngredientService
    {
        void InsertIngredient(IIngredient newIngredient);
        void DeleteIngredient(Guid recipeId, Guid productId);
    }
}
