using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public interface IDbEntityRowViewModel
    {
        DateTime CreatedOn { get; }
        DateTime ModifiedOn { get; }
        Model.DbEntity Entity { get; }
    }

    public interface IDbEntityRowViewModel<TEntity> : IDbEntityRowViewModel
        where TEntity : Model.DbEntity
    {
        new TEntity Entity { get; set; }
    }
}
