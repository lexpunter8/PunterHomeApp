using Microsoft.AspNetCore.Components;

namespace BlazorPunterHomeApp.Components.Recipe
{
    public partial class EditRecipeStepComponent : ComponentBase
    {
        [Parameter]
        public int StepNumber { get; set; }

        [Parameter]
        public string Text { get; set; }
    }
}
