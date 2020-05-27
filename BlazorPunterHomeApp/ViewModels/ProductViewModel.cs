using BlazorPunterHomeApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(ProductModel product)
        {
            CurrentSelectedProduct = product;
            CurrentSelectedProduct.PropertyChanged += CurrentSelectedProduct_PropertyChanged;
        }

        private void CurrentSelectedProduct_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // handle
        }

        public ProductModel CurrentSelectedProduct {get;set;}
    }
}
