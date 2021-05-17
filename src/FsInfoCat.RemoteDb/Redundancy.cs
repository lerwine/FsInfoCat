using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;

namespace FsInfoCat.RemoteDb
{
    public class Redundancy : IRemoteRedundancy
    {
        public HashSet<FsFile> Files => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IReadOnlyCollection<IFile> IRedundancy.Files => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFile> IRemoteRedundancy.Files => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();
    }
}
