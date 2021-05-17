using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class HostPlatform : IHostPlatform
    {
        public Guid Id => throw new NotImplementedException();

        public string DisplayName => throw new NotImplementedException();

        public PlatformType Type => throw new NotImplementedException();

        public Guid? DefaultFsTypeId => throw new NotImplementedException();

        public string Notes => throw new NotImplementedException();

        public bool IsInactive => throw new NotImplementedException();

        public FileSystem DefaultFSType => throw new NotImplementedException();

        public HashSet<HostDevice> HostDevices => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IRemoteFileSystem IHostPlatform.DefaultFSType => throw new NotImplementedException();

        IReadOnlyCollection<IHostDevice> IHostPlatform.HostDevices => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }
    }
}
