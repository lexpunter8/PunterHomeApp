using System;
using System.Collections.Generic;
using PunterHomeApp.Interfaces;

namespace PunterHomeApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDataAdapter myProductDataAdapter;

        public ProductService(IProductDataAdapter productDataAdapter)
        {
            myProductDataAdapter = productDataAdapter;
        }

        public void AddProduct(IProduct product)
        {
            myProductDataAdapter.AddProduct(product);
        }

        public IProduct GetProductById(Guid productId)
        {
            throw new NotImplementedException();
        }

        public IProduct GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IProduct> GetProducts()
        {
            return myProductDataAdapter.GetProducts();
        }
    }
}
