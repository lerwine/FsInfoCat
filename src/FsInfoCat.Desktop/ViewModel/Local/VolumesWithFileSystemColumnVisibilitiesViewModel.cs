using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class VolumesWithFileSystemColumnVisibilitiesViewModel : VolumesColumnVisibilitiesViewModel
    {
        #region FileSystemDisplayName Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="FileSystemDisplayName"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler FileSystemDisplayNamePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="FileSystemDisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FileSystemDisplayNameProperty = DependencyProperty.Register(nameof(FileSystemDisplayName), typeof(bool), typeof(VolumesWithFileSystemColumnVisibilitiesViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesWithFileSystemColumnVisibilitiesViewModel)?.FileSystemDisplayNamePropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool FileSystemDisplayName { get => (bool)GetValue(FileSystemDisplayNameProperty); set => SetValue(FileSystemDisplayNameProperty, value); }

        #endregion
        #region EffectiveReadOnly Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="EffectiveReadOnly"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler EffectiveReadOnlyPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="EffectiveReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveReadOnlyProperty = DependencyProperty.Register(nameof(EffectiveReadOnly), typeof(bool), typeof(VolumesWithFileSystemColumnVisibilitiesViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesWithFileSystemColumnVisibilitiesViewModel)?.EffectiveReadOnlyPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool EffectiveReadOnly { get => (bool)GetValue(EffectiveReadOnlyProperty); set => SetValue(EffectiveReadOnlyProperty, value); }

        #endregion
        #region EffectiveMaxNameLength Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="EffectiveMaxNameLength"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler EffectiveMaxNameLengthPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="EffectiveMaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty EffectiveMaxNameLengthProperty = DependencyProperty.Register(nameof(EffectiveMaxNameLength), typeof(bool), typeof(VolumesWithFileSystemColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesWithFileSystemColumnVisibilitiesViewModel)?.EffectiveMaxNameLengthPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool EffectiveMaxNameLength { get => (bool)GetValue(EffectiveMaxNameLengthProperty); set => SetValue(EffectiveMaxNameLengthProperty, value); }

        #endregion

        public VolumesWithFileSystemColumnVisibilitiesViewModel(IEnumerable<(DependencyProperty Property, string Description, int Order)> properties) : base(properties.Concat(new[]
        {
            (FileSystemDisplayNameProperty, FsInfoCat.Properties.Resources.DisplayName_FileSystem, 60),
            (EffectiveReadOnlyProperty, FsInfoCat.Properties.Resources.DisplayName_ReadOnly_Effective, 90),
            (EffectiveMaxNameLengthProperty, FsInfoCat.Properties.Resources.DisplayName_MaxNameLength_Effective, 110)
        }))
        { }

        public VolumesWithFileSystemColumnVisibilitiesViewModel(params (DependencyProperty Property, string Description, int Order)[] properties) : this(properties.AsEnumerable()) { }
    }
}
