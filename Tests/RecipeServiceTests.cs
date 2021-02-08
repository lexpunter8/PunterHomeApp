using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PunterHomeApp.Services;
using PunterHomeDomain;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestClass]
    public class RecipeServiceTests
    {
        private IRecipeDataAdapter myRecipeDataAdapter;
        private IProductDataAdapter myProductAdapter;
        private IShoppingListService myShoppingListService;
        private RecipeService myService;

        [TestInitialize]
        public void Init()
        {
            myRecipeDataAdapter = NSubstitute.Substitute.For<IRecipeDataAdapter>();
            myProductAdapter = NSubstitute.Substitute.For<IProductDataAdapter>();
            myShoppingListService = NSubstitute.Substitute.For<IShoppingListService>();
            myService = new RecipeService(myRecipeDataAdapter, myProductAdapter, myShoppingListService);

        }

        [TestMethod]
        public void When_UpdateRecipeStep_ThenGetRecipeStepsIsCalled()
        {
            myRecipeDataAdapter.GetStepForRecipe(Arg.Any<Guid>()).Returns(GetSteps());
            var step = new RecipeStep
            {
                Id = GetSteps()[1].Id,
                Order = 1,
                Text = "Step 1"
            };

            myService.UpdateStep(step);

            myRecipeDataAdapter.Received().GetStepForRecipe(Arg.Any<Guid>());
        }

        [TestMethod]
        public void When_UpdateRecipeStep_ThenOrderIsSetToNewOrder()
        {
            var steps = GetSteps();
            myRecipeDataAdapter.GetStepForRecipe(Arg.Any<Guid>()).Returns(steps);
            var step = new RecipeStep
            {
                Id = steps[1].Id,
                Order = 3,
                Text = "Step 2"
            };

            myService.UpdateStep(step);
            steps[1].Order.Should().Be(3);
        }

        [TestMethod]
        public void When_UpdateRecipeStepHighToLow_Then()
        {
            var steps = GetSteps();
            myRecipeDataAdapter.GetStepForRecipe(Arg.Any<Guid>()).Returns(steps);
            var step = new RecipeStep
            {
                Id = steps[0].Id,
                Order = 4,
            };

            var expectedResult = new Dictionary<Guid, int>
            {
                { myStepId0, 4 },
                { myStepId1, 1 },
                { myStepId2, 2 },
                { myStepId3, 3 }
            };

            myService.UpdateStep(step);


            foreach (var item in steps)
            {
                myRecipeDataAdapter.Received().UpdateStep(item.Id, null, expectedResult[item.Id]);
            }
        }

        [TestMethod]
        public void When_UpdateRecipeStepLowToHight_Then()
        {
            var steps = GetSteps();
            myRecipeDataAdapter.GetStepForRecipe(Arg.Any<Guid>()).Returns(steps);
            var step = new RecipeStep
            {
                Id = steps[3].Id,
                Order = 1
            };

            myService.UpdateStep(step);

            var expectedResult = new Dictionary<Guid, int>
            {
                { myStepId0, 2 },
                { myStepId1, 3 },
                { myStepId2, 4 },
                { myStepId3, 1 }
            };

            foreach (var item in steps)
            {
                myRecipeDataAdapter.Received().UpdateStep(item.Id, null, expectedResult[item.Id]);
            }
        }

        private Guid myStepId0 = Guid.Parse("3f2d6717-016a-4792-bd5b-70eb6c1d2e81");
        private Guid myStepId1 = Guid.Parse("3f2d6717-016a-4792-bd5b-70eb6c1d2e82");
        private Guid myStepId2 = Guid.Parse("3f2d6717-016a-4792-bd5b-70eb6c1d2e83");
        private Guid myStepId3 = Guid.Parse("3f2d6717-016a-4792-bd5b-70eb6c1d2e84");
        private List<RecipeStep> GetSteps()
        {
            return new List<RecipeStep>
            {
                new RecipeStep
                {
                    Id = myStepId0,
                    Order = 1,
                    Text = "Step 1"
                },
                new RecipeStep
                {
                    Id = myStepId1,
                    Order = 2,
                    Text = "Step 2"
                },
                new RecipeStep
                {
                    Id = myStepId2,
                    Order = 3,
                    Text = "Step 3"
                },
                new RecipeStep
                {
                    Id = myStepId3,
                    Order = 4,
                    Text = "Step 4"
                },
            };
        }
    }
}
