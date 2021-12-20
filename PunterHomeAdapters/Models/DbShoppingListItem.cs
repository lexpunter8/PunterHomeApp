using System;
using System.Collections.Generic;
using static Enums;

namespace PunterHomeAdapters.Models
{
    //public class DbShoppingListItemInfo
    //{
    //    public Guid Id { get; set; }
    //    public Guid ShoppingListItemId { get; set; }
    //    public DbShoppingListItem ShoppingListItem { get; set; }
    //    public List<DbShoppingListItemMeasurement> ShoppingListItemMeasurements { get; set; }
    //    public EUnitMeasurementType MeasurementType { get; set; }
    //    public int MeasurementAmount { get; set; }
    //    public EShoppingListReason Reason { get; set; }
    //    public DbRecipeShoppingListItem RecipeItem { get; set; }
    //}

    public class DbShoppingListRecipeItem
    {
        public Guid ShoppingListId { get; set; }
        public DbShoppingList ShoppingList { get; set; }
        public virtual DbRecipe? Recipe { get; set; }
        public Guid? RecipeId { get; set; }
        public int StaticCount { get; set; }
        public int DynamicCount { get; set; }
    }
}
