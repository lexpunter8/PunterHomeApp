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
            var recipeItems = context.ShoppingListRecipeItem.Include(i => i.Recipe).Where(i => i.ShoppingList.Id == shoppingListId).ToList();
            var productItems = context.ShoppingListProductMeasurementItem.Include(i => i.ProductQuantity).ThenInclude(ti => ti.Product).Where(i => i.ShoppingList.Id == shoppingListId).ToList();
            var textItems = context.ShoppingListTextItems.Where(i => i.FkShoppingListId == shoppingListId).ToList();

            foreach (var i in recipeItems)
            {
                var newItem = new ShoppingListItemModel
                {
                    ShoppingListId = shoppingListId,
                    StaticCount = i.StaticCount,
                    DynamicCount = i.DynamicCount
                };

                newItem.Recipe = new RecipeShoppingListItem
                {
                    RecipeId = i.Recipe.Id,
                    RecipeName = i.Recipe.Name
                };

                items.Add(newItem);
            }

            var groupedByProduct = productItems.GroupBy(g => g.ProductQuantity.Product);

            foreach (var i in groupedByProduct)
            {
                var newItem = new ShoppingListItemModel
                {
                    ShoppingListId = shoppingListId,
                    DynamicCount = 0
                };

                newItem.Product = new ProductShoppingListItem
                {
                    ProductId = i.Key.Id,
                    ProductName = i.Key.Name,
                    Measurements = new List<ProductShoppingListMeasurementModel>()
                };

                foreach (DbShoppingListProductMeasurementItem g in i)
                {
                    var q = context.ProductQuantities.FirstOrDefault(p => p.Id == g.ProductQuantityId);
                    var measurement = BaseMeasurement.GetMeasurement(q.UnitQuantityType);
                    measurement.UnitQuantityTypeVolume = q.QuantityTypeVolume;
                    measurement.ProductQuantityId = q.Id;
                    measurement.Barcode = q.Barcode;
                    newItem.Product.Measurements.Add(new ProductShoppingListMeasurementModel { Measurement = measurement, Count = g.Count });
                }
                items.Add(newItem);
            }

            items.AddRange(textItems.Select(i => new ShoppingListItemModel
            {
                Id = i.Id,
                ItemValue = i.Name,
                IsChecked = i.IsChecked
            }));
            return items;
        }


        public void AddProductToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            bool isAddRecipe = request.RecipeId != null && request.RecipeId != Guid.Empty;
            if (request.ShoppingListId == null || request.ShoppingListId == Guid.Empty)
            {
                request.ShoppingListId = context.ShoppingLists.First().Id;
            }

            if (isAddRecipe)
            {

                var existingRecipe = context.ShoppingListRecipeItem.Include(i => i.ShoppingList)
                                                            .Include(i => i.Recipe)
                                                            .FirstOrDefault(o => o.RecipeId == request.RecipeId);
                if (existingRecipe == null)
                {
                    existingRecipe = new DbShoppingListRecipeItem
                    {
                        ShoppingListId = request.ShoppingListId,
                        RecipeId = request.RecipeId,
                        StaticCount = request.RecipeOnlyAvailable ? 0 : request.NrOfPersons,
                        DynamicCount = request.RecipeOnlyAvailable ? request.NrOfPersons : 0
                    };
                    var recipe = context.Recipes.Include(r => r.Ingredients).ThenInclude(i => i.Product).First(r => r.Id == request.RecipeId);

                    foreach (var ingredient in recipe.Ingredients)
                    {
                        var shoppingListProduct = context.ShoppingListProducts.FirstOrDefault(sp => ingredient.Product.Id == sp.FkProductId);
                        if (shoppingListProduct == null)
                        {
                            context.ShoppingListProducts.Add(new DbShoppingListProduct
                            {
                                FkProductId = ingredient.ProductId,
                                FkShoppingListId = request.ShoppingListId,
                                IsChecked = false
                            });
                        }
                    }
                    context.ShoppingListRecipeItem.Add(existingRecipe);
                }
                else
                {
                    existingRecipe.DynamicCount += request.RecipeOnlyAvailable ? request.NrOfPersons : 0;
                    existingRecipe.StaticCount += request.RecipeOnlyAvailable ? 0 : request.NrOfPersons;
                }

                context.SaveChanges();
                return;
            }
            
            var existingProduct = context.ShoppingListProductMeasurementItem.Include(i => i.ProductQuantity)
                                                        .Include(i => i.ShoppingList)
                                                        .FirstOrDefault(o => o.ProductQuantityId == request.ProductMeasurementId);
            if (existingProduct == null)
            {
                existingProduct = new DbShoppingListProductMeasurementItem
                {
                    ProductQuantityId = request.ProductMeasurementId,
                    ShoppingListId = request.ShoppingListId,
                    Count = request.MeasurementAmount
                };

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
                context.ShoppingListProductMeasurementItem.Add(existingProduct);
            }
            else
            {

                existingProduct.Count += request.MeasurementAmount;
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

        public void RemoveProductFromShoppingList(Guid productId, Guid shoppinglistId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var item = context.ShoppingListProductsMeasurements
                                            .Include(i => i.FkShoppingListProduct)
                                            .FirstOrDefault(s => s.FkShoppingListProduct.FkProductId == productId 
                                                                && s.FkShoppingListProduct.FkShoppingListId == shoppinglistId);

            if (item == null)
            {
                return;
            }

            context.ShoppingListProductsMeasurements.Remove(item);

            //var productToRemove = context.ShoppingListProducts.FirstOrDefault(s => s.FkProductId == itemId);
            context.ShoppingListProducts.Remove(item.FkShoppingListProduct);

            context.SaveChanges();
            CleanupShoppinglist(shoppinglistId);
        }

        public void CleanupShoppinglist(Guid shoppingListId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var productsForShoppinglist = context.ShoppingListProducts.Where(p => p.FkShoppingListId == shoppingListId);
            
            // remove all text items
            context.ShoppingListTextItems.RemoveRange(context.ShoppingListTextItems.Where(p => p.FkShoppingListId == shoppingListId));

            if (productsForShoppinglist.Count() == 0)
            {
                var recipeItemsToRemove = context.ShoppingListRecipeItem.Where(s => s.ShoppingListId == shoppingListId);
                var productItemsToRemove = context.ShoppingListProductMeasurementItem.Where(s => s.ShoppingListId == shoppingListId);

                context.ShoppingListRecipeItem.RemoveRange(recipeItemsToRemove);
                context.ShoppingListProductMeasurementItem.RemoveRange(productItemsToRemove);
                context.SaveChanges();
                return;
            }

            //var rToRemove = context.ShoppingListRecipeItem.Include(r => r.Recipe)
            //                              .ThenInclude(r => r.Ingredients)
            //                              .Where(r => r.ShoppingListId == shoppingListId && r.Recipe.Ingredients.All(i => !productsForShoppinglist.Any(p => p.FkProductId == i.ProductId)));

            //var pToRemove = context.ShoppingListProductMeasurementItem.
        }

        public void UpdateChecked(Guid itemId, bool isChecked)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var productItem = context.ShoppingListProducts.FirstOrDefault(sl => sl.FkProductId == itemId);

            if (productItem == null)
            {
                context.ShoppingListTextItems.FirstOrDefault(s => s.Id == itemId).IsChecked = isChecked;
                context.SaveChanges();
                return;
            }
            productItem.IsChecked = isChecked;
            if (!isChecked)
            {
                var toRemove = context.ShoppingListProducts.Include(sp => sp.ProductsMeasurements).First(i => i.FkProductId == itemId);
                context.ShoppingListProductsMeasurements.RemoveRange(toRemove.ProductsMeasurements);
            }

            context.SaveChanges();
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

        public void UpdateProductQuantity(Guid shoppingListId, int quantityId, int delta)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var item = context.ShoppingListProductMeasurementItem.FirstOrDefault(p => p.ProductQuantityId == quantityId && p.ShoppingListId == shoppingListId);

            if (item == null)
            {
                return;
            }

            item.Count = Math.Max(0, item.Count + delta);
            context.SaveChanges();
        }

        public void UpdaterecipeQuantity(Guid shoppingListId, Guid recipeId, int delta)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var item = context.ShoppingListRecipeItem.FirstOrDefault(p => p.RecipeId == recipeId && p.ShoppingListId == shoppingListId);

            if (item == null)
            {
                return;
            }

            item.StaticCount = Math.Max(0, item.StaticCount + delta);
            context.SaveChanges();
        }

        public void SaveItemToShoppingList(Guid shoppingListID, string text)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            DbShoppingListItem newItem = new DbShoppingListItem
            {
                FkShoppingListId = shoppingListID,
                Name = text
            };
            context.ShoppingListTextItems.Add(newItem);
            context.SaveChanges();
        }
    }
}
