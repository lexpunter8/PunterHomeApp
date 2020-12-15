using System;

namespace PunterHomeAdapters.Models
{

    public class DbShoppingList
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
