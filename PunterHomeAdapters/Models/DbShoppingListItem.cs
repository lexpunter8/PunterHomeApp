using System;
using System.Collections.Generic;

namespace PunterHomeAdapters.Models
{
    public class DbShoppingListItem
    {
        public Guid Id { get; set; }
        public DbShoppingList ShoppingList { get; set; }
        public DbProductQuantity ProductQuantities { get; set; }
        public int Count { get; set; }
    }
}
