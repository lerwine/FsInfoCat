using FsInfoCat.Model;
using FsInfoCat.Model.Local;
using System;

namespace FsInfoCat.Internal
{
    internal class LocalDbService : ILocalDbService
    {
        private Func<ILocalDbContext> _factory;
        private string _connectionString;

        public ILocalDbContext GetDbContext()
        {
            var factory = _factory;
            if (factory is null)
                throw new InvalidOperationException("ContextFactory not initialized");
            return factory();
        }

        IDbContext IDbService.GetDbContext() => GetDbContext();

        public void SetContextFactory(Func<ILocalDbContext> factory)
        {
            if (_factory is null)
                _factory = factory;
            else
                throw new InvalidOperationException();
        }

        public string GetConnectionString() => _connectionString;

        public void SetConnectionString(string connectionString) => _connectionString = connectionString ?? "";
    }
}
