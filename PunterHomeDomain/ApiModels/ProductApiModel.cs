using System;
using static Enums;

namespace PunterHomeApp.ApiModels
{
    public class NewProductApiModel
    {
        public string Name { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
    }
}
