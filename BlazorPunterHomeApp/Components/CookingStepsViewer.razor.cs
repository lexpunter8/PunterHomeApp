using Microsoft.AspNetCore.Components;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlazorPunterHomeApp.Components
{
    public partial class CookingStepsViewer : ComponentBase
    {
        [Parameter]
        public List<RecipeStep> Steps { get; set; }

        [Parameter]
        public int CurrentStepIndex { get; set; }

        public RecipeStep CurrentStep => Steps.FirstOrDefault(f => f.Order == CurrentStepIndex);
        public RecipeStep PreviousStep => Steps.FirstOrDefault(f => f.Order == CurrentStepIndex - 1);

        public string Text => CurrentText();

        public string CurrentText()
        {
            string text = CurrentStep.Text;
            for (int x = 0; x < CurrentStep.Ingredients.Count; x++)
            {
                RecipeStepIngredient i = CurrentStep.Ingredients[x];
                text = text.Replace($"{{{x}}}", $"{i.ProductName} ({i.UnitQuantity} {i.UnitQuantityType})");
            }
            return text;
        }


        public void ChangeStep(int delta)
        {
            int newStep = CurrentStepIndex + delta;

            CurrentStepIndex = Math.Max(1, Math.Min(newStep, Steps.Count));
        }
    }
}
