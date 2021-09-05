using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : DbEntity, IVolumeRow
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = ColumnPropertyBuilder<string, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.DisplayName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnDisplayNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        protected virtual void OnDisplayNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region VolumeName Property Members

        /// <summary>
        /// Identifies the <see cref="VolumeName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VolumeNameProperty = ColumnPropertyBuilder<string, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.VolumeName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnVolumeNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        protected virtual void OnVolumeNamePropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Identifier Property Members

        /// <summary>
        /// Identifies the <see cref="Identifier"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdentifierProperty = ColumnPropertyBuilder<VolumeIdentifier, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.Identifier))
            .DefaultValue(VolumeIdentifier.Empty)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnIdentifierPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnIdentifierPropertyChanged(VolumeIdentifier oldValue, VolumeIdentifier newValue) { }

        #endregion
        #region ReadOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyProperty = ColumnPropertyBuilder<bool?, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.ReadOnly))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnReadOnlyPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnReadOnlyPropertyChanged(bool? oldValue, bool? newValue) { }

        #endregion
        #region MaxNameLength Property Members

        /// <summary>
        /// Identifies the <see cref="MaxNameLength"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MaxNameLengthProperty = ColumnPropertyBuilder<uint?, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.MaxNameLength))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnMaxNameLengthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnMaxNameLengthPropertyChanged(uint? oldValue, uint? newValue) { }

        #endregion
        #region Type Property Members

        /// <summary>
        /// Identifies the <see cref="Type"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TypeProperty = ColumnPropertyBuilder<DriveType, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.Type))
            .DefaultValue(DriveType.Unknown)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnTypePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnTypePropertyChanged(DriveType oldValue, DriveType newValue) { }

        #endregion
        #region Notes Property Members

        /// <summary>
        /// Identifies the <see cref="Notes"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty NotesProperty = ColumnPropertyBuilder<string, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        protected virtual void OnNotesPropertyChanged(string oldValue, string newValue) { }

        #endregion
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = ColumnPropertyBuilder<VolumeStatus, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(IVolumeRow.Status))
            .DefaultValue(VolumeStatus.Unknown)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
        protected virtual void OnStatusPropertyChanged(VolumeStatus oldValue, VolumeStatus newValue) { }

        #endregion
#pragma warning restore IDE0060 // Remove unused parameter

        public VolumeRowViewModel([DisallowNull] TEntity entity) : base(entity)
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
