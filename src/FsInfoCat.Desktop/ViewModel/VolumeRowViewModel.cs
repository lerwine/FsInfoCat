using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IVolumeRow
    {
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register(nameof(DisplayName), typeof(string),
            typeof(VolumeRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnDisplayNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string DisplayName { get => GetValue(DisplayNameProperty) as string; set => SetValue(DisplayNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="DisplayName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="DisplayName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="DisplayName"/> property.</param>
        private void OnDisplayNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnDisplayNamePropertyChanged Logic
        }

        #endregion
        #region VolumeName Property Members

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = DependencyProperty.Register(nameof(VolumeName), typeof(string),
            typeof(VolumeRowViewModel<TEntity>), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnVolumeNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string VolumeName { get => GetValue(VolumeNameProperty) as string; set => SetValue(VolumeNameProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="VolumeName"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="VolumeName"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="VolumeName"/> property.</param>
        private void OnVolumeNamePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnVolumeNamePropertyChanged Logic
        }

        #endregion
        #region Identifier Property Members

        /// <summary>
        /// Identifies the <see cref="Identifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(nameof(Identifier), typeof(VolumeIdentifier),
            typeof(VolumeRowViewModel<TEntity>), new PropertyMetadata(VolumeIdentifier.Empty, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnIdentifierPropertyChanged((VolumeIdentifier)e.OldValue, (VolumeIdentifier)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public VolumeIdentifier Identifier { get => (VolumeIdentifier)GetValue(IdentifierProperty); set => SetValue(IdentifierProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Identifier"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Identifier"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Identifier"/> property.</param>
        private void OnIdentifierPropertyChanged(VolumeIdentifier oldValue, VolumeIdentifier newValue)
        {
            // TODO: Implement OnIdentifierPropertyChanged Logic
        }

        #endregion
        #region ReadOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyProperty = DependencyProperty.Register(nameof(ReadOnly), typeof(bool?), typeof(VolumeRowViewModel<TEntity>),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnReadOnlyPropertyChanged((bool?)e.OldValue, (bool?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public bool? ReadOnly { get => (bool?)GetValue(ReadOnlyProperty); set => SetValue(ReadOnlyProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="ReadOnly"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="ReadOnly"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="ReadOnly"/> property.</param>
        private void OnReadOnlyPropertyChanged(bool? oldValue, bool? newValue)
        {
            // TODO: Implement OnReadOnlyPropertyChanged Logic
        }

        #endregion
        #region MaxNameLength Property Members

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = DependencyProperty.Register(nameof(MaxNameLength), typeof(uint?),
            typeof(VolumeRowViewModel<TEntity>), new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnMaxNameLengthPropertyChanged((uint?)e.OldValue, (uint?)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public uint? MaxNameLength { get => (uint?)GetValue(MaxNameLengthProperty); set => SetValue(MaxNameLengthProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="MaxNameLength"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="MaxNameLength"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="MaxNameLength"/> property.</param>
        private void OnMaxNameLengthPropertyChanged(uint? oldValue, uint? newValue)
        {
            // TODO: Implement OnMaxNameLengthPropertyChanged Logic
        }

        #endregion
        #region Type Property Members

        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(DriveType), typeof(VolumeRowViewModel<TEntity>),
                new PropertyMetadata(DriveType.Unknown, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnTypePropertyChanged((DriveType)e.OldValue, (DriveType)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public DriveType Type { get => (DriveType)GetValue(TypeProperty); set => SetValue(TypeProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Type"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Type"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Type"/> property.</param>
        private void OnTypePropertyChanged(DriveType oldValue, DriveType newValue)
        {
            // TODO: Implement OnTypePropertyChanged Logic
        }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = DependencyProperty.Register(nameof(Notes), typeof(string), typeof(VolumeRowViewModel<TEntity>),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnNotesPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Notes { get => GetValue(NotesProperty) as string; set => SetValue(NotesProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Notes"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Notes"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Notes"/> property.</param>
        private void OnNotesPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnNotesPropertyChanged Logic
        }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status), typeof(VolumeStatus), typeof(VolumeRowViewModel<TEntity>),
                new PropertyMetadata(VolumeStatus.Unknown, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as VolumeRowViewModel<TEntity>)?.OnStatusPropertyChanged((VolumeStatus)e.OldValue, (VolumeStatus)e.NewValue)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public VolumeStatus Status { get => (VolumeStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        private void OnStatusPropertyChanged(VolumeStatus oldValue, VolumeStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        #endregion

        public VolumeRowViewModel(TEntity entity) : base(entity)
        {
            DisplayName = entity.DisplayName;
            VolumeName = entity.VolumeName;
            Identifier = entity.Identifier;
            ReadOnly = entity.ReadOnly;
            MaxNameLength = entity.MaxNameLength;
            Type = entity.Type;
            Notes = entity.Notes;
            Status = entity.Status;
        }

        protected bool CheckEntityPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IVolumeRow.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Entity.DisplayName);
                    break;
                case nameof(IVolumeRow.VolumeName):
                    Dispatcher.CheckInvoke(() => VolumeName = Entity.VolumeName);
                    break;
                case nameof(IVolumeRow.Identifier):
                    Dispatcher.CheckInvoke(() => Identifier = Entity.Identifier);
                    break;
                case nameof(IVolumeRow.ReadOnly):
                    Dispatcher.CheckInvoke(() => ReadOnly = Entity.ReadOnly);
                    break;
                case nameof(IVolumeRow.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Entity.MaxNameLength);
                    break;
                case nameof(IVolumeRow.Type):
                    Dispatcher.CheckInvoke(() => Type = Entity.Type);
                    break;
                case nameof(IVolumeRow.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(IVolumeRow.Status):
                    Dispatcher.CheckInvoke(() => Status = Entity.Status);
                    break;
                default:
                    base.OnEntityPropertyChanged(propertyName);
                    return false;
            }
            return true;
        }
    }
}
