using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class DbProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Collection<DbIngredient> Ingredients { get; set; }
        public Collection<DbShoppingListProduct> ShoppingListProducts { get; set; }
        public IEnumerable<DbProductQuantity> ProductQuantities { get; set; }
        public IEnumerable<DbRecipeStepIngredient> RecipeStepIngredients { get; set; }
        public EMeasurementClass MeasurementClass { get; set; }
        public string MeasurementValues { get; set; }
    }
}
