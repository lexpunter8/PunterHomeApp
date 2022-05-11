using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PunterHomeDomain.Shared;
using PunterHomeDomain.ShoppingList;
using System;
using System.Linq;

namespace PunterHomeDomainTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //var a = ShoppingListAggregate.CreateNew("Test");

            //var pGuid = Guid.NewGuid();
            //a.AddProductItem(pGuid, new Liter().AddMeasurementAmount(1));
            //a.AddProductItem(pGuid, new MiliLiter().AddMeasurementAmount(200));

            //a.ProductItems.Count.Should().Be(1);

            //a.ProductItems.First().MeasurementType.Should().Be(PunterHomeDomain.Enums.EUnitMeasurementType.Liter);
            //a.ProductItems.First().Amount.Should().Be(1.2);
        }
    }
}
