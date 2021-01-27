using System;
namespace PunterHomeDomain.ApiModels
{
    public class RecipeToShoppingListRequestApiModel
    {
        public Guid RecipeId { get; set; }
        public Guid ShoppingListIdId { get; set; }
        public int NumberOfPersons { get; set; }
        public bool OnlyUnavailableItems { get; set; }
    }
}
