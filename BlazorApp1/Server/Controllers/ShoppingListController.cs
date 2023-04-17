using AutoMapper;
using HomeApp.Shared;
using HomeAppDomain.AggregateRoots;
using HomeAppDomain.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers
{
    public class InMemoryShoppingListRepo : IShoppingListRepository
    {
        public InMemoryShoppingListRepo(IShoppingListItemRepository itemRepository)
        {
            ItemRepository = itemRepository;
        }
        private List<ShoppingList> myShoppingLists = new List<ShoppingList>
        {
            new ShoppingList()
        };

        public IShoppingListItemRepository ItemRepository { get; }

        public IEnumerable<ShoppingList> GetAll()
        {
            return myShoppingLists;
        }

        public ShoppingList GetById(Guid id)
        {
            var sl = new ShoppingList();

            var items = ItemRepository.GetAll();
            sl.Items.Clear();
            sl.Items.AddRange(items);

            return sl;
        }

        public void Save(ShoppingList shoppingList)
        {
            var items = ItemRepository.GetAll().ToArray();
            foreach(var i in items)
            {
                ItemRepository.RemoveById(i.Id);
            }

            foreach(var item in shoppingList.Items)
            {
                ItemRepository.Save(item);
            }
        }
    }

    public class InMemoryShoppingListItemRepo : IShoppingListItemRepository
    {
        private List<ShoppingListItem> myShoppingLists = new List<ShoppingListItem>
            {
                new ShoppingListItem
                {
                    Id = Guid.NewGuid(),
                    AmountValueString = "200 gram",
                    ItemName = "Volkoren pasta",
                    Count = 2,
                    IsChecked = false
                },
                new ShoppingListItem
                {
                    Id = Guid.NewGuid(),
                    AmountValueString = "500 gram",
                    ItemName = "Kaas 50+",
                    Count = 1,
                    IsChecked = true
                }
            };

        public IEnumerable<ShoppingListItem> GetAll()
        {
            return myShoppingLists;
        }

        public ShoppingListItem GetById(Guid id)
        {
            return myShoppingLists.FirstOrDefault(f => f.Id == id);
        }

        public Task RemoveById(Guid id)
        {
            var o = GetById(id);
            if (o != null)
            {
                myShoppingLists.Remove(o);
            }
            return Task.CompletedTask;
        }

        public Task Save(ShoppingListItem item)
        {
            RemoveById(item.Id);
            myShoppingLists.Add(item);
            return Task.CompletedTask;
        }
    }

    [ApiController]
    [Route("[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly ILogger<ShoppingListController> _logger;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly Mapper mapper;

        public ShoppingListController(ILogger<ShoppingListController> logger, IShoppingListRepository shoppingListRepository, Mapper mapper)
        {
            _logger = logger;
            this.shoppingListRepository = shoppingListRepository;
            this.mapper = mapper;
        }

        [HttpPatch("{itemId}")]
        public IActionResult UpdateItem(Guid itemId, [FromBody] JsonPatchDocument<ShoppingListDto> patchDoc)
        {
            if (patchDoc != null)
            {
                var item = shoppingListRepository.GetById(itemId);

                var mappedItem = mapper.Map<ShoppingListDto>(item);

                patchDoc.ApplyTo(mappedItem, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                shoppingListRepository.Save(mapper.Map<ShoppingList>(mappedItem));
                return new ObjectResult(mappedItem);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }

}