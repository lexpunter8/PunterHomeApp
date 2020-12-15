using DataModels.Measurements;
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

namespace Tests
{
    [TestClass]
    public class UnitTest1
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
            var p = new ProductDetails
            {
                Id = pId,
                Name = "Test p",
                ProductQuantities = new List<BaseMeasurement>
                {
                    new MiliLiter
                    {
                        Quantity = 1,
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
                UnitQuantityType = EUnitMeasurementType.Ml
            };

            recipeService.IsIngedientAvailable(i, p).ShouldBeTrue();
        }

        [TestMethod]
        public void Test2()
        {
            Guid pId = Guid.NewGuid();
            var p = new ProductDetails
            {
                Id = pId,
                Name = "Test p",
                ProductQuantities = new List<BaseMeasurement>
                {
                    new Liter()
                    {
                        Quantity = 1,
                        UnitQuantityTypeVolume = 1
                    }
                }
            };

            var i = new Ingredient
            {
                ProductId = pId,
                ProductName = "Test p",
                RecipeId = Guid.NewGuid(),
                UnitQuantity = 100,
                UnitQuantityType = EUnitMeasurementType.Ml
            };

            recipeService.IsIngedientAvailable(i, p).ShouldBeTrue();
        }

        [TestMethod]
        public void When_QuanityIs2Times100Ml_AndNeededIs200Ml_ThenReturnTrue()
        {
            Guid pId = Guid.NewGuid();
            var p = new ProductDetails
            {
                Id = pId,
                Name = "Test p",
                ProductQuantities = new List<BaseMeasurement>
                {
                    new MiliLiter()
                    {
                        Quantity = 2,
                        UnitQuantityTypeVolume = 100
                    }
                }
            };

            var i = new Ingredient
            {
                ProductId = pId,
                ProductName = "Test p",
                RecipeId = Guid.NewGuid(),
                UnitQuantity = 200,
                UnitQuantityType = EUnitMeasurementType.Ml
            };

            recipeService.IsIngedientAvailable(i, p).ShouldBeTrue();
        }


        [TestMethod]
        public void When_QuanityIs2Times50MlAnd1x100Ml_AndNeededIs200Ml_ThenReturnTrue()
        {
            Guid pId = Guid.NewGuid();
            var p = new ProductDetails
            {
                Id = pId,
                Name = "Test p",
                ProductQuantities = new List<BaseMeasurement>
                {
                    new MiliLiter()
                    {
                        Quantity = 1,
                        UnitQuantityTypeVolume = 100
                    },
                    new MiliLiter()
                    {
                        Quantity = 2,
                        UnitQuantityTypeVolume = 50
                    }
                }
            };

            var i = new Ingredient
            {
                ProductId = pId,
                ProductName = "Test p",
                RecipeId = Guid.NewGuid(),
                UnitQuantity = 200,
                UnitQuantityType = EUnitMeasurementType.Ml
            };

            recipeService.IsIngedientAvailable(i, p).ShouldBeTrue();
        }

        [TestMethod]
        public void When_QuanityIs1Times50MlAnd1x100Ml_AndNeededIs200Ml_ThenReturnFalse()
        {
            Guid pId = Guid.NewGuid();
            var p = new ProductDetails
            {
                Id = pId,
                Name = "Test p",
                ProductQuantities = new List<BaseMeasurement>
                {
                    new MiliLiter()
                    {
                        Quantity = 1,
                        UnitQuantityTypeVolume = 100
                    },
                    new MiliLiter()
                    {
                        Quantity = 1,
                        UnitQuantityTypeVolume = 50
                    }
                }
            };

            var i = new Ingredient
            {
                ProductId = pId,
                ProductName = "Test p",
                RecipeId = Guid.NewGuid(),
                UnitQuantity = 200,
                UnitQuantityType = EUnitMeasurementType.Ml
            };

            recipeService.IsIngedientAvailable(i, p).ShouldBeFalse();
        }
    }
}
