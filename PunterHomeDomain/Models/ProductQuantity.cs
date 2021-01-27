using PunterHomeDomain.Interfaces;
using static Enums;

namespace PunterHomeDomain.Models
{
    public class ProductQuantity
    {
        public int ProductQuantityId { get; set; }
        public int UnitQuantityTypeVolume { get; set; }
        public EUnitMeasurementType MeasurementType { get; set; }
        public int Quantity { get; set; }
        public int Id => ProductQuantityId;
        public string Barcode { get; set; }
    }
}
