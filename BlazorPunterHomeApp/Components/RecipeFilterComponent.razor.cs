using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Components
{
    public partial class RecipeFilterComponent : ComponentBase
    {
        protected override Task OnParametersSetAsync()
        {
            RecipeState.Changed += RecipeState_Changed;
            return base.OnParametersSetAsync();
        }

        private void RecipeState_Changed(object sender, EventArgs e)
        {
            StateHasChanged();
        }

        public bool collapseNavMenu = false;

        public string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        public List<ERecipeType> RecipeTypes => Enum.GetValues(typeof(ERecipeType)).Cast<ERecipeType>().Where(r => r != ERecipeType.None).ToList();
        public void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        public List<REcipeTypeInfo> GetRecipeTypesInfo()
        {
            return RecipeTypes.Select(t => new REcipeTypeInfo
            {
                Type = t,
                Count = GetNrOfRecipesForType(t)
            }).OrderByDescending(r => r.Count).ToList();
        }

        public bool TypeFilterSelected;

        [Parameter]
        public SearchRecipeParameters FilterParameters { get; set; }
        public void TypeFilterChanged(ERecipeType type)
        {
            FilterParameters.Type = type;
            TypeFilterSelected = type != ERecipeType.None;
            FiltersChanged.InvokeAsync(new object());
        }

        [Parameter]
        public EventCallback FiltersChanged { get; set; }

        public int GetNrOfRecipesForType(ERecipeType type)
        {
            return RecipeState?.Recipes.Count(r => r.Type == type) ?? 0;
        }

        [Parameter]
        public RecipeFilterStateObject RecipeState { get; set; }
    }

    public class REcipeTypeInfo
    {
        public ERecipeType Type { get; set; }
        public int Count { get; set; }
    }

    public class RecipeFilterStateObject
    {
        public void RaiseChanged()
        {
            Changed?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Changed;
        public List<RecipeModel> Recipes { get; set; } = new List<RecipeModel>();
    }
}
