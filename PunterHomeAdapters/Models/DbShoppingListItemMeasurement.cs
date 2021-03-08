using System;

namespace PunterHomeAdapters.Models
{
    public class DbShoppingListItemMeasurement
    {
        public Guid ShoppingListItemId { get; set; }
        public int ProductQuantityId { get; set; }
        public DbProductQuantity ProductQuantity { get; set; }
        public DbShoppingListItem ShoppingListItem{ get; set; }

        public int Count { get; set; }
    }
}
