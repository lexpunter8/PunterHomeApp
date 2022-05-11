using System;
using System.Collections;
using System.Collections.Generic;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class AddProductToShoppingListRequest
    {
        public Guid ShoppingListId { get; set; }
        public Guid ProductId { get; set; }
        public double Amount { get; set; }

        /// <summary>
        /// times to add the productMeasurement
        /// </summary>
        public PunterHomeDomain.Enums.EUnitMeasurementType MeasurementType { get; set; }
    }

    public class AddMeasurementsToShoppingListItem
    {
        public Guid ShoppingListItemId { get; set; }
        public int MeasurementId { get; set; }
        public int Count { get; set; }
    }
}
