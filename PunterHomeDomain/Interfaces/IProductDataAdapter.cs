using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface IProductDataAdapter
    {
        Task<IEnumerable<IProduct>> GetProducts();
        void AddProduct(IProduct product);
        Task DeleteProduct(Guid id);
        Task AddQuantityToProduct(ProductQuantity value, Guid id);
    }
}
