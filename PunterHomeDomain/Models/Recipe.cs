using PunterHomeDomain.Enums;
using System;
using System.Collections.Generic;

namespace PunterHomeDomain.Models
{
    public class RecipeApiModel 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ERecipeType Type { get; set; }
        public IEnumerable<RecipeStep> Steps { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public bool IsAvailable { get; set; }
    }
}
