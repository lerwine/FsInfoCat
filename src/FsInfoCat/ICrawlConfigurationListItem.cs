﻿using System;

namespace FsInfoCat
{
    public interface ICrawlConfigurationListItem : ICrawlConfigurationRow
    {
        string AncestorNames { get; }

        Guid VolumeId { get; }

        string VolumeDisplayName { get; }

        string VolumeName { get; }

        VolumeIdentifier VolumeIdentifier { get; }

        string FileSystemDisplayName { get; }

        string FileSystemSymbolicName { get; }
    }
}
