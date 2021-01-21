using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<LightProduct>> GetProducts();
        ProductDetails GetProductById(Guid productId);
        ProductDetails GetProductByName(string productName);
        void AddProduct(NewProductApiModel product);
        Task<IEnumerable<LightProduct>> SearchProductsAsync(string searchText);
        Task<bool> TryDeleteProductById(Guid id);
        Task AddQuantityToProduct(ProductQuantity value, Guid id);
        Task<bool> Update(Guid id, string newName);
        Task UpdateProductQuantity(int id, ProductQuantity productQuantity);
        Task DeleteProductQuantityById(int id);
        Task UpdateProductQuantity(int id, int value, bool increase);
        void AddBarcodeToQuantity(int id, string barcode);
        Guid GetProductByQuantityBarcode(string barcode);
    }
}
