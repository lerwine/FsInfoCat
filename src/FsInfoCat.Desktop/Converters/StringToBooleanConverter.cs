using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    /// <summary>
    /// Converts <seealso cref="string"/> values to  <seealso cref="bool"/> values.
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class StringToBooleanConverter : ToValueConverterBase<string, bool>
    {
        #region NullSource Property Members

        /// <summary>
        /// Defines the name for the <see cref="NullSource"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_NullSource = "NullSource";

        /// <summary>
        /// Identifies the <see cref="NullSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(DependencyPropertyName_NullSource, typeof(bool?),
            typeof(StringToBooleanConverter), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Nullable{TTarget}"/> value to represent a null source value.
        /// </summary>
        public override bool? NullSource
        {
            get { return (bool?)(GetValue(NullSourceProperty)); }
            set { SetValue(NullSourceProperty, value); }
        }

        #endregion

        #region Empty Property Members

        /// <summary>
        /// Defines the name for the <see cref="Empty"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_Empty = "Empty";

        /// <summary>
        /// Identifies the <see cref="Empty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EmptyProperty = DependencyProperty.Register(DependencyPropertyName_Empty, typeof(bool?), typeof(StringToBooleanConverter),
                new PropertyMetadata(false));

        /// <summary>
        /// <seealso cref="Nullable{bool}"/> value to represent a empty string source value.
        /// </summary>
        public bool? Empty
        {
            get { return GetValue(EmptyProperty) as bool?; }
            set { SetValue(EmptyProperty, value); }
        }

        #endregion

        #region Whitespace Property Members

        /// <summary>
        /// Defines the name for the <see cref="Whitespace"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_Whitespace = "Whitespace";

        /// <summary>
        /// Identifies the <see cref="Whitespace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty WhitespaceProperty = DependencyProperty.Register(DependencyPropertyName_Whitespace, typeof(bool?), typeof(StringToBooleanConverter),
                new PropertyMetadata(false));

        /// <summary>
        /// <seealso cref="Nullable{bool}"/> value to represent a non-empty source value which only contains whitespace.
        /// </summary>
        public bool? Whitespace
        {
            get { return GetValue(WhitespaceProperty) as bool?; }
            set { SetValue(WhitespaceProperty, value); }
        }

        #endregion

        #region NonWhitespace Property Members

        /// <summary>
        /// Defines the name for the <see cref="NonWhitespace"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_NonWhitespace = "NonWhitespace";

        /// <summary>
        /// Identifies the <see cref="NonWhitespace"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NonWhitespaceProperty = DependencyProperty.Register(DependencyPropertyName_NonWhitespace, typeof(bool?), typeof(StringToBooleanConverter),
                new PropertyMetadata(true));

        /// <summary>
        /// <seealso cref="Nullable{bool}"/> value to represent a source value which contains at least one non-whitespace character.
        /// </summary>
        public bool? NonWhitespace
        {
            get { return GetValue(NonWhitespaceProperty) as bool?; }
            set { SetValue(NonWhitespaceProperty, value); }
        }

        #endregion

        /// <summary>
        /// Converts a <seealso cref="string"/> value to a <seealso cref="bool"/> value.
        /// </summary>
        /// <param name="value">The <seealso cref="string"/> produced by the binding source.</param>
        /// <param name="parameter">Parameter passed by the binding source.</param>
        /// <param name="culture">Culture specified through the binding source.</param>
        /// <returns><seealso cref="string"/> value converted to a <seealso cref="bool"/> or null value.</returns>
        public override bool? Convert(string value, object parameter, CultureInfo culture)
        {
            if (value.Length == 0)
                return Empty;

            if (value.Any(c => !Char.IsWhiteSpace(c)))
                return NonWhitespace;

            return Whitespace;
        }
    }
}
