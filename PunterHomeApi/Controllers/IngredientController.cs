using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;

namespace PunterHomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private IIngredientService myIngredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            myIngredientService = ingredientService;
        }

        // POST: api/Ingredient
        [HttpPost]
        public IActionResult Post([FromBody] Ingredient value)
        {
            try
            {
                myIngredientService.InsertIngredient(value);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // PUT: api/Ingredient/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{recipeId}/{productId}")]
        public void Delete(Guid recipeId, Guid productId)
        {
            myIngredientService.DeleteIngredient(recipeId, productId);
        }
    }
}
