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
    }

}
