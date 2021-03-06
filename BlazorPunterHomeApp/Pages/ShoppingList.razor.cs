﻿using BlazorPunterHomeApp.Data;
using Microsoft.AspNetCore.Components;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Pages
{
    public class ShoppingListDetailsViewModel
    {
        public ShoppingListDetailsViewModel(ShoppingListItemDetailsModel model)
        {
            Model = model;
        }

        public bool ShowInfo { get; set; }

        public ShoppingListItemDetailsModel Model { get; }
    }
    public partial class ShoppingList : ComponentBase
    {
        [Inject]
        public BlazorShoppingListService ShoppingListService { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await Refresh();
        }

        public List<ShoppingListDetailsViewModel> ListItems { get; set; } = new List<ShoppingListDetailsViewModel>();

        public async void AddQuantityToItem(ShoppingListItemDetailsModel item)
        {
            //await ShoppingListService.UpdateCountForItem(item.Id, true);
            //ListItems.FirstOrDefault(i => i.Id == item.Id).Quantity += 1;
            StateHasChanged();
        }
        public async void DecreaseQuantityToItem(ShoppingListItemDetailsModel item)
        {
            //await ShoppingListService.UpdateCountForItem(item.Id, false);
            //ListItems.FirstOrDefault(i => i.Id == item.Id).Quantity -= 1;
            StateHasChanged();
        }

        public async void DeleteItem(ShoppingListItemDetailsModel item)
        {
            //await ShoppingListService.DeleteItem(item.Id);
            //ListItems.Remove(ListItems.FirstOrDefault(i => i.Id == item.Id));
            StateHasChanged();
        }

        public async Task Refresh()
        {
            var items = await ShoppingListService.GetShoppingListItems();

            ListItems = items.Select(i => new ShoppingListDetailsViewModel(i)).ToList();
            StateHasChanged();
        }

        public void ShowInfo(ShoppingListDetailsViewModel item)
        {
            if (item.ShowInfo)
            {
                item.ShowInfo = false;
                return;
            }
            ListItems.ForEach(i => i.ShowInfo = false);
            item.ShowInfo = true;
        }
    }

}
