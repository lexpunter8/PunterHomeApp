//using System;
//namespace HomeApp.Frontend.View.Queries
//{
//	public class ShoppingListDto
//	{
//		public Guid Id { get; set; }
//		public List<ShoppingListItemDto> Items { get; set; }
//	}

//	public class ShoppingListItemDto
//	{
//	    public string ItemName { get; set; }
//        public string AmountValueString { get; set; }
//        public int Count { get; set; }
//    }

//	public interface ISchoppingListProvider
//	{
//		IEnumerable<ShoppingListDto> GetAllObjects<T>();
//		Task Insert(Guid shoppingListId, ShoppingListItemDto item);
//    }

//	public interface IShoppingListQueries
//	{
//		object GetAllObjects();
//		object FindById(Guid id);
//	}

//	public class DummyShoppingListQueries : ISchoppingListProvider
//    {
//		public List<ShoppingListDto> myShoppingLists = new List<ShoppingListDto>();

//        public IEnumerable<T> GetAllObjects<T>()
//        {
//            return (IEnumerable<T>)myShoppingLists;
//        }
//    }
//}

