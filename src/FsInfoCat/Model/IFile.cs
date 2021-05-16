using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    // TODO: Move to FsInfoCat module
    public interface IFile : ITimeStampedEntity
    {
        IHashCalculation HashCalculation { get; }
        IReadOnlyCollection<IFileComparison> Comparisons1 { get; }
        IReadOnlyCollection<IFileComparison> Comparisons2 { get; }
        Guid Id { get; }
        string Name { get; }
        ISubDirectory Parent { get; }
    }
}
