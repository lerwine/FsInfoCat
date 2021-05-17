using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class Volume : IRemoteVolume
    {
        public FsDirectory RootDirectory => throw new NotImplementedException();

        public FileSystem FileSystem => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public bool? CaseSensitiveSearch => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public string Identifier => throw new NotImplementedException();

        public bool IsInactive => throw new NotImplementedException();

        public long? MaxNameLength => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();

        public string VolumeName => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        ISubDirectory IVolume.RootDirectory => throw new NotImplementedException();

        IRemoteSubDirectory IRemoteVolume.RootDirectory => throw new NotImplementedException();

        IFileSystem IVolume.FileSystem => throw new NotImplementedException();

        IRemoteFileSystem IRemoteVolume.FileSystem => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
