using System.ComponentModel;
using System.Linq;

public class Enums
{
    public enum EUnitMeasurementType
    {
        [Description("Choose..")]
        None,
        [Description("Kilogram")]
        Kg,
        [Description("Gram")]
        Gr,
        [Description("Miligram")]
        Mg,
        [Description("Liter")]
        Liter,
        [Description("Deciliter")]
        Dl,
        [Description("Mililiter")]
        Ml,
        Cl,
    }

    public enum EMeasurementClass
    {
        Weight,
        Volume,
    }

    public static class Measurements
    {
        private static readonly EUnitMeasurementType[] _solidMeasermentTypes = { EUnitMeasurementType.Mg, EUnitMeasurementType.Gr, EUnitMeasurementType.Kg };
        private static readonly EUnitMeasurementType[] _liquidMeasermentTypes = { EUnitMeasurementType.Ml, EUnitMeasurementType.Dl, EUnitMeasurementType.Liter };
        public static EMeasurementClass GetMeasurementClassForEUnitQuantityType(EUnitMeasurementType type)
        {
            if (_solidMeasermentTypes.Contains(type))
            {
                return EMeasurementClass.Weight;
            }
            return EMeasurementClass.Volume;
        }

    }

}
