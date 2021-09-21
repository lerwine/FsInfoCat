using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(TimeSpan), typeof(string))]
    public class TimeSpanToStringConverter : ToClassConverterBase<TimeSpan, string>
    {
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(TimeSpanToStringConverter), new PropertyMetadata(""));

        public override string NullSource
        {
            get => GetValue(NullSourceProperty) as string;
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty ShortProperty = DependencyProperty.Register(nameof(Short), typeof(bool), typeof(TimeSpanToStringConverter), new PropertyMetadata(false));

        public bool Short
        {
            get => (bool)GetValue(ShortProperty);
            set => SetValue(ShortProperty, value);
        }

        public static TimeSpan? FromMediaDuration(ulong? value)
        {
            if (value.HasValue)
                return TimeSpan.FromMilliseconds(System.Convert.ToDouble(value.Value) / 10.0);
            return null;
        }

        public static ulong? ToMediaDuration(TimeSpan? value)
        {
            if (value.HasValue)
                return System.Convert.ToUInt64(value.Value.TotalMilliseconds * 10.0);
            return null;
        }

        public static string ToLongTimeSpanString(TimeSpan value)
        {
            if (value.Days == 0)
            {
                if (value.Hours == 0)
                {
                    if (value.Minutes == 0)
                        return (Math.Abs(value.Seconds) == 1) ? $"{value.Seconds} second" : $"{value.Seconds} seconds";
                    if (Math.Abs(value.Minutes) == 1)
                        return (Math.Abs(value.Seconds) == 1) ? $"{value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
                    return (Math.Abs(value.Seconds) == 1) ? $"{value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
                }
                if (Math.Abs(value.Hours) == 1)
                {
                    if (Math.Abs(value.Minutes) == 1)
                        return (Math.Abs(value.Seconds) == 1) ? $"{value.Hours} hour, {value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
                    return (Math.Abs(value.Seconds) == 1) ? $"{value.Hours} hour, {value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
                }
                if (Math.Abs(value.Minutes) == 1)
                    return (Math.Abs(value.Seconds) == 1) ? $"{value.Hours} hours, {value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
                return (Math.Abs(value.Seconds) == 1) ? $"{value.Hours} hours, {value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
            }
            if (Math.Abs(value.Days) == 1)
            {
                if (Math.Abs(value.Hours) == 1)
                {
                    if (Math.Abs(value.Minutes) == 1)
                        return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} day, {value.Hours} hour, {value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
                    return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} day, {value.Hours} hour, {value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
                }
                if (Math.Abs(value.Minutes) == 1)
                    return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} day, {value.Hours} hours, {value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
                return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} day, {value.Hours} hours, {value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
            }
            if (Math.Abs(value.Hours) == 1)
            {
                if (Math.Abs(value.Minutes) == 1)
                    return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} days, {value.Hours} hour, {value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
                return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} days, {value.Hours} hour, {value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
            }
            if (Math.Abs(value.Minutes) == 1)
                return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} days, {value.Hours} hours, {value.Minutes} minute, {value.Seconds} second" : $"{value.Minutes} minute, {value.Seconds} seconds";
            return (Math.Abs(value.Seconds) == 1) ? $"{value.Days} days, {value.Hours} hours, {value.Minutes} minutes, {value.Seconds} second" : $"{value.Minutes} minutes, {value.Seconds} seconds";
        }

        public override string Convert(TimeSpan value, object parameter, CultureInfo culture)
        {
            if (Short)
                return value.ToString("g");
            return ToLongTimeSpanString(value);
        }
    }
}
