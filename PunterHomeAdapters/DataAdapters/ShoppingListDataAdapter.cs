using DataModels;
using DataModels.Measurements;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using PunterHomeDomain.Services;
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
            var listItems = context.ShoppingListItems.Include(i => i.ProductQuantity).ThenInclude(p => p.Product).Include(i => i.Recipe).Where(i => i.ShoppingList.Id == shoppingListId).ToList();

            foreach (var i in listItems)
            {
                var newItem = new ShoppingListItemModel
                {
                    Id = i.Id,
                    ShoppingListId = shoppingListId,
                    IsChecked = i.IsChecked,
                    StaticCount = i.StaticCount,
                    DynamicCount = i.DynamicCount
                };

                if (i.Recipe != null)
                {
                    newItem.Recipe = new RecipeShoppingListItem
                    {
                        RecipeId = i.Recipe.Id,
                        RecipeName = i.Recipe.Name
                    };
                }

                if (i.ProductQuantity != null)
                {
                    newItem.Product = new ProductShoppingListItem
                    {
                        ProductId = i.ProductQuantity.ProductId,
                        ProductName = i.ProductQuantity.Product.Name,
                        Count = i.StaticCount
                    };
                    var q = context.ProductQuantities.FirstOrDefault(p => p.Id == i.ProductQuantityId);
                    var measurement = BaseMeasurement.GetMeasurement(q.UnitQuantityType);
                    measurement.UnitQuantityTypeVolume = q.QuantityTypeVolume * i.StaticCount;
                    measurement.ProductQuantityId = q.Id;
                    measurement.Barcode = q.Barcode;
                    newItem.Product.Measurement = measurement;
                    newItem.StaticCount = i.StaticCount;
                }
                items.Add(newItem);

            }
            return items;
        }


        public void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            bool isAddRecipe = request.RecipeId != null && request.RecipeId != Guid.Empty;

            var existingItem = context.ShoppingListItems.Include(i => i.ProductQuantity)
                                                        .Include(i => i.Recipe)
                                                        .FirstOrDefault(o => o.ProductQuantityId == request.ProductMeasurementId || o.RecipeId == request.RecipeId);
            if (existingItem == null)
            {
                existingItem = new DbShoppingListItem
                {
                    IsChecked = false,
                    ShoppingList = context.ShoppingLists.First(),
                    StaticCount = isAddRecipe ? request.RecipeOnlyAvailable ? 0 : request.NrOfPersons : request.MeasurementAmount,
                    DynamicCount = isAddRecipe ? request.RecipeOnlyAvailable ? request.NrOfPersons : 0 : 0
                };

                if (isAddRecipe)
                {
                    existingItem.ProductQuantityId = null;
                    existingItem.RecipeId = request.RecipeId;

                    var recipe = context.Recipes.Include(r => r.Ingredients).ThenInclude(i => i.Product).First(r => r.Id == request.RecipeId);

                    foreach (var ingredient in recipe.Ingredients)
                    {
                        var shoppingListProduct = context.ShoppingListProducts.FirstOrDefault(sp => ingredient.Product.Id == sp.FkProductId);
                        if (shoppingListProduct == null)
                        {
                            context.ShoppingListProducts.Add(new DbShoppingListProduct
                            {
                                FkProductId = ingredient.ProductId,
                                FkShoppingListId = shoppingListId,
                                IsChecked = false
                            });
                        }
                    }
                }

                if (!isAddRecipe)
                {
                    existingItem.ProductQuantityId = request.ProductMeasurementId;
                    existingItem.RecipeId = null;

                    var p = context.ProductQuantities.Include(pq => pq.Product).First(pq => pq.Id == request.ProductMeasurementId);
                    var shoppingListProduct = context.ShoppingListProducts.FirstOrDefault(sp => p.Product.Id == sp.FkProductId);
                    if (shoppingListProduct == null)
                    {
                        context.ShoppingListProducts.Add(new DbShoppingListProduct
                        {
                            FkProductId = p.ProductId,
                            FkShoppingListId = shoppingListId,
                            IsChecked = false
                        });
                    }
                }
                context.ShoppingListItems.Add(existingItem);
                context.SaveChanges();
                return;
            }
            
            if (isAddRecipe)
            {
                if (request.RecipeOnlyAvailable)
                {
                    existingItem.DynamicCount += request.NrOfPersons;
                }
                else
                {
                    existingItem.StaticCount += request.NrOfPersons;
                }
            }

            if (!isAddRecipe)
            {
                existingItem.StaticCount += request.MeasurementAmount;
            }
            context.SaveChanges();
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

            DbShoppingListItem item = context.ShoppingListItems.Include(i => i.ProductQuantity).Include(i => i.Recipe).FirstOrDefault(s => s.Id == itemId);
            
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

            context.ShoppingListProducts.FirstOrDefault(sl => sl.FkProductId == itemId).IsChecked = isChecked;

            if (!isChecked)
            {
                var toRemove = context.ShoppingListProducts.Include(sp => sp.ProductsMeasurements).First(i => i.FkProductId == itemId);
                context.ShoppingListProductsMeasurements.RemoveRange(toRemove.ProductsMeasurements);
            }

            context.SaveChanges();
        }

        

        private RecipeShoppingListItemModel GetRecipeShoppingListItem(DbRecipeShoppingListItem item)
        {
            if (item == null)
            {
                return null;
            }

            return new RecipeShoppingListItemModel
            {
                NrOfPersons = item.NrOfPersons,
                RecipeId = item.RecipeId,
                RecipeName = item.Recipe.Name
            };
        }

        public void AddProductToShoppingList(Guid listId, ShoppingListItemModel item)
        {
            throw new NotImplementedException();
        }

        public void AddMeasurementsToItem(Guid shoppingListItemId, int measurementId, int count)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var pq = context.ProductQuantities.First(pq => pq.Id == measurementId);
            var sp = context.ShoppingListProducts.First(sp => sp.FkProductId == pq.ProductId);

            context.ShoppingListProductsMeasurements.Add(new DbShoppingListProductsMeasurement
            {
                Count = count,
                FkProductQuantityId = measurementId,
                FkShoppingListProductId = sp.Id
            });

            context.SaveChanges();
        }

        public List<MeasurementForShopItemModel> GetMeasurementsForCheckedItem(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var result = context.ShoppingListProducts.Include(spq => spq.ProductsMeasurements).ThenInclude(m => m.FkProductQuantity)
                .FirstOrDefault(m => m.FkProductId == id && m.IsChecked);

            if (result == null)
            {
                return new List<MeasurementForShopItemModel>();
            }

            return result.ProductsMeasurements.Select(m => GetBaseMeasurement(m))
                .ToList();
        }

        private MeasurementForShopItemModel GetBaseMeasurement(DbShoppingListProductsMeasurement m)
        {
            var n = BaseMeasurement.GetMeasurement(m.FkProductQuantity.UnitQuantityType);
            n.Barcode = m.FkProductQuantity.Barcode;
            n.ProductQuantityId = m.FkProductQuantity.Id;
            n.UnitQuantityTypeVolume = m.FkProductQuantity.QuantityTypeVolume;
            return new MeasurementForShopItemModel(n)
            {
                Count = m.Count
            };
        }

        public List<ShoppingListProductModel> GetProductsForShoppingListId(Guid shoppingListId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var products = context.ShoppingListProducts.Include(sp => sp.ProductsMeasurements).Where(ps => ps.FkShoppingListId == shoppingListId).ToList();
            return products.Select(p => ConvertToShoppingListProductModel(p)).ToList();

        }

        private ShoppingListProductModel ConvertToShoppingListProductModel(DbShoppingListProduct p)
        {
            return new ShoppingListProductModel
            {
                ProductId = p.FkProductId,
                IsChecked = p.IsChecked,
                ProductsMeasurements = p.ProductsMeasurements.Select(s => ConvertToShoppingListProductMeasurement(s)).ToList()
            };
        }

        private ShoppingListProductMeasurement ConvertToShoppingListProductMeasurement(DbShoppingListProductsMeasurement m)
        {
            return new ShoppingListProductMeasurement
            {
                Count = m.Count,
                ProductQuantityId = m.FkProductQuantityId
            };
        }


    }
}
