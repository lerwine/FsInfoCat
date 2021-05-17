using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface ISubDirectory : ITimeStampedEntity, IValidatableObject
    {
        Guid Id { get; }

        string Name { get; }
        DirectoryCrawlFlags CrawlFlags { get; }

        ISubDirectory Parent { get; }

        IVolume Volume { get; }

        IReadOnlyCollection<IFile> Files { get; }

        IReadOnlyCollection<ISubDirectory> SubDirectories { get; }
    }
}
