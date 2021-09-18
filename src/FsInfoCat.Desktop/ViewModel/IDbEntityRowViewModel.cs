using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface IDbEntityRowViewModel
    {
        DateTime CreatedOn { get; }
        DateTime ModifiedOn { get; }
        DbEntity Entity { get; }
    }

    public interface IDbEntityRowViewModel<TEntity> : IDbEntityRowViewModel
        where TEntity : DbEntity
    {
        new TEntity Entity { get; set; }
    }
}
