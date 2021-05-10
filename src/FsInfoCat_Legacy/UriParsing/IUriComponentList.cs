using System.Collections.Generic;

namespace FsInfoCat.UriParsing
{
    public interface IUriComponentList<TComponent> : IReadOnlyList<TComponent>, IUriComponent
        where TComponent : class, IUriComponent
    {
    }
}
