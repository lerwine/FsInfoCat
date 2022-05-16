using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Upstream
{
    /// <summary>
    /// Interface for entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="IUpstreamDbEntity" />
    /// <seealso cref="ISymbolicName" />
    /// <seealso cref="IEquatable{IUpstreamSymbolicName}" />
    /// <seealso cref="Local.ILocalSymbolicName" />
    public interface IUpstreamSymbolicName : IUpstreamSymbolicNameRow, ISymbolicName, IEquatable<IUpstreamSymbolicName>
    {
        /// <summary>
        /// Gets the file system that this symbolic name refers to.
        /// </summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new IUpstreamFileSystem FileSystem { get; }
    }
}
