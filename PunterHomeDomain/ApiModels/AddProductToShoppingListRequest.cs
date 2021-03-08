using System;
using System.Collections;
using System.Collections.Generic;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class AddProductToShoppingListRequest
    {
        public Guid ShoppingListId { get; set; }
        public int ProductMeasurementId { get; set; }

        /// <summary>
        /// times to add the productMeasurement
        /// </summary>
        public int MeasurementAmount { get; set; }

        public bool RecipeOnlyAvailable { get; set; }
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
