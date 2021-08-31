using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface IDbEntityRowViewModel
    {
        DateTime CreatedOn { get; }
        DateTime ModifiedOn { get; }
    }

    public interface IDbEntityRowViewModel<TEntity> : IDbEntityRowViewModel
        where TEntity : DbEntity
    {
        TEntity Entity { get; set; }
    }
}
