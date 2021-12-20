using System;
using System.Collections.Generic;

namespace PunterHomeAdapters.Models
{
    public class DbShoppingListProductsMeasurement
    {
        public Guid Id { get; set; }

        public DbShoppingListProduct FkShoppingListProduct { get; set; }
        public Guid FkShoppingListProductId { get; set; }

        public DbProductQuantity FkProductQuantity { get; set; }
        public int FkProductQuantityId { get; set; }

        public int Count { get; set; }
    }

    public class DbShoppingListProduct
    {
        public Guid  Id { get; set; }

        public DbShoppingList FkShoppingList { get; set; }
        public Guid FkShoppingListId { get; set; }

        public DbProduct FkProduct { get; set; }
        public Guid FkProductId{ get; set; }

        public List<DbShoppingListProductsMeasurement> ProductsMeasurements { get; set; }

        public bool IsChecked { get; set; }
    }

    public class DbShoppingListItem
    {
        public Guid Id { get; set; }

        public DbShoppingList FkShoppingList { get; set; }
        public Guid FkShoppingListId { get; set; }

        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }

    public class DbShoppingList
    {
        public Guid Id { get; set; }

        List<DbShoppingListProduct> ShoppingListProducts { get; set; }
        List<DbShoppingListProductMeasurementItem> ProductMeasurementItems { get; set; }
        List<DbShoppingListRecipeItem> RecipeItems { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
