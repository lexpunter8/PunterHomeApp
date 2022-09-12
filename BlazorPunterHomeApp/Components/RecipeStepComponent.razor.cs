using Microsoft.AspNetCore.Components;

namespace BlazorPunterHomeApp.Components
{
    public partial class RecipeStepComponent : ComponentBase
    {
        [Parameter]
        public int StepNumber { get; set; }

        [Parameter]
        public string Text { get; set; }

        [Parameter]
        public bool ShowButtons { get; set; }
    }
}
