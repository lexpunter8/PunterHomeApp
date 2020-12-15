using Newtonsoft.Json;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Data
{
    public class RecipeService
    {
        public async Task InsertIngredients(List<IngredientModel> ingredients)
        {
            foreach(var i in ingredients)
            {
                await InsertIngredient(i);
            }
        }

        public async Task InsertIngredient(IngredientModel ingredient)
        {
            try
            {
                var json = JsonConvert.SerializeObject(ingredient);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri("http://localhost:5005/api/ingredient"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();
            }
            catch (Exception)
            {


            }
        }
    }
}
