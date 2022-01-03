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

        protected override async Task OnParametersSetAsync()
        {
            Recipe = await RecipeService.GetRecipeById(RecipeId);
        }
    }
}
