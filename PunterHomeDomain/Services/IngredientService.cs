using PunterHomeApp.Services;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientDataAdapter myIngredientDataAdapter;

        public IngredientService(IIngredientDataAdapter ingredientDataAdapter)
        {
            myIngredientDataAdapter = ingredientDataAdapter;
        }
        public void DeleteIngredient(IIngredient newIngredient)
        {
            throw new NotImplementedException();
        }

        public void InsertIngredient(IIngredient newIngredient)
        {
            myIngredientDataAdapter.Insert(newIngredient);
        }
    }
}
