﻿using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Data
{
    public class ShoppingListItem
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int MeasurementVolume { get; set; }
        public int Quantity { get; set; }

        public bool IsChecked { get; set; }
    }

    public class SelectableShoppingListItem
    {
        public SelectableShoppingListItem(ShoppingListShopItem item)
        {
            Item = item;
        }

        public ShoppingListShopItem Item { get; }
    }
}
