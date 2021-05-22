namespace FsInfoCat
{
    public interface IRedundancy : IDbEntity
    {
        string Reference { get; set; }

        FileRedundancyStatus Status { get; set; }

        string Notes { get; set; }

        IFile File { get; set; }

        IRedundantSet RedundantSet { get; set; }
    }
}
