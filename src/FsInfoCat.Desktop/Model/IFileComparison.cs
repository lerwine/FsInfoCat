using System;

namespace FsInfoCat.Desktop.Model
{
    [System.Obsolete("Use FsInfoCat.Model.IFileComparison")]
    public interface IFileComparison
    {
        bool AreEqual { get; }
        IFile File1 { get; }
        IFile File2 { get; }
        Guid FileId1 { get; }
        Guid FileId2 { get; }
    }
}
