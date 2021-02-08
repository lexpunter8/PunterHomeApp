using System;
using System.Collections.Generic;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class DbShoppingListItemInfo
    {
        public Guid Id { get; set; }
        public Guid ShoppingListItemId { get; set; }
        public DbShoppingListItem ShoppingListItem { get; set; }
        public List<DbShoppingListItemMeasurement> ShoppingListItemMeasurements { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int MeasurementAmount { get; set; }
        public EShoppingListReason Reason { get; set; }
        public DbRecipeShoppingListItem RecipeItem { get; set; }
    }

    public class DbShoppingListItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ShoppingListId { get; set; }
        public DbShoppingList ShoppingList { get; set; }
        public List<DbShoppingListItemInfo> ItemInfos { get; set; }
        public List<DbShoppingListItemMeasurement> ShoppingListItemMeasurements { get; set; }
        public DbProduct Product { get; set; }
        public bool IsChecked { get; set; }
    }

    public class DbRecipeShoppingListItem
    {
        public Guid RecipeId { get; set; }
        public Guid ShoppingListItemId { get; set; }
        public DbShoppingListItemInfo ShoppingListItem { get; set; }
        public DbRecipe Recipe { get; set; }
        public int NrOfPersons { get; set; }
    }
}
