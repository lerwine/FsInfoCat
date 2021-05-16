using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using System;

namespace FsInfoCat.Internal
{
    internal class RemoteDbService : IRemoteDbService
    {
        public IRemoteDbContext GetDbContext()
        {
            throw new NotImplementedException();
        }

        public void SetContextFactory(Func<IRemoteDbContext> factory)
        {
            throw new NotImplementedException();
        }

        IDbContext IDbService.GetDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
