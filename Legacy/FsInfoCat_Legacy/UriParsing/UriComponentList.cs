using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.UriParsing
{
    public abstract class UriComponentList<TComponent> : ReadOnlyCollection<TComponent>, IUriComponentList<TComponent>
        where TComponent : class, IUriComponent
    {
        protected UriComponentList(bool isWellFormed) : base(Array.Empty<TComponent>())
        {
            IsWellFormed = isWellFormed;
        }

        protected UriComponentList(IList<TComponent> list) : base((list ?? throw new ArgumentNullException(nameof(list))).ToArray())
        {
            if (list.Any(e => e is null))
                throw new ArgumentOutOfRangeException(nameof(list));
            IsWellFormed = list.All(e => e.IsWellFormed);
        }

        protected UriComponentList(IList<TComponent> list, bool isWellFormed) : base((list ?? throw new ArgumentNullException(nameof(list))).ToArray())
        {
            if (list.Any(e => e is null))
                throw new ArgumentOutOfRangeException(nameof(list));
            IsWellFormed = isWellFormed;
        }

        public bool IsWellFormed { get; }

        public abstract override string ToString();
    }
}
