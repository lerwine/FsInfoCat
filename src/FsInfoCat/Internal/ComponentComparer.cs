using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Internal
{
    class ComponentComparer<T> : IEqualityComparer<T>, IEqualityComparer
        where T : class
    {
        public static readonly ReferenceEqualityComparer<T> Instance = new ReferenceEqualityComparer<T>();
        private readonly PropertyDescriptor[] _properties;

        public ComponentComparer(PropertyDescriptor[] properties)
        {
            _properties = properties;
        }

        public bool Equals(T x, T y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }

        bool IEqualityComparer.Equals(object x, object y) => (x is null) ? y is null : ((x is string a) ? (y is string b) && a == b : !(y is string) && x.Equals(y));

        int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : ((obj is T t) ? GetHashCode(t) : obj.GetHashCode());
    }
}
