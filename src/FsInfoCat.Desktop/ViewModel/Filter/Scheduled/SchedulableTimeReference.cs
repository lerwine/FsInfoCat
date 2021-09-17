using System;

namespace FsInfoCat.Desktop.ViewModel.Filter.Scheduled
{
    public abstract class SchedulableTimeReference : TimeReference
    {
        internal static bool AreSame(SchedulableTimeReference x, SchedulableTimeReference y)
        {
            if (x is null)
                return y is null || (y is RelativeScheduleTime rst && rst.IsZero());
            if (y is null)
                return x is RelativeScheduleTime rst && rst.IsZero();
            if (x is RelativeScheduleTime rsx)
                return y is RelativeScheduleTime rsy && rsx.ToDateTime().Equals(rsy.ToDateTime());
            return (x is Absolute xa) ? y is Absolute ya && xa.Value == ya.Value : y is not Absolute;
        }
    }
}
