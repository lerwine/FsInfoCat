using FsInfoCat.Model.Upstream;
using System.Globalization;
using System.Windows;

namespace FsInfoCat.Desktop.Converters
{
    public class AccountToStringConverter : ToClassConverterBase<IUserProfile, string>
    {

        public static readonly DependencyProperty NullSourceProperty =
            DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(AccountToStringConverter),
                new PropertyMetadata(""));

        public override string NullSource
        {
            get { return GetValue(NullSourceProperty) as string; }
            set { SetValue(NullSourceProperty, value); }
        }

        public override string Convert(IUserProfile value, object parameter, CultureInfo culture) => throw new System.NotImplementedException();//value.DisplayName
    }
}
