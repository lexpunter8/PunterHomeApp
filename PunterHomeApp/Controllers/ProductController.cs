using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PunterHomeAdapters.Models;
using PunterHomeApp.ApiModels;
using PunterHomeApp.Helpers;
using PunterHomeApp.Interfaces;

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
        public async Task<IEnumerable<ProductApiModel>> Get()
        {
            var products = await myProductService.GetProducts();
            return ConvertProducts(products);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IProduct Get(int id)
        {
            return myProductService.GetProductById(Guid.NewGuid());
        }

        // GET api/values/5
        [HttpGet("search")]
        public async Task<IEnumerable<ProductApiModel>> Get(string searchText)
        {
            if (searchText == null)
            {
                var products = await myProductService.GetProducts();
                return ConvertProducts(products);
            }
            return ConvertProducts(await myProductService.SearchProductsAsync(searchText).ConfigureAwait(false));
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]DbProduct value)
        {
            value.Id = Guid.NewGuid();
            myProductService.AddProduct(value);
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


        private List<ProductApiModel> ConvertProducts(IEnumerable<IProduct> products)
        {
            var convertedProducts = new List<ProductApiModel>();
            var pr = products.ToList();
            pr.ForEach(p => convertedProducts.Add(new ProductApiModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Quantity = p.Quantity,
                UnitQuantity = p.UnitQuantity,
                UnitQuantityType = EnumHelpers.GetEnumDescription(p.UnitQuantityType)
            }));
            return convertedProducts;
        }
    }

    public class SearchApiModel
    {
        public string SearchText { get; set; }
    }
}
