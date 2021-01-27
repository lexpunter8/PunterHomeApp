using DataModels.Measurements;
using Newtonsoft.Json;
using PropertyChanged;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace BlazorPunterHomeApp.Data
{
    public class ProductModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; set; }

        public string Name { get; set; }
        public IEnumerable<BaseMeasurement> ProductQuantities { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }
        public List<ProductTagModel> Tags { get; set; } = new List<ProductTagModel>();
    }

    public class ProductDetailsViewModel : INotifyPropertyChanged
    {
        public ProductDetailsViewModel(ProductDetails productModel)
        {
            ProductModel = productModel;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        //public IEnumerable<ProductQuantity> ProductQuantities { get; set; }

        public ProductDetails ProductModel { get; }

        public string GetQuantityString()
        {
            if (ProductModel.MeasurementAmounts == null)
            {
                return "None";
            }

            string s = string.Empty;

            for (int i = 0; i < ProductModel.MeasurementAmounts.Values.Count; i++)
            {
                s += $"{ProductModel.MeasurementAmounts.Values[i]}";
                if (i == ProductModel.MeasurementAmounts.Values.Count - 1)
                {
                    continue;
                }
            }
            return s;
        }
    }

}
