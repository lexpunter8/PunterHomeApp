using System;
using System.Collections.Generic;

namespace PunterHomeAdapters.Models
{
    public class DbProductQuantity
    {
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public DbProduct Product { get; set; }

        public List<DbShoppingListProductsMeasurement> ShoppingListProductsMeasurements { get; set; }

        public IEnumerable<DbShoppingListProductMeasurementItem> DbShoppingListsItems { get; set; }
        public int QuantityTypeVolume { get; set; }
        public Enums.EUnitMeasurementType UnitQuantityType { get; set; }
        public double UnitQuantity { get; set; }
        public string Barcode { get; set; }
    }
}
