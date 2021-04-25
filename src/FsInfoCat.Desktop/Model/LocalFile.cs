using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalFile : IFile
    {
        private readonly ReadOnlyCollectionDelegateWrapper<LocalComparison, IFileComparison> _comparisonsWrapper1;
        private readonly ReadOnlyCollectionDelegateWrapper<LocalComparison, IFileComparison> _comparisonsWrapper2;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public LocalDirectory ParentDirectory { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public LocalChecksumCalculation ChecksumCalculation { get; set; }

        public List<LocalComparison> Comparisons1 { get; set; }

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
