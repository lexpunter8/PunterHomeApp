using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Shared;
using PunterHomeDomain.ShoppingList;
using System;
using System.Linq;

namespace PunterHomeDomainTests
{
    [TestClass]
    public class UnitTest1
    {
        private ShoppingListAggregate myShoppingList;

        [TestInitialize]
        public void Initialize()
        {

            myShoppingList = ShoppingListAggregate.CreateNew("Test");

        }

        [TestMethod]
        public void TestMethod1()
        {
            Guid productId = Guid.NewGuid();
            myShoppingList.AddProductItem(productId, 200, EUnitMeasurementType.Gr);
            myShoppingList.AddProductItem(productId, 200, EUnitMeasurementType.Gr);

            myShoppingList.ProductItems.First(f => f.ProductId == productId).Amount.Should().Be(400);
            myShoppingList.ProductItems.First(f => f.ProductId == productId).MeasurementType.Should().Be((int)EUnitMeasurementType.Gr);
        }

        [TestMethod]
        public void Add_Existing_ProductWithDifferentMeasurementType()
        {
            Guid productId = Guid.NewGuid();
            myShoppingList.AddProductItem(productId, 200, EUnitMeasurementType.Gr);
            myShoppingList.AddProductItem(productId, 1, EUnitMeasurementType.Kg);

            myShoppingList.ProductItems.First(f => f.ProductId == productId).Amount.Should().Be(1200);
            myShoppingList.ProductItems.First(f => f.ProductId == productId).MeasurementType.Should().Be((int)EUnitMeasurementType.Gr);
        }
    }
}
