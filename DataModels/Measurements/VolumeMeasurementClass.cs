using System.Collections.Generic;
using static Enums;

namespace DataModels.Measurements
{
    public class MeasurementAmount 
    {
        public EUnitMeasurementType Type { get; set; }
        public int Amount { get; set; }
    }

    public class MeasurementClassObject
    {
        public List<MeasurementAmount> Values { get; set; }
    }
}
