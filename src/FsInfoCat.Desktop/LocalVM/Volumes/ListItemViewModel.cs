using FsInfoCat.Desktop.Converters;
using FsInfoCat.Desktop.ViewModel;
using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace FsInfoCat.Desktop.LocalVM.Volumes
{
    public class ListItemViewModel : VolumeListItemWithFileSystemViewModel<VolumeListItemWithFileSystem>, ILocalCrudEntityRowViewModel<VolumeListItemWithFileSystem>
    {
        #region UpstreamId Property Members

        private static readonly DependencyPropertyKey UpstreamIdPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UpstreamId), typeof(Guid?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="UpstreamId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty UpstreamIdProperty = UpstreamIdPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the value of the primary key for the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        public Guid? UpstreamId { get => (Guid?)GetValue(UpstreamIdProperty); private set => SetValue(UpstreamIdPropertyKey, value); }

        #endregion
        #region LastSynchronizedOn Property Members

        private static readonly DependencyPropertyKey LastSynchronizedOnPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LastSynchronizedOn), typeof(DateTime?),
            typeof(ListItemViewModel), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="LastSynchronizedOn"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LastSynchronizedOnProperty = LastSynchronizedOnPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the date and time when the current entity was sychronized with the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// date and time when the current entity was sychronized with the corresponding <see cref="Upstream.IUpstreamDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="UpstreamId" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="UpstreamId" /> should not be <see langword="null" />, either.
        /// </remarks>
        public DateTime? LastSynchronizedOn { get => (DateTime?)GetValue(LastSynchronizedOnProperty); private set => SetValue(LastSynchronizedOnPropertyKey, value); }

        #endregion
        #region IdentifierDisplayText Property Members

        private static readonly DependencyPropertyKey IdentifierDisplayTextPropertyKey = DependencyPropertyBuilder<ListItemViewModel, string>
            .Register(nameof(IdentifierDisplayText))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadOnly();

        /// <summary>
        /// Identifies the <see cref="IdentifierDisplayText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdentifierDisplayTextProperty = IdentifierDisplayTextPropertyKey.DependencyProperty;

        public string IdentifierDisplayText { get => GetValue(IdentifierDisplayTextProperty) as string; private set => SetValue(IdentifierDisplayTextPropertyKey, value); }

        private void UpdateIdentifierDisplayText(string volumeName, VolumeIdentifier identifier)
        {
            if (string.IsNullOrWhiteSpace(volumeName) || !(identifier.SerialNumber.HasValue  || identifier.UUID.HasValue))
                IdentifierDisplayText = VolumeIdentifierToStringConverter.Convert(identifier);
            else
                IdentifierDisplayText = identifier.IsEmpty() ? volumeName : $"v{volumeName} ({VolumeIdentifierToStringConverter.Convert(identifier)})";
        }

        #endregion

        protected override void OnVolumeNamePropertyChanged(string oldValue, string newValue)
        {
            UpdateIdentifierDisplayText(newValue, Identifier);
            base.OnVolumeNamePropertyChanged(oldValue, newValue);
        }

        protected override void OnIdentifierPropertyChanged(VolumeIdentifier oldValue, VolumeIdentifier newValue)
        {
            UpdateIdentifierDisplayText(VolumeName, newValue);
            base.OnIdentifierPropertyChanged(oldValue, newValue);
        }

        public ListItemViewModel([DisallowNull] VolumeListItemWithFileSystem entity) : base(entity)
        {
            UpstreamId = entity.UpstreamId;
            LastSynchronizedOn = entity.LastSynchronizedOn;
            UpdateIdentifierDisplayText(VolumeName, Identifier);
        }
    }
}
