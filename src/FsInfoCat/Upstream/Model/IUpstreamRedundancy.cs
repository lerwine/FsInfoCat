using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream.Model
{
    /// <summary>
    /// Associates a file with a redundancy set.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IRedundancy" />
    /// <seealso cref="IHasMembershipKeyReference{IUpstreamRedundantSet, IUpstreamFile}" />
    /// <seealso cref="IEquatable{IUpstreamRecordedTVPropertySet}" />
    /// <seealso cref="Local.Model.ILocalRedundancy" />
    public interface IUpstreamRedundancy : IUpstreamDbEntity, IRedundancy, IHasMembershipKeyReference<IUpstreamRedundantSet, IUpstreamFile>, IEquatable<IUpstreamRedundancy>
    {
        /// <summary>
        /// Gets the file that belongs to the redundancy set.
        /// </summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile File { get; }

        /// <summary>
        /// Gets the redundancy set.
        /// </summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        new IUpstreamRedundantSet RedundantSet { get; }
    }
}
