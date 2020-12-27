using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    public class StringMapConverter : ToClassConverterBase<string, string>
    {
        #region Null Property Members

        /// <summary>
        /// Defines the name for the <see cref="Null"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_Null = "Null";

        /// <summary>
        /// Identifies the <see cref="Null"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullProperty = DependencyProperty.Register(DependencyPropertyName_Null, typeof(string), typeof(StringMapConverter),
                new PropertyMetadata(""));

        /// <summary>
        /// <seealso cref="Nullable{string}"/> value to represent a null source value.
        /// </summary>
        public string Null
        {
            get { return GetValue(NullProperty) as string; }
            set { SetValue(NullProperty, value); }
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
        public static readonly DependencyProperty EmptyProperty = DependencyProperty.Register(DependencyPropertyName_Empty, typeof(string), typeof(StringMapConverter),
                new PropertyMetadata(""));

        /// <summary>
        /// <seealso cref="Nullable{""}"/> value to represent a empty string source value.
        /// </summary>
        public string Empty
        {
            get { return GetValue(EmptyProperty) as string; }
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
        public static readonly DependencyProperty WhitespaceProperty = DependencyProperty.Register(DependencyPropertyName_Whitespace, typeof(string), typeof(StringMapConverter),
                new PropertyMetadata(""));

        /// <summary>
        /// <seealso cref="Nullable{string}"/> value to represent a non-empty source value which only contains whitespace.
        /// </summary>
        public string Whitespace
        {
            get { return GetValue(WhitespaceProperty) as string; }
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
        public static readonly DependencyProperty NonWhitespaceProperty = DependencyProperty.Register(DependencyPropertyName_NonWhitespace, typeof(string), typeof(StringMapConverter),
                new PropertyMetadata(""));

        /// <summary>
        /// <seealso cref="Nullable{""}"/> value to represent a source value which contains at least one non-whitespace character.
        /// </summary>
        public string NonWhitespace
        {
            get { return GetValue(WhitespaceProperty) as string; }
            set { SetValue(WhitespaceProperty, value); }
        }

        #endregion

        /// <summary>
        /// Converts a <seealso cref="string"/> value to a <seealso cref="string"/> value.
        /// </summary>
        /// <param name="value">The <seealso cref="string"/> produced by the binding source.</param>
        /// <param name="parameter">Parameter passed by the binding source.</param>
        /// <param name="culture">Culture specified through the binding source.</param>
        /// <returns><seealso cref="string"/> value converted to a <seealso cref="string"/> or null value.</returns>
        public override string Convert(string value, object parameter, CultureInfo culture)
        {
            if (value.Length == 0)
                return Empty;

            if (value.Any(c => !Char.IsWhiteSpace(c)))
                return NonWhitespace;

            return Whitespace;
        }
    }
}
