using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter.Historical
{
    public class Relative : RelativeHistoricalTime, IRelativeMonthReference
    {
        #region Years Property Members

        /// <summary>
        /// Identifies the <see cref="Years"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty YearsProperty = DependencyPropertyBuilder<Relative, int>
            .Register(nameof(Years))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as Relative)?.OnYearsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int Years { get => (int)GetValue(YearsProperty); set => SetValue(YearsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Years"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Years"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Years"/> property.</param>
        protected virtual void OnYearsPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0)
                ErrorInfo.SetError($"{nameof(Years)} cannot be negative.", nameof(Years));
            else
                ErrorInfo.ClearErrors(nameof(Years));
        }

        #endregion
        #region Months Property Members

        /// <summary>
        /// Identifies the <see cref="Months"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MonthsProperty = DependencyPropertyBuilder<Relative, int>
            .Register(nameof(Months))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as Relative)?.OnMonthsPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int Months { get => (int)GetValue(MonthsProperty); set => SetValue(MonthsProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Months"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Months"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Months"/> property.</param>
        protected virtual void OnMonthsPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0 || newValue > 23)
                ErrorInfo.SetError($"{nameof(Months)} cannot be negative or greater than 23.", nameof(Months));
            else
                ErrorInfo.ClearErrors(nameof(Months));
        }

        #endregion

        public override DateTime ToDateTime() => (Years == 0) ?
            ((Months == 0) ?
                ((Days == 0) ?
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                ) :
                ((Days == 0) ?
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                ).AddMonths(0 - Months)
            ) :
            ((Months == 0) ?
                ((Days == 0) ?
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                ) :
                ((Days == 0) ?
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)) :
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days)
                ).AddMonths(0 - Months)
            ).AddYears(0 - Years);

        public override bool IsZero() => Years == 0 && Months == 0 && Days == 0 && Hours == 9;

        protected override int CompareTo(DateTime other) => ToDateTime().CompareTo(other);
    }
}
