using System;
using System.Runtime.Serialization;

namespace FsInfoCat.Test.ComponentSupport
{
    public class BaseComparableExample : NotComparableExample, IEquatable<NotComparableExample>, IComparable<NotComparableExample>
    {
    }
}
