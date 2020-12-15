using System;
using System.Collections.Generic;
using DataModels.Measurements;
using PunterHomeApp.Services;
using PunterHomeDomain.Interfaces;

namespace PunterHomeDomain.Models
{
    public class LightProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductDetails
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<BaseMeasurement> ProductQuantities { get; set; }
        public MeasurementClassObject MeasurementAmounts { get; set; }
    }

}
