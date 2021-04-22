using System;

namespace FsInfoCat.Desktop.Model
{
    public interface IFileComparison
    {
        bool AreEqual { get; }
        IFile File1 { get; }
        IFile File2 { get; }
        Guid FileId1 { get; }
        Guid FileId2 { get; }
    }
}
