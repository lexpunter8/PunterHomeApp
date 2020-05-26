using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PunterHomeAdapters.Models;
using PunterHomeApp.ApiModels;
using PunterHomeApp.Helpers;
using PunterHomeDomain.Interfaces;

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
        public IActionResult Get(int id)
        {
            var result = myProductService.GetProductById(Guid.NewGuid());
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
            return null;
            //if (searchText == null)
            //{
            //    var products = await myProductService.GetProducts();
            //    return ConvertProducts(products);
            //}
            //return ConvertProducts(await myProductService.SearchProductsAsync(searchText).ConfigureAwait(false));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]DbProduct value)
        {
            //value.Id = Guid.NewGuid();
            //myProductService.AddProduct(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]IProduct value)
        {

        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        private List<ProductApiModel> ConvertProducts(IEnumerable<DbProduct> products)
        {
            var convertedProducts = new List<ProductApiModel>();
            var pr = products.ToList();
            pr.ForEach(p => convertedProducts.Add(new ProductApiModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,

            }));
            return convertedProducts;
        }
    }

    public class SearchApiModel
    {
        public string SearchText { get; set; }
    }
}
