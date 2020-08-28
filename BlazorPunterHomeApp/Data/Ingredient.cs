using BlazorPunterHomeApp.crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Data
{
    public class IngredientModel
    {
        public ProductModel Product { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitQuantityType UnitQuantityType { get; set; }
    }

    public class RecipeStepModel
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
    }
}
