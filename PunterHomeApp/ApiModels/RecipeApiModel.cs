using System;
using System.Collections.Generic;
using PunterHomeApp.Services;
using PunterHomeDomain.ApiModels;
using static Enums;

namespace PunterHomeApp.ApiModels
{
    public class RecipeApiModel1
    {
        public string Name { get; set; }
        public List<string> Steps { get; set; }
        public List<ApiIngredientModel> Ingredients { get; set; }
    }
    public class ApiIngredientModel1
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
    }

}
