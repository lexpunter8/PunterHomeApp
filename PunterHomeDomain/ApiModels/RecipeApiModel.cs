using System;
using System.Collections.Generic;
using PunterHomeApp.Services;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Models;
using static Enums;
using EUnitMeasurementType = Enums.EUnitMeasurementType;

namespace PunterHomeDomain.ApiModels
{
    public class RecipeDetailsApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ERecipeType Type { get; set; }
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

    public class NewRecipeApiModel
    {
        public string Name { get; set; }

        public ERecipeType Type { get; set; }

    }


    public class ImportRecipeApiModel
    {
        public string Url { get; set; }

    }

}
