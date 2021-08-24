using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public class VolumesColumnVisibilitiesViewModel : VolumeRowColumnVisibilitiesViewModel
    {
        #region RootPath Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="RootPath"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler RootPathPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="RootPath"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RootPathProperty = DependencyProperty.Register(nameof(RootPath), typeof(bool), typeof(VolumesColumnVisibilitiesViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesColumnVisibilitiesViewModel)?.RootPathPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool RootPath { get => (bool)GetValue(RootPathProperty); set => SetValue(RootPathProperty, value); }

        #endregion
        #region AccessErrorCount Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="AccessErrorCount"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler AccessErrorCountPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="AccessErrorCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AccessErrorCountProperty = DependencyProperty.Register(nameof(AccessErrorCount), typeof(bool), typeof(VolumesColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesColumnVisibilitiesViewModel)?.AccessErrorCountPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool AccessErrorCount { get => (bool)GetValue(AccessErrorCountProperty); set => SetValue(AccessErrorCountProperty, value); }

        #endregion
        #region PersonalTagCount Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="PersonalTagCount"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler PersonalTagCountPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="PersonalTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty PersonalTagCountProperty = DependencyProperty.Register(nameof(PersonalTagCount), typeof(bool), typeof(VolumesColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesColumnVisibilitiesViewModel)?.PersonalTagCountPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool PersonalTagCount { get => (bool)GetValue(PersonalTagCountProperty); set => SetValue(PersonalTagCountProperty, value); }

        #endregion
        #region SharedTagCount Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="SharedTagCount"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler SharedTagCountPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="SharedTagCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SharedTagCountProperty = DependencyProperty.Register(nameof(SharedTagCount), typeof(bool), typeof(VolumesColumnVisibilitiesViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as VolumesColumnVisibilitiesViewModel)?.SharedTagCountPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool SharedTagCount { get => (bool)GetValue(SharedTagCountProperty); set => SetValue(SharedTagCountProperty, value); }

        #endregion

        public VolumesColumnVisibilitiesViewModel(IEnumerable<(DependencyProperty Property, string Description, int Order)> properties) : base(properties.Concat(new[]
        {
            (RootPathProperty, FsInfoCat.Properties.Resources.DisplayName_RootPath, 30),
            (PersonalTagCountProperty, FsInfoCat.Properties.Resources.DisplayName_PersonalTags, 120),
            (SharedTagCountProperty, FsInfoCat.Properties.Resources.DisplayName_SharedTags, 130),
            (AccessErrorCountProperty, FsInfoCat.Properties.Resources.DisplayName_AccessErrors, 140)
        })) { }

        public VolumesColumnVisibilitiesViewModel(params (DependencyProperty Property, string Description, int Order)[] properties) : this(properties.AsEnumerable()) { }
    }
}
