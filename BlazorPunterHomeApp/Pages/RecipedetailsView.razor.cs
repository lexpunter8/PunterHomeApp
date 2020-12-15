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
    public partial class RecipedetailsViewBase : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public BlazorRecipeService RecipeService { get; set; }

        [Inject]
        public IModalService Modal { get; set; }

        public int IngredientMultiplier { get; set; } = 1;
        public RecipeDetailsApiModel Recipedetails { get; set; }

        protected async override void OnParametersSet()
        {
            Recipedetails = await RecipeService.GetRecipeById(Id);
            StateHasChanged();

        }
        public bool Open { get; set; }

        public void OnClose()
        {

        }

        public string newRecipeName = string.Empty;
        public string newStepText = string.Empty;

        public RecipeModel[] recipes = new RecipeModel[0];
        public List<ProductModel> products = new List<ProductModel>();

        public async Task RemoveIngredient(Guid id)
        {
            await RecipeService.RemoveIngredient(Recipedetails.Id, id);
            await Refresh();
        }

        public void ChangePersons(int x)
        {
            IngredientMultiplier = x;
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
            Recipedetails = await RecipeService.GetRecipeById(Recipedetails.Id);
            StateHasChanged();
        }
    }
}
