using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IRedundancy" />
    public interface IUpstreamRedundancy : IUpstreamDbEntity, IRedundancy
    {
        /// <summary>Gets the file that belongs to the redundancy set.</summary>
        /// <value>The file that belongs to the redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_File), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFile File { get; }

        /// <summary>Gets the redundancy set.</summary>
        /// <value>The redundancy set.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_RedundantSet), ResourceType = typeof(Properties.Resources))]
        new IUpstreamRedundantSet RedundantSet { get; }
    }
}
