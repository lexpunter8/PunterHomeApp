using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using PunterHomeDomain.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp
{
    public class BlazorShoppingListService
    {
        public Guid ShoppingListId;

        public async Task AddToShoppingList(Guid shoppingListId, int prodQuanId)
        {
            try
            {
                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/shoppinglist/{ShoppingListId}/{prodQuanId}"), null);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


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

        public async Task<List<ShoppingListItem>> GetShoppingListItems()
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
                    return new List<ShoppingListItem>();
                }
                Uri uriGet = new Uri($"http://localhost:5005/api/shoppinglist/{result.First().Id}");
                var response1 = await httpClient.GetAsync(uriGet, HttpCompletionOption.ResponseHeadersRead);
                string responseString1 = await response1.Content.ReadAsStringAsync();
                var result1 = JsonConvert.DeserializeObject<List<ShoppingListItemApiModel>>(responseString1);

                ShoppingListId = result.First().Id;


                return result1.Select(r => new ShoppingListItem
                {
                    Id = r.Id,
                    MeasurementType = r.MeasurementType,
                    MeasurementVolume= r.Volume,
                    ProductName = r.ProductName,
                    Quantity = r.Count,
                    IsChecked = r.IsChecked
                }).ToList();
            }
            catch (Exception)
            {
                return null;
            }
            return new List<ShoppingListItem>
            {
                new ShoppingListItem
                {
                    Id = Guid.NewGuid(),
                    ProductName = "TProduct 1",
                    MeasurementType = Enums.EUnitMeasurementType.Ml,
                    MeasurementVolume = 100,
                    Quantity = 2
                },
                new ShoppingListItem
                {
                    Id = Guid.NewGuid(),
                    ProductName = "TProduct 2",
                    MeasurementType = Enums.EUnitMeasurementType.Gr,
                    MeasurementVolume = 100,
                    Quantity = 1
                },
                new ShoppingListItem
                {
                    Id = Guid.NewGuid(),
                    ProductName = "TProduct 3",
                    MeasurementType = Enums.EUnitMeasurementType.Ml,
                    MeasurementVolume = 500,
                    Quantity = 2
                }
            };
        }
    }
}
