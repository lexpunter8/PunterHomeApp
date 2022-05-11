using System;

namespace PunterHomeDomain.ShoppingList
{
    public partial class ShoppingListAggregate
    {
        public static ShoppingListAggregate CreateNew(string name) => new ShoppingListAggregate(Guid.NewGuid(), name, EShoppingListStatus.Active, DateTime.Now);
    }
}
