using System;

namespace FsInfoCat.ExpressionFilter.Historical
{
    public abstract class HistoricalTimeReference : TimeReference
    {
        internal static bool AreSame(HistoricalTimeReference x, HistoricalTimeReference y)
        {
            if (x is null)
                return y is null || (y is RelativeHistoricalTime rst && rst.IsZero());
            if (y is null)
                return x is RelativeHistoricalTime rst && rst.IsZero();
            if (x is RelativeHistoricalTime rsx)
                return y is RelativeHistoricalTime rsy && rsx.ToDateTime().Equals(rsy.ToDateTime());
            return (x is Absolute xa) ? y is Absolute ya && xa.Value == ya.Value : y is not Absolute;
        }
    }
}
