using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>Base interface for a database entity that represents a file system node.</summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="IDbFsItem" />
    public interface IUpstreamDbFsItem : IUpstreamDbEntity, IDbFsItem
    {
        /// <summary>Gets the parent subdirectory.</summary>
        /// <value>The parent <see cref="IUpstreamSubdirectory" /> or <see langword="null" /> if this is the root <see cref="IUpstreamSubdirectory" />.</value>
        /// <remarks>
        /// If the current entity is a <see cref="IUpstreamSubdirectory" /> and this is <see langword="null" />,
        /// then <see cref="IUpstreamSubdirectory.Volume" /> should not be <see langword="null" />, and vice-versa;
        /// otherwise, if the current entity is a <see cref="IUpstreamFile" />, then this should never be <see langword="null" />.
        /// </remarks>
        [Display(Name = nameof(Properties.Resources.DisplayName_Parent), ResourceType = typeof(Properties.Resources))]
        new IUpstreamSubdirectory Parent { get; }

        /// <summary>Gets the access errors for the current file system item.</summary>
        /// <value>The access errors for the current file system item.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_AccessErrors), ResourceType = typeof(Properties.Resources))]
        new IEnumerable<IUpstreamAccessError> AccessErrors { get; }
    }
}
