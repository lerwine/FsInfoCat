using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter.Scheduled
{
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
            .OnChanged((d, oldValue, newValue) => (d as RelativeScheduleTime)?.OnDaysPropertyChanged(newValue))
            .AsReadWrite();

        public int Days { get => (int)GetValue(DaysProperty); set => SetValue(DaysProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Days"/> dependency property has changed.
        /// </summary>
        /// <param name="newValue">The new value of the <see cref="Days"/> property.</param>
        protected virtual void OnDaysPropertyChanged(int newValue)
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

        public abstract bool IsZero();
    }
}
