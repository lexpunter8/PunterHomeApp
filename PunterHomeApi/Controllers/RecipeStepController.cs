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
    public class RecipeStepController : ControllerBase
    {
        private readonly IRecipeService myRecipeService;

        public RecipeStepController(IRecipeService recipeService)
        {
            myRecipeService = recipeService;
        }
        // GET: api/RecipeStep
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/RecipeStep/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/RecipeStep
        [HttpPost("{id}")]
        public IActionResult Post([FromBody] RecipeStep value, Guid id)
        {
            try
            {
                myRecipeService.AddStep(value, id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT: api/RecipeStep/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                myRecipeService.RemoveStep(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
