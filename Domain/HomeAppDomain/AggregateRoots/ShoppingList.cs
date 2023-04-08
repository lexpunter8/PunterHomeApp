using System;
namespace HomeAppDomain.AggregateRoots
{
    public class PatchableAggregateRoot
    {

    }

	public class ShoppingListItem
    {
        public string ItemName { get; set; }
        public string AmountValueString { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; }
        public Guid Id { get; set; }
    }

	public class ShoppingList : PatchableAggregateRoot  
	{
		private List<ShoppingListItem> myItems;
		public ShoppingList()
		{
			myItems = new List<ShoppingListItem>();

            myItems = new List<ShoppingListItem>
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
        }

        public List<ShoppingListItem> Items => myItems;

		public void IncreaseItem(Guid itemId)
		{
			var item = myItems.FirstOrDefault(f => f.Id == itemId);

			if (item == null)
			{
				return;
			}

			item.Count++;
		}


        public void DecreaseItem(Guid itemId)
        {
            var item = myItems.FirstOrDefault(f => f.Id == itemId);

            if (item == null)
            {
                return;
            }

            item.Count--;

            if (item.Count < 1)
            {
                RemoveItem(itemId);
            }
        }

        public void CheckItem(Guid itemId)
        {
            var item = myItems.FirstOrDefault(f => f.Id == itemId);

            if (item == null)
            {
                return;
            }

            item.IsChecked = !item.IsChecked;
        }

        public Guid AddItem(string itemName, string amountValue)
        {
            var createdGuid = Guid.NewGuid();
            myItems.Add(new ShoppingListItem
            {
                Id = createdGuid,
                Count = 1,
                ItemName = itemName,
                AmountValueString = amountValue
            });
            return createdGuid;
        }

        public void RemoveItem(Guid itemId)
        {
            myItems.RemoveAll(r => r.Id == itemId);
        }

        public ShoppingListItem? GetItem(Guid itemId)
        {
            var item = myItems.FirstOrDefault(f => f.Id == itemId);
            return item;
        }
    }
}

