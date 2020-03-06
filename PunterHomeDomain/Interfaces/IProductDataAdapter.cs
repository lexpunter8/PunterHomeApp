using System.Collections.Generic;
using PunterHomeAdapters.Models;

namespace PunterHomeApp.Interfaces
{
    public interface IProductDataAdapter
    {
        IEnumerable<Product> GetProducts();
        void AddProduct(Product product);
    }
}
