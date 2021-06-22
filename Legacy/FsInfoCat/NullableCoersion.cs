using System.ComponentModel;

namespace FsInfoCat
{
    internal class NullableCoersion<T> : Coersion<T?>
        where T : struct
    {
        public override bool TryCoerce(object obj, out T? result)
        {
            if (obj is null)
                result = null;
            else if (obj is T t)
                result = t;
            else
            {
                result = null;
                return false;
            }
            return true;
        }
    }
}
