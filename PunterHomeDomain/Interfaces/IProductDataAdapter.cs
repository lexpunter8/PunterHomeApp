using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface IProductDataAdapter
    {
        Task<IEnumerable<LightProduct>> GetProducts();
        Task<IEnumerable<ProductDetails>> GetAllProductDetails();
        void AddProduct(LightProduct product);
        Task DeleteProduct(Guid id);
        Task AddQuantityToProduct(ProductQuantity value, Guid id);
        Task<bool> Update(Guid id, string newName);
        Task UpdateProductQuantity(int id, ProductQuantity productQuantity);
        Task DeleteProductQuantityById(int id);

        Task IncreaseProductQuantity(int id, int value);
        Task DereaseProductQuantity(int id, int value);
        ProductDetails GetProductById(Guid productId);
    }
}
