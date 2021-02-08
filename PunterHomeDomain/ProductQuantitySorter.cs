using DataModels.Measurements;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PunterHomeDomain
{
    class ProductQuantitySorter : IComparer<BaseMeasurement>
    {
        public int Compare([AllowNull] BaseMeasurement x, [AllowNull] BaseMeasurement y)
        {
            var xVal = x.ConvertTo(y.MeasurementType);
            var yVal = y.ConvertTo(y.MeasurementType);

            if (xVal == yVal)
            {
                return 0;
            }

            if (yVal > xVal)
            {
                return -1;
            }
            return 1;
        }
    }
}