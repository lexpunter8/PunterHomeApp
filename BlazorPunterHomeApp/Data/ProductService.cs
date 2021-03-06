﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PunterHomeApp.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Data
{
    public class ProductService : IProductService
    {
        public event EventHandler RefreshRequired;

        public async Task Update(ProductModel product)
        {
            foreach (var q in product.ProductQuantities)
            {
                var json = JsonConvert.SerializeObject(q);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();
                Uri uri = new Uri($"http://localhost:5005/api/productquantity/{q.ProductQuantityId}");
                var response = await httpClient.PutAsync(uri, data);
                string responseString = await response.Content.ReadAsStringAsync();
            }


            RefreshRequired?.Invoke(this, new EventArgs());
        }

        public async Task IncreaseProductQuantity(int id)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/productquantity/{id}/increase");
            var response = await httpClient.PutAsync(uri, null);
            string responseString = await response.Content.ReadAsStringAsync();
        }

        public async Task DecreaseProductQuantity(int id)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/productquantity/{id}/decrease");
            var response = await httpClient.PutAsync(uri, null);
            string responseString = await response.Content.ReadAsStringAsync();
        }

        public async Task<List<ProductModel>> GetProducts()
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri("http://localhost:5005/api/product");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductModel[]>(responseString);
             var t = result.OrderBy(q => q.Name).ToArray();

            return t.ToList();
        }

        internal Task AddQuantityToProduct(object newProductQuantity, ProductDetailsViewModel product)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDetailsViewModel> GetProductById(Guid id)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/{id}");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDetails>(responseString);
            return new ProductDetailsViewModel(result);
        }

        public async Task<bool> DeleteProduct(ProductDetailsViewModel productToDelete)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/{productToDelete.ProductModel.Id}");
            var response = await httpClient.DeleteAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // handle error
                return false;
            }

            RefreshRequired?.Invoke(this, new EventArgs());
            return true;
        }

        public async Task<bool> DeleteProductQuantity(int id)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/productquantity/{id}");
            var response = await httpClient.DeleteAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // handle error
                return false;
            }

            RefreshRequired?.Invoke(this, new EventArgs());
            return true;
        }

        public async Task AddProduct(NewProductApiModel product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(product);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri("http://localhost:5005/api/product"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();
            }
            catch (Exception)
            {


            }
        }


        public async Task AddQuantityToProduct(ProductQuantity quantity, ProductDetailsViewModel product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(quantity);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/productquantity/{product.ProductModel.Id}"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }

        public async Task<List<ProductModel>> SearchProducts(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                var products = await GetProducts();
                return products.ToList();
            }

            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/search/{searchText}");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductModel[]>(responseString).ToList();

            return result;
        }

        public async void AddBarcodeToQuantity(int id, string barcode)
        {
            try
            {
                var client = new HttpClient();

                var response = await client.PutAsync(new Uri($"http://localhost:5005/api/productquantity/{id}/barcode/{barcode}"), null);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }

        public async Task<Guid> GetProductIdByBarcode(string barcode)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/barcode/{barcode}");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<string>(responseString);
            if (Guid.TryParse(result, out Guid guidResult))
            {
                return guidResult;
            }
            return Guid.Empty;
        }
    }

}
