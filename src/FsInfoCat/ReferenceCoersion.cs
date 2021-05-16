namespace FsInfoCat
{
    internal class ReferenceCoersion<T> : Coersion<T>
        where T : class
    {
        public override bool TryCoerce(object obj, out T result)
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
