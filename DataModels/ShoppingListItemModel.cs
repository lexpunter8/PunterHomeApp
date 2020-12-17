using System;
using static Enums;

namespace DataModels
{
    public class ShoppingListItemModel
    {
        public Guid Id { get; set; }
        public Guid ShoppingListId { get; set; }
        public int Count { get; set; }

        public string ProductName { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int Volume { get; set; }
        public bool IsChecked { get; set; }

        public int ProductQuantityId { get; set; }

    }
}
