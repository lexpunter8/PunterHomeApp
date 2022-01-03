using System;

namespace PunterHomeDomain.Commands.RecipeStepCommand.Requests
{
    public class CreateRecipeStep
    {
        public string Text { get; set; }
        public int Order { get; set; }
        public Guid RecipeId { get; set; }
    }
    public class AddIngredientToRecipeStep
    {
        public Guid RecipeStepId { get; set; }
        public Guid IngredientId { get; internal set; }
    }


    public class RemoveIngredientFromRecipeStep
    {
        public Guid RecipeStepId { get; set; }
        public Guid IngredientId { get; internal set; }
    }
}