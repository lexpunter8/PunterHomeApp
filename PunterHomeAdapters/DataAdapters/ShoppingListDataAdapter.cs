using DataModels;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PunterHomeAdapters.DataAdapters
{

    public class ShoppingListDataAdapter : IShoppingListDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public ShoppingListDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public List<ShoppingListModel> GetShoppingLists()
        {
            using var context = new HomeAppDbContext(myDbOptions);

            return context.ShoppingLists.Select(s => s.Convert()).ToList();
        }

        public List<ShoppingListItemModel> GetItemsForShoppingList(Guid shoppingListId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var items = new List<ShoppingListItemModel>();
            var listItems = context.ShoppingListItems.Include(si => si.ProductQuantities).Where(i => i.ShoppingList.Id == shoppingListId).ToList();

            foreach (var i in listItems)
            {
                var prodQuan = context.ProductQuantities.Include(x => x.ProductId).FirstOrDefault(p => p.Id == i.ProductQuantities.Id);
                DbProduct p = context.Products.FirstOrDefault(p => prodQuan.ProductId.Id == p.Id);

                var newItem = new ShoppingListItemModel
                {
                    Id = i.Id,
                    Count = i.Count,
                    ShoppingListId = shoppingListId,
                    MeasurementType = prodQuan.UnitQuantityType,
                    ProductName = p.Name,
                    Volume = prodQuan.QuantityTypeVolume,
                    IsChecked = i.Checked,
                    ProductQuantityId = prodQuan.Id
                };

                items.Add(newItem);

            }
            return items;
        }

        public void AddProductToShoppingList(Guid shoppingListId, int productQuantyId, int count = 1)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var list = context.ShoppingLists.FirstOrDefault(s => s.Id == shoppingListId);
            var productQuan = context.ProductQuantities.FirstOrDefault(s => s.Id == productQuantyId);
            if (list == null)
            {
                throw new KeyNotFoundException($"{nameof(DbShoppingList)} with id {shoppingListId} not found");
            }

            if (productQuan == null)
            {
                throw new KeyNotFoundException($"{nameof(DbProductQuantity)} with id {productQuantyId} not found");
            }

            var item = context.ShoppingListItems.Include(i => i.ShoppingList).Include(i => i.ProductQuantities)
                .FirstOrDefault(s => s.ShoppingList.Id == shoppingListId && s.ProductQuantities.Id == productQuantyId);

            if (item != null)
            {
                item.Count += count;
                context.SaveChanges();
                return;
            }

            var newItem = new DbShoppingListItem
            {
                ShoppingList = list,
                Count = count,
                ProductQuantities = productQuan
            };

            context.ShoppingListItems.Add(newItem);
            context.SaveChanges();
        }

        public void UpdateShoppingListCount(Guid shoppingListItemId, int delta)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var item = context.ShoppingListItems.FirstOrDefault(s => s.Id == shoppingListItemId);

            if (item == null)
            {
                throw new KeyNotFoundException($"{nameof(DbShoppingListItem)} with id {shoppingListItemId} not found");
            }

            item.Count += delta;

            context.SaveChanges();
        }

        public void RemoveProductFromShoppingList(Guid itemId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var item = context.ShoppingListItems.FirstOrDefault(s => s.Id == itemId);
            if (item == null)
            {
                throw new KeyNotFoundException($"{nameof(DbShoppingListItem)} with id {itemId} not found");
            }

            context.ShoppingListItems.Remove(item);
            context.SaveChanges();
        }

        public void UpdateChecked(Guid itemId, bool isChecked)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            context.ShoppingListItems.FirstOrDefault(sl => sl.Id == itemId).Checked = isChecked;
            context.SaveChanges();
        }
    }
}
