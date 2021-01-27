using System;
using System.Collections.Generic;
using static Enums;

namespace DataModels.Measurements
{
    public class MeasurementAmount 
    {
        public EUnitMeasurementType Type { get; set; }
        public double Amount { get; set; }

        public override string ToString()
        {
            return $"{Amount} {Type}";
        }
    }

    public class MeasurementClassObject
    {
        public List<MeasurementAmount> Values { get; set; } = new List<MeasurementAmount>();

        public void Add(MeasurementAmount newAmount)
        {
            double totalAmount = GetTotalAmount(newAmount.Type);

            totalAmount += newAmount.Amount;
            if (totalAmount < 0)
            {
                totalAmount = 0;
            }
            Values.Clear();
            Values.Add(new MeasurementAmount
            {
                Type = newAmount.Type,
                Amount = totalAmount
            });
        }

        public double GetTotalAmount(EUnitMeasurementType type)
        {
            double totalAmount = 0f;

            Values.ForEach(v =>
            {
                var baseM = BaseMeasurement.GetMeasurement(v.Type);
                baseM.UnitQuantityTypeVolume = v.Amount;
                totalAmount += baseM.ConvertTo(type);
            }
                );

            return totalAmount;
        }
    }
}
