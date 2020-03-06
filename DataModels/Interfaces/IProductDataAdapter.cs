using System.Collections.Generic;
using System.Threading.Tasks;
using PunterHomeAdapters.Models;

namespace PunterHomeApp.Interfaces
{
    public interface IProductDataAdapter
    {
        Task<IEnumerable<IProduct>> GetProducts();
        void AddProduct(DbProduct product);
    }
}
