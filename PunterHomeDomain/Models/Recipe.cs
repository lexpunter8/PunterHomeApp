using PunterHomeApp.Services;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;

namespace PunterHomeDomain.Models
{

    public class RecipeAggregate : IAggregateRoot
    {
        private List<IngredientValueObject> myIngredients = new List<IngredientValueObject>();
        private List<RecipeStepValueObject> mySteps = new List<RecipeStepValueObject>();
        public RecipeAggregate(string name)
        {
            Name = name;
        }

        public Guid Id { get; }
        public string Name { get; }
        public ERecipeType Type { get; }
        public IEnumerable<RecipeStepValueObject> Steps => mySteps;
        public IEnumerable<IngredientValueObject> Ingredients => myIngredients;
        public bool IsAvailable { get; set; }

        public void AddIngredient(IngredientValueObject ingredient)
        {
            myIngredients.Add(ingredient);
        }


        public void AddStep(string step)
        {
            mySteps.Add(new RecipeStepValueObject
            {
                Text = step,
                Order = mySteps.Count + 1
            });
        }
    }
    public class RecipeApiModel : IName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ERecipeType Type { get; set; }
        public IEnumerable<RecipeStep> Steps { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public bool IsAvailable { get; set; }

        public void AddIngredient(Guid ingredientId)
        {
            throw new NotImplementedException();
        }
    }
}
