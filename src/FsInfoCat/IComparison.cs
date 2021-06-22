using System;

namespace FsInfoCat
{
    /// <summary>
    /// The results of a byte-for-byte comparison of 2 files.
    /// </summary>
    /// <remarks></remarks>
    public interface IComparison : IDbEntity
    {
        Guid SourceFileId { get; set; }

        Guid TargetFileId { get; set; }

        bool AreEqual { get; set; }

        DateTime ComparedOn { get; set; }

        IFile SourceFile { get; set; }

        IFile TargetFile { get; set; }
    }
}
