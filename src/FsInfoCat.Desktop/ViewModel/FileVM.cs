using System;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class FileVM : DependencyObject
    {

        private static readonly DependencyPropertyKey NamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Name), typeof(string), typeof(FileVM), new PropertyMetadata(""));

        public static readonly DependencyProperty NameProperty = NamePropertyKey.DependencyProperty;

        public string Name
        {
            get => GetValue(NameProperty) as string;
            private set => SetValue(NamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey BaseNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(BaseName), typeof(string), typeof(FileVM), new PropertyMetadata(""));

        public static readonly DependencyProperty BaseNameProperty = BaseNamePropertyKey.DependencyProperty;

        public string BaseName
        {
            get => GetValue(BaseNameProperty) as string;
            private set => SetValue(BaseNamePropertyKey, value);
        }

        private static readonly DependencyPropertyKey ExtensionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Extension), typeof(string), typeof(FileVM), new PropertyMetadata(""));

        public static readonly DependencyProperty ExtensionProperty = ExtensionPropertyKey.DependencyProperty;

        public string Extension
        {
            get => GetValue(ExtensionProperty) as string;
            private set => SetValue(ExtensionPropertyKey, value);
        }

        private static readonly DependencyPropertyKey LengthPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Length), typeof(long), typeof(FileVM), new PropertyMetadata(0L));

        public static readonly DependencyProperty LengthProperty = LengthPropertyKey.DependencyProperty;

        public long Length
        {
            get => (long)GetValue(LengthProperty);
            private set => SetValue(LengthPropertyKey, value);
        }

        private static readonly DependencyPropertyKey AttributesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Attributes), typeof(FileAttributes), typeof(FileVM), new PropertyMetadata(FileAttributes.Normal));

        public static readonly DependencyProperty AttributesProperty = AttributesPropertyKey.DependencyProperty;

        public FileAttributes Attributes
        {
            get => (FileAttributes)GetValue(AttributesProperty);
            private set => SetValue(AttributesPropertyKey, value);
        }

        private static readonly DependencyPropertyKey CreationTimePropertyKey = DependencyProperty.RegisterReadOnly(nameof(CreationTime), typeof(DateTime), typeof(FileVM), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty CreationTimeProperty = CreationTimePropertyKey.DependencyProperty;

        public DateTime CreationTime
        {
            get => (DateTime)GetValue(CreationTimeProperty);
            private set => SetValue(CreationTimePropertyKey, value);
        }

        private static readonly DependencyPropertyKey LastWriteTImePropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastWriteTIme), typeof(DateTime), typeof(FileVM), new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty LastWriteTImeProperty = LastWriteTImePropertyKey.DependencyProperty;

        public DateTime LastWriteTIme
        {
            get { return (DateTime)GetValue(LastWriteTImeProperty); }
            private set { SetValue(LastWriteTImePropertyKey, value); }
        }

        internal FileVM(FileInfo file)
        {
            Name = file.Name;
            BaseName = Path.GetFileNameWithoutExtension(Name);
            Extension = file.Extension.StartsWith(".") ? file.Name.Substring(1) : file.Name;
            Length = file.Length;
            Attributes = file.Attributes;
            CreationTime = file.CreationTime;
            LastWriteTIme = file.LastWriteTime;
        }
    }
}
