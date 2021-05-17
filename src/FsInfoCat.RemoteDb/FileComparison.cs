using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;

namespace FsInfoCat.RemoteDb
{
    public class FileComparison : IRemoteFileComparison
    {
        public FsFile File1 => throw new NotImplementedException();

        public FsFile File2 => throw new NotImplementedException();

        public bool AreEqual => throw new NotImplementedException();

        public Guid FileId1 => throw new NotImplementedException();

        public Guid FileId2 => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IFile IFileComparison.File1 => throw new NotImplementedException();

        IRemoteFile IRemoteFileComparison.File1 => throw new NotImplementedException();

        IFile IFileComparison.File2 => throw new NotImplementedException();

        IRemoteFile IRemoteFileComparison.File2 => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();
    }
}
