using System;
using System.Collections.Generic;
using PunterHomeApp.Services;

namespace PunterHomeDomain.Models
{
    public class Recipe : IRecipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RecipeStep> Steps { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
    }
}
