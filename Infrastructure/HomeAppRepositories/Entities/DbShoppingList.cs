using System;
using Microsoft.EntityFrameworkCore;

namespace HomeAppRepositories.Entities
{
    [PrimaryKey(nameof(Id))]
	public class DbShoppingListItem
	{
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string AmountValueString { get; set; }
        public int Count { get; set; }
        public bool IsChecked { get; set; }
    }

	
}

