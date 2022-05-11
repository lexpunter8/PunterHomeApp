using PunterHomeApp.Services;
using PunterHomeDomain.Interfaces;
using System;
using static Enums;

namespace PunterHomeDomain.Models
{
    public class RecipeStepIngredient
    {
        public Guid ProductId { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public Guid RecipeStepId { get; set; }

        public string ProductName { get; set; }
    }
    public class Ingredient : IIngredient
    {
        public Guid ProductId { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public Guid RecipeId { get; set; }
        public string ProductName { get; set; }
    }

    public class IngredientAggregate : IAggregateRoot
    {
        public Guid ProductId { get; set; }
        public int UnitQuantity { get; set; }
        public EUnitMeasurementType UnitQuantityType { get; set; }
        public Guid RecipeId { get; set; }
        public string ProductName { get; set; }
    }

    public interface IMeaserumentClass
    {
        double ConvertTo(EUnitMeasurementType measurementType);
    }

    //public class Converter
    //{
    //    public BaseMeasurement Convert(ProductQuantity productQuantity)
    //    {

    //    }
    //    public static BaseMeasurement GetMeasurementForEUnitQuantityType(EMeasurementClass type)
    //    {
    //        if (type == EMeasurementClass.Volume)
    //        {
    //            return EMeasurementClass.Weight;
    //        }
    //        return EMeasurementClass.Volume;
    //    }
    //}

    //public abstract class BaseMeasurement : IMeaserumentClass
    //{
    //    public BaseMeasurement(ProductQuantity[] productQuantities)
    //    {
    //        ProductQuantities = productQuantities;
    //    }

    //    public ProductQuantity[] ProductQuantities { get; }

    //    public virtual double ConvertTo(EUnitMeasurementType measurementType)
    //    {
    //        return 0f;
    //    }

    //    public bool Has(IEnumerable<ProductQuantity> productQuantities, )
    //    {

    //    }
    //}

    //public class VolumeMeasurement : BaseMeasurement
    //{
    //    public VolumeMeasurement(ProductQuantity[] productQuantities) : base(productQuantities)
    //    {
    //    }

    //    public int Liter { get; set; }
    //    public int DeciLiter { get; set; }
    //    public int MiliLiter { get; set; }

    //    public override double ConvertTo(EUnitMeasurementType measurementType)
    //    {
    //        if (!new[] { EUnitMeasurementType.Dl, EUnitMeasurementType.Ml }.Contains(measurementType))
    //        {
    //            return -1;
    //        }

    //        var total = ProductQuantity.Quantity * ProductQuantity.UnitQuantityTypeVolume;
    //        switch (measurementType)
    //        {
    //            case EUnitMeasurementType.Liter:
    //                return total;
    //            case EUnitMeasurementType.Dl:
    //                return total / 10;
    //            case EUnitMeasurementType.Cl:
    //                return total / 100;
    //            case EUnitMeasurementType.Ml:
    //                return total / 1000;
    //            default:
    //                return -1;
    //        }
    //    }
    //}

    //public class MiliLiter : BaseMeasurement
    //{
    //    public MiliLiter(ProductQuantity productQuantity) : base(productQuantity)
    //    {
    //    }

    //    public override double ConvertTo(EUnitMeasurementType measurementType)
    //    {
    //        if (!new[] {EUnitMeasurementType.Dl, EUnitMeasurementType.Ml }.Contains(measurementType))
    //        {
    //            return -1;
    //        }

    //        var total = ProductQuantity.Quantity * ProductQuantity.UnitQuantityTypeVolume;
    //        switch (measurementType)
    //        {
    //            case EUnitMeasurementType.Liter:
    //                return total / 1000;
    //            case EUnitMeasurementType.Dl:
    //                return total / 100;
    //            case EUnitMeasurementType.Cl:
    //                return total * 10;
    //            case EUnitMeasurementType.Ml:
    //                return total;
    //            default:
    //                return -1;
    //        }
    //    }
    //}
}
