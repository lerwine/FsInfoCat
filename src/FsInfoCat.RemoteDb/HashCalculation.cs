using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;
using System.Collections.Generic;

namespace FsInfoCat.RemoteDb
{
    public class HashCalculation : IRemoteHashCalculation
    {
        public HashSet<FsFile> Files => throw new NotImplementedException();

        public byte[] Data => throw new NotImplementedException();

        public Guid Id => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public Guid CreatedById => throw new NotImplementedException();

        public Guid ModifiedById => throw new NotImplementedException();

        public UserProfile CreatedBy => throw new NotImplementedException();

        public UserProfile ModifiedBy => throw new NotImplementedException();

        public DateTime CreatedOn => throw new NotImplementedException();

        public DateTime ModifiedOn => throw new NotImplementedException();

        IReadOnlyCollection<IFile> IHashCalculation.Files => throw new NotImplementedException();

        IReadOnlyCollection<IRemoteFile> IRemoteHashCalculation.Files => throw new NotImplementedException();

        IReadOnlyCollection<byte> IHashCalculation.Data => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => throw new NotImplementedException();

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => throw new NotImplementedException();

        public bool TryGetMD5Checksum(out UInt128 result)
        {
            throw new NotImplementedException();
        }
    }
}
