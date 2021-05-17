using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IFile : ITimeStampedEntity, IValidatableObject
    {
        IHashCalculation HashCalculation { get; }
        IReadOnlyCollection<IFileComparison> Comparisons1 { get; }
        IReadOnlyCollection<IFileComparison> Comparisons2 { get; }
        Guid Id { get; }
        string Name { get; }

        FileStatus Status { get; set; }

        ISubDirectory Parent { get; }
    }
}
