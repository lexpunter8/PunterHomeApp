using System;
using System.Collections.Generic;
using PunterHomeApp.Services;
using PunterHomeDomain.Interfaces;

namespace PunterHomeDomain.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<BaseMeasurement> ProductQuantities { get; set; }

    }

}
