using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DevHelperUI.Converters
{
    /// <summary>
    /// Base class for non-nullable target values.
    /// </summary>
    /// <typeparam name="TSource">Type of source value being converted to.</typeparam>
    /// <typeparam name="TTarget">Type of target value being converted to.</typeparam>
    public abstract class ToValueConverterBase<TSource, TTarget> : DependencyObject, IValueConverter
        where TTarget : struct
    {
        /// <summary>
        /// <see cref="Nullable{TTarget}"/> value to represent a null source value.
        /// </summary>
        public abstract TTarget? NullSource { get; set; }

        /// <summary>
        /// Converts a <typeparamref name="TSource"/> value to a <typeparamref name="TTarget"/> value.
        /// </summary>
        /// <param name="value">The <typeparamref name="TSource"/> produced by the binding source.</param>
        /// <param name="parameter">Parameter passed by the binding source.</param>
        /// <param name="culture">Culture specified through the binding source.</param>
        /// <returns><typeparamref name="TSource"/> value converted to a <typeparamref name="TTarget"/> or null value.</returns>
        public abstract TTarget? Convert(TSource value, object parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullSource;

            return (value is TSource source) ? Convert(source, parameter, culture) : value;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
