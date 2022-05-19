namespace FsInfoCat.Desktop.ViewModel
{
    public interface IEntityEditResult<TEntity>
        where TEntity : Model.DbEntity
    {
        TEntity ItemEntity { get; }

        EntityEditResultState State { get; }
    }
}
