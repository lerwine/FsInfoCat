using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(SqlAuthenticationMethod), typeof(bool))]
    public class SqlAuthenticationMethodToBooleanConverter : DependencyObject, IValueConverter
    {

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(SqlAuthenticationMethod), typeof(SqlAuthenticationMethodToBooleanConverter),
                new PropertyMetadata(SqlAuthenticationMethod.NotSpecified));

        public SqlAuthenticationMethod CurrentValue
        {
            get { return (SqlAuthenticationMethod)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }

        public bool? Convert(SqlAuthenticationMethod? value, SqlAuthenticationMethod? parameter) =>
            value.HasValue ? parameter.HasValue && value.Value == parameter.Value : !parameter.HasValue;

        public SqlAuthenticationMethod? ConvertBack(bool? value, SqlAuthenticationMethod? parameter)
        {
            if (value.HasValue)
                return value.Value ? CurrentValue : parameter;
            return null;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool result)
                return result;
            return Convert(value as SqlAuthenticationMethod?, parameter as SqlAuthenticationMethod?);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SqlAuthenticationMethod authenticationMethod)
                return authenticationMethod;
            return ConvertBack(value as bool?, parameter as SqlAuthenticationMethod?);
        }
    }
}
