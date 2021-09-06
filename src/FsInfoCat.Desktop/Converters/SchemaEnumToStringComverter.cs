using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;

namespace FsInfoCat.Desktop.Converters
{
    public abstract class SchemaEnumToStringComverter<T> : ToClassConverterBase<T, string>
        where T : struct, Enum
    {
        private readonly Dictionary<DisplayVerbosity, Dictionary<T, string>> _maps = new();
        private Dictionary<T, string> _map = new();
        private readonly bool _hasFlags;

        public static readonly DependencyProperty DisplayProperty = DependencyProperty.Register(nameof(Display), typeof(DisplayVerbosity), typeof(SchemaEnumToStringComverter<T>),
                new PropertyMetadata(DisplayVerbosity.Name, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as SchemaEnumToStringComverter<T>).OnDisplayPropertyChanged((DisplayVerbosity)e.OldValue, (DisplayVerbosity)e.NewValue)));

        public DisplayVerbosity Display
        {
            get { return (DisplayVerbosity)GetValue(DisplayProperty); }
            set { this.SetValue(DisplayProperty, value); }
        }


        public static readonly DependencyProperty EmptyProperty = DependencyProperty.Register(nameof(Empty), typeof(string), typeof(SchemaEnumToStringComverter<T>),
                new PropertyMetadata(FsInfoCat.Properties.Resources.DisplayName_None_Parentheses));

        public string Empty
        {
            get { return GetValue(EmptyProperty) as string; }
            set { SetValue(EmptyProperty, value); }
        }


        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(SchemaEnumToStringComverter<T>),
                new PropertyMetadata(""));

        public override string NullSource
        {
            get { return GetValue(NullSourceProperty) as string; }
            set { SetValue(NullSourceProperty, value); }
        }

        protected SchemaEnumToStringComverter()
        {
            _hasFlags = typeof(T).GetCustomAttributes(typeof(FlagsAttribute), false).Length > 0;
        }

        protected virtual void OnDisplayPropertyChanged(DisplayVerbosity oldValue, DisplayVerbosity newValue)
        {
            lock (_maps)
            {
                if (!_maps.ContainsKey(oldValue))
                    _maps.Add(oldValue, _map);
                if (_maps.ContainsKey(newValue))
                    _map = _maps[newValue];
                else
                    _map = new();
            }
        }

        public static IEnumerable<string> GetFlagValues(T value, DisplayVerbosity display)
        {
            switch (display)
            {
                case DisplayVerbosity.ShortName:
                    foreach (T v in Enum.GetValues<T>())
                    {
                        if (value.HasFlag(v))
                            yield return v.TryGetShortName(out string t) ? t : v.ToString("F");
                    }
                    break;
                case DisplayVerbosity.Description:
                    foreach (T v in Enum.GetValues<T>())
                    {
                        if (value.HasFlag(v))
                            yield return v.TryGetDescription(out string t) ? t : v.ToString("F");
                    }
                    break;
                default:
                    foreach (T v in Enum.GetValues<T>())
                    {
                        if (value.HasFlag(v))
                            yield return v.TryGetDisplayName(out string t) ? t : v.ToString("F");
                    }
                    break;
            }
        }

        public override string Convert(T value, object parameter, CultureInfo culture)
        {
            string t;
            lock (_maps)
            {
                if (_map.ContainsKey(value))
                    t = _map[value];
                else
                {
                    if (_hasFlags)
                    {
                        using IEnumerator<string> enumerator = GetFlagValues(value, Display).GetEnumerator();
                        if (enumerator.MoveNext())
                        {
                            t = enumerator.Current;
                            if (enumerator.MoveNext())
                            {
                                StringBuilder sb = new(t);
                                do { _ = sb.Append("; ").Append(enumerator.Current); } while (enumerator.MoveNext());
                                t = sb.ToString();
                            }
                        }
                        else
                            t = "";
                    }
                    else if (!(Display switch
                    {
                        DisplayVerbosity.ShortName => value.TryGetShortName(out t),
                        DisplayVerbosity.Description => value.TryGetDescription(out t),
                        _ => value.TryGetDisplayName(out t)
                    }))
                        t = value.ToString("F");
                    _map.Add(value, t);
                }
            }
            return (t.Length > 0) ? t : Empty;
        }
    }
}
