﻿using System;
namespace HomeApp.Frontend.View.Models
{
    public class ShoppingListItemViewModel
	{
		public string ItemName { get; set; }
		public string AmountValueString { get; set; }
		public int Count { get; set; } = 1;

        public ShoppingListViewModel ShoppingList { get; set; }

        public void Decrease()
        {
			Count--;

			if (Count < 1)
			{
				ShoppingList?.RemoveItem(this);
			}
        }
    }

    public class ShoppingListViewModel
    {
		public ShoppingListViewModel()
		{
			Items = new List<ShoppingListItemViewModel>();
			AddItem(new ShoppingListItemViewModel
			{

				AmountValueString = "200 gram",
				ItemName = "Volkren pasta",
				Count = 1
			});

			AddItem(new ShoppingListItemViewModel
			{

				AmountValueString = "1 kg",
				ItemName = "Kaas 30+",
				Count = 3
			});
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
    }
}

