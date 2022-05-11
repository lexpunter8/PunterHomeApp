using PunterHomeDomain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PunterHomeDomain.Shared
{
    public class BaseMeasurement
    {
        public BaseMeasurement(EUnitMeasurementType measurementType)
        {
            MeasurementType = measurementType;
        }

        public string Barcode { get; set; }
        public int ProductQuantityId { get; set; }
        public EUnitMeasurementType MeasurementType { get; }
        public double UnitQuantityTypeVolume { get; set; }

        public BaseMeasurement AddMeasurementAmount(double amount)
        {
            UnitQuantityTypeVolume += amount;
            return this;
        }

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
                case EUnitMeasurementType.Mg:
                    return new MiliGram();
                case EUnitMeasurementType.Piece:
                    return new Piece();
                default:
                    throw new NotImplementedException();
            }
        }

        public static BaseMeasurement GetMeasurement(BaseMeasurement m)
        {
            BaseMeasurement baseMeasurement;
            switch (m.MeasurementType)
            {
                case EUnitMeasurementType.Liter:
                    baseMeasurement = new Liter();
                    break;
                case EUnitMeasurementType.Ml:
                    baseMeasurement = new MiliLiter();
                    break;
                case EUnitMeasurementType.Dl:
                    baseMeasurement = new DeciLiter();
                    break;
                case EUnitMeasurementType.Kg:
                    baseMeasurement = new KiloGram();
                    break;
                case EUnitMeasurementType.Gr:
                    baseMeasurement = new Gram();
                    break;
                case EUnitMeasurementType.Mg:
                    baseMeasurement = new MiliGram();
                    break;
                case EUnitMeasurementType.Piece:
                    baseMeasurement = new Piece();
                    break;
                default:
                    throw new NotImplementedException();
            }
            baseMeasurement.ProductQuantityId = m.ProductQuantityId;
            baseMeasurement.UnitQuantityTypeVolume = m.UnitQuantityTypeVolume;
            baseMeasurement.Barcode = m.Barcode;
            return baseMeasurement;
        }
    }

    public class Piece : BaseMeasurement
    {
        public Piece() : base(EUnitMeasurementType.Piece)
        {
        }

        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            return UnitQuantityTypeVolume;
        }
    }

    public class MiliLiter : BaseMeasurement
    {
        public MiliLiter() : base(EUnitMeasurementType.Ml)
        {
        }

        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            EUnitMeasurementType[] items = new[] { EUnitMeasurementType.Dl, EUnitMeasurementType.Ml, EUnitMeasurementType.Liter, EUnitMeasurementType.Cl };
            if (!items.Contains(measurementType))
            {
                return -1;
            }

            var total = UnitQuantityTypeVolume;
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
            if (!new[] { EUnitMeasurementType.Kg, EUnitMeasurementType.Gr, EUnitMeasurementType.Mg }.Contains(measurementType))
            {
                return 0;
            }

            var total = UnitQuantityTypeVolume;
            switch (measurementType)
            {
                case EUnitMeasurementType.Mg:
                    return total * 1000000;
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
            if (!new[] { EUnitMeasurementType.Kg, EUnitMeasurementType.Gr, EUnitMeasurementType.Mg }.Contains(measurementType))
            {
                return 0;
            }

            var total = UnitQuantityTypeVolume;
            switch (measurementType)
            {
                case EUnitMeasurementType.Mg:
                    return total * 1000;
                case EUnitMeasurementType.Gr:
                    return total;
                case EUnitMeasurementType.Kg:
                    return total / 1000;
                default:
                    return 0;
            }
        }
    }


    public class MiliGram : BaseMeasurement
    {
        public MiliGram() : base(EUnitMeasurementType.Mg)
        {
        }

        public override double ConvertTo(EUnitMeasurementType measurementType)
        {
            if (!new[] { EUnitMeasurementType.Kg, EUnitMeasurementType.Gr, EUnitMeasurementType.Mg }.Contains(measurementType))
            {
                return 0;
            }

            var total = UnitQuantityTypeVolume;
            switch (measurementType)
            {
                case EUnitMeasurementType.Mg:
                    return total;
                case EUnitMeasurementType.Gr:
                    return total / 1000;
                case EUnitMeasurementType.Kg:
                    return total / 1000000;
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

            var total = UnitQuantityTypeVolume;
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

            var total = UnitQuantityTypeVolume;
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
