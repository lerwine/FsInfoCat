using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    // TODO: Move to FsInfoCat module
    public interface ISubDirectory : ITimeStampedEntity
    {
        IReadOnlyCollection<IFile> Files { get; }

        Guid Id { get; }

        string Name { get; }

        ISubDirectory Parent { get; }

        IReadOnlyCollection<ISubDirectory> SubDirectories { get; }

        IVolume Volume { get; }
    }
}
