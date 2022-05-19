using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(Model.VolumeIdentifier), typeof(string))]
    public class VolumeIdentifierToStringConverter : ToClassConverterBase<Model.VolumeIdentifier, string>
    {
        #region NullSource Property Members

        /// <summary>
        /// Identifies the <see cref="NullSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(VolumeIdentifierToStringConverter), new PropertyMetadata(""));

        public override string NullSource { get => GetValue(NullSourceProperty) as string; set => SetValue(NullSourceProperty, value); }

        #endregion

        public override string Convert(Model.VolumeIdentifier value, object parameter, CultureInfo culture) => Convert(value);

        public static string Convert(Model.VolumeIdentifier value)
        {
            if (value.IsEmpty())
                return "";
            if (value.SerialNumber.HasValue)
                return Model.VolumeIdentifier.ToVsnString(value.SerialNumber.Value, true);
            if (value.UUID.HasValue)
                return value.UUID.Value.ToString("d");
            return value.Location.IsUnc ? value.Location.LocalPath : value.Location.AbsoluteUri;
        }
    }
}
