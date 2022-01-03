using System;

namespace PunterHomeApi.Shared
{
    public class AddIngredientToRecipeStepRequest
    {
        public Guid RecipeStepId { get; set; }
        public Guid IngredientId { get; set; }
    }
    public class RemoveIngredientFromRecipeStepRequest
    {
        public Guid RecipeStepId { get; set; }
        public Guid IngredientId { get; set; }
    }
}
