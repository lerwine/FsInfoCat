using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.Desktop.Model
{
    public class LocalVolume : IVolume
    {
        private readonly ReadOnlyCollectionDelegateWrapper<LocalDirectory, ISubDirectory> _subdirectoriesWrapper;

        public Guid Id { get; set; }

        public string DisplayName { get; set; }

        public string RootPathName { get; set; }

        public string DriveFormat { get; set; }

        public string VolumeName { get; set; }

        public string Identifier { get; set; }

        public long MaxNameLength { get; set; }

        public bool CaseSensitive { get; set; }

        public DriveType DriveType { get; set; }

        public bool IsInactive { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public List<LocalDirectory> SubDirectories { get; set; }

        IReadOnlyCollection<ISubDirectory> IVolume.SubDirectories => _subdirectoriesWrapper;

        public LocalVolume()
        {
            _subdirectoriesWrapper = new ReadOnlyCollectionDelegateWrapper<LocalDirectory, ISubDirectory>(() => SubDirectories);
        }
    }
}
