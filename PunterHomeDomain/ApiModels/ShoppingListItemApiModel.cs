using System;
using System.Collections.Generic;
using System.Text;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class ShoppingListItemApiModel
    {
        public Guid Id { get; set; }
        public Guid ShoppingListId { get; set; }
        public int Count { get; set; }

        public string ProductName { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int Volume { get; set; }

        public bool IsChecked { get; set; }
    }

    public class ShoppingListApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }
        public List<ShoppingListItemApiModel> Items { get; set; }
    }
}
