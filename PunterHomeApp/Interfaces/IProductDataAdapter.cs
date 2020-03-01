using System.Collections.Generic;

namespace PunterHomeApp.Interfaces
{
    public interface IProductDataAdapter
    {
        IEnumerable<IProduct> GetProducts();
        void AddProduct(IProduct product);
    }
}
