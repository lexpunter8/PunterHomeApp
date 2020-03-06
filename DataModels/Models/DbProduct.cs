using System;
using System.Collections.ObjectModel;
using PunterHomeApp.Interfaces;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class DbProduct : IProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitQuantityType UnitQuantityType { get; set; }

        public Collection<DbIngredient> Ingredients { get; set; }
    }
}
