using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Associates a file with a redundancy set.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="IRedundancy" />
    /// <seealso cref="IHasMembershipKeyReference{ILocalRedundantSet, ILocalFile}" />
    /// <seealso cref="IEquatable{ILocalRedundancy}" />
    /// <seealso cref="Upstream.Model.IUpstreamRedundancy" />
    public interface ILocalRedundancy : ILocalDbEntity, IRedundancy, IHasMembershipKeyReference<ILocalRedundantSet, ILocalFile>, IEquatable<ILocalRedundancy>
    {
        /// <summary>
        /// Gets the file that belongs to the redundancy set.
        /// </summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        new ILocalFile File { get; }

        /// <summary>
        /// Gets the redundancy set.
        /// </summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        new ILocalRedundantSet RedundantSet { get; }
    }
}
