using System;
using System.Collections.Generic;
using PunterHomeApp.Services;
using PunterHomeDomain.Models;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class RecipeDetailsApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RecipeStep> Steps { get; set; } 
        public List<ApiIngredientModel> Ingredients { get; set; }

        public bool IsRecipeAvailable => Ingredients.TrueForAll(i => i.IsAvaliable);
    }
    public class ApiIngredientModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public bool IsAvaliable { get; set; }
    }

}
