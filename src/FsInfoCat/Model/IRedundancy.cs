using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IRedundancy : IValidatableObject
    {
        Guid Id { get; }

        IReadOnlyCollection<IFile> Files { get; }
    }
}
