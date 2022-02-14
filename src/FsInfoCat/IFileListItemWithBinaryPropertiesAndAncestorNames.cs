using System;

namespace FsInfoCat
{
    public interface IFileListItemWithBinaryPropertiesAndAncestorNames : IFileListItemWithAncestorNames, IEquatable<IFileListItemWithBinaryPropertiesAndAncestorNames>
    {
        long Length { get; }

        MD5Hash? Hash { get; }
    }
}
