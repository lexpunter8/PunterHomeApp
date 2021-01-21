using Blazored.Modal.Services;
using Blazorise;
using BlazorPunterHomeApp.Components;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp
{
    public interface IBarcodeScannerService
    {
        Task<BarcodeResult> ScanBarcode(IModalService modalService);
    }
    public class BarcodeScannerService : IBarcodeScannerService
    {
        private bool myIsScanning;

        public async Task<BarcodeResult> ScanBarcode(IModalService modalService)
        {

            myIsScanning = true;
            var moviesModal = modalService.Show<BarcodeScannerComponent>("Scanner");
            var result = await moviesModal.Result;
            if (result.Data is BarcodeResult barcodeResult)
            {
                myIsScanning = false;
                return barcodeResult;
            }
            myIsScanning = false;
            return null;
        }
    }
}
