using System;
using System.Collections.Generic;
using System.Text;
using static Enums;

namespace PunterHomeDomain.Models
{
    public class RecipeShoppingListItemModel
    {
            public Guid ShoppingListItemId { get; set; }
            public Guid RecipeId { get; set; }
            public int NrOfPersons { get; set; }
            public string RecipeName { get; set; }

    }
        public class ProductShoppingListItemModel
        {

            public Guid ShoppingListItemId { get; set; }
            public Guid ProductId { get; set; }

            public EUnitMeasurementType MeasurementType { get; set; }
            public int MeasurementAmount { get; set; }
        }
}
