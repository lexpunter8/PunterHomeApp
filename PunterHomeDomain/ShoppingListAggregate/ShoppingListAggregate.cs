using DataModels.Measurements;
using PunterHomeDomain.Enums;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Enums;
using BaseMeasurement = PunterHomeDomain.Shared.BaseMeasurement;
using EUnitMeasurementType = PunterHomeDomain.Enums.EUnitMeasurementType;

namespace PunterHomeDomain.ShoppingList
{
    public partial class ShoppingListAggregate : IAggregateRoot
    {
        private List<ShoppingListProductItem> myProductItems;
        private List<ShoppingListRecipeItem> myRecipeItems;
        private List<ShoppingListTextItem> myTextItems;

        private ShoppingListAggregate()
        {
            myTextItems = new List<ShoppingListTextItem>();
            myRecipeItems = new List<ShoppingListRecipeItem>();
            myProductItems = new List<ShoppingListProductItem>();
        }
        public ShoppingListAggregate(Guid id, string name, EShoppingListStatus status, DateTime createTime) : this()
        {
            Id = id;
            Name = name;
            Status = status;
            CreateTime = createTime;
        }


        public IReadOnlyCollection<ShoppingListRecipeItem> RecipeItems => myRecipeItems;
        public IReadOnlyCollection<ShoppingListTextItem> TextItems => myTextItems;
        public IReadOnlyCollection<ShoppingListProductItem> ProductItems => myProductItems;

        public Guid Id { get; }
        public string Name { get; private set; }
        public EShoppingListStatus Status { get; private set; }
        public DateTime CreateTime { get; private set; }

        public void AddRecipeItem(Guid recipeId, int amount = 1)
        {
            var existing = myRecipeItems.FirstOrDefault(a => a.RecipeId == recipeId);
            if (existing != null)
            {
                existing.Amount += amount;
                return;
            }

            myRecipeItems.Add(new ShoppingListRecipeItem
            {
                RecipeId = recipeId,
                Amount = amount
            });
        }

        public void AddProductItem(Guid productId, double amount, EUnitMeasurementType measurementType)
        {
            var existing = myProductItems.FirstOrDefault(a => a.ProductId == productId);
            if (existing != null && existing.MeasurementType == (int)measurementType)
            {
                existing.Amount += amount;
                return;
            }

            // exists but different measurement type
            if (existing != null)
            {
                var newMeasurement = BaseMeasurement.GetMeasurement(measurementType);
                newMeasurement.AddMeasurementAmount(amount);

                double convertedNewAmount = newMeasurement.ConvertTo((EUnitMeasurementType)existing.MeasurementType);
                existing.Amount += convertedNewAmount;
                return;
            }

            myProductItems.Add(new ShoppingListProductItem
            {
                Amount = amount,
                ProductId = productId,
                MeasurementType = (int)measurementType
            });
        }

        public void AddTextItem(string text)
        {
            var existing = myTextItems.FirstOrDefault(a => a.Value == text);
            if (existing != null || string.IsNullOrEmpty(text))
            {

                return;
            }

            myTextItems.Add(new ShoppingListTextItem
            {
                Value = text
            });
        }


        public void RemoveTextItem(string text)
        {
            var existing = myTextItems.FirstOrDefault(a => a.Value == text);
            if (existing == null)
            {
                // TODO thow some exception
                return;
            }

            myTextItems.Remove(existing);
        }

        public void StartShopping()
        {
            Status = EShoppingListStatus.Shopping;
        }

        public void RemoveRecipe(Guid recipeId)
        {
            myRecipeItems.RemoveAll(r => r.RecipeId == recipeId);
        }

        public void RemoveProduct(Guid productId)
        {
            myProductItems.RemoveAll(r => r.ProductId == productId);
        }

        public void CheckItem(string item, bool isChecked)
        {
            var existing = myTextItems.FirstOrDefault(a => a.Value == item);
            if (existing == null)
            {
                // TODO thow some exception
                return;
            }

            existing.IsChecked = isChecked;
        }

        public void CheckItem(Guid productId, bool isChecked)
        {
            var existing = myProductItems.FirstOrDefault(a => a.ProductId == productId);
            if (existing == null)
            {
                // TODO thow some exception
                return;
            }

            existing.IsChecked = isChecked;
        }

        public void FinishShoppingList()
        {
            myProductItems.Clear();
            myTextItems.Clear();

            Status = EShoppingListStatus.Closed;
        }
    }

    public class ShoppingListTextItem
    {
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }
    
    public class ShoppingListRecipeItem
    {
        public Guid RecipeId { get; set; }
        public int Amount { get; set; }
    }

    public class ShoppingListProductItem
    {
        public Guid ProductId { get; set; }
        public double Amount { get; set; }
        public int MeasurementType { get; set; }
        public bool IsChecked { get; set; }
    }
}
