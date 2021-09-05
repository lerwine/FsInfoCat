using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class AudioPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(AudioPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as AudioPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        private AudioPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class CrawlJobPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(CrawlJobPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as CrawlJobPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public CrawlJobPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class DocumentPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(DocumentPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as DocumentPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public DocumentPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class DRMPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(DRMPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as DRMPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public DRMPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class GPSPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(GPSPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as GPSPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public GPSPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class ImagePropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(ImagePropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as ImagePropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public ImagePropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class MediaPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(MediaPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as MediaPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public MediaPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class MusicPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(MusicPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as MusicPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public MusicPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class PhotoPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(PhotoPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as PhotoPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public PhotoPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class RecordedTVPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(RecordedTVPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as RecordedTVPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public RecordedTVPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
    public class SummaryPropertiesPropertiesColumnVisibilityOptions : ColumnVisibilityOptionsViewModel
    {
        #region MyProperty Property Members

        /// <summary>
        /// Identifies the <see cref="MyProperty"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(MyProperty), typeof(bool),
            typeof(SummaryPropertiesPropertiesColumnVisibilityOptions), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as SummaryPropertiesPropertiesColumnVisibilityOptions)?.RaiseColumnVisibilityPropertyChanged(e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool MyProperty { get => (bool)GetValue(MyPropertyProperty); set => SetValue(MyPropertyProperty, value); }

        #endregion

        public SummaryPropertiesPropertiesColumnVisibilityOptions(IEnumerable<ColumnProperty> columnProperties) : base(columnProperties) { }
    }
}
