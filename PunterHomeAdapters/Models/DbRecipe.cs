using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PunterHomeAdapters.Models
{
    public class DbRecipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<DbRecipeStep> Steps { get; set; } = new List<DbRecipeStep>();
        public List<DbIngredient> Ingredients { get; set; } = new List<DbIngredient>();
    }

}
