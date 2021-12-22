using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PunterHomeApi.Shared;
using PunterHomeDomain;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;

namespace PunterHomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeStepController : ControllerBase
    {
        private readonly IRecipeService myRecipeService;
        private readonly IRecipeStepRepository recipeStepRepository;

        public RecipeStepController(IRecipeService recipeService, IRecipeStepRepository recipeStepRepository)
        {
            myRecipeService = recipeService;
            this.recipeStepRepository = recipeStepRepository;
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
        public async Task<IActionResult> Post([FromBody] RecipeStep value, Guid id)
        {
            try
            {
                await recipeStepRepository.SaveAsync(new RecipeStepAggregate(Guid.NewGuid(), id, value.Text, value.Order, new List<RecipeStepIngredient>()));
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("ingredienttostep")]
        public async Task<IActionResult> Post([FromBody] AddIngredientToRecipeStepRequest request)
        {
            try
            {
                var recipeStep = await recipeStepRepository.GetAsync(request.RecipeStepId);
                recipeStep.AddIngredient(request.IngredientId, request.RecipeStepId);
                await recipeStepRepository.SaveAsync(recipeStep);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT: api/RecipeStep/5
        [HttpPut]
        public IActionResult Put([FromBody] RecipeStep value)
        {
            try
            {
                myRecipeService.UpdateStep(value);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

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
