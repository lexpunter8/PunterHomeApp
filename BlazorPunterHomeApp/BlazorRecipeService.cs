﻿using BlazorPunterHomeApp.Data;
using Newtonsoft.Json;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp
{
    public interface IBlazorrecipeService
    {
        Task<RecipeDetailsApiModel> GetRecipeById(Guid id);
    }
    public class BlazorRecipeService : IBlazorrecipeService
    {
        public async Task<RecipeDetailsApiModel> GetRecipeById(Guid id)
        {
            try
            {
                var httpClient = new HttpClient();
                Uri uri = new Uri($"http://localhost:5005/api/recipe/{id}");
                var response = await httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                string responseString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RecipeDetailsApiModel>(responseString);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task AddStep(RecipeStep newStep, Guid recipeId)
        {
            try
            {
                var json = JsonConvert.SerializeObject(newStep);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/RecipeStep/{recipeId}"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }

        public async Task RemoveIngredient(Guid recipeId, Guid productId)
        {
            try
            {
                var client = new HttpClient();

                var response = await client.DeleteAsync(new Uri($"http://localhost:5005/api/ingredient/{recipeId}/{productId}"));

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }
    }

}
