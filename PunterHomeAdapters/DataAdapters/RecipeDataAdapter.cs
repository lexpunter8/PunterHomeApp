﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Models;
using PunterHomeApp.Services;
using PunterHomeDomain;
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

                DbProduct product = context.Products.FirstOrDefault(p => p.Id.Equals(ingredient.ProductId));
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

        public RecipeApiModel GetRecipeById(Guid recipeId)
        {
            using var context = new HomeAppDbContext(myDbOptions);


            var result = context.Recipes
                                    .Include(r => r.Ingredients)
                                    .ThenInclude(i => i.Recipe).Include(r => r.Steps)
                                    .ThenInclude(s => s.Recipe)
                                    .FirstOrDefault(d => d.Id == recipeId);

            result.Ingredients.ForEach(i => i.Product = context.Products.Include(p => p.ProductQuantities).FirstOrDefault(p => p.Id == i.ProductId));
            //result.Ingredients.ForEach(i => i.Product = context.Products.FirstOrDefault(p => p.Id == i.ProductId));


            return DbRecipeToRecipe(result);
        }

        private RecipeApiModel DbRecipeToRecipe(DbRecipe dbRecipe)
        {
            return new RecipeApiModel
            {
                Id = dbRecipe.Id,
                Name = dbRecipe.Name,
                Steps = dbRecipe.Steps.Select(r => Convert(r)),
                Ingredients = dbRecipe.Ingredients.Select(ConvertDbIngredient)
            };
        }

        public void SaveRecipe(Services.IRecipe recipe)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            // I create the library 
            var dbRecipe = new DbRecipe
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Steps = recipe.Steps.Select(rs => Convert(rs)).ToList()
            };

            var ingredients = recipe.Ingredients.Select(r => new DbIngredient
            {
                Recipe = dbRecipe,
                ProductId = r.ProductId,
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

        private RecipeApiModel ConvertDbRecipeToRecipe(DbRecipe recipe)
        {
            return new RecipeApiModel
            {
                Name = recipe.Name,
                Id = recipe.Id,
                Steps = recipe.Steps.Select(s => Convert(s)),
                Ingredients = recipe.Ingredients.Select(i => ConvertDbIngredient(i))
            };
        }

        private Ingredient ConvertDbIngredient(DbIngredient dbIngredient)
        {
            return new Ingredient
            {
                ProductId = dbIngredient.ProductId,
                RecipeId = dbIngredient.RecipeId,
                ProductName = dbIngredient.Product.Name,
                UnitQuantity = dbIngredient.UnitQuantity,
                UnitQuantityType = dbIngredient.UnitQuantityType
            };
        }

        private RecipeStep Convert(DbRecipeStep recipe)
        {
            return new RecipeStep
            {
                Id = recipe.Id,
                Order = recipe.Order,
                Text = recipe.Text
            };
        }

        private DbRecipeStep Convert(IRecipeStep recipe)
        {
            return new DbRecipeStep
            {
                Id = recipe.Id,
                Order = recipe.Order,
                Text = recipe.Text
            };
        }

        public async Task<IEnumerable<RecipeApiModel>> GetAllRecipes()
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var result = await context.Recipes.Include(r => r.Ingredients).ThenInclude(i => i.Recipe).Include(r => r.Steps).ThenInclude(s => s.Recipe).ToListAsync();

            result.ForEach(r => r.Ingredients.ForEach(i => i.Product = context.Products.FirstOrDefault(p => p.Id == i.ProductId)));


            return result.Select(r => ConvertDbRecipeToRecipe(r));
        }

        public void SaveRecipe(RecipeApiModel recipe)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            context.Recipes.Add(new DbRecipe
            {
                Name = recipe.Name
            });

            context.SaveChanges();
        }

        public void UpdateRecipe(Guid id, string newName)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var recipe = context.Recipes.FirstOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                return;
            }

            recipe.Name = newName;

            context.SaveChanges();
        }

        public void DeleteById(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var recipe = context.Recipes.FirstOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                return;
            }

            context.Recipes.Remove(recipe);

            context.SaveChanges();
        }
    }
}

