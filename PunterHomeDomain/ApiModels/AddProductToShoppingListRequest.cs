using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.ApiModels
{
    public class AddProductToShoppingListRequest
    {
        public Guid ProductId { get; set; }
        public global::Enums.EUnitMeasurementType MeasurementType { get; set; }
        public int MeasurementAmount { get; set; }
        public global::Enums.EShoppingListReason Reason { get; set; }
        public Guid ShoppingListId { get; set; }

        public Guid RecipeId { get; set; }
        public int NrOfPersons { get; set; }
    }
}
