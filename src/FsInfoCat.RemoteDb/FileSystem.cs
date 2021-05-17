using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace FsInfoCat.RemoteDb
{
    public class FileSystem : IRemoteFileSystem
    {
        public HashSet<Volume> Volumes => throw new NotImplementedException();

        public HashSet<SymbolicName> SymbolicNames => throw new NotImplementedException();

        public SymbolicName DefaultSymbolicName => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public bool CaseSensitiveSearch => throw new NotImplementedException();

        public bool ReadOnly => throw new NotImplementedException();

        public long MaxNameLength => throw new NotImplementedException();

        public DriveType? DefaultDriveType => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();

        public bool IsInactive => throw new NotImplementedException();

        public Guid DefaultSymbolicNameId => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IReadOnlyCollection<IVolume> IFileSystem.Volumes => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteVolume> IRemoteFileSystem.Volumes => throw new NotImplementedException();

        IReadOnlyCollection<IFsSymbolicName> IFileSystem.SymbolicNames => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteSymbolicName> IRemoteFileSystem.SymbolicNames => throw new NotImplementedException();

        IFsSymbolicName IFileSystem.DefaultSymbolicName => throw new NotImplementedException();

        IRemoteSymbolicName IRemoteFileSystem.DefaultSymbolicName => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
