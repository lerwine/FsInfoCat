using System;

namespace FsInfoCat
{
    public interface IDbService
    {
        string GetConnectionString();
        void SetConnectionString(string connectionString);
        Model.IDbContext GetDbContext();
    }

    public interface IDbService<TDbContext> : IDbService
        where TDbContext : Model.IDbContext
    {
        void SetContextFactory(Func<TDbContext> factory);

        new TDbContext GetDbContext();
    }
}
