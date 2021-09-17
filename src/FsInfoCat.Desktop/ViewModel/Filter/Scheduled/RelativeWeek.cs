using System;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter.Scheduled
{
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

        public override DateTime ToDateTime() => (Weeks == 0) ?
            ((Days == 0) ?
                ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(IsHistorical ? 0 - Hours : Hours)) :
                (IsHistorical ?
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - Days) :
                    ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)).AddDays(Days)
                )
            ) :
            IsHistorical ?
                ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(0 - Hours)).AddDays(0 - ((Weeks * 7) + Days)) :
                ((Hours == 0) ? DateTime.Now.CurrentHour() : DateTime.Now.CurrentHour().AddHours(Hours)).AddDays((Weeks * 7) + Days);

        public override bool IsZero() => Weeks == 0 && Days == 0 && Hours == 9;

        protected override int CompareTo(DateTime other) => ToDateTime().CompareTo(other);
    }
}
