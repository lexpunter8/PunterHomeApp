using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PunterHomeApp.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PunterHomeApp.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService myProductService;

        public ProductController(IProductService productService)
        {
            myProductService = productService;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<IProduct> products = await myProductService.GetProducts();
            return Ok(products);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = myProductService.GetProductById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // GET api/values/5
        [HttpGet("search")]
        public async Task<IActionResult> Get(string searchText)
        {
            if (searchText == null)
            {
                var products = await myProductService.GetProducts();
                return Ok(products);
            }
            var result = myProductService.SearchProductsAsync(searchText);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]NewProductApiModel value)
        {
            try
            {
                myProductService.AddProduct(value);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok();
        }

        // PUT api/values/5
        [HttpPut("{id}/{newName}")]
        public async Task<IActionResult> Put(Guid id, string newName)
        {
            bool result = await myProductService.Update(id, newName);

            if (result)
            {
                Ok();
            }
            return BadRequest();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = await myProductService.TryDeleteProductById(id);
            if (result)
            {
                BadRequest();
            }

            return Ok();
        }


        private List<NewProductApiModel> ConvertProducts(IEnumerable<IProduct> products)
        {
            var convertedProducts = new List<NewProductApiModel>();
            var pr = products.ToList();
            pr.ForEach(p => convertedProducts.Add(new NewProductApiModel
            {
                Name = p.Name
            }));
            return convertedProducts;
        }
    }

    public class SearchApiModel
    {
        public string SearchText { get; set; }
    }
}
