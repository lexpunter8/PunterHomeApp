using System;
using System.Collections.Generic;
using PunterHomeApp.Services;
using PunterHomeDomain.Models;
using static Enums;

namespace PunterHomeDomain.ApiModels
{
    public class RecipeDetailsApiModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<RecipeStep> Steps { get; set; } 
        //    = new List<RecipeStep>
        //{
        //    new RecipeStep
        //    {
        //        Text = "some text bla bla bla",
        //        Order = 1
        //    },
        //    new RecipeStep
        //    {
        //        Text = "some text bla bla bla m,et bla fkjab fksakjfg skjdgf ksdgfkjsdg fksgdfious dgfkjsdg fkjsgdkjfg sdiukf gksjdfg",
        //        Order = 2
        //    }
        //};
        public List<ApiIngredientModel> Ingredients { get; set; }
    }
    public class ApiIngredientModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public bool IsAvaliable { get; set; }
    }

}
