using System;
using PunterHomeApp.Interfaces;

namespace PunterHomeDomain.Models
{
    public class Product : IProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "Unkown";
        public int Quantity { get; set; }
        public int UnitQuantity { get; set; }
        public Enums.EUnitQuantityType UnitQuantityType { get; set; }
    }
}
