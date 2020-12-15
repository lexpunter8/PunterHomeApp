using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PunterHomeDomain.Interfaces;
using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;

namespace PunterHomeApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDataAdapter myProductDataAdapter;

        public ProductService(IProductDataAdapter productDataAdapter)
        {
            myProductDataAdapter = productDataAdapter;
        }

        public void AddProduct(NewProductApiModel product)
        {
            myProductDataAdapter.AddProduct(new LightProduct
            {
                Name = product.Name,
                //ProductQuantities = new List<BaseMeasurement>
                //{
                //    new BaseMeasurement(product.UnitQuantityType)
                //    {
                //        UnitQuantityTypeVolume = product.UnitQuantity
                //    }
                //}
            });
        }

        public async Task AddQuantityToProduct(ProductQuantity value, Guid id)
        {
            await myProductDataAdapter.AddQuantityToProduct(value, id);
        }

        public async Task DeleteProductQuantityById(int id)
        {
            await myProductDataAdapter.DeleteProductQuantityById(id);
        }

        public ProductDetails GetProductById(Guid productId)
        {
            return myProductDataAdapter.GetProductById(productId);
        }

        public ProductDetails GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LightProduct>> GetProducts()
        {
            return await myProductDataAdapter.GetProducts();
        }

        public async Task<IEnumerable<LightProduct>> SearchProductsAsync(string searchText)
        {
            var result = await myProductDataAdapter.GetProducts();
            return result.Where(p => p.Name.ToLower().Contains(searchText.ToLower()));
        }

        public async Task<bool> TryDeleteProductById(Guid id)
        {
            try
            {
                await myProductDataAdapter.DeleteProduct(id);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> Update(Guid id, string newName)
        {
            try
            {
                await myProductDataAdapter.Update(id, newName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task UpdateProductQuantity(int id, ProductQuantity productQuantity)
        {
            await myProductDataAdapter.UpdateProductQuantity(id, productQuantity);
        }

        public async Task UpdateProductQuantity(int id, int value, bool increase)
        {
            if (increase)
            {
                await myProductDataAdapter.IncreaseProductQuantity(id, value);
                return;
            }
            await myProductDataAdapter.DereaseProductQuantity(id, value);
        }
    }
}
