using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PunterHomeDomain.Models
{
    public interface IRecipeStep
    {
        Guid Id { get; set; }
        int Order { get; set; }
        string Text { get; set; }
    }

    public class RecipeStep : IRecipeStep
    {
        public Guid RecipeId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public List<RecipeStepIngredient> Ingredients { get; set; } = new List<RecipeStepIngredient>();
    }


    public class RecipeStepAggregate : IAggregateRoot
    {
        private RecipeStepAggregate()
        {

        }
        public RecipeStepAggregate(Guid id,
                                   Guid recipeId,
                                   string instruction,
                                   int order,
                                   List<RecipeStepIngredient> ingredients)
        {
            Id = id;
            RecipeId = recipeId;
            Text = instruction;
            Order = order;
            Ingredients = ingredients;
        }

        public Guid Id { get; private set; }
        public Guid RecipeId { get; private set; }
        public int Order { get; private set; }
        public string Text { get; private set; }
        public List<RecipeStepIngredient> Ingredients { get; private set; }

        public void AddIngredient(Guid ingredientId, Guid recipeStepId)
        {
            if (Ingredients.Any(i => i.ProductId == ingredientId))
            {
                throw new InvalidOperationException("Ingredient already added to step");
            }

            Ingredients.Add(new RecipeStepIngredient
            {
                ProductId = ingredientId,
                RecipeStepId = recipeStepId,
                UnitQuantity = 0,
                UnitQuantityType = 0,
            });
        }


        public void RemoveIngredient(Guid ingredientId, Guid recipeStepId)
        {
            var ingredient = Ingredients.FirstOrDefault(i => i.ProductId == ingredientId);

            if (ingredient == null)
            {
                throw new InvalidOperationException("Ingredient doesn't exist for this recipestep");
            }

            Ingredients.Remove(ingredient);
        }

    }

    public class RecipeStepValueObject
    {
        public Guid RecipeStepId { get; set; }
        public Guid  RecipeId { get; set; }
        public int Order { get; set; }
    }
}
