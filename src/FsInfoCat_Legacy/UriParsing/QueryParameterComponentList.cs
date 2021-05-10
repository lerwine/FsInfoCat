using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.UriParsing
{
    public sealed class QueryParameterComponentList<TParameter> : UriComponentList<TParameter>
        where TParameter : class, IUriParameterElement
    {
        private GenericWrapper _wrapper;

        public QueryParameterComponentList(bool isWellFormed) : base(isWellFormed) { }

        public QueryParameterComponentList(IList<TParameter> list) : base(list) { }

        internal IUriComponentList<IUriParameterElement> GetGenericWraper()
        {
            GenericWrapper wrapper = _wrapper;
            if (wrapper is null)
                _wrapper = wrapper = new GenericWrapper(this);
            return wrapper;
        }
        public override string ToString()
        {
            switch (Count)
            {
                case 0:
                    return "?";
                case 1:
                    return $"?{ToString(this[0])}";
                default:
                    return $"?{String.Join("&", Items.Select(ToString))}";
            }
        }

        public static string ToString(TParameter parameter)
        {
            if (parameter is null)
                return "";
            string key = parameter.Key;
            string value = parameter.Value ?? "";
            return (key is null) ? value : $"{key}={value}";
        }

        private class GenericWrapper : IUriComponentList<IUriParameterElement>
        {
            private readonly QueryParameterComponentList<TParameter> _backingList;

            internal GenericWrapper(QueryParameterComponentList<TParameter> backingList)
            {
                _backingList = backingList ?? throw new ArgumentNullException(nameof(backingList));
            }

            public IUriParameterElement this[int index] => _backingList[index];

            public int Count => _backingList.Count;

            public bool IsWellFormed => _backingList.IsWellFormed;

            public IEnumerator<IUriParameterElement> GetEnumerator() => _backingList.Cast<IUriParameterElement>().GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => _backingList.Cast<IUriParameterElement>().GetEnumerator();
        }
    }
}
