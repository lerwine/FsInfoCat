namespace FsInfoCat.Desktop.ViewModel
{
    public interface IEntityEditResult<TEntity>
        where TEntity : DbEntity
    {
        TEntity ItemEntity { get; }

        EntityEditResultState State { get; }
    }
}
