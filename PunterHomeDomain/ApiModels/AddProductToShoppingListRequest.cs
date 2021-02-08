using System;
using System.Collections;
using System.Collections.Generic;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class AddProductToShoppingListRequest
    {
        public Guid ProductId { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int MeasurementAmount { get; set; }
        public EShoppingListReason Reason { get; set; }
        public Guid ShoppingListId { get; set; }

        public Guid RecipeId { get; set; }
        public int NrOfPersons { get; set; }
    }

    public class AddMeasurementsToShoppingListItem
    {
        public Guid ShoppingListItemId { get; set; }
        public int MeasurementId { get; set; }
        public int Count { get; set; }
    }
}
