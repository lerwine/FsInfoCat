using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using System;

namespace FsInfoCat.Internal
{
    internal class LocalDbService : ILocalDbService
    {
        public ILocalDbContext GetDbContext()
        {
            throw new NotImplementedException();
        }

        public void SetContextFactory(Func<ILocalDbContext> factory)
        {
            throw new NotImplementedException();
        }

        IDbContext IDbService.GetDbContext()
        {
            throw new NotImplementedException();
        }
    }
}
