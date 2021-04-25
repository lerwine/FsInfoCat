using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface IFile
    {
        IChecksumCalculation ChecksumCalculation { get; }
        IReadOnlyCollection<IFileComparison> Comparisons1 { get; }
        IReadOnlyCollection<IFileComparison> Comparisons2 { get; }
        DateTime CreatedOn { get; }
        Guid Id { get; }
        DateTime ModifiedOn { get; }
        string Name { get; }
        ISubDirectory ParentDirectory { get; }
    }
}
