using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class VolumeRowColumnVisibilitiesViewModel : ColumnVisibilitiesViewModel
    {
        #region DisplayName Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="DisplayName"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler DisplayNamePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.DisplayNamePropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool DisplayName { get => (bool)GetValue(DisplayNameProperty); set => SetValue(DisplayNameProperty, value); }

        #endregion
        #region VolumeName Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="VolumeName"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler VolumeNamePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = DependencyProperty.Register(nameof(VolumeName), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.VolumeNamePropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool VolumeName { get => (bool)GetValue(VolumeNameProperty); set => SetValue(VolumeNameProperty, value); }

        #endregion
        #region Identifier Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Identifier"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler IdentifierPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Identifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(nameof(Identifier), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.IdentifierPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool Identifier { get => (bool)GetValue(IdentifierProperty); set => SetValue(IdentifierProperty, value); }

        /// </summary>
        #endregion
        #region Status Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler StatusPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.StatusPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool Status { get => (bool)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// </summary>
        #endregion
        #region Type Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Type"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler TypePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.TypePropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool Type { get => (bool)GetValue(TypeProperty); set => SetValue(TypeProperty, value); }

        /// </summary>
        #endregion
        #region ReadOnly Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="ReadOnly"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler ReadOnlyPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="ReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.ReadOnlyPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool ReadOnly { get => (bool)GetValue(ReadOnlyProperty); set => SetValue(ReadOnlyProperty, value); }

        /// </summary>
        #endregion
        #region MaxNameLength Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="MaxNameLength"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler MaxNameLengthPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = DependencyProperty.Register(nameof(MaxNameLength), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.MaxNameLengthPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MaxNameLength { get => (bool)GetValue(MaxNameLengthProperty); set => SetValue(MaxNameLengthProperty, value); }

        /// </summary>
        #endregion
        #region Notes Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler NotesPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(bool), typeof(VolumeRowColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumeRowColumnVisibilitiesViewModel)?.NotesPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool Notes { get => (bool)GetValue(NotesProperty); set => SetValue(NotesProperty, value); }

        /// </summary>
        #endregion

        public VolumeRowColumnVisibilitiesViewModel(IEnumerable<(DependencyProperty Property, string Description, int Order)> properties) : base(properties.Concat(new[]
        {
            (DisplayNameProperty, FsInfoCat.Properties.Resources.DisplayName_DisplayName, 10),
            (VolumeNameProperty, FsInfoCat.Properties.Resources.DisplayName_VolumeName, 20),
            (IdentifierProperty, FsInfoCat.Properties.Resources.DisplayName_Identifier, 40),
            (TypeProperty, FsInfoCat.Properties.Resources.DisplayName_DriveType, 50),
            (StatusProperty, FsInfoCat.Properties.Resources.DisplayName_Status, 70),
            (ReadOnlyProperty, FsInfoCat.Properties.Resources.DisplayName_ReadOnly, 80),
            (MaxNameLengthProperty, FsInfoCat.Properties.Resources.DisplayName_MaxNameLength, 100),
            (NotesProperty, FsInfoCat.Properties.Resources.DisplayName_Notes, 150)
        })) { }

        public VolumeRowColumnVisibilitiesViewModel(params (DependencyProperty Property, string Description, int Order)[] properties) : this(properties.AsEnumerable()) { }
    }
}
