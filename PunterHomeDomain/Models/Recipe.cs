using System;
using System.Collections.Generic;
using PunterHomeApp.Services;

namespace PunterHomeDomain.Models
{
    public class Recipe : IRecipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Steps { get; set; }
        public IEnumerable<IIngredient> Ingredients { get; set; }
    }
}
