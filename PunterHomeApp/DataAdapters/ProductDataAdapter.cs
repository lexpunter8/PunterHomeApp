using System;
using System.Collections.Generic;
using PunterHomeApp.Interfaces;

namespace PunterHomeApp.DataAdapters
{
    public class ProductDataAdapter : IProductDataAdapter
    {
        private List<IProduct> myProducts = new List<IProduct>
        {
            new Product
            {
                Name = "TestProduct",
                Quantity = 1,
                UnitQuantity = 100,
                UnitQuantityType = Enums.EUnitQuantityType.Gr,
                Id = Guid.NewGuid()
            }
        };

        public IEnumerable<IProduct> GetProducts()
        {
            return myProducts;
        }
    }
}
