using System.Globalization;

namespace FsInfoCat.Desktop.Converters
{
    public class AccountToStringConverter : ToClassConverterBase<Account, string>
    {
        public override string Convert(Account value, object parameter, CultureInfo culture) => value.LoginName;
    }
}
