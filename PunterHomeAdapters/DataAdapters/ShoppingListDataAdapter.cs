using DataModels;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
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
            var listItems = context.ShoppingListItems.Include(s => s.RecipeItem).Include(r => r.RecipeItem).ThenInclude(r => r.Recipe).Include(i => i.Product).Where(i => i.ShoppingList.Id == shoppingListId).ToList();

            foreach (var i in listItems)
            {
                var newItem = new ShoppingListItemModel
                {
                    Id = i.Id,
                    ShoppingListId = shoppingListId,
                    IsChecked = i.IsChecked,
                    MeasurementAmount = i.MeasurementAmount,
                    MeasurementType = i.MeasurementType,
                    Reason = i.Reason,
                    RecipeItem = i.RecipeItem == null ? null : new RecipeShoppingListItemModel
                    {
                        ShoppingListItemId = i.Id,
                        NrOfPersons = i.RecipeItem.NrOfPersons,
                        RecipeId = i.RecipeItem.RecipeId,
                        RecipeName = i.RecipeItem.Recipe.Name
                    },
                    ProductId = i.Product.Id,
                    ProductName = i.Product.Name
                };

                items.Add(newItem);

            }
            return items;
        }

        public void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var newItem = new DbShoppingListItem
            {
                IsChecked = false,
                Product = context.Products.First(p => p.Id == request.ProductId),
                MeasurementType = request.MeasurementType,
                MeasurementAmount = request.MeasurementAmount,
                Reason = request.Reason,
                ShoppingList = context.ShoppingLists.First()
            };

            if (request.RecipeId != null && request.RecipeId != Guid.Empty)
            {
                newItem.RecipeItem = new DbRecipeShoppingListItem
                {
                    RecipeId = request.RecipeId,
                    NrOfPersons = request.NrOfPersons
                };
            }

            context.ShoppingListItems.Add(newItem);
            context.SaveChanges();

            //    var list = context.ShoppingLists.FirstOrDefault(s => s.Id == shoppingListId);
            //    var productQuan = context.ProductQuantities.FirstOrDefault(s => s.Id == productQuantyId);
            //    if (list == null)
            //    {
            //        throw new KeyNotFoundException($"{nameof(DbShoppingList)} with id {shoppingListId} not found");
            //    }

            //    if (productQuan == null)
            //    {
            //        throw new KeyNotFoundException($"{nameof(DbProductQuantity)} with id {productQuantyId} not found");
            //    }

            //    var item = context.ShoppingListItems.Include(i => i.ShoppingList).Include(i => i.ProductQuantities)
            //        .FirstOrDefault(s => s.ShoppingList.Id == shoppingListId && s.ProductQuantities.Id == productQuantyId);

            //    if (item != null)
            //    {
            //        item.Count += count;
            //        context.SaveChanges();
            //        return;
            //    }

            //    var newItem = new DbShoppingListItem
            //    {
            //        ShoppingList = list,
            //        Count = count,
            //        ProductQuantities = productQuan
            //    };

            //    context.ShoppingListItems.Add(newItem);
            //    context.SaveChanges();
        }

        public void UpdateShoppingListCount(Guid shoppingListItemId, int delta)
        {
            //using var context = new HomeAppDbContext(myDbOptions);

            //var item = context.ShoppingListItems.FirstOrDefault(s => s.Id == shoppingListItemId);

            //if (item == null)
            //{
            //    throw new KeyNotFoundException($"{nameof(DbShoppingListItem)} with id {shoppingListItemId} not found");
            //}

            //item.Count += delta;

            //context.SaveChanges();
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

            context.ShoppingListItems.FirstOrDefault(sl => sl.Id == itemId).IsChecked = isChecked;
            context.SaveChanges();
        }

        public void AddProductToShoppingList(Guid listId, ShoppingListItemModel item)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            context.ShoppingListItems.Add(new DbShoppingListItem
            {
                ShoppingList = context.ShoppingLists.First(s => s.Id == listId),
                Product = context.Products.First(p => p.Id == item.ProductId),
                MeasurementType = item.MeasurementType,
                MeasurementAmount= item.MeasurementAmount,
                IsChecked = false,
                Reason = item.Reason
            });
            context.SaveChanges();
        }
    }
}
