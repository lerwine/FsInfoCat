using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public abstract class NullableSchedulableDateTimeFilter<TEntity>([DisallowNull] string propertyName) : NullableDateTimeFilter<TEntity>(propertyName)
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
