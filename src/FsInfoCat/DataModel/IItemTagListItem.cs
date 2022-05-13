using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for a list item entity that associates an <see cref="ITagDefinition"/> with an <see cref="IFile"/>, <see cref="ISubdirectory"/> or <see cref="IVolume"/>.
    /// </summary>
    /// <seealso cref="IItemTagRow" />
    /// <seealso cref="IEquatable{IItemTagListItem}" />
    public interface IItemTagListItem : IItemTagRow, IEquatable<IItemTagListItem>
    {
        /// <summary>
        /// Gets the name of the tag.
        /// </summary>
        /// <value>The <see cref="ITagDefinitionRow.Name"/> of the tag.</value>
        string Name { get; }

        /// <summary>
        /// Gets the description of the tag.
        /// </summary>
        /// <value>The <see cref="ITagDefinitionRow.Description"/> of the tag.</value>
        string Description { get; }
    }
}
