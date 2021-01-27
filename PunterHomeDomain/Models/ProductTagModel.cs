using System;

namespace PunterHomeDomain.Models
{
    public class ProductTagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProductId { get; set; }
    }
}
