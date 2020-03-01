using System;
using System.ComponentModel;
using PunterHomeApp.Interfaces;
using static Enums;

namespace PunterHomeApp
{
    public class Product : IProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitQuantityType UnitQuantityType { get; set; }

    }
}

public class Enums
{
    public enum EUnitQuantityType
    {
        [Description("Kilogram")]
        Kg,

        [Description("Gram")]
        Gr
    }
}
