using PropertyChanged;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using static Enums;

namespace BlazorPunterHomeApp.Data
{
    public class ProductModel : INotifyPropertyChanged
    {
        private Guid id;

        public event PropertyChangedEventHandler PropertyChanged;

        public Guid Id { 
            get => id; 
            set => id = value; 
        }

        public string Name { get; set; }
        public IEnumerable<ProductQuantity> ProductQuantities { get; set; }

        [JsonIgnore]
        public static List<EUnitQuantityType> SelectableUnitQuantityTypes => Enum.GetValues(typeof(EUnitQuantityType)).Cast<EUnitQuantityType>().ToList();

        [JsonIgnore]
        public int Quantity { get; set; }

        [JsonIgnore]
        public ProductQuantity NewProductQuantity { get; set; } = new ProductQuantity();
    }

}
