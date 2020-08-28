using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Data
{
    public interface IProductService
    {
        event EventHandler RefreshRequired;

        Task AddProduct(NewProductApiModel product);
        Task AddQuantityToProduct(ProductQuantity quantity, ProductModel product);
        Task<bool> DeleteProduct(ProductModel productToDelete);
        Task<bool> DeleteProductQuantity(int id);
        Task<ProductModel[]> GetProducts();
        Task Update(ProductModel product);
    }
}