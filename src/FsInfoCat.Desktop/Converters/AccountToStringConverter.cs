using System.Globalization;
using System.Windows;

namespace FsInfoCat.Desktop.Converters
{
    public class AccountToStringConverter : ToClassConverterBase<Account, string>
    {

        public static readonly DependencyProperty NullSourceProperty =
            DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(AccountToStringConverter),
                new PropertyMetadata(""));

        public override string NullSource
        {
            get { return GetValue(NullSourceProperty) as string; }
            set { SetValue(NullSourceProperty, value); }
        }

        public override string Convert(Account value, object parameter, CultureInfo culture) => value.LoginName;
    }
}
