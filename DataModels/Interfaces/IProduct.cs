using System;
using static Enums;

namespace PunterHomeApp.Interfaces
{
    public interface IProduct
    {
        Guid Id { get; set; }
        string Name { get; set; }
        int Quantity { get; set; }
        int UnitQuantity { get; set; }
        EUnitQuantityType UnitQuantityType { get; set; }
    }
}
