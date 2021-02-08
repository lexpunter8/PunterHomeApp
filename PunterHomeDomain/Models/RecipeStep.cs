using System;
using System.Collections.Generic;
using System.Text;

namespace PunterHomeDomain.Models
{
    public interface IRecipeStep
    {
        Guid Id { get; set; }
        int Order { get; set; }
        string Text { get; set; }
    }

    public class RecipeStep : IRecipeStep
    {
        public Guid RecipeId { get; set; }
        public Guid Id { get; set; }
        public int Order { get; set; }
        public string Text { get; set; }
    }
}
