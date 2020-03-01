using System;
namespace PunterHomeApp.ApiModels
{
    public class ProductApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int UnitQuantity { get; set; }
        public string UnitQuantityType { get; set; }
    }
}
