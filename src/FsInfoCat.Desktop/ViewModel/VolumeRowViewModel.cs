using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class VolumeRowViewModel<TEntity> : DbEntityRowViewModel<TEntity>
        where TEntity : Model.DbEntity, Model.IVolumeRow
    {
        #region DisplayName Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayName"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayNameProperty = ColumnPropertyBuilder<string, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.DisplayName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnDisplayNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.VolumeName))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnVolumeNamePropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        public static readonly DependencyProperty IdentifierProperty = ColumnPropertyBuilder<Model.VolumeIdentifier, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.Identifier))
            .DefaultValue(Model.VolumeIdentifier.Empty)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnIdentifierPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.VolumeIdentifier Identifier { get => (Model.VolumeIdentifier)GetValue(IdentifierProperty); set => SetValue(IdentifierProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Identifier"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Identifier"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Identifier"/> property.</param>
        protected virtual void OnIdentifierPropertyChanged(Model.VolumeIdentifier oldValue, Model.VolumeIdentifier newValue) { }

        #endregion
        #region ReadOnly Property Members

        /// <summary>
        /// Identifies the <see cref="ReadOnly"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ReadOnlyProperty = ColumnPropertyBuilder<bool?, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.ReadOnly))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnReadOnlyPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.MaxNameLength))
            .DefaultValue(null)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnMaxNameLengthPropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.Type))
            .DefaultValue(DriveType.Unknown)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnTypePropertyChanged(oldValue, newValue))
            .AsReadWrite();

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
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.Notes))
            .DefaultValue("")
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnNotesPropertyChanged(oldValue, newValue))
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default).AsReadWrite();

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
        public static readonly DependencyProperty StatusProperty = ColumnPropertyBuilder<Model.VolumeStatus, VolumeRowViewModel<TEntity>>
            .RegisterEntityMapped<TEntity>(nameof(Model.IVolumeRow.Status))
            .DefaultValue(Model.VolumeStatus.Unknown)
            .OnChanged((d, oldValue, newValue) => (d as VolumeRowViewModel<TEntity>)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.VolumeStatus Status { get => (Model.VolumeStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        protected virtual void OnStatusPropertyChanged(Model.VolumeStatus oldValue, Model.VolumeStatus newValue) { }

        #endregion

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
                case nameof(Model.IVolumeRow.DisplayName):
                    Dispatcher.CheckInvoke(() => DisplayName = Entity.DisplayName);
                    break;
                case nameof(Model.IVolumeRow.VolumeName):
                    Dispatcher.CheckInvoke(() => VolumeName = Entity.VolumeName);
                    break;
                case nameof(Model.IVolumeRow.Identifier):
                    Dispatcher.CheckInvoke(() => Identifier = Entity.Identifier);
                    break;
                case nameof(Model.IVolumeRow.ReadOnly):
                    Dispatcher.CheckInvoke(() => ReadOnly = Entity.ReadOnly);
                    break;
                case nameof(Model.IVolumeRow.MaxNameLength):
                    Dispatcher.CheckInvoke(() => MaxNameLength = Entity.MaxNameLength);
                    break;
                case nameof(Model.IVolumeRow.Type):
                    Dispatcher.CheckInvoke(() => Type = Entity.Type);
                    break;
                case nameof(Model.IVolumeRow.Notes):
                    Dispatcher.CheckInvoke(() => Notes = Entity.Notes);
                    break;
                case nameof(Model.IVolumeRow.Status):
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
