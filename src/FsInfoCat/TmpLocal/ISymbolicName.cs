using M = FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{

    /// <summary>
    /// Interface for entities that represent a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="ILocalDbEntity" />
    /// <seealso cref="M.ISymbolicName" />
    /// <seealso cref="IEquatable{ILocalSymbolicName}" />
    /// <seealso cref="Upstream.Model.ISymbolicName" />
    public interface ILocalSymbolicName : ILocalSymbolicNameRow, M.ISymbolicName, IEquatable<ILocalSymbolicName>
    {
        /// <summary>
        /// Gets the file system that this symbolic name refers to.
        /// </summary>
        /// <value>The file system entity that represents the file system type that this symbolic name refers to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(Properties.Resources))]
        new ILocalFileSystem FileSystem { get; }
    }
}
