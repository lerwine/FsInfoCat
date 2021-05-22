namespace FsInfoCat
{
    public interface ISymbolicName : IDbEntity
    {
        string Name { get; set; }

        string Notes { get; set; }

        int Priority { get; set; }

        bool IsInactive { get; set; }

        IFileSystem FileSystem { get; set; }
    }
}
