using FluentAssert;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PunterHomeApp.Services;
using PunterHomeDomain;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using static Enums;

namespace Test
{
    [TestClass]
    public class MeaserementsTests
    {
        private RecipeService recipeService;
        private IRecipeDataAdapter myRecipeAdpater;
        private IProductDataAdapter myProductAdpater;

        [TestInitialize]
        public void init()
        {
            myRecipeAdpater = Substitute.For<IRecipeDataAdapter>();
            myProductAdpater = Substitute.For<IProductDataAdapter>();
            recipeService = new RecipeService(myRecipeAdpater, myProductAdpater);
        }

        [TestMethod]
        public void Test()
        {
            Guid pId = Guid.NewGuid();
            var p = new Product
            {
                Id = pId,
                Name = "Test p",
                ProductQuantities = new List<ProductQuantity>
                {
                    new ProductQuantity
                    {
                        Quantity = 1,
                        UnitQuantityType = EUnitMeasurementType.Gr,
                        UnitQuantityTypeVolume = 200
                    }
                }
            };

            var i = new Ingredient
            {
                ProductId = pId,
                ProductName = "Test p",
                RecipeId = Guid.NewGuid(),
                UnitQuantity = 100,
                UnitQuantityType = EUnitMeasurementType.Gr
            };

            recipeService.CalculateIngedientAvailability(i, p).ShouldBeTrue();
        }
    }
}
