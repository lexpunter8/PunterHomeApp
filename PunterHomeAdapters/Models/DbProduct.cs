using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PunterHomeApp.Interfaces;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class DbProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Collection<DbIngredient> Ingredients { get; set; }
        public IEnumerable<DbProductQuantity> ProductQuantities { get; set; }
    }
}
