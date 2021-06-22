using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Models.Volumes
{
    public interface IVolumeSetItem : IVolumeInfo
    {
        /// <summary>
        /// The root URI of the parent volume for mounted volumes.
        /// </summary>
        IFileUriKey MountPointParentUri { get; }
    }
}
