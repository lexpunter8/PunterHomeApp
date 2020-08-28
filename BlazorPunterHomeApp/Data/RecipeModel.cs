using BlazorPunterHomeApp.crud;
using Newtonsoft.Json;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorPunterHomeApp.Data
{
    public class RecipeModel : ICrudObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RecipeStepModel> Steps { get; set; }
        public IEnumerable<IngredientModel> Ingredients { get; set; }

        [JsonIgnore]
        public RecipeStepModel NewRecipestep { get; set; } = new RecipeStepModel();

        [JsonIgnore]
        public IngredientModel NewIngredient { get; set; } = new IngredientModel();
    }
}
