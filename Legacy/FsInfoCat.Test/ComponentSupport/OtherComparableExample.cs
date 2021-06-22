using System;

namespace FsInfoCat.Test.ComponentSupport
{
    public class OtherComparableExample : IEquatable<int?>, IComparable<int?>
    {
        public int CompareTo(int? other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(int? other)
        {
            throw new NotImplementedException();
        }
    }

    public class OtherComparableExample<T> : OtherComparableExample, IEquatable<T>, IComparable<T>
        where T : IEquatable<OtherComparableExample>
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
