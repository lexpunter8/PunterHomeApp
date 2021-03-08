using DataModels.Measurements;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    //public class ShoppingListItemInfoModel
    //{
    //    public Guid Id { get; set; }
    //    public EUnitMeasurementType MeasurementType { get; set; }
    //    public int MeasurementAmount { get; set; }
    //    public RecipeShoppingListItemModel RecipeItem { get; set; }
    //    public EShoppingListReason Reason { get; set; }

    //}

    public class ShopItemMeasurementAmount
    {
        public Guid ProductId { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public double StaticMeasurementAmount { get; set; }
        public double DynamicMeasurementAmount { get; set; }
    }


    public class ProductShoppingListItem
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        public BaseMeasurement Measurement { get; set; }
        public int Count { get; set; }
    }
    public class RecipeShoppingListItem
    {
        public Guid RecipeId { get; set; }
        public string RecipeName { get; set; }
        public int StaticCount { get; set; }
        public int DynamicCount { get; set; }
    }

    public class ShoppingListItemModel
    {
        public Guid Id { get; set; }
        public Guid ShoppingListId { get; set; }

        public bool IsChecked { get; set; }
        public RecipeShoppingListItem Recipe { get; set; }
        public ProductShoppingListItem Product { get; set; }
        public int StaticCount { get; set; }
        public int DynamicCount { get; set; }
        public bool IsProduct => Product != null;

        //public Guid Id { get; set; }
        //public EUnitMeasurementType MeasurementType { get; set; }
        //public double MeasurementVolume { get; set; }
        //public int Count { get; set; }
        //public string Reason { get; set; }
        //public string ProductName { get; set; }
        //public bool IsChecked { get; set; }
        //public Guid ShoppingListId { get; set; }
        //public Guid ProductId { get; set; }
    }

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
