using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;

namespace RazorShared.Areas.Modals
{
    public partial class ScanBarCodeComponent : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public static string BarcodeValue = "no barcode found";
        private BarcodeFromJsBridge BarcodeFromJSs = new BarcodeFromJsBridge();
        private DotNetObjectReference<BarcodeFromJsBridge> BarcodeFromJSsReference;

        protected async override void OnInitialized()
        {
            BarcodeFromJSsReference = DotNetObjectReference.Create(BarcodeFromJSs);
            await JSRuntime.InvokeVoidAsync("setDotNetReference", BarcodeFromJSsReference);

            BarcodeFromJSs.ValueChanged += BarcodeFromJsChanged;
            await JSRuntime.InvokeVoidAsync("startScanner");
        }

        private void BarcodeFromJsChanged(object o, EventArgs a)
        {
            BarcodeValue = BarcodeFromJSs.Value;
            StateHasChanged();
        }

        private async void IncrementCount()
        {
        }
    }
}
