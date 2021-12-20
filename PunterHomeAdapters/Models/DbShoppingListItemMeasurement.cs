using System;

namespace PunterHomeAdapters.Models
{
    public class DbShoppingListProductMeasurementItem
    {
        public Guid ShoppingListId { get; set; }
        public DbShoppingList ShoppingList { get; set; }
        public int ProductQuantityId { get; set; }
        public DbProductQuantity ProductQuantity { get; set; }

        public int Count { get; set; }
    }
}
