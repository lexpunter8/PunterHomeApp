using Microsoft.AspNetCore.Components;
using PunterHomeDomain.ApiModels;
using System;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Pages
{
    public partial class CookingPage : ComponentBase
    {

        [Parameter]
        public Guid RecipeId { get; set; }
        [Parameter]
        public int Step { get; set; }

        [Inject]
        public BlazorRecipeService RecipeService { get; set; }
        public RecipeDetailsApiModel Recipe { get; private set; }

        public int IngredientMultiplier { get; set; } = 2;

        public void ChangePersons(int x)
        {
            IngredientMultiplier = x;

            if (IngredientMultiplier < 1)
            {
                IngredientMultiplier = 1;
            }
        }
        protected override async Task OnParametersSetAsync()
        {
            Recipe = await RecipeService.GetRecipeById(RecipeId);
        }
    }
}
