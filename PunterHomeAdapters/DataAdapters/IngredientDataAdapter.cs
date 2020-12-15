using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters.Models;
using PunterHomeApp.Services;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PunterHomeAdapters.DataAdapters
{
    public class IngredientDataAdapter : IIngredientDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public IngredientDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public void Delete(Guid productId, Guid recipeId)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var ingredientToRemove = context.Ingredients.FirstOrDefault(i => i.ProductId == productId && i.RecipeId == recipeId);

            if (ingredientToRemove == null)
            {
                return;
            }

            context.Ingredients.Remove(ingredientToRemove);
            context.SaveChanges();
        }

        public void Insert(IIngredient ingredient)
        {
            DbIngredient newIngredient = new DbIngredient
            {
                ProductId = ingredient.ProductId,
                RecipeId = ingredient.RecipeId,
                UnitQuantity = ingredient.UnitQuantity,
                UnitQuantityType = ingredient.UnitQuantityType
            };

            using var context = new HomeAppDbContext(myDbOptions);

            context.Ingredients.Add(newIngredient);
            context.SaveChanges();
        }
    }
}
