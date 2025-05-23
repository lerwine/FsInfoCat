using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    /// <summary>
    /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
    /// <see cref="IValidatableObject" /> interface.
    /// </summary>
    /// <seealso cref="IAccessError" />
    /// <seealso cref="IPropertiesRow" />
    /// <seealso cref="IBinaryPropertySet" />
    /// <seealso cref="IComparison" />
    /// <seealso cref="ICrawlConfigurationRow" />
    /// <seealso cref="ICrawlJobLogRow" />
    /// <seealso cref="IDbFsItemRow" />
    /// <seealso cref="IFileSystemRow" />
    /// <seealso cref="IItemTagRow" />
    /// <seealso cref="ITagDefinitionRow" />
    /// <seealso cref="IRedundancy" />
    /// <seealso cref="IRedundantSetRow" />
    /// <seealso cref="ISymbolicNameRow" />
    /// <seealso cref="IVolumeRow" />
    /// <seealso cref="IDbContext" />
    [EntityInterface]
    public interface IDbEntity : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the database entity creation date/time.
        /// </summary>
        /// <value>The date and time when the database entity was created.</value>
        /// <remarks>
        /// For local databases, this value is the system-<see cref="DateTimeKind.Local" /> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.CreatedOn), ResourceType = typeof(Properties.Resources))]
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the database entity modification date/time.
        /// </summary>
        /// <value>The date and time when the database entity was last modified.</value>
        /// <remarks>
        /// For local databases, this value is the system-<see cref="DateTimeKind.Local" /> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.ModifiedOn), ResourceType = typeof(Properties.Resources))]
        DateTime ModifiedOn { get; set; }
    }
}
