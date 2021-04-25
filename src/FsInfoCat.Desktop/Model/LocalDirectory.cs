using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalDirectory : ISubDirectory
    {
        private readonly ReadOnlyCollectionDelegateWrapper<LocalFile, IFile> _filesWrapper;
        private readonly ReadOnlyCollectionDelegateWrapper<LocalDirectory, ISubDirectory> _subdirectoriesWrapper;

        public Guid Id { get; set; }

        public string Name { get; set; }

        public LocalVolume Volume { get; set; }

        public LocalDirectory ParentDirectory { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public List<LocalFile> Files { get; set; }

        public List<LocalDirectory> SubDirectories { get; set; }

        IReadOnlyCollection<IFile> ISubDirectory.Files => _filesWrapper;

        ISubDirectory ISubDirectory.ParentDirectory => ParentDirectory;

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories => _subdirectoriesWrapper;

        IVolume ISubDirectory.Volume => Volume;

        public LocalDirectory()
        {
            _filesWrapper = new ReadOnlyCollectionDelegateWrapper<LocalFile, IFile>(() => Files);
            _subdirectoriesWrapper = new ReadOnlyCollectionDelegateWrapper<LocalDirectory, ISubDirectory>(() => SubDirectories);
        }
    }
}
