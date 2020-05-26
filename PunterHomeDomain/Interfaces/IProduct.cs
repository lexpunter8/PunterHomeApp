using System;
using System.Collections.Generic;
using static Enums;

namespace PunterHomeDomain.Interfaces
{
    public interface IProduct
    {
        Guid Id { get; set; }
        string Name { get; set; }
        IEnumerable<IProductQuantity> ProductQuantities { get; set; }
    }

    public interface IProductQuantity
    {
        int Id { get; set; }
        int UnitQuantityTypeVolume { get; set; }
        EUnitQuantityType UnitQuantityType { get; set; }
        int Quantity { get; set; }
    }
}
