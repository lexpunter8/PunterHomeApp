using AutoMapper;
using HomeApp.Shared;
using HomeAppDomain.AggregateRoots;
using HomeAppDomain.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShoppingListItemController : ControllerBase
    {
        private readonly ILogger<ShoppingListItemController> _logger;
        private readonly IShoppingListItemRepository shoppingListItemRepository;
        private readonly Mapper mapper;

        public ShoppingListItemController(ILogger<ShoppingListItemController> logger, IShoppingListItemRepository shoppingListItemRepository, Mapper mapper)
        {
            _logger = logger;
            this.shoppingListItemRepository = shoppingListItemRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(shoppingListItemRepository.GetAll().Select(s => mapper.Map<ShoppingListItemDto>(s)));

        }

        [HttpPatch("{itemId}")]
        public async Task<IActionResult> UpdateItem(Guid itemId, [FromBody] JsonPatchDocument<ShoppingListItemDto> patchDoc)
        {
            if (patchDoc != null)
            {
                var p = mapper.Map<JsonPatchDocument<ShoppingListItem>>(patchDoc);
                var item = shoppingListItemRepository.GetById(itemId);


                p.ApplyTo(item, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var mappedItem = mapper.Map<ShoppingListItemDto>(item);
                await shoppingListItemRepository.Save(item);
                return new ObjectResult(mappedItem);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            await shoppingListItemRepository.RemoveById(itemId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody] ShoppingListItemDto item)
        {
            item.Id = Guid.NewGuid();
            item.Count = 1;

            await shoppingListItemRepository.Save(mapper.Map<ShoppingListItem>(item));
            return Created("", item);
        }

        [HttpPatch]
        public IActionResult JsonPatchWithModelState([FromBody] JsonPatchDocument<ShoppingListItemDto> patchDoc)
        {
            if (patchDoc != null)
            {
                var customer = new ShoppingListItemDto();

                patchDoc.ApplyTo(customer, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }

}