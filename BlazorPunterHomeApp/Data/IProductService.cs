using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Data
{
    public interface IProductService
    {
        event EventHandler RefreshRequired;

        Task AddProduct(NewProductApiModel product);
        Task AddQuantityToProduct(ProductQuantity quantity, ProductDetailsViewModel product);
        Task<bool> DeleteProduct(ProductDetailsViewModel productToDelete);
        Task<bool> DeleteProductQuantity(int id);
        Task<ProductModel[]> GetProducts();
        Task Update(ProductModel product);
        Task<List<ProductModel>> SearchProducts(string searchText);
    }
}