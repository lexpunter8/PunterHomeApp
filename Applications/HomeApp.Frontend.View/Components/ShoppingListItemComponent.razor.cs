using HomeApp.Frontend.View.Models;
using Microsoft.AspNetCore.Components;

namespace HomeApp.Frontend.View.Components
{
    public partial class ShoppingListItemComponent : ComponentBase
    {
        public bool IsEditting { get; set; }
        public DateTime myLastEditDate = DateTime.MinValue;
        public System.Timers.Timer myTimer;

        [Parameter]
        public ShoppingListItemViewModel Item { get; set; }

        protected override void OnParametersSet()
        {
            Item.PropertyChanged += Item_PropertyChanged;
            base.OnParametersSet();
        }

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

        private void Edit()
        {
            if (Item.IsChecked)
            {
                return;
            }
            IsEditting = true;

            myLastEditDate = DateTime.Now;
            myTimer = new System.Timers.Timer();
            myTimer.Interval = 2000;
            myTimer.Elapsed += MyTimer_Elapsed;

            myTimer.Start();
        }

        private void MyTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            StopEdit();
        }

        private void StopEdit()
        {
            IsEditting = false;
            StateHasChanged();
            myTimer.Stop();
        }

        private void OnCheckedChanged(bool isChecked)
        {
            Item.ChangeChecked(isChecked);
            StopEdit();
        }

        private async void IncreaseCount()
        {
            await Item.Increase();
            myLastEditDate = DateTime.Now;
            myTimer.Stop();
            myTimer.Start();
        }

        private async Task DecreaseCount()
        {
            await Item.Decrease();
            myLastEditDate = DateTime.Now;
            myTimer.Stop();
            myTimer.Start();
        }
    }
}
