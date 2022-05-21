using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Generic interface for subdirectory access error entities that occurred on the local host machine.
    /// </summary>
    /// <seealso cref="IEquatable{ILocalSubdirectoryAccessError}" />
    /// <seealso cref="ILocalAccessError" />
    /// <seealso cref="ISubdirectoryAccessError" />
    /// <seealso cref="ILocalDbContext.SubdirectoryAccessErrors" />
    /// <seealso cref="ILocalSubdirectory.AccessErrors" />
    /// <seealso cref="Upstream.Model.ISubdirectoryAccessError" />
    public interface ILocalSubdirectoryAccessError : ILocalAccessError, ISubdirectoryAccessError, IEquatable<ILocalSubdirectoryAccessError>
    {
        /// <summary>
        /// Gets the target subdirectory to which the access error applies.
        /// </summary>
        /// <value>The <see cref="ILocalSubdirectory" /> entity that this error applies to.</value>
        [Display(Name = nameof(Properties.Resources.DisplayName_Target), ResourceType = typeof(Properties.Resources))]
        new ILocalSubdirectory Target { get; }
    }
}
