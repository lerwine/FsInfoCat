using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    /// <summary>
    /// Converts <seealso cref="bool"/> values to  <seealso cref="Visibility"/> values.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BooleanToVisibilityConverter : ToValueConverterBase<bool, Visibility>
    {
        #region NullSource Property Members

        /// <summary>
        /// Defines the name for the <see cref="NullSource"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_NullSource = "NullSource";

        /// <summary>
        /// Identifies the <see cref="NullSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(DependencyPropertyName_NullSource, typeof(Visibility?),
            typeof(BooleanToVisibilityConverter), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Nullable{TTarget}"/> value to represent a null source value.
        /// </summary>
        public override Visibility? NullSource
        {
            get { return (Visibility?)GetValue(NullSourceProperty); }
            set { SetValue(NullSourceProperty, value); }
        }

        #endregion

        #region True Property Members

        /// <summary>
        /// Defines the name for the <see cref="True"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_True = "True";

        /// <summary>
        /// Identifies the <see cref="True"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TrueProperty = DependencyProperty.Register(DependencyPropertyName_True, typeof(Visibility?), typeof(BooleanToVisibilityConverter),
                new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Value when source is true.
        /// </summary>
        public Visibility? True
        {
            get { return GetValue(TrueProperty) as Visibility?; }
            set { SetValue(TrueProperty, value); }
        }

        #endregion

        #region False Property Members

        /// <summary>
        /// Defines the name for the <see cref="False"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_False = "False";

        /// <summary>
        /// Identifies the <see cref="False"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FalseProperty = DependencyProperty.Register(DependencyPropertyName_False, typeof(Visibility?), typeof(BooleanToVisibilityConverter),
                new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Value when source is false.
        /// </summary>
        public Visibility? False
        {
            get { return GetValue(FalseProperty) as Visibility?; }
            set { SetValue(FalseProperty, value); }
        }

        #endregion

        /// <summary>
        /// Converts a <seealso cref="bool"/> value to a <seealso cref="Visibility"/> value.
        /// </summary>
        /// <param name="value">The <seealso cref="bool"/> produced by the binding source.</param>
        /// <param name="parameter">Parameter passed by the binding source.</param>
        /// <param name="culture">Culture specified through the binding source.</param>
        /// <returns><seealso cref="bool"/> value converted to a <seealso cref="Visibility"/> or null value.</returns>
        public override Visibility? Convert(bool value, object parameter, CultureInfo culture)
        {
            return value ? True : False;
        }
    }
}
