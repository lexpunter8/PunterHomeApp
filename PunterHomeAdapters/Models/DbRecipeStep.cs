using System;
using System.Collections.Generic;
using System.Text;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class RecipeStepOrder
    {
        public Guid RecipeStepId { get; set; }
        public Guid RecipeId { get; set; }
        public int Order { get; set; }
    }
    public class DbRecipeStep
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public Guid RecipeId { get; set; }
        public DbRecipe Recipe { get; set; }

        public List<DbRecipeStepIngredient> Ingredients { get; set;}

    }

    public class DbRecipeStepIngredient
    {
        public Guid RecipeStepId { get; set; }

        public Guid ProductId { get; set; }

        public virtual DbRecipeStep RecipeStep { get; set; }
        public virtual DbProduct Product { get; set; }

        public double UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
    }
}
