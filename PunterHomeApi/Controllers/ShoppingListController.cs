using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModels;
using DataModels.Measurements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PunterHomeApi.Queries;
using PunterHomeApi.Shared;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Services;
using PunterHomeDomain.ShoppingList;

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
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly IShoppingListQueries shoppingListQueries;

        public ShoppingListController(IShoppingListService shoppingListService, IShoppingListRepository shoppingListRepository, IShoppingListQueries shoppingListQueries)
        {
            myShoppingListService = shoppingListService;
            this.shoppingListRepository = shoppingListRepository;
            this.shoppingListQueries = shoppingListQueries;
        }

        // GET: api/ShoppingList
        [HttpGet]
        [Produces(typeof(IEnumerable<ShoppingListDto>))]
        public IActionResult Get()
        {
            return DoRequestValidation(() => shoppingListQueries.GetShoppingLists());
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
        [Produces(typeof(Guid))]
        public async Task<IActionResult> CreateShoppingList([FromBody] string value)
        {
            try
            {
                var result = ShoppingListAggregate.CreateNew(value);
                await shoppingListRepository.SaveAsync(result);
                return Ok(result.Id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        // POST: api/ShoppingList
        [HttpPost("{id}/text")]
        public async Task<IActionResult> AddTextItemToShoppingList(Guid id, [FromBody] string value)
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(id);
                shoppingList.AddTextItem(value);
                await shoppingListRepository.SaveAsync(shoppingList);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }


        // POST: api/ShoppingList
        [HttpDelete("{id}/text")]
        public async Task<IActionResult> RemoveTextItemFromShoppingList(Guid id, [FromBody] string value)
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(id);
                shoppingList.RemoveTextItem(value);
                await shoppingListRepository.SaveAsync(shoppingList);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{shoppinglistId}/text")]
        [Produces(typeof(IEnumerable<ShoppingListItemDto>))]
        public IActionResult GetTextItemsForShoppingList(Guid shoppinglistId)
        {
            try
            {
                var result = shoppingListQueries.GetTextItemsForShoppingList(shoppinglistId);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{shoppinglistId}/product")]
        [Produces(typeof(IEnumerable<ShoppingListProductItemDto>))]
        public IActionResult GetProductItemsForShoppingList(Guid shoppinglistId)
        {
            try
            {
                var result = shoppingListQueries.GetProductItemsForShoppingList(shoppinglistId);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{shoppinglistId}/recipe")]
        [Produces(typeof(IEnumerable<ShoppingListRecipeItemDto>))]
        public IActionResult GetRecipeItemsForShoppingList(Guid shoppinglistId)
        {
            try
            {
                var result = shoppingListQueries.GetRecipeItemsForShoppingList(shoppinglistId);
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: api/ShoppingList
        [HttpPost("{shoppinglistId}/product")]
        public async Task<IActionResult> AddProductItemToShoppingList(Guid shoppinglistId, [FromBody] AddProductToShoppingListRequest request)
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(shoppinglistId);
                shoppingList.AddProductItem(request.ProductId, request.Amount, request.MeasurementType);
                await shoppingListRepository.SaveAsync(shoppingList);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Add recipe to shoppinglist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPost("{shoppinglistId}/recipe/{amount}")]
        public async Task<IActionResult> AddRecipeItemToShoppingList(Guid shoppinglistId, [FromBody] Guid recipeId, int amount)
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(shoppinglistId);
                shoppingList.AddRecipeItem(recipeId);
                await shoppingListRepository.SaveAsync(shoppingList);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Add recipe to shoppinglist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPut("{shoppinglistId}/shop")]
        public async Task<IActionResult> SetShoppingShoppingList(Guid shoppinglistId)
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(shoppinglistId);

                foreach (var recipe in shoppingList.RecipeItems.ToArray())
                {
                    var ingredients = shoppingListQueries.GetIngredientForRecipe(recipe.RecipeId);

                    foreach (var ingredient in ingredients)
                    {
                        shoppingList.AddProductItem(ingredient.ProductId, ingredient.UnitQuantity, (PunterHomeDomain.Enums.EUnitMeasurementType)ingredient.UnitQuantityType);
                    }
                    shoppingList.RemoveRecipe(recipe.RecipeId);
                }

                shoppingList.StartShopping();
                await shoppingListRepository.SaveAsync(shoppingList);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// Add recipe to shoppinglist
        /// </summary>
        /// <param name="id"></param>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPut("{shoppinglistId}/close")]
        public async Task<IActionResult> CloseShoppingList(Guid shoppinglistId)
        {
            try
            {
                ShoppingListAggregate shoppingList = await shoppingListRepository.GetAsync(shoppinglistId);

                
                shoppingList.FinishShoppingList();
                await shoppingListRepository.SaveAsync(shoppingList);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        //// POST: api/ShoppingList
        //[HttpPost("addmeasurement")]
        //public async Task AddMeasurementToShoppingList([FromBody] AddMeasurementsToShoppingList request)
        //{
        //    var result = await shoppingListRepository.GetAsync(request.ShoppingListId);
        //    result.AddProductItem(request.MeasurementId, request.Count);
        //    await shoppingListRepository.SaveAsync(result);
        //}

        // POST: api/ShoppingList
        [HttpPost("addtext")]
        public async Task AddTextToShoppingListAsync([FromBody] AddTextToShoppingList request)
        {
            var result = await shoppingListRepository.GetAsync(request.ShoppingListId);
            result.AddTextItem(request.Text);
            await shoppingListRepository.SaveAsync(result);
        }

        // POST: api/ShoppingList
        [HttpPost("addrecipe")]
        public async Task AddRecipeToShoppingListAsync([FromBody] AddRecipeToShoppingListItem request)
        {
            var result = await shoppingListRepository.GetAsync(request.ShoppingListId);
            result.AddRecipeItem(request.RecipeId, request.Amount);
            await shoppingListRepository.SaveAsync(result);
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

        [HttpPut("checked/item/{shoppingListID}/{item}/{isChecked}")]
        public async Task SetItemChecked(Guid shoppingListID, string item, bool isChecked)
        {
            var shoppingList = await shoppingListRepository.GetAsync(shoppingListID);
            shoppingList.CheckItem(item, isChecked);
            await shoppingListRepository.SaveAsync(shoppingList);
        }


        [HttpPut("checked/product/{shoppingListID}/{productId}/{isChecked}")]
        public async Task SetProductChecked(Guid shoppingListID, Guid productId, bool isChecked)
        {
            var shoppingList = await shoppingListRepository.GetAsync(shoppingListID);
            shoppingList.CheckItem(productId, isChecked);
            await shoppingListRepository.SaveAsync(shoppingList);
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
        //public static ShoppingListApiModel Convert(this ShoppingListModel model)
        //{
        //    return new ShoppingListApiModel
        //    {
        //        Id = model.Id,
        //        Name = model.Name,
        //        CreateTime = model.CreateTime,
        //        IsShoppingActive = model.IsShoppingActive,
        //    };
        //}

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
