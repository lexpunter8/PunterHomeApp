﻿using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PunterHomeDomain.Interfaces
{
    public interface IProductDataAdapter
    {
        Task<IEnumerable<LightProduct>> GetProducts();
        Task<IEnumerable<ProductDetails>> GetAllProductDetails();
        Task DeleteProduct(Guid id);
        Task AddQuantityToProduct(ProductQuantity value, Guid id);
        Task<bool> Update(Guid id, string newName);
        Task UpdateProductQuantity(int id, ProductQuantity productQuantity);
        Task DeleteProductQuantityById(int id);
        void AddBarcodeToQuantity(int id, string barcode);
        Task IncreaseProductQuantity(int id, int value);
        Task DereaseProductQuantity(int id, int value);
        ProductDetails GetProductById(Guid productId);
        Guid GetProductIdForBarcode(string barcode);
        void AddProduct(LightProduct product, out Guid newID);
    }
}
