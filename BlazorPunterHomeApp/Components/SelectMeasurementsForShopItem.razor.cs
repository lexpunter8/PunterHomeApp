using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Enums;

namespace BlazorPunterHomeApp.Components
{
    public class SelectableMeasurement
    {
        public SelectableMeasurement(BaseMeasurement measurement)
        {
            Measurement = measurement;
        }

        public int SelectedCount { get; set; }
        public BaseMeasurement Measurement { get; }
    }
    public partial class SelectMeasurementsForShopItem : ComponentBase
    {
        [Parameter] public List<SelectableMeasurement> MeasurementsOptions { get; set; }
        [Parameter] public double RequiredAmount { get; set; }
        [Parameter] public EUnitMeasurementType MeasurementType { get; set; }

        [Parameter] public EventCallback OnConfirm { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }
        public bool ShowModal { get; set; }
        public double CurrentSelectedAmount { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            StateHasChanged();
        }

        public void Show()
        {
            ShowModal = true;
            StateHasChanged();
        }

        public void Hide()
        {
            ShowModal = false;
            StateHasChanged();
        }

        public async void MeasurementClicked(SelectableMeasurement m)
        {
            m.SelectedCount++;

            double currentAmount = 0f;
            foreach (var item in MeasurementsOptions.Where(m => m.SelectedCount > 0))
            {
                currentAmount += item.SelectedCount * BaseMeasurement.GetMeasurement(item.Measurement).ConvertTo(MeasurementType);
            }
            CurrentSelectedAmount = currentAmount;

            if (CurrentSelectedAmount >= RequiredAmount)
            {
                await OnConfirm.InvokeAsync(this);
            }
        }
    }

}
