using DataModels;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Interfaces;
using PunterHomeAdapters.Models;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.ShoppingList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShoppingListStatus = PunterHomeDomain.ShoppingList.EShoppingListStatus;

namespace PunterHomeApi.Queries
{
    public class ShoppingListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public EShoppingListStatus Status { get; set; }
    }


    public class ShoppingListProductItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public double Amount { get; set; }
        public int MeasurementType { get; set; }
        public bool IsCheck { get; internal set; }
    }

    public class ShoppingListRecipeItemDto
    {
        public Guid Id { get; set; }
        public int Amount { get; set; }
        public string RecipeName { get; set; }
        public Guid RecipeId { get; set; }
    }
    public interface IShoppingListQueries
    {
        ShoppingListDto GetShoppingList(Guid id);

        IEnumerable<ShoppingListItemDto> GetTextItemsForShoppingList(Guid id);
        IEnumerable<ShoppingListDto> GetShoppingLists();
        IEnumerable<ShoppingListProductItemDto> GetProductItemsForShoppingList(Guid shoppinglistId);
        IEnumerable<ShoppingListRecipeItemDto> GetRecipeItemsForShoppingList(Guid shoppinglistId);
        IEnumerable<DbIngredient> GetIngredientForRecipe(Guid recipeId);
    }

    public interface ICqrsCommand<T>
    {
        T DoCommand();
    }

    public class MoveUncheckedShoppinglistItems : ICqrsCommand<Task<bool>>
    {
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly Guid recipeId;

        public MoveUncheckedShoppinglistItems(IShoppingListRepository shoppingListRepository, Guid recipeId)
        {
            this.shoppingListRepository = shoppingListRepository;
            this.recipeId = recipeId;
        }
        public async Task<bool> DoCommand()
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(recipeId);

                IEnumerable<ShoppingListProductItem> uncheckedProductItems = shoppingList.ProductItems.Where(w => !w.IsChecked).ToArray();
                var uncheckedTextItems = shoppingList.TextItems.Where(w => !w.IsChecked).ToArray();

                ShoppingListAggregate newShoppingList = ShoppingListAggregate.CreateNew(shoppingList.Name);

                foreach (var item in uncheckedProductItems)
                {
                    newShoppingList.AddProductItem(item.ProductId, item.Amount, (EUnitMeasurementType)item.MeasurementType);
                    shoppingList.RemoveProduct(item.ProductId);
                }

                foreach (var item in uncheckedTextItems)
                {
                    newShoppingList.AddTextItem(item.Value);
                    shoppingList.RemoveTextItem(item.Value);
                }
                shoppingList.FinishShoppingList();

                await shoppingListRepository.SaveAsync(newShoppingList);
            }
            catch (Exception)
            {

                throw;
            }
            return true;
        }
    }

    public class ShoppingListItemDto
    {
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }

    public class ShoppingListQueries : IShoppingListQueries
    {
        private readonly IShoppingListQueryRepository repository;
        private readonly HomeAppDbContext dbContext;

        public ShoppingListQueries(HomeAppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<ShoppingListDto> GetShoppingLists()
        {
            return dbContext.ShoppingLists.Where(w => w.Status != EShoppingListStatus.Closed).Select(s => new ShoppingListDto
            {
                CreateTime = s.CreateTime,
                Id = s.Id,
                Name = s.Name,
                Status = s.Status
            });
        }


        public ShoppingListDto GetShoppingList(Guid id)
        {
            var s = dbContext.ShoppingLists.FirstOrDefault(f => f.Id == id);
            
            if (s == null)
            {
                throw new NotImplementedException();
            }
            
            return new ShoppingListDto
            {
                CreateTime = s.CreateTime,
                Id = s.Id,
                Name = s.Name,
                Status = s.Status
            };
        }

        public IEnumerable<ShoppingListItemDto> GetTextItemsForShoppingList(Guid id)
        {
            return dbContext.ShoppingLists.Include(i => i.TextItems).SingleOrDefault(s => s.Id == id)?.TextItems.Select(s => new ShoppingListItemDto
            {
                IsChecked = s.IsChecked,
                Value = s.Value
            });
        }

        public IEnumerable<ShoppingListProductItemDto> GetProductItemsForShoppingList(Guid shoppinglistId)
        {

            var shoppingList = dbContext.ShoppingLists.Include(i => i.ProductItems).SingleOrDefault(s => s.Id == shoppinglistId);
            return shoppingList.ProductItems.Select(r => new ShoppingListProductItemDto
            {
                Amount = r.Amount,
                MeasurementType = r.MeasurementType,
                ProductName = dbContext.Products.First(f => f.Id == r.ProductId).Name,
                ProductId = r.ProductId,
                IsCheck = r.IsChecked
            }).ToList();
            throw new NotImplementedException();
        }

        public IEnumerable<ShoppingListRecipeItemDto> GetRecipeItemsForShoppingList(Guid shoppinglistId)
        {
            var shoppingList = dbContext.ShoppingLists.Include(i => i.RecipeItems).SingleOrDefault(s => s.Id == shoppinglistId);
            return shoppingList.RecipeItems.Select(r => new ShoppingListRecipeItemDto
            {
                Amount = r.Amount,
                RecipeId = r.RecipeId,
                RecipeName = dbContext.Recipes.First().Name
            }).ToList();
            
            //return dbContext.ShoppingListRecipe.Where(s => EF.Property<Guid>(s, "ShoppingListAggregateId") == shoppinglistId).Select(s => new ShoppingListRecipeItemDto
            //{
            //    Amount = s.Amount,
            //    Id = EF.Property<Guid>(s, "Id"),
            //    RecipeId = s.RecipeId,
            //    RecipeName = dbContext.Recipes.FirstOrDefault(f => f.Id == s.RecipeId).Name,
            //});
        }

        public IEnumerable<DbIngredient> GetIngredientForRecipe(Guid recipeId)
        {
            return dbContext.Ingredients.Where(w => w.RecipeId == recipeId).ToList();
        }
    }
}
