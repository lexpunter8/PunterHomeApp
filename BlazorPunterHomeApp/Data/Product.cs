using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using static Enums;

namespace BlazorPunterHomeApp.Data
{
    public class ProductModel 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ProductQuantity> ProductQuantities { get; set; }

        [JsonIgnore]
        public static List<string> SelectableUnitQuantityTypes => Enum.GetNames(typeof(EUnitQuantityType)).Cast<string>().ToList();

        [JsonIgnore]
        public int Quantity { get; set; }

        public ProductQuantity NewProductQuantity { get; set; } = new ProductQuantity();

        public void AddQuantity()
        {
        }
    }

}
