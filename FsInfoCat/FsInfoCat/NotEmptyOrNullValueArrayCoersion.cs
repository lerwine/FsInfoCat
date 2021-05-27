namespace FsInfoCat
{
    public class NotEmptyOrNullValueArrayCoersion<T> : ArrayCoersion<T>
    {
        public NotEmptyOrNullValueArrayCoersion(ICoersion<T> coersion) : base(coersion) { }

        public NotEmptyOrNullValueArrayCoersion() : this(null) { }

        public static T[] NullIfEmpty(T[] array) => (array is null || array.Length == 0) ? null : array;

        public override T[] Cast(object obj) => NullIfEmpty(base.Cast(obj));

        public override T[] Coerce(object obj) => NullIfEmpty(base.Coerce(obj));

        public override T[] Normalize(T[] obj) => (obj is null || obj.Length == 0) ? null : obj;

        public override bool Equals(T[] x, T[] y)
        {
            return base.Equals(NullIfEmpty(x), NullIfEmpty(y));
        }

        public override int GetHashCode(T[] obj)
        {
            return base.GetHashCode(NullIfEmpty(obj));
        }

        public override bool TryCast(object obj, out T[] result)
        {
            bool r = base.TryCast(obj, out result);
            if (!(result is null || result.Length > 0))
                result = null;
            return r;
        }

        public override bool TryCoerce(object obj, out T[] result)
        {
            bool r = base.TryCoerce(obj, out result);
            if (!(result is null || result.Length > 0))
                result = null;
            return r;
        }
    }
}
