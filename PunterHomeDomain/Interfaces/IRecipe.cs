using System;
using System.Collections.Generic;
using PunterHomeApp.Services;

namespace PunterHomeDomain.Interfaces
{
    public interface IRecipe
    {
        string Name { get; set; }
        List<string> Steps { get; set; }
        List<IIngredient> Ingredients { get; set; } 
    }
}
