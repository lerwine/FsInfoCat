using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(FileAttributes), typeof(string))]
    public class FileAttributesToStringConverter : ToClassConverterBase<FileAttributes, string>
    {
        private static readonly Dictionary<FileAttributes, string> _map = new();
        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(FileAttributesToStringConverter), new PropertyMetadata(""));

        public override string NullSource
        {
            get { return GetValue(NullSourceProperty) as string; }
            set { SetValue(NullSourceProperty, value); }
        }

        public static IEnumerable<string> GetDisplayText(FileAttributes value)
        {
            if (value.HasFlag(FileAttributes.ReadOnly))
                yield return FsInfoCat.Properties.Resources.DisplayName_ReadOnly;
            if (value.HasFlag(FileAttributes.Hidden))
                yield return FsInfoCat.Properties.Resources.DisplayName_Hidden;
            if (value.HasFlag(FileAttributes.System))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_System;
            if (value.HasFlag(FileAttributes.Archive))
                yield return FsInfoCat.Properties.Resources.DisplayName_Archive;
            if (value.HasFlag(FileAttributes.Device))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_Device;
            if (value.HasFlag(FileAttributes.Temporary))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_Temporary;
            if (value.HasFlag(FileAttributes.SparseFile))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_SparseFile;
            if (value.HasFlag(FileAttributes.ReparsePoint))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_ReparsePoint;
            if (value.HasFlag(FileAttributes.Compressed))
                yield return FsInfoCat.Properties.Resources.DisplayName_Compressed;
            if (value.HasFlag(FileAttributes.Offline))
                yield return FsInfoCat.Properties.Resources.DisplayName_Offline;
            if (value.HasFlag(FileAttributes.NotContentIndexed))
                yield return FsInfoCat.Properties.Resources.DisplayName_NotIndexed;
            if (value.HasFlag(FileAttributes.Encrypted))
                yield return FsInfoCat.Properties.Resources.DisplayName_Encrypted;
            if (value.HasFlag(FileAttributes.IntegrityStream))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_IntegrityStream;
            if (value.HasFlag(FileAttributes.NoScrubData))
                yield return FsInfoCat.Properties.Resources.DisplayName_FileAttributes_NoScrubData;
        }

        public override string Convert(FileAttributes value, object parameter, CultureInfo culture)
        {
            string t;
            lock (_map)
            {
                if (_map.TryGetValue(value, out string s))
                    return s;
                using IEnumerator<string> enumerator = GetDisplayText(value).GetEnumerator();
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
                _map.Add(value, t);
            }
            return t;
        }
    }
}
