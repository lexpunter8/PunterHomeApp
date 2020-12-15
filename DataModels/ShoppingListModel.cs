using System;

namespace DataModels
{
    public class ShoppingListModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
