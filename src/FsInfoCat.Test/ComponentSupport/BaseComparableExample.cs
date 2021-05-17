using System;

namespace FsInfoCat.Test.ComponentSupport
{
    public class BaseComparableExample : NotComparableExample, IEquatable<NotComparableExample>, IComparable<NotComparableExample>
    {
    }
}
