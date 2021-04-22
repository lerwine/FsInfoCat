using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Desktop.Model
{
    public class LocalVolume : IVolume
    {
        private readonly ReadOnlyListDelegateWrapper<LocalDirectory, ISubDirectory> _subdirectoriesWrapper;

        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(1024)]
        public string RootPathName { get; set; }

        [Required]
        [StringLength(256)]
        public string DriveFormat { get; set; }

        [Required]
        [StringLength(128)]
        public string VolumeName { get; set; }

        [Required]
        [StringLength(1024)]
        public string Identifier { get; set; }

        [Required]
        public long MaxNameLength { get; set; }

        [Required]
        public bool CaseSensitive { get; set; }

        [Required]
        public bool IsInactive { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        public List<LocalDirectory> SubDirectories { get; set; }

        IReadOnlyList<ISubDirectory> IVolume.SubDirectories => _subdirectoriesWrapper;

        public LocalVolume()
        {
            _subdirectoriesWrapper = new ReadOnlyListDelegateWrapper<LocalDirectory, ISubDirectory>(() => SubDirectories);
        }
    }
}
