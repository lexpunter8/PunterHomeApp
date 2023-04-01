using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Components
{
    public partial class CheckBox : ComponentBase
    {
        public bool IsChecked { get; set; }

        [Parameter]
        public EventCallback<bool> CheckedChanged { get; set; }


        private void Cliked()
        {
            IsChecked = !IsChecked;
            CheckedChanged.InvokeAsync(IsChecked);
        }
    }
}
