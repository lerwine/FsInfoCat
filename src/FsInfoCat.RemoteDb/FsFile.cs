using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class FsFile : IRemoteFile
    {
        public FsFile()
        {
            Redundancies = new HashSet<Redundancy>();
            Comparisons1 = new HashSet<FileComparison>();
            Comparisons2 = new HashSet<FileComparison>();
        }

        public HashCalculation HashCalculation { get; set; }

        public Guid Id { get; set; }

        private string _name = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_FS_NAME, ErrorMessage = Constants.ERROR_MESSAGE_NAME_LENGTH)]
        public string Name { get => _name; set => _name = value ?? ""; }

        public FileStatus Status { get; set; }

        public FsDirectory Parent { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public HashSet<Redundancy> Redundancies { get; private set; }

        public HashSet<FileComparison> Comparisons1 { get; set; }

        public HashSet<FileComparison> Comparisons2 { get; set; }

        IReadOnlyCollection<IRemoteFileComparison> IRemoteFile.Comparisons1 => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFileComparison> IRemoteFile.Comparisons2 => throw new NotImplementedException();

        IRemoteSubDirectory IRemoteFile.Parent => throw new NotImplementedException();

        IHashCalculation IFile.HashCalculation => throw new NotImplementedException();

        IReadOnlyCollection<IFileComparison> IFile.Comparisons1 => throw new NotImplementedException();

        IReadOnlyCollection<IFileComparison> IFile.Comparisons2 => throw new NotImplementedException();

        ISubDirectory IFile.Parent => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
