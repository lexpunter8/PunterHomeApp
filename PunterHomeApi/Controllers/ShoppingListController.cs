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
using PunterHomeDomain.Services;

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
        [Produces(typeof(ShoppingListModel))]
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
            return DoRequestValidation<IEnumerable<ShoppingListItemModel>>(() => myShoppingListService.GetItemsForShoppingList(id));
        }

        [HttpGet("{id}/shop")]
        public IActionResult GetShopListItem(Guid id)
        {
            try
            {
                var result = myShoppingListService.GetShoppingListShopItems(id);
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



        // POST: api/ShoppingList
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        // POST: api/ShoppingList
        [HttpPost("addmeasurement")]
        public void Post([FromBody] List<AddMeasurementsToShoppingListItem> request)
        {
            myShoppingListService.AddMeasurementsToShopItems(request);
        }

        // POST: api/ShoppingList
        [HttpPost("{shoppingListID}")]
        public void Post(Guid shoppingListID, [FromBody] AddProductToShoppingListRequest request)
        {
            myShoppingListService.AddProductToShoppingList(shoppingListID, request);
        }

        // POST: api/ShoppingList
        [HttpPost("minproduct/{shoppingListID}/{newProductQuantity}")]
        public void AddMinToList(Guid shoppingListID, [FromBody] ProductAmountApiModel productAmount)
        {
            myShoppingListService.AddMinimumAmountToShoppingList(shoppingListID, productAmount.ProductId, new MeasurementAmount { Amount = productAmount.VolumeAmount, Type = productAmount.Type });
        }


        // POST: api/ShoppingList
        [HttpPost("additem/{shoppingListID}")]
        public void AddMinToList(Guid shoppingListID, [FromBody] TextApiModel text)
        {
            myShoppingListService.AddItemToShoppingList(shoppingListID, text.Text);
        }

        // PUT: api/ShoppingList/5
        [HttpPut("plus/{id}")]
        public void Add(Guid id)
        {
            myShoppingListService.UpdateShoppingListCount(id, 1);
        }
        // PUT: api/ShoppingList/5
        [HttpPut("min/{id}")]
        public void Put(Guid id)
        {
            myShoppingListService.UpdateShoppingListCount(id, -1);
        }

        [HttpPut("checked/{id}")]
        public void UpdateChecked(Guid id)
        {
            myShoppingListService.UpdateChecked(id, true);
        }
        [HttpPut("unchecked/{id}")]
        public void UpdateUnChecked(Guid id)
        {
            myShoppingListService.UpdateChecked(id, false);
        }

        [HttpPut("updateproducts/{id}")]
        public void UpdateCheckedProducts(Guid id)
        {
            myShoppingListService.AddQuantityToProductForCheckedItems(id);
        }


        [HttpPut("add/product/{shoppinglistId}/{prodQuanId}")]
        public void AddQuantityToProductShoppinglistItem(Guid shoppinglistId, int prodQuanId)
        {
            myShoppingListService.UpdateProductQuantity(shoppinglistId, prodQuanId, 1);
        }

        [HttpPut("decrease/product/{shoppinglistId}/{prodQuanId}")]
        public void decreaseQuantityToProductShoppinglistItem(Guid shoppinglistId, int prodQuanId)
        {
            myShoppingListService.UpdateProductQuantity(shoppinglistId, prodQuanId, -1);
        }

        [HttpPut("add/recipe/{shoppinglistId}/{recipeId}")]
        public void AddQuantityToRecipeShoppinglistItem(Guid shoppinglistId, Guid recipeId)
        {
            myShoppingListService.UpdaterecipeQuantity(shoppinglistId, recipeId, 1);
        }

        [HttpPut("decrease/recipe/{shoppinglistId}/{recipeId}")]
        public void DecreaseQuantityToRecipeShoppinglistItem(Guid shoppinglistId, Guid recipeId)
        {
            myShoppingListService.UpdaterecipeQuantity(shoppinglistId, recipeId, -1);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{itemId}")]
        public void Delete(Guid itemId)
        {
            myShoppingListService.RemoveProductFromShoppingList(itemId);
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

        //public static ShoppingListItemApiModel Convert(this ShoppingListItemModel model)
        //{
        //    return new ShoppingListItemApiModel
        //    {
        //        Id = model.Id,
        //        MeasurementType = model.MeasurementType,
        //        ShoppingListId = model.ShoppingListId,
        //        Volume = model.MeasurementAmount,
        //        IsChecked = model.IsChecked
        //    };
        //}
    }
}
