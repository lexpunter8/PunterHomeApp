using System;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class DbShoppingListItem
    {
        public Guid Id { get; set; }
        public DbShoppingList ShoppingList { get; set; }
        public DbProduct Product { get; set; }
        public DbRecipeShoppingListItem RecipeItem { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int MeasurementAmount { get; set; }
        public bool IsChecked { get; set; }
        public EShoppingListReason Reason { get; set; }
    }

    public class DbRecipeShoppingListItem
    {
        public Guid RecipeId { get; set; }
        public Guid ShoppingListItemId { get; set; }
        public DbShoppingListItem ShoppingListItem { get; set; }
        public DbRecipe Recipe { get; set; }
        public int NrOfPersons { get; set; }
    }
}
