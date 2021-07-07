using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Old
{
    /// <summary>
    /// Base interface for all database entity objects which track the creation and modification dates as well as implementing the
    /// <see cref="IValidatableObject" /> and  <see cref="IRevertibleChangeTracking" /> interfaces.
    /// </summary>
    /// <seealso cref="IValidatableObject" />
    /// <seealso cref="IRevertibleChangeTracking" />
    /// <seealso cref="IComparison" />
    /// <seealso cref="ICrawlConfiguration" />
    /// <seealso cref="IDbFsItem" />
    /// <seealso cref="IFileSystem" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="IRedundancy" />
    /// <seealso cref="IRedundantSet" />
    /// <seealso cref="ISymbolicName" />
    /// <seealso cref="IVolume" />
    /// <seealso cref="Local.ILocalDbEntity" />
    /// <seealso cref="Upstream.IUpstreamDbEntity" />
    public interface IDbEntity : IValidatableObject, IRevertibleChangeTracking
    {
        /// <summary>
        /// Gets or sets the database entity creation date/time.
        /// </summary>
        /// <value>The date and time when the database entity was created.</value>
        /// <remarks>For local databases, this value is the system-<see cref="DateTimeKind.Local"/> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_CreatedOn), ResourceType = typeof(Properties.Resources))]
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the database entity modification date/time.
        /// </summary>
        /// <value>The date and time when the database entity was last modified.</value>
        /// <remarks>For local databases, this value is the system-<see cref="DateTimeKind.Local"/> date and time. For upstream (remote) databases, this is the
        /// <see cref="DateTimeKind.Utc">UTC</see> date and time.</remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_ModifiedOn), ResourceType = typeof(Properties.Resources))]
        DateTime ModifiedOn { get; set; }
    }
}
