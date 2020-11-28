using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace PunterHomeDomain.Conversions
{
    public static class IngredientApiModelIngredientConversion
    {
        public static ApiIngredientModel Convert(Ingredient a)
        {
            return new ApiIngredientModel
            {
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                UnitQuantity = a.UnitQuantity,
                UnitQuantityType = a.UnitQuantityType
            };
        }


        public static Ingredient Convert(ApiIngredientModel a)
        {
            return new Ingredient
            {
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                UnitQuantity = a.UnitQuantity,
                UnitQuantityType = a.UnitQuantityType
            };
        }
    }
}
