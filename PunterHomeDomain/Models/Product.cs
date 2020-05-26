using System;
using System.Collections.Generic;
using PunterHomeDomain.Interfaces;

namespace PunterHomeDomain.Models
{
    public class Product : IProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<IProductQuantity> ProductQuantities { get; set; }
    }
}
