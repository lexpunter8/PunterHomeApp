using BlazorPunterHomeApp.crud;
using BlazorPunterHomeApp.Data;
using Newtonsoft.Json;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.ViewModels
{
    public class RecipeViewModel : BaseViewModel
    {
        private RecipeService myRecipeService;
        private BaseHttpCrudHandler<RecipeModel> myRecipeApiHandler = new BaseHttpCrudHandler<RecipeModel>("http://localhost:5005/api/recipe");
        private BaseHttpCrudHandler<IngredientModel> myIngredientApiHandler = new BaseHttpCrudHandler<IngredientModel>("http://localhost:5005/api/ingredient");
        private BaseHttpCrudHandler<RecipeStepModel> myStepsApiHandler = new BaseHttpCrudHandler<RecipeStepModel>("http://localhost:5005/api/recipestep");
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
    }
}
