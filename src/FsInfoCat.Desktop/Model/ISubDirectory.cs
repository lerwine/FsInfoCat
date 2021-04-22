using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface ISubDirectory
    {
        DateTime CreatedOn { get; }
        IReadOnlyList<IFile> Files { get; }
        Guid Id { get; }
        DateTime ModifiedOn { get; }
        string Name { get; }
        ISubDirectory ParentDirectory { get; }
        Guid? ParentId { get; }
        IReadOnlyList<ISubDirectory> SubDirectories { get; }
        IVolume Volume { get; }
        Guid VolumeId { get; }
    }
}
