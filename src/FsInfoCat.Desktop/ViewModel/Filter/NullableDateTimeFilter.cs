using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class NullableDateTimeFilter<TEntity> : NullableValueFilter<TEntity, DateTime>
        where TEntity : class
    {
        #region PeriodType Property Members

        /// <summary>
        /// Identifies the <see cref="PeriodType"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeriodTypeProperty = DependencyPropertyBuilder<NullableDateTimeFilter<TEntity>, RelativePeriodType>
            .Register(nameof(PeriodType))
            .DefaultValue(RelativePeriodType.Weeks)
            .OnChanged((d, oldValue, newValue) => (d as NullableDateTimeFilter<TEntity>)?.OnPeriodTypePropertyChanged(newValue))
            .AsReadWrite();

        public RelativePeriodType PeriodType { get => (RelativePeriodType)GetValue(PeriodTypeProperty); set => SetValue(PeriodTypeProperty, value); }

        private void OnPeriodTypePropertyChanged(RelativePeriodType newValue) => OnPeriodChanged(Period, Days, newValue);

        #endregion
        #region Period Property Members

        /// <summary>
        /// Identifies the <see cref="Period"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PeriodProperty = DependencyPropertyBuilder<NullableDateTimeFilter<TEntity>, int>
            .Register(nameof(Period))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as NullableDateTimeFilter<TEntity>)?.OnPeriodPropertyChanged(newValue))
            .AsReadWrite();

        public int Period { get => (int)GetValue(PeriodProperty); set => SetValue(PeriodProperty, value); }

        private void OnPeriodPropertyChanged(int newValue) => OnPeriodChanged(newValue, Days, PeriodType);

        private void OnPeriodChanged(int period, int days, RelativePeriodType type)
        {
            if (period < 0)
                ErrorInfo.SetError((type == RelativePeriodType.Months) ? FsInfoCat.Properties.Resources.ErrorMessage_InvalidMonths : FsInfoCat.Properties.Resources.ErrorMessage_InvalidWeeks, nameof(Period));
            else
                ErrorInfo.ClearErrors(nameof(Period));
            if (days < 0 || days > ((type == RelativePeriodType.Months) ? 31 : 6))
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidDays, nameof(Days));
            else
                ErrorInfo.ClearErrors(nameof(Days));
        }

        #endregion
        #region Days Property Members

        /// <summary>
        /// Identifies the <see cref="Days"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DaysProperty = DependencyPropertyBuilder<NullableDateTimeFilter<TEntity>, int>
            .Register(nameof(Days))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as NullableDateTimeFilter<TEntity>)?.OnDaysPropertyChanged(newValue))
            .AsReadWrite();

        public int Days { get => (int)GetValue(DaysProperty); set => SetValue(DaysProperty, value); }

        private void OnDaysPropertyChanged(int newValue) => OnPeriodChanged(Period, newValue, PeriodType);

        #endregion
        #region Hours Property Members

        /// <summary>
        /// Identifies the <see cref="Hours"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HoursProperty = DependencyPropertyBuilder<NullableDateTimeFilter<TEntity>, int>
            .Register(nameof(Hours))
            .DefaultValue(0)
            .OnChanged((d, oldValue, newValue) => (d as NullableDateTimeFilter<TEntity>)?.OnHoursPropertyChanged(newValue))
            .AsReadWrite();

        public int Hours { get => (int)GetValue(HoursProperty); set => SetValue(HoursProperty, value); }

        private void OnHoursPropertyChanged(int newValue)
        {
            if (newValue < 0 || newValue > 23)
                ErrorInfo.SetError(FsInfoCat.Properties.Resources.ErrorMessage_InvalidHours, nameof(Hours));
            else
                ErrorInfo.ClearErrors(nameof(Hours));
        }

        #endregion

        protected NullableDateTimeFilter([DisallowNull] string propertyName) : base(propertyName) { }

        protected override DateTime GetComparisonValue()
        {
            int p = Period;
            int d = Days;
            int h = Hours;
            if (p > 0)
            {
                if (d > 0)
                {
                    if (h > 0)
                        return ((PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(0 - p).AddDays(0 - d) : DateTime.Now.AddDays(0 - ((p * 7) + d))).AddHours(h);
                    return (PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(0 - p).AddDays(0 - d) : DateTime.Now.AddDays(0 - ((p * 7) + d));
                }
                if (h > 0)
                    return ((PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(0 - p) : DateTime.Now.AddDays(0 - (p * 7))).AddHours(h);
                return (PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(0 - p) : DateTime.Now.AddDays(0 - (p * 7));
            }
            if (d > 0)
            {
                if (h > 0)
                    return DateTime.Now.AddDays(0 - d).AddHours(0 - h);
                return DateTime.Now.AddDays(0 - d);
            }
            if (h > 0)
                return DateTime.Now.AddHours(0 - h);
            return DateTime.Now;
        }
    }
    public abstract class NullableSchedulableDateTimeFilter<TEntity> : NullableDateTimeFilter<TEntity>
        where TEntity : class
    {
        #region IsHistorical Property Members

        /// <summary>
        /// Identifies the <see cref="IsHistorical"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsHistoricalProperty = DependencyPropertyBuilder<NullableSchedulableDateTimeFilter<TEntity>, bool>
            .Register(nameof(IsHistorical))
            .DefaultValue(false)
            .AsReadWrite();

        public bool IsHistorical { get => (bool)GetValue(IsHistoricalProperty); set => SetValue(IsHistoricalProperty, value); }

        #endregion

        protected NullableSchedulableDateTimeFilter([DisallowNull] string propertyName) : base(propertyName) { }

        protected override DateTime GetComparisonValue()
        {
            if (IsHistorical)
                return base.GetComparisonValue();
            int p = Period;
            int d = Days;
            int h = Hours;
            if (p > 0)
            {
                if (d > 0)
                {
                    if (h > 0)
                        return ((PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(p).AddDays(d) : DateTime.Now.AddDays((p * 7) + d)).AddHours(h);
                    return (PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(p).AddDays(d) : DateTime.Now.AddDays((p * 7) + d);
                }
                if (h > 0)
                    return ((PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(p) : DateTime.Now.AddDays(p * 7)).AddHours(h);
                return (PeriodType == RelativePeriodType.Months) ? DateTime.Now.AddMonths(p) : DateTime.Now.AddDays(p * 7);
            }
            if (d > 0)
            {
                if (h > 0)
                    return DateTime.Now.AddDays(d).AddHours(h);
                return DateTime.Now.AddDays(d);
            }
            if (h > 0)
                return DateTime.Now.AddHours(h);
            return DateTime.Now;
        }
    }
}
