﻿using System;
using PunterHomeApp.Models;
using static Enums;

namespace PunterHomeAdapters.Models
{
    public class DbIngredient
    {
        public Guid RecipeId { get; set; }
        public Guid ProductId { get; set; }

        public virtual DbRecipe Recipe {get;set;}
        public virtual DbProduct Product { get; set; }

        public int UnitQuantity { get; set; }
        public EUnitQuantityType UnitQuantityType { get; set; } 
    }
}
