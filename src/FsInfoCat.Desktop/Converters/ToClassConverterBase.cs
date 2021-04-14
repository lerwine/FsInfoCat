using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    public abstract class ToClassConverterBase<TSource, TTarget> : DependencyObject, IValueConverter
        where TTarget : class
    {
        /// <summary>
        /// <see cref="TTarget"/> value to represent a null source value.
        /// </summary>
        public abstract TTarget NullSource { get; set; }

        /// <summary>
        /// Converts a <typeparamref name="TSource"/> value to a <typeparamref name="TTarget"/> value.
        /// </summary>
        /// <param name="value">The <typeparamref name="TSource"/> produced by the binding source.</param>
        /// <param name="parameter">Parameter passed by the binding source.</param>
        /// <param name="culture">Culture specified through the binding source.</param>
        /// <returns><typeparamref name="TSource"/> value converted to a <typeparamref name="TTarget"/> value.</returns>
        public abstract TTarget Convert(TSource value, object parameter, CultureInfo culture);

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return NullSource;

            if (!(value is TSource))
                return value;

            return Convert((TSource)value, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
