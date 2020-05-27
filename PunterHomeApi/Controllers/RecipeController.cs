using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PunterHomeApp.ApiModels;
using PunterHomeApp.Services;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PunterHomeApp.Controllers
{
    [Route("api/[controller]")]
    public class RecipeController : Controller
    {
        private readonly IRecipeService recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }
        // GET: api/values
        [HttpGet]
        public IEnumerable<RecipeApiModel> Get()
        {
            return recipeService.GetAllRecipes().Select(r => new RecipeApiModel
            {
                Name = r.Name,
                Steps = new List<string>(r.Steps),
                Ingredients = r.Ingredients.Select(ConvertRecipeToApiModel).ToList()
            });
        }

        private ApiIngredientModel ConvertRecipeToApiModel(IIngredient ingredient)
        {
            return new ApiIngredientModel
            {
                ProductId = ingredient.Product.Id,
                ProductName = ingredient.Product.Name,
                UnitQuantity = ingredient.UnitQuantity,
                UnitQuantityType = ingredient.UnitQuantityType
            };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]RecipeApiModel value)
        {

            recipeService.CreateRecipe(value.Name, value.Steps.ToList(), value.Ingredients.Select(i => new Ingredient
                    {
                         Product = new Product { Id = i.ProductId },
                         UnitQuantity = i.UnitQuantity,
                         UnitQuantityType = i.UnitQuantityType
                    }).ToList()
            );
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
