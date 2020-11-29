using BlazorPunterHomeApp.crud;
using BlazorPunterHomeApp.Data;
using BlazorPunterHomeApp.Pages;
using Newtonsoft.Json;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        private RecipeService myRecipeService;
        private BaseHttpCrudHandler<RecipeModel> myRecipeApiHandler = new BaseHttpCrudHandler<RecipeModel>("http://localhost:5005/api/recipe");
        private BaseHttpCrudHandler<IngredientModel> myIngredientApiHandler = new BaseHttpCrudHandler<IngredientModel>("http://localhost:5005/api/ingredient");
        private BaseHttpCrudHandler<RecipeStep> myStepsApiHandler = new BaseHttpCrudHandler<RecipeStep>("http://localhost:5005/api/recipestep");
        private readonly IProductService productService;

        public RecipeViewModel(IProductService productService)
        {
            this.productService = productService;
        }

        public RecipeDetailsApiModel CurrentSelectedRecipe { get; set; } = new RecipeDetailsApiModel();

        public async Task<bool> CreateNewRecipe(string newRecipeName)
        {
            return await myRecipeApiHandler.Create(newRecipeName);
        }

        public async Task<RecipeModel[]> GetRecipes()
        {
            var r = await myRecipeApiHandler.GetAll();
            return r.ToArray();
        }


        public async Task<RecipeDetailsApiModel> GetRecipeDetails(Guid id)
        {
            var r = await myRecipeApiHandler.GetById<RecipeDetailsApiModel>(id);
            return r;
        }

        public async Task<List<ProductModel>> GetAllProduct()
        {
            var products = await productService.GetProducts();
            return products.ToList();
        }

        public async Task AddStep(string text)
        {
            try
            {
                var step = new RecipeStep
                {
                    Order = CurrentSelectedRecipe.Steps.Count + 1,
                    Text = text
                };
                var json = JsonConvert.SerializeObject(step);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var client = new HttpClient();

                var response = await client.PostAsync(new Uri($"http://localhost:5005/api/RecipeStep/{CurrentSelectedRecipe.Id}"), data);

                string result = response.Content.ReadAsStringAsync().Result;

                client.Dispose();

            }
            catch (Exception)
            {


            }
        }
    }
}
