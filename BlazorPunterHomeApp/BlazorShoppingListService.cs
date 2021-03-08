using BlazorPunterHomeApp.Components;
using BlazorPunterHomeApp.Data;
using DataModels.Measurements;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp
{
    public class BlazorShoppingListService
    {
        public Guid ShoppingListId;

        public async Task AddToShoppingList(Guid shoppingListId, AddProductToShoppingListRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/shoppinglist/{ShoppingListId}"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }

        public async Task<List<ShoppingListShopItem>> GetShoppingListToShopItems()
        {

            try
            {
                    var httpClient = new HttpClient();
                if (ShoppingListId == null)
                {
                    Uri uri = new Uri($"http://localhost:5005/api/shoppinglist");
                    var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                    string responseString = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ShoppingListApiModel[]>(responseString);

                    if (result == null || result.Length == 0)
                    {
                        return new List<ShoppingListShopItem>();
                    } 
                    ShoppingListId = result.First().Id;
                }
                Uri uriGet = new Uri($"http://localhost:5005/api/shoppinglist/{ShoppingListId}/shop");
                var response1 = await httpClient.GetAsync(uriGet, HttpCompletionOption.ResponseHeadersRead);
                string responseString1 = await response1.Content.ReadAsStringAsync();
                var result1 = JsonConvert.DeserializeObject<List<ShoppingListShopItem>>(responseString1);



                return result1;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async void AddMeasurementsToShoppingItem(List<SelectableMeasurement> measurementsOptions, Guid itemId)
        {
            List<AddMeasurementsToShoppingListItem> measurements = new List<AddMeasurementsToShoppingListItem>();
            foreach (var item in measurementsOptions.Where(i => i.SelectedCount > 0))
            {
                measurements.Add(new AddMeasurementsToShoppingListItem
                {
                    Count = item.SelectedCount,
                    MeasurementId = item.Measurement.ProductQuantityId,
                    ShoppingListItemId = itemId,
                    
                });

            }

            try
            {
                var json = JsonConvert.SerializeObject(measurements);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/shoppinglist/addmeasurement"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }

        public async Task<List<BaseMeasurement>> GetMeasurementsForProduct(Guid productID)
        {

            try
            {
                var httpClient = new HttpClient();
                Uri uri = new Uri($"http://localhost:5005/api/product/measeurements/{productID}");
                var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<BaseMeasurement>>(responseString);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateCountForItem(Guid shoppingListItemId, bool add)
        {
            try
            {
                var client = new HttpClient();
                string c = add ? "plus" : "min";
                var response = await client.PutAsync(new Uri($"http://localhost:5005/api/shoppinglist/{c}/{shoppingListItemId}"), null);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }

        public async Task AddQuantityForCheckedItems(Guid shoppingListItemId)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.PutAsync(new Uri($"http://localhost:5005/api/shoppinglist/updateproducts/{shoppingListItemId}"), null);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }
        public async Task UpdateCheckedForItem(Guid shoppingListItemId, bool isChecked)
        {
            try
            {
                var client = new HttpClient();
                string c = isChecked ? "checked" : "unchecked";
                var response = await client.PutAsync(new Uri($"http://localhost:5005/api/shoppinglist/{c}/{shoppingListItemId}"), null);
                string result = response.Content.ReadAsStringAsync().Result;
                client.Dispose();
            }
            catch (Exception)
            {


            }
        }
        public async Task DeleteItem(Guid shoppingListItemId)
        {
            try
            {
                var client = new HttpClient();
                var response = await client.DeleteAsync(new Uri($"http://localhost:5005/api/shoppinglist/{shoppingListItemId}"));
                string result = response.Content.ReadAsStringAsync().Result;
                client.Dispose();
            }
            catch (Exception)
            {


            }
        }

        public async Task<List<ShoppingListItemModel>> GetShoppingListItems()
        {
            try
            {
                var httpClient = new HttpClient();
                Uri uri = new Uri($"http://localhost:5005/api/shoppinglist");
                var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ShoppingListApiModel[]>(responseString);

                if (result == null || result.Length == 0)
                {
                    return new List<ShoppingListItemModel>();
                }
                Uri uriGet = new Uri($"http://localhost:5005/api/shoppinglist/{result.First().Id}");
                var response1 = await httpClient.GetAsync(uriGet, HttpCompletionOption.ResponseHeadersRead);
                string responseString1 = await response1.Content.ReadAsStringAsync();
                var result1 = JsonConvert.DeserializeObject<List<ShoppingListItemModel>>(responseString1);

                ShoppingListId = result.First().Id;


                return result1;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
