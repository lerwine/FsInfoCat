using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using System;

namespace FsInfoCat.Internal
{
    internal class UpstreamDbService : IUpstreamDbService
    {
        public string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        public IUpstreamDbContext GetDbContext()
        {
            throw new NotImplementedException();
        }

        public void SetConnectionString(string connectionString)
        {
            throw new NotImplementedException();
        }

        public void SetContextFactory(Func<IUpstreamDbContext> factory)
        {
            throw new NotImplementedException();
        }

        IDbContext IDbService.GetDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
