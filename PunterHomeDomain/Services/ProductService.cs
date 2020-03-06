using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeAdapters.Models;
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
            myProductDataAdapter.AddProduct(product as DbProduct);
        }

        public IProduct GetProductById(Guid productId)
        {
            throw new NotImplementedException();
        }

        public IProduct GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IProduct>> GetProducts()
        {
            return await myProductDataAdapter.GetProducts();
        }
    }
}
