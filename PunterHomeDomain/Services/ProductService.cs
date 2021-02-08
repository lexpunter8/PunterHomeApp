using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PunterHomeDomain.Interfaces;
using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using PunterHomeDomain.Services;
using DataModels.Measurements;

namespace PunterHomeApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductDataAdapter myProductDataAdapter;
        private readonly IProductTagService productTagService;

        public ProductService(IProductDataAdapter productDataAdapter, IProductTagService productTagService)
        {
            myProductDataAdapter = productDataAdapter;
            this.productTagService = productTagService;
        }

        public void AddBarcodeToQuantity(int id, string barcode)
        {
            myProductDataAdapter.AddBarcodeToQuantity(id, barcode);
        }

        public void AddProduct(NewProductApiModel product)
        {
            if (string.IsNullOrEmpty(product.Name))
            {
                return;
            }

            myProductDataAdapter.AddProduct(new LightProduct
            {
                Name = product.Name
            }, out Guid newId);

            myProductDataAdapter.AddQuantityToProduct(new ProductQuantity
            {
                Barcode = product.Barcode,
                MeasurementType = product.UnitQuantityType,
                UnitQuantityTypeVolume = product.UnitQuantity
            }, newId);
        }

        public async Task AddQuantityToProduct(ProductQuantity value, Guid id)
        {
            await myProductDataAdapter.AddQuantityToProduct(value, id);
        }

        public async Task DeleteProductQuantityById(int id)
        {
            await myProductDataAdapter.DeleteProductQuantityById(id);
        }

        public IEnumerable<BaseMeasurement> GetMeasurementsForProduct(Guid id)
        {
            return myProductDataAdapter.GetProductById(id).ProductQuantities;
        }

        public ProductDetails GetProductById(Guid productId)
        {
            var product = myProductDataAdapter.GetProductById(productId);
            product.Tags = productTagService.GetTagsForProductAsync(productId).Result;
            return product;
        }

        public ProductDetails GetProductByName(string productName)
        {
            throw new NotImplementedException();
        }

        public Guid GetProductByQuantityBarcode(string barcode)
        {
            return myProductDataAdapter.GetProductIdForBarcode(barcode);
        }

        public async Task<IEnumerable<LightProduct>> GetProducts()
        {
            return await myProductDataAdapter.GetProducts();
        }

        public async Task<IEnumerable<LightProduct>> SearchProductsAsync(string searchText)
        {
            var result = await myProductDataAdapter.GetProducts();

            BaseFilter<LightProduct> filter = new NameFilter<LightProduct>(searchText);

            return filter.Filter(result);
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
