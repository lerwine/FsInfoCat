using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace FsInfoCat.Desktop.Converters
{
    [ValueConversion(typeof(DriveType), typeof(string))]
    public class DriveTypeToStringConverter : ToClassConverterBase<DriveType, string>
    {
        public static readonly DependencyProperty UseLongDescriptionProperty = DependencyProperty.Register(nameof(UseLongDescription), typeof(bool), typeof(DriveTypeToStringConverter),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DriveTypeToStringConverter).OnUseLongDescriptionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool UseLongDescription
        {
            get { return (bool)GetValue(UseLongDescriptionProperty); }
            set { this.SetValue(UseLongDescriptionProperty, value); }
        }

        protected virtual void OnUseLongDescriptionPropertyChanged(bool oldValue, bool newValue)
        {
            if (newValue)
            {
                Unknown = FsInfoCat.Properties.Resources.DisplayName_Unknown;
                NoRootDirectory = FsInfoCat.Properties.Resources.DisplayName_DriveType_NoRootDirectory;
                Removable = FsInfoCat.Properties.Resources.DisplayName_DriveType_Removable;
                Fixed = FsInfoCat.Properties.Resources.DisplayName_DriveType_Fixed;
                Network = FsInfoCat.Properties.Resources.DisplayName_DriveType_Network;
                CDRom = FsInfoCat.Properties.Resources.DisplayName_DriveType_CDRom;
                Ram = FsInfoCat.Properties.Resources.DisplayName_DriveType_Ram;
            }
            else
            {
                Unknown = FsInfoCat.Properties.Resources.Description_DriveType_Unknown;
                NoRootDirectory = FsInfoCat.Properties.Resources.Description_DriveType_NoRootDirectory;
                Removable = FsInfoCat.Properties.Resources.Description_DriveType_Removable;
                Fixed = FsInfoCat.Properties.Resources.Description_DriveType_Fixed;
                Network = FsInfoCat.Properties.Resources.Description_DriveType_Network;
                CDRom = FsInfoCat.Properties.Resources.Description_DriveType_CDRom;
                Ram = FsInfoCat.Properties.Resources.Description_DriveType_Ram;
            }
        }

        public static readonly DependencyProperty NullSourceProperty = DependencyProperty.Register(nameof(NullSource), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue as string) ?? ""
        });

        public override string NullSource
        {
            get => GetValue(NullSourceProperty) as string;
            set => SetValue(NullSourceProperty, value);
        }

        public static readonly DependencyProperty UnknownProperty = DependencyProperty.Register(nameof(Unknown), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_Unknown :
                FsInfoCat.Properties.Resources.DisplayName_Unknown
        });

        public string Unknown
        {
            get => GetValue(UnknownProperty) as string;
            set => SetValue(UnknownProperty, value);
        }

        public static readonly DependencyProperty NoRootDirectoryProperty = DependencyProperty.Register(nameof(NoRootDirectory), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_NoRootDirectory :
                FsInfoCat.Properties.Resources.DisplayName_DriveType_NoRootDirectory
        });

        public string NoRootDirectory
        {
            get => GetValue(NoRootDirectoryProperty) as string;
            set => SetValue(NoRootDirectoryProperty, value);
        }

        public static readonly DependencyProperty RemovableProperty = DependencyProperty.Register(nameof(Removable), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_Removable :
                FsInfoCat.Properties.Resources.DisplayName_DriveType_Removable
        });

        public string Removable
        {
            get => GetValue(RemovableProperty) as string;
            set => SetValue(RemovableProperty, value);
        }

        public static readonly DependencyProperty FixedProperty = DependencyProperty.Register(nameof(Fixed), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_Fixed :
                FsInfoCat.Properties.Resources.DisplayName_DriveType_Fixed
        });

        public string Fixed
        {
            get => GetValue(FixedProperty) as string;
            set => SetValue(FixedProperty, value);
        }

        public static readonly DependencyProperty NetworkProperty = DependencyProperty.Register(nameof(Network), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_Network :
                FsInfoCat.Properties.Resources.DisplayName_DriveType_Network
        });

        public string Network
        {
            get => GetValue(NetworkProperty) as string;
            set => SetValue(NetworkProperty, value);
        }

        public static readonly DependencyProperty CDRomProperty = DependencyProperty.Register(nameof(CDRom), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_CDRom :
                FsInfoCat.Properties.Resources.DisplayName_DriveType_CDRom
        });

        public string CDRom
        {
            get => GetValue(CDRomProperty) as string;
            set => SetValue(CDRomProperty, value);
        }

        public static readonly DependencyProperty RamProperty = DependencyProperty.Register(nameof(Ram), typeof(string), typeof(DriveTypeToStringConverter), new PropertyMetadata()
        {
            CoerceValueCallback = (DependencyObject d, object baseValue) => (baseValue is string s) ? s :
                ((d as DriveTypeToStringConverter)?.UseLongDescription ?? false) ? FsInfoCat.Properties.Resources.Description_DriveType_Ram :
                FsInfoCat.Properties.Resources.DisplayName_DriveType_Ram
        });

        public string Ram
        {
            get => GetValue(RamProperty) as string;
            set => SetValue(RamProperty, value);
        }

        public override string Convert(DriveType value, object parameter, CultureInfo culture) => value switch
        {
            DriveType.NoRootDirectory => NoRootDirectory,
            DriveType.Removable => Removable,
            DriveType.Fixed => Fixed,
            DriveType.Network => Network,
            DriveType.CDRom => CDRom,
            DriveType.Ram => Ram,
            _ => Unknown
        };

        protected override object ConvertBack(string target, object parameter, CultureInfo culture)
        {
            if (target == NoRootDirectory)
                return DriveType.NoRootDirectory;
            if (target == Removable)
                return DriveType.Removable;
            if (target == Fixed)
                return DriveType.Fixed;
            if (target == Network)
                return DriveType.Network;
            if (target == CDRom)
                return DriveType.CDRom;
            if (target == Ram)
                return DriveType.Ram;
            if (target == Unknown)
                return DriveType.Unknown;
            return target;
        }
    }
}
