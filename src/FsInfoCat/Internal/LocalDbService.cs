using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using System;

namespace FsInfoCat.Internal
{
    internal class LocalDbService : ILocalDbService
    {
        private Func<ILocalDbContext> _factory;

        public ILocalDbContext GetDbContext()
        {
            throw new NotImplementedException();
        }

        IDbContext IDbService.GetDbContext() => GetDbContext();

        public void SetContextFactory(Func<ILocalDbContext> factory)
        {
            if (_factory is null)
                _factory = factory;
            else
                throw new InvalidOperationException();
        }
    }
}
