using M = FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base interface for all database entity objects for the database which is hosted on the local machine.
    /// </summary>
    /// <seealso cref="ILocalDbContext" />
    /// <seealso cref="M.IDbEntity" />
    /// <seealso cref="ILocalBinaryPropertySet" />
    /// <seealso cref="ILocalComparison" />
    /// <seealso cref="ILocalCrawlConfiguration" />
    /// <seealso cref="ILocalCrawlConfigurationListItem" />
    /// <seealso cref="ILocalCrawlConfigurationRow" />
    /// <seealso cref="ILocalCrawlJobLogRow" />
    /// <seealso cref="ILocalDbFsItemRow" />
    /// <seealso cref="ILocalFileSystem" />
    /// <seealso cref="ILocalFileSystemRow" />
    /// <seealso cref="ILocalItemTagRow" />
    /// <seealso cref="ILocalPropertiesRow" />
    /// <seealso cref="ILocalRedundancy" />
    /// <seealso cref="ILocalRedundantSetRow" />
    /// <seealso cref="ILocalSymbolicNameRow" />
    /// <seealso cref="ILocalTagDefinitionRow" />
    /// <seealso cref="ILocalVolumeRow" />
    /// <seealso cref="ILocalAccessError.Target" />
    /// <seealso cref="ILocalItemTag.Tagged" />
    /// <seealso cref="Upstream.Model.IDbEntity" />
    public interface ILocalDbEntity : M.IDbEntity
    {
        /// <summary>
        /// Gets the value of the primary key for the corresponding <see cref="Upstream.Model.IDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// The value of the primary key of the corresponding <see cref="Upstream.Model.IDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="LastSynchronizedOn" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="LastSynchronizedOn" /> should not be <see langword="null" />, either.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_UpstreamId), ResourceType = typeof(Properties.Resources))]
        Guid? UpstreamId { get; }

        /// <summary>
        /// Gets the date and time when the current entity was sychronized with the corresponding <see cref="Upstream.Model.IDbEntity">upstream (remote) database entity</see>.
        /// </summary>
        /// <value>
        /// date and time when the current entity was sychronized with the corresponding <see cref="Upstream.Model.IDbEntity">upstream (remote) database entity</see>;
        /// otherwise, <see langword="null" /> if there is no corresponding entity.
        /// </value>
        /// <remarks>
        /// If this value is <see langword="null" />, then <see cref="UpstreamId" /> should also be <see langword="null" />.
        /// Likewise, if this is not <see langword="null" />, then <see cref="UpstreamId" /> should not be <see langword="null" />, either.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_LastSynchronizedOn), ResourceType = typeof(Properties.Resources))]
        DateTime? LastSynchronizedOn { get; }
    }
}
