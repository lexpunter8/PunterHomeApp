using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PunterHomeAdapters.Models;

namespace PunterHomeApp.Models
{
    public class DbRecipe
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<string> Steps { get; set; }
        public List<DbIngredient> Ingredients { get; set; } = new List<DbIngredient>();
    }

}
