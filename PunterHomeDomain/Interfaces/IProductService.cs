using System;
using System.Collections.Generic;

namespace PunterHomeApp.Interfaces
{
    public interface IProductService
    {
        IEnumerable<IProduct> GetProducts();
        IProduct GetProductById(Guid productId);
        IProduct GetProductByName(string productName);
        void AddProduct(IProduct product);
    }
}
