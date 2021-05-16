using System;

namespace FsInfoCat
{
    public interface IDbService
    {
        Model.IDbContext GetDbContext();
    }

    public interface IDbService<TDbContext> : IDbService
        where TDbContext : Model.IDbContext
    {
        void SetContextFactory(Func<TDbContext> factory);

        new TDbContext GetDbContext();
    }
}
