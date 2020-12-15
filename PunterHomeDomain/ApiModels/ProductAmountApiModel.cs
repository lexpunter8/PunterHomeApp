using System;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class ProductAmountApiModel
    {
        public Guid ProductId { get; set; }
        public EUnitMeasurementType Type { get; set; }
        public int VolumeAmount { get; set; }
    }
}
