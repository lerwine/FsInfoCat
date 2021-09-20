using FsInfoCat.Desktop.ViewModel;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    /// <summary>
    /// Converts <seealso cref="bool"/> values to their inverted values.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ThreeStateBooleanConverter : ToValueConverterBase<bool, bool>
    {
        #region NullSource Property Members

        /// <summary>
        /// Defines the name for the <see cref="NullSource"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_NullSource = "NullSource";

        /// <summary>
        /// Identifies the <see cref="NullSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullSourceProperty = DependencyPropertyBuilder<ThreeStateBooleanConverter, bool?>
            .Register(nameof(NullSource))
            .DefaultValue(false)
            .AsReadWrite();

        /// <summary>
        /// <see cref="Nullable{TTarget}"/> value to represent a null source value.
        /// </summary>
        public override bool? NullSource
        {
            get { return (bool?)GetValue(NullSourceProperty); }
            set { SetValue(NullSourceProperty, value); }
        }

        #endregion
        #region IfTrue Property Members

        /// <summary>
        /// Identifies the <see cref="IfTrue"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IfTrueProperty = DependencyPropertyBuilder<ThreeStateBooleanConverter, bool?>
            .Register(nameof(IfTrue))
            .DefaultValue(true)
            .AsReadWrite();

        public bool? IfTrue { get => (bool?)GetValue(IfTrueProperty); set => SetValue(IfTrueProperty, value); }

        #endregion
        #region IfFalse Property Members

        /// <summary>
        /// Identifies the <see cref="IfFalse"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IfFalseProperty = DependencyPropertyBuilder<ThreeStateBooleanConverter, bool?>
            .Register(nameof(IfFalse))
            .DefaultValue(false)
            .AsReadWrite();

        public bool? IfFalse { get => (bool?)GetValue(IfFalseProperty); set => SetValue(IfFalseProperty, value); }

        #endregion
        /// <summary>
        /// Converts a <seealso cref="bool"/> value to its inverse value.
        /// </summary>
        /// <param name="value">The <seealso cref="bool"/> produced by the binding source.</param>
        /// <param name="parameter">Parameter passed by the binding source.</param>
        /// <param name="culture">Culture specified through the binding source.</param>
        /// <returns><seealso cref="bool"/> value converted to its inverse value.</returns>
        public override bool? Convert(bool value, object parameter, CultureInfo culture) { return value ? IfTrue : IfFalse; }

        protected override bool ConvertBack(bool? target, object parameter, CultureInfo culture)
        {
            return !(target ?? NullSource ?? true);
        }
    }
}
