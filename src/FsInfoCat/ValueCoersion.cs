using System;
using System.ComponentModel;

namespace FsInfoCat
{
    internal class ValueCoersion<T> : Coersion<T>
        where T : struct
    {
        public override bool TryCoerce(object obj, out T result)
        {
            if (obj is T t)
            {
                result = t;
                return true;
            }
            result = default;
            return false;
        }
    }
}
