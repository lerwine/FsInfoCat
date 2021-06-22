using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using System;

namespace FsInfoCat.Internal
{
    internal class UpstreamDbService : IUpstreamDbService
    {
        private string _connectionString;
        private Func<IUpstreamDbContext> _factory;
        public string GetConnectionString() => _connectionString;

        public void SetConnectionString(string connectionString) => _connectionString = connectionString ?? "";

        public IUpstreamDbContext GetDbContext()
        {
            var factory = _factory;
            if (factory is null)
                throw new InvalidOperationException("ContextFactory not initialized");
            return factory();
        }

        IDbContext IDbService.GetDbContext() => GetDbContext();

        public void SetContextFactory(Func<IUpstreamDbContext> factory)
        {
            if (_factory is null)
                _factory = factory;
            else
                throw new InvalidOperationException();
        }
    }
}
