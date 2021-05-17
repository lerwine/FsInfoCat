using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    [System.Obsolete("Use FsInfoCat.Model.IFile")]
    public interface IFile
    {
        IHashCalculation HashCalculation { get; }
        IReadOnlyCollection<IFileComparison> Comparisons1 { get; }
        IReadOnlyCollection<IFileComparison> Comparisons2 { get; }
        Guid Id { get; }
        string Name { get; }
        ISubDirectory Parent { get; }
    }
}
