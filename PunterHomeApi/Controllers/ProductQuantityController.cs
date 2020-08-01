﻿using System;
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
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}