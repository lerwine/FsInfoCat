namespace FsInfoCat
{
    public interface IFileListItemWithBinaryPropertiesAndAncestorNames : IFileListItemWithAncestorNames
    {
        long Length { get; }

        MD5Hash? Hash { get; }
    }
}

