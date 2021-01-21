using PunterHomeDomain.Interfaces;

namespace PunterHomeDomain.Models
{
    public class ProductQuantity
    {
        public int ProductQuantityId { get; set; }
        public int UnitQuantityTypeVolume { get; set; }
        public Enums.EUnitMeasurementType MeasurementType { get; set; }
        public int Quantity { get; set; }
        public int Id => ProductQuantityId;
        public string Barcode { get; set; }
    }
}
