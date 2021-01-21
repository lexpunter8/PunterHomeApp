using BlazorPunterHomeApp.crud;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Data
{
    public class IngredientModel
    {
        public Guid ProductId { get; set; }
        public Guid RecipeId { get; set; }
        public string ProductName { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
    }

    public class RecipeStepModel
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
    }
}
