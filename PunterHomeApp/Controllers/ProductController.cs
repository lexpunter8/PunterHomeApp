using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<ProductApiModel> Get()
        {
            return ConvertProducts(myProductService.GetProducts());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IProduct Get(int id)
        {
            return myProductService.GetProductById(Guid.NewGuid());
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]Product value)
        {
            value.Id = Guid.NewGuid();
            myProductService.AddProduct(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Product value)
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
            products.ToList().ForEach(p => convertedProducts.Add(new ProductApiModel
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
}
