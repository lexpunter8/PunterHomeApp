using Microsoft.AspNetCore.Http;
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
    public class ProductService
    {
        public async Task Update(ProductModel product)
        {
            var json = JsonConvert.SerializeObject(product);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product?{product.Id}");
            var response = await httpClient.PutAsync(uri, data);
            string responseString = await response.Content.ReadAsStringAsync();
        }

        public async Task<ProductModel[]> GetForecastAsync()
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri("http://localhost:5005/api/product");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductModel[]>(responseString);

            return result;
        }

        public async Task<bool> Delete(ProductModel productToDelete)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/{productToDelete.Id}");
            var response = await httpClient.DeleteAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                // handle error
                return false;
            }

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

        public async Task AddQuantityToProduct(ProductQuantity quantity, ProductModel product)
        {
            try
            {
                var json = JsonConvert.SerializeObject(quantity);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/productquantity/{product.Id}"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

                product.ProductQuantities = product.ProductQuantities.Concat(new[] { quantity });
                product.NewProductQuantity = new ProductQuantity();
            }
            catch (Exception)
            {


            }
        }
    }

}
