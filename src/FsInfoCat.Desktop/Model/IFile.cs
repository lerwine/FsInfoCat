using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface IFile
    {
        IChecksumCalculation ChecksumCalculation { get; }
        Guid? ChecksumId { get; }
        IReadOnlyList<IFileComparison> Comparisons1 { get; }
        IReadOnlyList<IFileComparison> Comparisons2 { get; }
        DateTime CreatedOn { get; }
        Guid DirectoryId { get; }
        Guid Id { get; }
        DateTime ModifiedOn { get; }
        string Name { get; }
        ISubDirectory ParentDirectory { get; }
    }
}
