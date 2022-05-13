using System;

namespace FsInfoCat
{
    /// <summary>
    /// Generic interface for file list item entities which also includes length and hash information as well as a delimited listing of parent subdirectory names.
    /// </summary>
    /// <seealso cref="IFileListItemWithAncestorNames" />
    /// <seealso cref="IEquatable{IFileListItemWithBinaryPropertiesAndAncestorNames}" />
    public interface IFileListItemWithBinaryPropertiesAndAncestorNames : IFileListItemWithAncestorNames, IEquatable<IFileListItemWithBinaryPropertiesAndAncestorNames>
    {
        /// <summary>
        /// Gets the length of the file.
        /// </summary>
        /// <value>The length of the file, in bytes.</value>
        long Length { get; }

        /// <summary>
        /// Gets the MD5 checksum hash.
        /// </summary>
        /// <value>The MD5 checksum hash value or <see langword="null"/> if no hash value has been calculated.</value>
        MD5Hash? Hash { get; }
    }
}
