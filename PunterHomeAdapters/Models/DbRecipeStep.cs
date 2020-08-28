using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeAdapters.Models
{
    public class DbRecipeStep
    {
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
        public DbRecipe Recipe { get; set; }

    }
}
