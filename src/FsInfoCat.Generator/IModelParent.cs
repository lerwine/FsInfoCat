namespace FsInfoCat.Generator
{
    public interface IModelParent : IModelDefinition
    {
        ModelCollection Components { get; }
    }
}
