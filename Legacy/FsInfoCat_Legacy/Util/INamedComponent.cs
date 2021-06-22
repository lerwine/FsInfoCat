using System.ComponentModel;

namespace FsInfoCat.Util
{
    public interface INamedComponent : IComponent
    {
        string Name { get; }
    }
}
