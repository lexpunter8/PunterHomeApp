using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using PunterHomeDomain;
using PunterHomeDomain.ApiModels;
using PunterHomeDomain.Conversions;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
using static Enums;

namespace PunterHomeApp.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeDataAdapter recipeAdapter;
        private readonly IProductDataAdapter productDataAdapter;

        public RecipeService(IRecipeDataAdapter recipeAdapter, IProductDataAdapter productDataAdapter)
        {
            this.recipeAdapter = recipeAdapter;
            this.productDataAdapter = productDataAdapter;
        }

        public bool AddIngredient(Guid recipeId, IIngredient ingredient)
        {
            recipeAdapter.AddIngredient(recipeId, ingredient);
            return true;
        }

        public IEnumerable<RecipeApiModel> GetAllRecipes()
        {
            return recipeAdapter.GetAllRecipes().Result;
        }

        public void CreateRecipe(string recipeName)
        {
            recipeAdapter.SaveRecipe(new RecipeApiModel
            {
                Name = recipeName
            });
        }

        public void DeleteRecipeById(Guid id)
        {
            recipeAdapter.DeleteById(id);
        }

        public void UpdateRecipe(Guid id, string newName)
        {
            recipeAdapter.UpdateRecipe(id, newName);
        }

        public async Task<RecipeDetailsApiModel> GetRecipeSummaryDetails(Guid recipeId)
        {
            var recipe = recipeAdapter.GetRecipeById(recipeId);
            IEnumerable<Product> products = await productDataAdapter.GetProducts();

            var recipeDetails = new RecipeDetailsApiModel
            {
                Id = recipe.Id,
                Ingredients = recipe.Ingredients.Select(s => {
                        var r = IngredientApiModelIngredientConversion.Convert(s);
                        r.IsAvaliable = CalculateIngedientAvailability(s, products.FirstOrDefault(i => i.Id == s.ProductId));
                        return r;
                    }).ToList(),
                Name = recipe.Name,
                Steps = recipe.Steps.ToList(),
            };
            return recipeDetails;
        }

        public bool CalculateIngedientAvailability(Ingredient i, Product p)
        {
            bool productHasSameMeasurementClass = p.ProductQuantities.Any(pq => Measurements.GetMeasurementClassForEUnitQuantityType(pq.MeasurementType) == Measurements.GetMeasurementClassForEUnitQuantityType(i.UnitQuantityType));

            if (!productHasSameMeasurementClass)
            {
                return false;
            }

            return GetTotalProductQuantitiesToMeasuremenType(p.ProductQuantities, i.UnitQuantityType) >= i.UnitQuantity;
        }

        private double GetTotalProductQuantitiesToMeasuremenType(IEnumerable<BaseMeasurement> productQuantities, EUnitMeasurementType type)
        {
            var total = 0.0;
            foreach(var pq in productQuantities)
            {
                total += pq.ConvertTo(type);
            }
            return total;
        }
    }


    public class BaseMeasurement
    {
        public BaseMeasurement(EUnitMeasurementType measurementType)
        {
            MeasurementType = measurementType;
        }

        public int ProductQuantityId { get; set; }
        public EUnitMeasurementType MeasurementType { get; }
        public int UnitQuantityTypeVolume { get; set; }
        public int Quantity { get; set; }

        public virtual double ConvertTo(EUnitMeasurementType measurementType)
        {
            return 0f;
        }

        public static BaseMeasurement GetMeasurement(EUnitMeasurementType measurementType)
        {
            switch (measurementType)
            {
                case EUnitMeasurementType.Liter:
                    return new Liter();
                case EUnitMeasurementType.Ml:
                    return new MiliLiter();
                case EUnitMeasurementType.Dl:
                    return new DeciLiter();
                case EUnitMeasurementType.Kg:
                    return new KiloGram();
                case EUnitMeasurementType.Gr:
                    return new Gram();
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class MiliLiter : BaseMeasurement
    {
        public MiliLiter() : base(EUnitMeasurementType.Ml)
        {
        }

        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            if (!new[] { EUnitMeasurementType.Dl, EUnitMeasurementType.Ml }.Contains(measurementType))
            {
                return -1;
            }

            var total = UnitQuantityTypeVolume* Quantity;
            switch (measurementType)
            {
                case EUnitMeasurementType.Liter:
                    return total / 1000;
                case EUnitMeasurementType.Dl:
                    return total / 100;
                case EUnitMeasurementType.Cl:
                    return total / 10;
                case EUnitMeasurementType.Ml:
                    return total;
                default:
                    return 0;
            }
        }
    }

    public class KiloGram : BaseMeasurement
    {
        public KiloGram() : base(EUnitMeasurementType.Kg)
        {
        }
        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            if (!new[] { EUnitMeasurementType.Kg, EUnitMeasurementType.Gr }.Contains(measurementType))
            {
                return 0;
            }

            var total = UnitQuantityTypeVolume * Quantity;
            switch (measurementType)
            {
                case EUnitMeasurementType.Gr:
                    return total * 1000;
                case EUnitMeasurementType.Kg:
                    return total;
                default:
                    return 0;
            }
        }
    }
    public class Gram : BaseMeasurement
    {
        public Gram() : base(EUnitMeasurementType.Gr)
        {
        }

        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            if (!new[] { EUnitMeasurementType.Kg, EUnitMeasurementType.Gr }.Contains(measurementType))
            {
                return 0;
            }

            var total = UnitQuantityTypeVolume * Quantity;
            switch (measurementType)
            {
                case EUnitMeasurementType.Gr:
                    return total;
                case EUnitMeasurementType.Kg:
                    return total / 1000;
                default:
                    return 0;
            }
        }
    }
    public class DeciLiter : BaseMeasurement
    {
        public DeciLiter() : base(EUnitMeasurementType.Dl)
        {
        }
        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            if (!new[] { EUnitMeasurementType.Dl, EUnitMeasurementType.Ml }.Contains(measurementType))
            {
                return -1;
            }

            var total = UnitQuantityTypeVolume * Quantity;
            switch (measurementType)
            {
                case EUnitMeasurementType.Liter:
                    return total / 10;
                case EUnitMeasurementType.Dl:
                    return total;
                case EUnitMeasurementType.Cl:
                    return total * 10;
                case EUnitMeasurementType.Ml:
                    return total * 100;
                default:
                    return -1;
            }
        }
    }


    public class Liter : BaseMeasurement
    {
        public Liter() : base(EUnitMeasurementType.Liter)
        {
        }

        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            if (!new[] { EUnitMeasurementType.Dl, EUnitMeasurementType.Ml }.Contains(measurementType))
            {
                return -1;
            }

            var total = UnitQuantityTypeVolume * Quantity;
            switch (measurementType)
            {
                case EUnitMeasurementType.Liter:
                    return total;
                case EUnitMeasurementType.Dl:
                    return total * 10;
                case EUnitMeasurementType.Cl:
                    return total * 100;
                case EUnitMeasurementType.Ml:
                    return total * 1000;
                default:
                    return -1;
            }
        }
    }

    public interface IRecipe
    {
        string Name { get; set; }
        IEnumerable<RecipeStep> Steps { get; set; }
        IEnumerable<Ingredient> Ingredients { get; set; }
        Guid Id { get; set; }
    }

    public interface IIngredient
    {
        public Guid ProductId { get; set; }
        public Guid RecipeId { get; set; }
        public string ProductName { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }

    }
}
