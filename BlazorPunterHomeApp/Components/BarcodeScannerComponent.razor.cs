using Blazored.Modal;
using Blazored.Modal.Services;
using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RazorShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Components
{
    public partial class BarcodeScannerComponent : ComponentBase
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        public int test { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        public string BarcodeValue = "no barcode found";
        private BarcodeFromJsBridge BarcodeFromJSs = new BarcodeFromJsBridge();
        private DotNetObjectReference<BarcodeFromJsBridge> BarcodeFromJSsReference;

        protected async override void OnInitialized()
        {
            BarcodeFromJSsReference = DotNetObjectReference.Create(BarcodeFromJSs);
            await JSRuntime.InvokeVoidAsync("setDotNetReference", BarcodeFromJSsReference);

            BarcodeFromJSs.ValueChanged += BarcodeFromJsChanged;
            BarcodeFromJSs.CamIdChanged += CamIdFromJsChanged;
            await JSRuntime.InvokeVoidAsync("startScanner");
        }

        private void BarcodeFromJsChanged(object o, EventArgs a)
        {
            BarcodeValue = BarcodeFromJSs.Value;
            StateHasChanged();
            Finish();
        }


        private void CamIdFromJsChanged(object o, EventArgs a)
        {
        }

        private async void Finish()
        {
            await JSRuntime.InvokeVoidAsync("stopScanner");
            await BlazoredModal.CloseAsync(ModalResult.Ok(new BarcodeResult
            {
                Barcode = BarcodeValue
            }));
        }
    }

    public class BarcodeResult
    {
        public string Barcode { get; set; }
    }
}
