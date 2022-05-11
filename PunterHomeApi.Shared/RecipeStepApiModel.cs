using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Enums;

namespace PunterHomeApi.Shared
{
    public class RecipeStepIngredientApiModel
    {

        public Guid ProductId { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public Guid RecipeStepId { get; set; }
        public string ProductName { get; set; }
    }
    public class RecipeStepApiModel
    {
        public Guid RecipeId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public List<RecipeStepIngredientApiModel> Ingredients { get; set; } = new List<RecipeStepIngredientApiModel>();

    }

    public class AddTextToShoppingList
    {
        public Guid ShoppingListId { get; set; }
        public string Text { get; set; }
    }


    public class AddMeasurementsToShoppingList
    {
        public Guid ShoppingListId { get; set; }
        public int MeasurementId { get; set; }
        public int Count { get; set; }
    }


    public class AddRecipeToShoppingListItem
    {
        public Guid ShoppingListId { get; set; }
        public Guid RecipeId { get; set; }
        public int Amount { get; set; }
    }
}
