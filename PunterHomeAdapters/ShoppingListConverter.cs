using DataModels;
using PunterHomeAdapters.Models;

namespace PunterHomeAdapters
{
    public static class ShoppingListConverter
    {
        public static ShoppingListModel Convert(this DbShoppingList item)
        {
            return new ShoppingListModel
            {

                Id = item.Id,
                Name = item.Name,
                CreateTime = item.CreateTime

            };
        }

        public static DbShoppingList Convert(this ShoppingListModel item)
        {
            return new DbShoppingList
            {

                Id = item.Id,
                Name = item.Name,
                CreateTime = item.CreateTime
            };
        }
    }
}
