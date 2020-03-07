using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Models;
using PunterHomeApp.Interfaces;
using PunterHomeApp.Models;
using PunterHomeApp.Services;
using PunterHomeDomain.Models;

namespace PunterHomeApp.DataAdapters
{
    public class RecipeDataAdapter : IRecipeDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;

        public RecipeDataAdapter(DbContextOptions<HomeAppDbContext> options)
        {
            myDbOptions = options;
        }

        public Task<bool> AddIngredient(Guid recipeId, IIngredient ingredient)
        {
            return Task.Run(() =>
            {
                using var context = new HomeAppDbContext(myDbOptions);

                DbProduct product = context.Products.FirstOrDefault(p => p.Id.Equals(ingredient.Product.Id));
                if (product == null)
                {
                    return false;
                }
                DbRecipe recipe = context.Recipes.FirstOrDefault(p => p.Id.Equals(recipeId));
                if (product == null)
                {
                    return false;
                }
                context.Ingredients.AddAsync(new DbIngredient
                {
                    Product = product,
                    Recipe = recipe,
                    UnitQuantity = ingredient.UnitQuantity,
                    UnitQuantityType = ingredient.UnitQuantityType
                });
                context.SaveChanges();
                return true;
            });
        }

        public IRecipe GetRecipeById(Guid recipeId)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            return DbRecipeToRecipe(context.Recipes.Single(r => r.Id == recipeId));
        }

        private Recipe DbRecipeToRecipe(DbRecipe dbRecipe)
        {
            return new Recipe
            {
                Id = dbRecipe.Id,
                Name = dbRecipe.Name,
                Steps = dbRecipe.Steps,
                Ingredients = dbRecipe.Ingredients.Select(ConvertDbIngredient)
            };
        }

        public void SaveRecipe(IRecipe recipe)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            // I create the library 
            var dbRecipe = new DbRecipe
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Steps = new List<string>(recipe.Steps)
            };

            var ingredients = recipe.Ingredients.Select(r => new DbIngredient
            {
                Recipe = dbRecipe,
                ProductId = r.Product.Id,
                UnitQuantity = r.UnitQuantity,
                UnitQuantityType = r.UnitQuantityType
            }).ToList();

            // Linking the books (Library2Book table) to the library
            dbRecipe.Ingredients.AddRange(ingredients);

            // Adding the data to the DbContext.
            context.Recipes.Add(dbRecipe);

            // Save the changes and everything should be working!
            context.SaveChanges();
        }

        private DbProduct ConvertProductToDbProduct(IProduct product)
        {
            return new DbProduct
            {
                Id = product.Id,
                UnitQuantityType = product.UnitQuantityType,
                UnitQuantity = product.UnitQuantity,
                Name = product.Name,
                Quantity = product.Quantity
            };
        }

        private DbRecipe ConvertRecipeToDbRecipe(IRecipe recipe)
        {
            return new DbRecipe
            {
            };
        }

        private DbIngredient ConvertIngredientToDbIngredient(IIngredient ingredient)
        {
            return new DbIngredient
            {
                Product = new DbProduct
                {
                    Id = ingredient.Product.Id,

                }
            };
        }

        public IEnumerable<IRecipe> GetAllRecipes()
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var recipes = context.Recipes.Include(r => r.Ingredients).ThenInclude(i => i.Product).ToList();


            return recipes.Select(r => new Recipe
                {
                    Id = r.Id,
                    Name = r.Name,
                    Steps = r.Steps,
                    Ingredients = r.Ingredients.Select(ConvertDbIngredient)
            });
        }

        private IIngredient ConvertDbIngredient(DbIngredient dbIngredient)
        {
            return new Ingredient
            {
                Product = dbIngredient.Product,
                UnitQuantity = dbIngredient.UnitQuantity,
                UnitQuantityType = dbIngredient.UnitQuantityType
            };
        }
    }
}
