using Blazored.Modal;
using Blazored.Modal.Services;
using Blazorise;
using BlazorPunterHomeApp.Components;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeApiConnector;
using PunterHomeApiConnector.Interfaces;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiIngredientModel = PunterHomeDomain.ApiModels.ApiIngredientModel;
using RecipeStep = PunterHomeDomain.Models.RecipeStep;

namespace BlazorPunterHomeApp.Pages
{
    public class TextInputModel
    {
        public string Text { get; set; } = string.Empty;
    }
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

        [Inject]
        IShoppingListApiConnector ShoppingListApiConnector { get; set; }

        public bool IsEditting { get; set; }

        public int IngredientMultiplier { get; set; } = 1;
        public PunterHomeDomain.ApiModels.RecipeDetailsApiModel Recipedetails { get; set; }
        public List<EditableRecipeStep> RecipeSteps { get; set; } = new List<EditableRecipeStep>();
        public SearchSelectProductComponent SearchProductControl { get; set; }

        public ElementReference AddInstructionTextContol { get; set; }
        public ElementReference FocusElement { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
            Focus();

        }
        public async void RemoveIngredientFromStep(Guid stepId, Guid ingredientId)
        {
            await RecipeService.RemoveIngedientFromRecipeStep(ingredientId, stepId);
        }
        public void AddIngredientToStep(Guid stepId, Guid ingredientId)
        {
            RecipeService.AddIngredientToStop(ingredientId, stepId);
        }

        public void Focus()
        {
            //if (FocusElement.Context != null)
            //{
            //    FocusElement.FocusAsync();
            //}
        }
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

        public TextInputModel newStepText { get; set; } = new TextInputModel();
        public IEnumerable<ShoppingListDto> AllShoppingLists { get; private set; } 

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

        public async void UpdateStep(EditableRecipeStep step, bool isCancel = false)
        {
            if (isCancel)
            {
                step.IsEditting = false;
                await Refresh();
                return;
            }
            await RecipeService.UpdateStep(step.Step);
            step.IsEditting = false;
            StateHasChanged();
        }

        public async void AddStep()
        {
            await RecipeService.AddStep(new RecipeStep
            {
                Order = Recipedetails.Steps.Count + 1,
                Text = newStepText.Text
            }, Recipedetails.Id);
            newStepText.Text = string.Empty;

            await Refresh();
            await AddInstructionTextContol.FocusAsync();
        }

        public async void AddIngredientsToShoppingList(bool onlyUnavailable = false)
        {
            //ShoppingListApiConnector.AddRecipeItem()
            //await RecipeService.AddToShoppingList(Recipedetails.Id, IngredientMultiplier, Guid.Empty, onlyUnavailable);

            ShowModal();
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

        public void ToggleIsEdtting()
        {
            IsEditting = !IsEditting;

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
            Focus();
        }
        // reference to the modal component
        public Modal modalRef;
        public string SelectedShoppingList { get; set; }

        public async Task ShowModal()
        {
            AllShoppingLists = await ShoppingListApiConnector.GetItems();
            await modalRef.Show();
        }

        public Task HideAndSaveModal()
        {
            ShoppingListApiConnector.AddRecipeItem(Guid.Parse(SelectedShoppingList), Recipedetails.Id, IngredientMultiplier);
            return modalRef.Hide();
        }

        public Task HideModal()
        {
            return modalRef.Hide();
        }

        public ChangeStepModelViewModel ChangeStepModelViewModel { get; set; } = new ChangeStepModelViewModel();
        //public Task ShowChangeText()
        //{

        //}
    }

    public class ChangeStepModelViewModel
    {
        public Modal Modal;

        public string Text { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
        public void Initialize(string text, List<IngredientModel> ingredients)
        {
            Text = text;
            Ingredients = ingredients;
        }

        public async Task Show()
        {
            await Modal.Show();
        }

        public Task HideAndSave()
        {
            return Modal.Hide();
        }

        public Task HideModal()
        {
            return Modal.Hide();
        }
    }
}
