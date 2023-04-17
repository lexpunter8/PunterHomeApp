using System;
using HomeAppDomain.AggregateRoots;
using HomeAppDomain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HomeAppRepositories.Entities;

namespace HomeAppRepositories.Repositories
{
    public class ShoppingListItemRepository : IShoppingListItemRepository
    {
        private readonly HomeAppContext context;
        private readonly Mapper mapper;

        public ShoppingListItemRepository(HomeAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IEnumerable<ShoppingListItem> GetAll()
        {
            return context.ShoppingListItem.ToList();
        }

        public ShoppingListItem GetById(Guid id)
        {
            var item = context.ShoppingListItem.FirstOrDefault(w => w.Id == id);
            return item;
        }

        public async Task RemoveById(Guid id)
        {
            var item = context.ShoppingListItem.FirstOrDefault(w => w.Id == id);
            context.ShoppingListItem.Remove(item);
            await context.SaveChangesAsync();
        }

        public async Task Save(ShoppingListItem item)
        {
            if (context.ShoppingListItem.Any(a => a.Id == item.Id))
            {
                context.ShoppingListItem.Update(item);
            }
            else
            {
                context.ShoppingListItem.Add(item);
            }
            await context.SaveChangesAsync();
        }
    }

    public class ShoppingListRepository : IShoppingListRepository
	{
        private readonly HomeAppContext context;

        public ShoppingListRepository(HomeAppContext context)
		{
            this.context = context;
        }

        public IEnumerable<ShoppingList> GetAll()
        {
            throw new NotImplementedException();
        }

        public ShoppingList GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Save(ShoppingList shoppingList)
        {
            throw new NotImplementedException();
        }
    }
}

