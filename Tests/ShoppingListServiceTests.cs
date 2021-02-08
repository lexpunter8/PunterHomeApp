using DataModels.Measurements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using PunterHomeDomain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestClass]
    public class ShoppingListServiceTests
    {
        private IShoppingListDataAdapter myShoopingListAdapter;
        private ShoppingListService myService;
        private IProductDataAdapter myProductAdapter;
        private IRecipeDataAdapter myRecipeAdapter;

        [TestInitialize]
        public void Initialize()
        {
            myShoopingListAdapter = Substitute.For<IShoppingListDataAdapter>();
            myProductAdapter = Substitute.For<IProductDataAdapter>();
            myRecipeAdapter = Substitute.For<IRecipeDataAdapter>();

            myService = new ShoppingListService(myShoopingListAdapter, myProductAdapter, myRecipeAdapter);
        }

        [TestMethod]
        public void test()
        {
            myShoopingListAdapter.GetItemsForShoppingList(Arg.Any<Guid>()).Returns(GetShoppingListItems());
            myProductAdapter.GetProductById(Arg.Any<Guid>()).Returns(myProduct);
            myShoopingListAdapter.GetInfoItemsForShoppingListItem(Arg.Any<Guid>()).Returns(GetInfoItems());
            var result = myService.GetShoppingListShopItems(Guid.NewGuid());
        }

        private List<ShoppingListItemInfoModel> GetInfoItems()
        {
            return new List<ShoppingListItemInfoModel>
            {
                new ShoppingListItemInfoModel
                {
                    Id = Guid.NewGuid(),
                    MeasurementAmount = 100,
                    MeasurementType = Enums.EUnitMeasurementType.Gr,
                    Reason = Enums.EShoppingListReason.Manual
                },
                new ShoppingListItemInfoModel
                {
                    Id = Guid.NewGuid(),
                    MeasurementAmount = 500,
                    MeasurementType = Enums.EUnitMeasurementType.Gr,
                    Reason = Enums.EShoppingListReason.Recipe,
                    RecipeItem = new RecipeShoppingListItemModel
                    {
                        NrOfPersons = 2,
                        RecipeId = Guid.NewGuid(),
                        RecipeName = "rec1",
                        ShoppingListItemId = Guid.NewGuid()
                    }
                },
            };
        }

        private List<ShoppingListItemModel> GetShoppingListItems()
        {
            return new List<ShoppingListItemModel>
            {
                new ShoppingListItemModel
                {
                    ProductId = myProduct.Id,
                    Id = Guid.NewGuid(),
                    IsChecked = false,
                    ProductName = myProduct.Name,
                    ShoppingListId = Guid.NewGuid()
                }
            };
        }

        private List<ShoppingListItemDetailsModel> GetDetailsItems()
        {
            return new List<ShoppingListItemDetailsModel>
            {
                new ShoppingListItemDetailsModel
                {
                    DynamicAmountAvailable = 500,
                    DynamicAmountRequested = 1000,
                    MeasurementType = Enums.EUnitMeasurementType.Gr,
                    StaticAmount = 300,
                    ProductName = "prod1",
                    ProductId = Guid.NewGuid(),

                }
            };
        }

        private ProductDetails myProduct = new ProductDetails
        {
            Id = Guid.NewGuid(),
            MeasurementAmounts = new MeasurementClassObject
            {
                Values = new List<MeasurementAmount>
                {
                    new MeasurementAmount
                    {
                        Amount = 300,
                        Type = Enums.EUnitMeasurementType.Gr
                    }
                }
            },
            Name = "prod1",
            ProductQuantities = new List<BaseMeasurement>{
                new Gram
                {
                    Barcode = string.Empty,
                    ProductQuantityId = 1,
                    UnitQuantityTypeVolume = 100
                },
                new KiloGram
                {
                    Barcode = string.Empty,
                    ProductQuantityId = 2,
                    UnitQuantityTypeVolume = 1
                },
                new Gram()
                {
                    Barcode = string.Empty,
                    ProductQuantityId = 2,
                    UnitQuantityTypeVolume = 250
                }
            },
        };
            
    }
}
