using System;

namespace FsInfoCat.Model
{
    public interface IFileComparison : ITimeStampedEntity
    {
        bool AreEqual { get; }
        IFile File1 { get; }
        IFile File2 { get; }
        Guid FileId1 { get; }
        Guid FileId2 { get; }
    }
}
