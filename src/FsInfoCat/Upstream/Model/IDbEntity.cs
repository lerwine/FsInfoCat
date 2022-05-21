using FsInfoCat.Model;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Base interface for all database entity objects for the database which is hosted on the local machine.
    /// </summary>
    /// <seealso cref="IFileAction" />
    /// <seealso cref="IGroupMembershipRow" />
    /// <seealso cref="IHostDeviceRow" />
    /// <seealso cref="IHostPlatformRow" />
    /// <seealso cref="IMitigationTaskRow" />
    /// <seealso cref="ISubdirectoryActionRow" />
    /// <seealso cref="IUpstreamAccessError" />
    /// <seealso cref="IUpstreamBinaryPropertySet" />
    /// <seealso cref="IUpstreamComparison" />
    /// <seealso cref="IUpstreamCrawlConfigurationRow" />
    /// <seealso cref="IUpstreamCrawlJobLogRow" />
    /// <seealso cref="IUpstreamDbFsItemRow" />
    /// <seealso cref="IUpstreamFileSystemRow" />
    /// <seealso cref="IUpstreamItemTagRow" />
    /// <seealso cref="IUpstreamPropertiesRow" />
    /// <seealso cref="IUpstreamRedundancy" />
    /// <seealso cref="IUpstreamRedundantSetRow" />
    /// <seealso cref="IUpstreamSymbolicNameRow" />
    /// <seealso cref="IUpstreamTagDefinitionRow" />
    /// <seealso cref="IUpstreamVolumeRow" />
    /// <seealso cref="IUserGroupRow" />
    /// <seealso cref="IUserProfileRow" />
    /// <seealso cref="IDbEntity" />
    /// <seealso cref="Local.Model.IDbEntity" />
    /// <seealso cref="IUpstreamDbContext" />
    public interface IUpstreamDbEntity : IDbEntity
    {
        /// <summary>
        /// Gets the user who created the current record.
        /// </summary>
        /// <value>The user who created the current record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatedBy), ResourceType = typeof(Properties.Resources))]
        IUserProfile CreatedBy { get; }

        /// <summary>
        /// Gets the user who last modified the current record.
        /// </summary>
        /// <value>The user who last modified the current record.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_ModifiedBy), ResourceType = typeof(Properties.Resources))]
        IUserProfile ModifiedBy { get; }
    }
}
