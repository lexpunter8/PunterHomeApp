using PunterHomeDomain.Commands.RecipeStepCommand.Requests;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PunterHomeDomain.Commands.RecipeStepCommand
{
    public interface IRecipeStepCommanHandlers
    {
        Task Handle(CreateRecipeStep request);
        Task Handle(AddIngredientToRecipeStep request);
        Task Handle(RemoveIngredientFromRecipeStep request);
    }

    public class RecipeStepCommanHandlers : IRecipeStepCommanHandlers
    {
        private readonly IRecipeStepRepository recipeStepRepository;

        public RecipeStepCommanHandlers(IRecipeStepRepository recipeStepRepository)
        {
            this.recipeStepRepository = recipeStepRepository;
        }

        public async Task Handle(AddIngredientToRecipeStep request)
        {
            var recipeStep = await recipeStepRepository.GetAsync(request.RecipeStepId);
            recipeStep.AddIngredient(request.IngredientId, request.RecipeStepId);
            await recipeStepRepository.SaveAsync(recipeStep);
        }


        public async Task Handle(RemoveIngredientFromRecipeStep request)
        {
            var recipeStep = await recipeStepRepository.GetAsync(request.RecipeStepId);
            recipeStep.RemoveIngredient(request.IngredientId, request.RecipeStepId);
            await recipeStepRepository.SaveAsync(recipeStep);
        }

        public async Task Handle(CreateRecipeStep request)
        {
            await recipeStepRepository.SaveAsync(new RecipeStepAggregate(Guid.NewGuid(), request.RecipeId, request.Text, request.Order, new List<RecipeStepIngredient>()));
        }
    }
}
