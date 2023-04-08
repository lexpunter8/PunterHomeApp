using System;
using HomeAppDomain.AggregateRoots;

namespace HomeAppDomain.Interfaces
{
	public interface IShoppingListRepository
	{
		IEnumerable<ShoppingList> GetAll();
        ShoppingList GetById(Guid id);
        void Save(ShoppingList shoppingList);
    }

	public interface IShoppingListItemRepository
	{
		IEnumerable<ShoppingListItem> GetAll();
        ShoppingListItem GetById(Guid id);
        void Save(ShoppingListItem item);
		void RemoveById(Guid id);
    }
}

