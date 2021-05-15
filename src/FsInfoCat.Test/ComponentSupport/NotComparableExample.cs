using System;

namespace FsInfoCat.Test.ComponentSupport
{
    public class NotComparableExample
    {
        public int CompareTo(NotComparableExample other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(NotComparableExample other)
        {
            throw new NotImplementedException();
        }
    }

    public class NotComparableExample<T>
        where T : IEquatable<T>, IComparable<T>
    {
        public int CompareTo(T other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(T other)
        {
            throw new NotImplementedException();
        }
    }
}
