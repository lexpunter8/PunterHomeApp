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
                Uri uri = new Uri($"http://localhost:5005/api/productquantity/{q.Id}");
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

        public async Task<ProductModel[]> GetProducts()
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri("http://localhost:5005/api/product");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductModel[]>(responseString);

            return result;
        }

        public async Task<ProductDetailsModel> GetProductById(Guid id)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/{id}");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductDetailsModel>(responseString);

            return result;
        }

        public async Task<bool> DeleteProduct(ProductDetailsModel productToDelete)
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


        public async Task AddQuantityToProduct(ProductQuantity quantity, ProductDetailsModel product)
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
            }
            catch (Exception)
            {


            }
        }

        public async Task<List<ProductModel>> SearchProducts(string searchText)
        {
            var httpClient = new HttpClient();
            Uri uri = new Uri($"http://localhost:5005/api/product/search/{searchText}");
            var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ProductModel[]>(responseString).ToList();

            return result;
        }
    }

}
