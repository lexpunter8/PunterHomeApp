using PunterHomeDomain.Interfaces;

namespace PunterHomeDomain.Models
{
    public class ProductQuantity : IProductQuantity
    {
        public int UnitQuantityTypeVolume { get; set; }
        public Enums.EUnitQuantityType UnitQuantityType { get; set; }
        public int Quantity { get; set; }
        public int Id { get; set; }
    }
}
