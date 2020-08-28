using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                Id = r.Id,
                Name = r.Name,
                Steps = r.Steps.ToList(),
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
        [HttpPost("{name}")]
        public IActionResult Post(string name)
        {
            try
            {
                recipeService.CreateRecipe(name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}/{name}")]
        public IActionResult Put(Guid id, string name)
        {
            try
            {
                recipeService.UpdateRecipe(id, name);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                recipeService.DeleteRecipeById(id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }
    }
}
