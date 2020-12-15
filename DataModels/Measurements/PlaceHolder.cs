using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Enums;

namespace DataModels.Measurements
{


    public class BaseMeasurement
    {
        public BaseMeasurement(EUnitMeasurementType measurementType)
        {
            MeasurementType = measurementType;
        }

        public int ProductQuantityId { get; set; }
        public EUnitMeasurementType MeasurementType { get; }
        public double UnitQuantityTypeVolume { get; set; }
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

            var total = UnitQuantityTypeVolume * Quantity;
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

}
