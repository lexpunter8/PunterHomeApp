using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Components;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Pages
{
    public class EditableRecipeStep
    {
        public EditableRecipeStep(RecipeStep step)
        {
            Step = step;
        }

        public RecipeStep Step { get; }
        public bool IsEditting { get; set; }
        public bool IsOrderEditting { get; set; }
    }
    public partial class RecipedetailsViewBase : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public BlazorRecipeService RecipeService { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        public bool IsEditting { get; set; }

        public int IngredientMultiplier { get; set; } = 1;
        public RecipeDetailsApiModel Recipedetails { get; set; }
        public List<EditableRecipeStep> RecipeSteps { get; set; } = new List<EditableRecipeStep>();
        protected async override void OnParametersSet()
        {
            await Refresh();
            StateHasChanged();

        }
        public bool Open { get; set; }

        public void OnClose()
        {

        }

        public async void IngredientAddedHandler(IngredientModel ingredient)
        {
            Recipedetails.Ingredients.Add(new ApiIngredientModel
            {
                ProductId = ingredient.ProductId,
                ProductName = ingredient.ProductName,
                UnitQuantity = ingredient.UnitQuantity,
                UnitQuantityType = ingredient.UnitQuantityType
            });

            await Refresh();
            StateHasChanged();
        }
        
        public string newStepText = string.Empty;


        public async Task RemoveIngredient(Guid id)
        {
            await RecipeService.RemoveIngredient(Recipedetails.Id, id);
            await Refresh();
        }

        public async void ChangePersons(int x)
        {
            IngredientMultiplier = x;
            var newIngredients = await RecipeService.GetIngredientsForRecipeById(Recipedetails.Id, x);

            foreach (var item in newIngredients)
            {
                Recipedetails.Ingredients.FirstOrDefault(i => i.ProductId == item.ProductId).IsAvaliable = item.IsAvaliable;
            }
            StateHasChanged();
        }

        public async void Drop(EditableRecipeStep step)
        {
            startDragStep.Step.Order = step.Step.Order;
            await RecipeService.UpdateStep(startDragStep.Step);
            await Refresh();
        }

        EditableRecipeStep startDragStep;
        public void StartDrag(EditableRecipeStep step)
        {
            startDragStep = step;
        }

        public async void UpdateStep(EditableRecipeStep step)
        {
            await RecipeService.UpdateStep(step.Step);
            step.IsEditting = false;
            StateHasChanged();
        }

        public async void AddStep()
        {
            await RecipeService.AddStep(new RecipeStep
            {
                Order = Recipedetails.Steps.Count + 1,
                Text = newStepText
            }, Recipedetails.Id);
            newStepText = string.Empty;

            await Refresh();
        }

        public async void AddIngredientsToShoppingList(bool onlyUnavailable = false)
        {
            await RecipeService.AddToShoppingList(Recipedetails.Id, IngredientMultiplier, Guid.Empty, onlyUnavailable);
        }

        public async void ShowAddProductModal()
        {
            var parameters = new ModalParameters();
            parameters.Add(nameof(SearchSelectProductComponent.ExistingProductIds), Recipedetails.Ingredients.Select(p => p.ProductId).ToArray());
            parameters.Add(nameof(SearchSelectProductComponent.RecipeId), Recipedetails.Id);

            var adddQuantityModal = Modal.Show<SearchSelectProductComponent>("Add ingredients", parameters);

            var result = await adddQuantityModal.Result;

            if (!result.Cancelled)
            {
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            Recipedetails = await RecipeService.GetRecipeById(Id);
            RecipeSteps = Recipedetails.Steps.Select(s => new EditableRecipeStep(s)).ToList();
            StateHasChanged();
        }

        public void ToggleOrderEditting(EditableRecipeStep step)
        {
            if (!IsEditting)
            {
                return;
            }
            var currentSelected = RecipeSteps.FirstOrDefault(s => s.IsOrderEditting);
            if (currentSelected != null)
            {
                startDragStep = currentSelected;
                Drop(step);
                return;
            }
            bool currentVal = step.IsOrderEditting;
            RecipeSteps.ForEach(s => s.IsOrderEditting = false);
            RecipeSteps.ForEach(s => s.IsEditting = false);

            step.IsOrderEditting = !currentVal;
        }

        public void ToggleTextEditting(EditableRecipeStep step)
        {
            if (!IsEditting)
            {
                return;
            }
            bool currentVal = step.IsEditting;
            RecipeSteps.ForEach(s => s.IsEditting = false);
            RecipeSteps.ForEach(s => s.IsOrderEditting = false);

            step.IsEditting = !currentVal;
        }
    }
}
