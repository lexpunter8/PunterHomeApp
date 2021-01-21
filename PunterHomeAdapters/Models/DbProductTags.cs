using System;

namespace PunterHomeAdapters.Models
{
    public class DbProductTags
    {
        public Guid TagId { get; set; }
        public Guid ProductId { get; set; }

        public virtual DbProduct Product { get; set; }
        public virtual DbProductTag Tag { get; set; }
    }
}
