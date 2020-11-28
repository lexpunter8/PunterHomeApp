using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProducts();
        Product GetProductById(Guid productId);
        Product GetProductByName(string productName);
        void AddProduct(NewProductApiModel product);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchText);
        Task<bool> TryDeleteProductById(Guid id);
        Task AddQuantityToProduct(ProductQuantity value, Guid id);
        Task<bool> Update(Guid id, string newName);
        Task UpdateProductQuantity(int id, ProductQuantity productQuantity);
        Task DeleteProductQuantityById(int id);
        Task UpdateProductQuantity(int id, int value, bool increase);
    }
}
