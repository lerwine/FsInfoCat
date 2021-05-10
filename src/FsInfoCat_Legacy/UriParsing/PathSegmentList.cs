using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.UriParsing
{
    public class PathSegmentList<TSegment> : UriComponentList<TSegment>, IPathSegmentList<TSegment>
        where TSegment : class, IUriPathSegment
    {
        private GenericWrapper _wrapper;

        public PathSegmentList(bool isWellFormed) : base(isWellFormed) { }

        public PathSegmentList(IList<TSegment> list) : base(list)
        {
            if (list.Skip(1).Any(s => !s.Delimiter.HasValue))
                throw new ArgumentOutOfRangeException(nameof(list), "1 or more nested segments lack a delimiter.");
        }

        public bool IsRooted { get; }

        public override string ToString()
        {
            // TODO: Implement ToString()
            throw new NotImplementedException();
        }

        internal IPathSegmentList<IUriPathSegment> GetGenericWraper()
        {
            GenericWrapper wrapper = _wrapper;
            if (wrapper is null)
                _wrapper = wrapper = new GenericWrapper(this);
            return wrapper;
        }

        private class GenericWrapper : IPathSegmentList<IUriPathSegment>
        {
            private readonly PathSegmentList<TSegment> _backingList;

            internal GenericWrapper(PathSegmentList<TSegment> backingList)
            {
                _backingList = backingList ?? throw new ArgumentNullException(nameof(backingList));
            }

            public IUriPathSegment this[int index] => _backingList[index];

            public int Count => _backingList.Count;

            public bool IsWellFormed => _backingList.IsWellFormed;

            public bool IsRooted => _backingList.IsRooted;

            public IEnumerator<IUriPathSegment> GetEnumerator() => _backingList.Cast<IUriPathSegment>().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _backingList.Cast<IUriPathSegment>().GetEnumerator();
        }
    }
}
