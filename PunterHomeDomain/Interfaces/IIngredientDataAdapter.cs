using PunterHomeApp.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.Interfaces
{
    public interface IIngredientDataAdapter
    {
        void Insert(IIngredient ingredient);
        void Delete(IIngredient ingredient);
    }
}
