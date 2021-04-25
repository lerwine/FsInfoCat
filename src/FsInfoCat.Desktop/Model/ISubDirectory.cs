using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface ISubDirectory
    {
        DateTime CreatedOn { get; }
        IReadOnlyCollection<IFile> Files { get; }
        Guid Id { get; }
        DateTime ModifiedOn { get; }
        string Name { get; }
        ISubDirectory ParentDirectory { get; }
        IReadOnlyCollection<ISubDirectory> SubDirectories { get; }
        IVolume Volume { get; }
    }
}
