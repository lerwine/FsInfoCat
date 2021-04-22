using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalFile : IFile
    {
        private ReadOnlyCollectionDelegateWrapper<LocalComparison, IFileComparison> _comparisonsWrapper1;
        private ReadOnlyCollectionDelegateWrapper<LocalComparison, IFileComparison> _comparisonsWrapper2;

        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        public Guid DirectoryId { get; set; }

        [Required]
        [ForeignKey(nameof(DirectoryId))]
        public LocalDirectory ParentDirectory { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }

        public Guid? CalculationId { get; set; }

        [ForeignKey(nameof(CalculationId))]
        public LocalChecksumCalculation ChecksumCalculation { get; set; }

        [InverseProperty(nameof(LocalComparison.FileId1))]
        public List<LocalComparison> Comparisons1 { get; set; }

        [InverseProperty(nameof(LocalComparison.FileId2))]
        public List<LocalComparison> Comparisons2 { get; set; }

        IChecksumCalculation IFile.ChecksumCalculation => ChecksumCalculation;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons1 => _comparisonsWrapper1;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons2 => _comparisonsWrapper2;

        ISubDirectory IFile.ParentDirectory => ParentDirectory;

        public LocalFile()
        {
            _comparisonsWrapper1 = new ReadOnlyCollectionDelegateWrapper<LocalComparison, IFileComparison>(() => Comparisons1);
            _comparisonsWrapper2 = new ReadOnlyCollectionDelegateWrapper<LocalComparison, IFileComparison>(() => Comparisons2);
        }
    }
}