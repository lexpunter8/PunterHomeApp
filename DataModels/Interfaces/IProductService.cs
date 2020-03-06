using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeApp.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<IProduct>> GetProducts();
        IProduct GetProductById(Guid productId);
        IProduct GetProductByName(string productName);
        void AddProduct(IProduct product);
    }
}
