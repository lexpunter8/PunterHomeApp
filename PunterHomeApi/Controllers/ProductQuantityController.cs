using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PunterHomeApi.Controllers
{
    [Route("api/[controller]")]
    public class ProductQuantityController : Controller
    {
        private IProductService myProductService;

        public ProductQuantityController(IProductService productService)
        {
            myProductService = productService;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost("{id}")]
        public async Task<IActionResult> Post([FromBody]ProductQuantity value, Guid id)
        {
            await myProductService.AddQuantityToProduct(value, id);
            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductQuantity productQuantity)
        {

            try
            {
                await myProductService.UpdateProductQuantity(id, productQuantity);
            } catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }


        // PUT api/<controller>/5
        [HttpPut("{id}/increase")]
        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            try
            {
                await myProductService.UpdateProductQuantity(id, 1, true);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        // PUT api/<controller>/5
        [HttpPut("{id}/decrease")]
        public async Task<IActionResult> DecreaseQuantity(int id)
        {

            try
            {
                await myProductService.UpdateProductQuantity(id, 1, false);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }


        // PUT api/<controller>/5
        [HttpPut("{id}/barcode/{barcode}")]
        public async Task<IActionResult> AddBarcode(int id, string barcode)
        {

            try
            {
                myProductService.AddBarcodeToQuantity(id, barcode);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await myProductService.DeleteProductQuantityById(id);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
