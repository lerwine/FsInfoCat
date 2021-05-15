using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    // TODO: Move to FsInfoCat module
    public interface ISubDirectory
    {
        DateTime CreatedOn { get; }
        IReadOnlyCollection<IFile> Files { get; }
        Guid Id { get; }
        DateTime ModifiedOn { get; }
        string Name { get; }
        ISubDirectory Parent { get; }
        IReadOnlyCollection<ISubDirectory> SubDirectories { get; }
        IVolume Volume { get; }
    }
}
