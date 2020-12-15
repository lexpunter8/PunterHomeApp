using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using DataModels.Measurements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;

namespace PunterHomeApi.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult DoRequestValidation<T>(Func<T> action)
        {
            try
            {
                T result = action.Invoke();
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListController : BaseController
    {
        private readonly IShoppingListService myShoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            myShoppingListService = shoppingListService;
        }

        // GET: api/ShoppingList
        [HttpGet]
        public IActionResult Get()
        {
            return DoRequestValidation(() => myShoppingListService.GetShoppingLists().Select(r => r.Convert()));
            //try
            //{
            //    var result = (IEnumerable<ShoppingListModel>) ;
            //    if (result == null)
            //    {
            //        return NotFound();
            //    }
            //    return Ok(result);
            //}
            //catch (Exception e)
            //{
            //    return BadRequest();
            //}
        }

        // GET: api/ShoppingList/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(Guid id)
        {
            return DoRequestValidation<IEnumerable<ShoppingListItemApiModel>>(() => myShoppingListService.GetItemsForShoppingList(id).Select(s => s.Convert()));
        }

        // POST: api/ShoppingList
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST: api/ShoppingList
        [HttpPost("{shoppingListID}/{newProductQuantity}")]
        public void Post(Guid shoppingListID, int newProductQuantity)
        {
            myShoppingListService.AddProductToShoppingList(shoppingListID, newProductQuantity);
        }

        // POST: api/ShoppingList
        [HttpPost("min/{shoppingListID}/{newProductQuantity}")]
        public void AddMinToList(Guid shoppingListID, [FromBody] ProductAmountApiModel productAmount)
        {
            myShoppingListService.AddMinimumAmountToShoppingList(shoppingListID, productAmount.ProductId, new MeasurementAmount { Amount = productAmount.VolumeAmount, Type = productAmount.Type });
        }

        // PUT: api/ShoppingList/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public static class ShoppingListConverter
    {
        public static ShoppingListApiModel Convert(this ShoppingListModel model)
        {
            return new ShoppingListApiModel
            {
                Id = model.Id,
                Name = model.Name,
                CreateTime = model.CreateTime
            };
        }

        public static ShoppingListItemApiModel Convert(this ShoppingListItemModel model)
        {
            return new ShoppingListItemApiModel
            {
                Count = model.Count,
                Id = model.Id,
                MeasurementType = model.MeasurementType,
                ProductName = model.ProductName,
                ShoppingListId = model.ShoppingListId,
                Volume = model.Volume
            };
        }
    }
}
