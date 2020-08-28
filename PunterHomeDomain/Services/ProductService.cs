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
            myProductDataAdapter.AddProduct(new Product
            {
                Name = product.Name,
                ProductQuantities = new List<IProductQuantity>
                {
                    new ProductQuantity
                    {
                        UnitQuantityTypeVolume = product.UnitQuantity,
                        UnitQuantityType = product.UnitQuantityType
                    }
                }
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

        public IProduct GetProductById(Guid productId)
        {
            throw new NotImplementedException();
        }

        public IProduct GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IProduct>> GetProducts()
        {
            return await myProductDataAdapter.GetProducts();
        }

        public async Task<IEnumerable<IProduct>> SearchProductsAsync(string searchText)
        {
            var result = await myProductDataAdapter.GetProducts();
            return result.Where(p => p.Name.Contains(searchText));
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
    }
}
