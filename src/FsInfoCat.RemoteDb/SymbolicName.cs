using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class SymbolicName : IRemoteSymbolicName
    {
        public FileSystem FileSystem => throw new NotImplementedException();

        public HashSet<FileSystem> DefaultFileSystems => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public Guid FileSystemId => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();

        public bool IsInactive => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IFileSystem IFsSymbolicName.FileSystem => throw new NotImplementedException();

        IRemoteFileSystem IRemoteSymbolicName.FileSystem => throw new NotImplementedException();

        IReadOnlyCollection<IFileSystem> IFsSymbolicName.DefaultFileSystems => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFileSystem> IRemoteSymbolicName.DefaultFileSystems => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
