using Microsoft.AspNetCore.Mvc;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PunterHomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTagController : ControllerBase
    {
        private readonly IProductTagService productTagService;

        public ProductTagController(IProductTagService productTagService)
        {
            this.productTagService = productTagService;
        }
        // GET: api/<ProductTagController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var restult = await productTagService.GetAllTagsAsync();
                return Ok(restult);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET api/<ProductTagController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var restult = await productTagService.GetTagsForProductAsync(id);
                return Ok(restult);
            }
            catch
            {
                return BadRequest();
            }
        }

        // POST api/<ProductTagController>
        [HttpPost("{productId}/{tagId}")]
        public void Post(Guid productId, Guid tagId)
        {
            try
            {
                productTagService.AddTagToProduct(productId, tagId);

            }catch
            {

            }
        }

        // PUT api/<ProductTagController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductTagController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
