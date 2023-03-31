using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PunterHomeAdapters;
using PunterHomeAdapters.Models;
using PunterHomeApp.Services;
using PunterHomeDomain;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Models;

namespace PunterHomeApp.DataAdapters
{
    public class RecipeDataAdapter : IRecipeDataAdapter
    {
        private DbContextOptions<HomeAppDbContext> myDbOptions;
        private readonly IMapper mapper;

        public RecipeDataAdapter(DbContextOptions<HomeAppDbContext> options, IMapper mapper)
        {
            myDbOptions = options;
            this.mapper = mapper;
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
                                    .ThenInclude(i => i.Recipe).Include(r => r.Steps).ThenInclude(S => S.Ingredients)
                                    .FirstOrDefault(d => d.Id == recipeId);

            result.Ingredients.ForEach(i => i.Product = context.Products.FirstOrDefault(p => p.Id == i.ProductId));
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
                Type = recipe.Type,
                Steps = recipe.Steps.Select(s => Convert(s)),
                Ingredients = recipe.Ingredients?.Select(i => ConvertDbIngredient(i))
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
                Text = recipe.Text,
                RecipeId = recipe.Recipe.Id,
                Ingredients = recipe.Ingredients?.Select(s => mapper.Map<RecipeStepIngredient>(s)).ToList()
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

        public void SaveRecipe(string name, ERecipeType type)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            context.Recipes.Add(new DbRecipe
            {
                Name = name,
                Type = type
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

        public void AddStep(string text, int order, Guid recipeId)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var recipe = context.Recipes.FirstOrDefault(r => r.Id == recipeId);
            if (recipe == null)
            {
                return;
            }

            var step = new DbRecipeStep
            {
                Id = Guid.NewGuid(),
                Order = order,
                Text = text,
                Recipe = recipe
            };

            context.RecipeSteps.Add(step);
            context.SaveChanges();
        }

        public void RemoveStep(Guid stepId)
        {
            using var context = new HomeAppDbContext(myDbOptions);
            var step = context.RecipeSteps.FirstOrDefault(r => r.Id == stepId);
            if (step == null)
            {
                return;
            }

            context.RecipeSteps.Remove(step);
            context.SaveChanges();
        }

        public void UpdateStep(Guid stepId, string text = null, int order = 0)
        {
            if (text == null && order == 0)
            {
                return;
            }

            using var context = new HomeAppDbContext(myDbOptions);

            var step = context.RecipeSteps.FirstOrDefault(s => s.Id == stepId);

            if (text != null)
            {
                step.Text = text;
            }

            if (order != 0)
            {
                step.Order = order;
            }

            context.SaveChanges();
        }

        public IEnumerable<RecipeStep> GetStepForRecipe(Guid guid)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            return context.RecipeSteps
                .Include(s => s.Recipe)
                .Where(r => r.Recipe.Id == guid)
                .Select(s => new RecipeStep
                            {
                                Id = s.Id,
                                Order = s.Order,
                                RecipeId = s.Recipe.Id,
                                Text = s.Text
                            })
                .ToList();
        }

        public RecipeStepAggregate GetRecipeStep(Guid id)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var result = context.RecipeSteps.FirstOrDefault(rs => rs.Id == id);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return new RecipeStepAggregate(result.Id, result.Recipe.Id, result.Text, result.Order, new List<RecipeStepIngredient>());
        }

        public void SaveRecipe(RecipeAggregate recipe)
        {
            using var context = new HomeAppDbContext(myDbOptions);

            var existingRecipe = context.Recipes.Include(i => i.Ingredients).Include(i => i.Steps).FirstOrDefault(f => f.Name.ToLower() == recipe.Name.ToLower());

            if (existingRecipe == null)
            {
                existingRecipe = new DbRecipe
                {
                    Name = recipe.Name
                };
                context.Recipes.Add(existingRecipe);
            }

            existingRecipe.Steps.Clear();
            foreach (var step in recipe.Steps)
            {
                existingRecipe.Steps.Add(new DbRecipeStep
                {
                    Recipe = existingRecipe,
                    Text = step.Text,
                    Order = step.Order
                });
            }


            existingRecipe.Ingredients.Clear();
            foreach (var ingredient in recipe.Ingredients)
            {
                var existingProduct = context.Products.FirstOrDefault(p => p.Name.ToLower() == ingredient.ProductName.ToLower());

                if (existingProduct == null)
                {
                    existingProduct = new DbProduct
                    {
                        Name = ingredient.ProductName,
                    };

                    context.Products.Add(existingProduct);
                }

                existingRecipe.Ingredients.Add(new DbIngredient
                {
                    Recipe= existingRecipe,
                    UnitQuantity = (int)ingredient.UnitQuantity,
                    UnitQuantityType = ingredient.UnitQuantityType,
                    Product = existingProduct
                });
            }

            context.SaveChanges();
        }
    }

}

