using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter.Scheduled
{
    public abstract class SchedulableTimeReference : TimeReference
    {
    }

    public class Absolute : SchedulableTimeReference, IAbsoluteTimeReference
    {
        #region Value Property Members

        /// <summary>
        /// Identifies the <see cref="Value"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyPropertyBuilder<Absolute, DateTime>
            .Register(nameof(Value))
            .AsReadWrite();

        public DateTime Value { get => (DateTime)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

        #endregion

        protected override int CompareTo(DateTime other) => Value.CompareTo(other);
    }

    public abstract class RelativeScheduleTime : SchedulableTimeReference, IRelativeTimeReference
    {
        #region IsHistorical Property Members

        /// <summary>
        /// Identifies the <see cref="IsHistorical"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHistoricalProperty = DependencyPropertyBuilder<RelativeScheduleTime, bool>
            .Register(nameof(IsHistorical))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IsHistorical { get => (bool)GetValue(IsHistoricalProperty); set => SetValue(IsHistoricalProperty, value); }

        #endregion
        #region Days Property Members

        /// <summary>
        /// Identifies the <see cref="Days"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DaysProperty = DependencyPropertyBuilder<RelativeScheduleTime, int>
            .Register(nameof(Days))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as RelativeScheduleTime)?.OnDaysPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int Days { get => (int)GetValue(DaysProperty); set => SetValue(DaysProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Days"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Days"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Days"/> property.</param>
        protected virtual void OnDaysPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0)
                ErrorInfo.SetError($"{nameof(Days)} cannot be negative.", nameof(Days));
            else
                ErrorInfo.ClearErrors(nameof(Days));
        }

        #endregion
        #region Hours Property Members

        /// <summary>
        /// Identifies the <see cref="Hours"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HoursProperty = DependencyPropertyBuilder<RelativeScheduleTime, int>
            .Register(nameof(Hours))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as RelativeScheduleTime)?.OnHoursPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int Hours { get => (int)GetValue(HoursProperty); set => SetValue(HoursProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Hours"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Hours"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Hours"/> property.</param>
        protected virtual void OnHoursPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0 || newValue > 23)
                ErrorInfo.SetError($"{nameof(Hours)} cannot be negative or greater than 23.", nameof(Hours));
            else
                ErrorInfo.ClearErrors(nameof(Hours));
        }

        #endregion
    }

    public class RelativeWeek : RelativeScheduleTime, IRelativeWeekReference
    {
        #region Weeks Property Members

        /// <summary>
        /// Identifies the <see cref="Weeks"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WeeksProperty = DependencyPropertyBuilder<RelativeWeek, int>
            .Register(nameof(Weeks))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as RelativeWeek)?.OnWeeksPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public int Weeks { get => (int)GetValue(WeeksProperty); set => SetValue(WeeksProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Weeks"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Weeks"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Weeks"/> property.</param>
        protected virtual void OnWeeksPropertyChanged(int oldValue, int newValue)
        {
            if (newValue < 0)
                ErrorInfo.SetError($"{nameof(Weeks)} cannot be negative.", nameof(Weeks));
            else
                ErrorInfo.ClearErrors(nameof(Weeks));
        }

        #endregion

        protected override int CompareTo(DateTime other)
        {
            throw new NotImplementedException();
        }
    }

    public class Relative : RelativeScheduleTime, IRelativeMonthReference
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

        protected override int CompareTo(DateTime other)
        {
            throw new NotImplementedException();
        }
    }

    public class Range : TimeRange<SchedulableTimeReference>, IScheduleTimeRange
    {
    }
}
