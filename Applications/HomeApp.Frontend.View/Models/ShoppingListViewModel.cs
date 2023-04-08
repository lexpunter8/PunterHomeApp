using HomeApp.Frontend.View.Pages;
using HomeApp.Frontend.ViewModels;
using HomeApp.Shared;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace HomeApp.Frontend.View.Models
{
    public class ShoppingListItemViewModel : BaseModelViewModel<ShoppingListItemDto>
    {
        private readonly HttpClient server;

        public ShoppingListItemViewModel(HttpClient server, ShoppingListItemDto s, ShoppingListViewModel parent)
		{
            this.server = server;
			FromModel(s);
            ShoppingList = parent;
        }

        public Guid Id { get; set; }

        public string ItemName { get; private set; }
		public string AmountValueString { get; private set; }
		public int Count { get; private set; } = 1;
        public bool IsChecked { get; private set; }

        public ShoppingListViewModel ShoppingList { get; set; }

        public async Task Decrease()
        {

            try
            {
                if (Count < 2)
                {
                    await server.DeleteAsync($"shoppinglistitem/{Id}");
                    ShoppingList.RemoveItem(this);
                    return;
                }
                var patch = new JsonPatchDocument<ShoppingListItemDto>().Add(a => a.Count, Count - 1);
                var response = await server.PatchAsyncInternal($"shoppinglistitem/{Id}", patch);

                if (response.IsSuccessStatusCode)
                {
                    var responseValue = await response.Content.ReadFromJsonAsync<ShoppingListItemDto>();
                    Count = responseValue.Count;
                }

            }
            catch (Exception e)
            {
                return;
            }

        }

		public async Task Increase()
		{
			try
			{
                Console.WriteLine($"Send value: {Count + 1}");
                var patch = new JsonPatchDocument<ShoppingListItemDto>().Add(a => a.Count, Count + 1);
				var response = await server.PatchAsyncInternal($"shoppinglistitem/{Id}", patch);

                if (response.IsSuccessStatusCode)
                {
                    var responseValue = await response.Content.ReadFromJsonAsync<ShoppingListItemDto>();
                    Count = responseValue.Count;
                    Console.WriteLine($"Send value: {Count}");
                }
			}
			catch (Exception e)
			{
				return;
			}
		}

        public override void FromModel(ShoppingListItemDto model)
        {
			Id = model.Id;
			ItemName = model.ItemName;
			AmountValueString = model.AmountValueString;
			Count = model.Count;
			IsChecked = model.IsChecked;
        }

        internal async void ChangeChecked(bool v)
        {
            try
            {
                var patch = new JsonPatchDocument<ShoppingListItemDto>().Add(a => a.IsChecked, v);
                var response = await server.PatchAsyncInternal($"shoppinglistitem/{Id}", patch);

                if (response.IsSuccessStatusCode)
                {
                    var responseValue = await response.Content.ReadFromJsonAsync<ShoppingListItemDto>();
                    IsChecked = responseValue.IsChecked;
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }

    public class ShoppingListViewModel
    {
        private readonly HttpClient server;

        public ShoppingListViewModel(HttpClient server)
		{
			Items = new List<ShoppingListItemViewModel>();
            this.server = server;
        }
		public event EventHandler PropertyChanged;
        public List<ShoppingListItemViewModel> Items { get; set; }

		public void AddItem(ShoppingListItemViewModel item)
		{
			if (string.IsNullOrWhiteSpace(item.ItemName))
			{
				return;
			}
			item.ShoppingList = this;
			Items.Add(item);
			PropertyChanged?.Invoke(this, EventArgs.Empty);
		}

        internal void RemoveItem(ShoppingListItemViewModel shoppingListItemViewModel)
        {
			Items.RemoveAll(a => a.ItemName == shoppingListItemViewModel.ItemName);
			PropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        internal async void RemoveAllItems()
        {
            var patch = new JsonPatchDocument<ShoppingListDto>();

            int index = 0;
            foreach (var item in Items)
            {
                patch.Remove(r => r.Items, 0);
            }

            var result = await server.PatchInternal($"shoppinglist/{Guid.NewGuid()}", patch);

            if (result == null || result.Items.Count != 0)
            {
                return;
            }

            Refresh(result.Items);
        }

		public async void RemoveAllCheckdItems()
        {
            var patch = new JsonPatchDocument<ShoppingListDto>();

            int index = 0;
            foreach (var item in Items)
            {
                if (!item.IsChecked)
                {
                    index++;
                    continue;
                }
                patch.Remove(r => r.Items, index);
            }

            var result = await server.PatchInternal($"shoppinglist/{Guid.NewGuid()}", patch);

            if (result == null)
            {
                return;
            }

            Refresh(result.Items);
        }

		public async void ResetItems()
        {
            var patch = new JsonPatchDocument<ShoppingListDto>();

            int index = 0;
            foreach (var item in Items)
            {
                patch.Replace(r => r.Items, new ShoppingListItemDto
                {
                    IsChecked = false,
                    AmountValueString = item.AmountValueString,
                    Count = item.Count,
                    Id = item.Id,
                    ItemName = item.ItemName
                }, index++);
            }

            var result = await server.PatchInternal($"shoppinglist/{Guid.NewGuid()}", patch);
            if (result == null)
            {
                return;
            }

            Refresh(result.Items);
            return;

            foreach (var item in Items)
            {
                item.ChangeChecked(false);
            }
            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void Refresh(List<ShoppingListItemDto> items)
        {
            Items.Clear();
            Items.AddRange(items.Select(s => new ShoppingListItemViewModel(server, s, this)));

            PropertyChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}

