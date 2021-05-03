namespace FsInfoCat.Desktop.Model.Validation
{
    public interface IComponentRelay
    {
        object Component { get; }
    }

    public interface IComponentRelay<TComponent> : IComponentRelay
    {
        new TComponent Component { get; }
    }
}
