using System;
using System.Collections.Generic;

namespace DataModels
{
    public class ShoppingListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class ShoppingListProductModel
    {
        public Guid ProductId { get; set; }
        public List<ShoppingListProductMeasurement> ProductsMeasurements { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ShoppingListProductMeasurement
    {
        public Guid Id { get; set; }
        public int ProductQuantityId { get; set; }

        public int Count { get; set; }
    }

}
