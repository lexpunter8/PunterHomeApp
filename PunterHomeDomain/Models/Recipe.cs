using PunterHomeApp.Services;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Interfaces;
using System;
using System.Collections.Generic;

namespace PunterHomeDomain.Models
{

    public class RecipeAggregate : IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ERecipeType Type { get; set; }
        public IEnumerable<Guid> StepsIds { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public bool IsAvailable { get; set; }
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
