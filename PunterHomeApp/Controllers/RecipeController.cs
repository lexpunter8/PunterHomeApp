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
            throw new NotImplementedException();
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
        [HttpPost("{name}")]
        public void Post(string name)
        {
            recipeService.CreateRecipe(name);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
