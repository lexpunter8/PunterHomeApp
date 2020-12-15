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
        public IEnumerable<ProductQuantity> ProductQuantities { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }
    }

    public class ProductDetailsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { get; set; }

        public string Name { get; set; }
        public IEnumerable<ProductQuantity> ProductQuantities { get; set; }

        public int Quantity { get; set; }
        public MeasurementClassObject MeasurementAmounts { get; set; }

        public string GetQuantityString()
        {
            if (MeasurementAmounts == null)
            {
                return "None";
            }

            string s = string.Empty;

            for (int i = 0; i < MeasurementAmounts.Values.Count; i++)
            {
                s += $"{MeasurementAmounts.Values[i]}";
                if (i == MeasurementAmounts.Values.Count - 1)
                {
                    continue;
                }
            }
            return s;
        }
    }

}
