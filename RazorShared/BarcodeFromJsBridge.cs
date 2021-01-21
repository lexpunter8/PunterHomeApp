using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorShared
{
    public class BarcodeFromJsBridge
    {
        public string Value { get; set; }
        public event EventHandler ValueChanged;

        [JSInvokable]
        public void SetBarcode(string value)
        {
            Value = value;
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
