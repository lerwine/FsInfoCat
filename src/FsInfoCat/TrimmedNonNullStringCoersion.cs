using System.Collections.Generic;

namespace FsInfoCat
{
    // TODO: Document TrimmedNonNullStringCoersion class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class TrimmedNonNullStringCoersion(IEqualityComparer<string> comparer) : NonNullStringCoersion(comparer)
    {
        public static new readonly TrimmedNonNullStringCoersion Default = new(null);

        public override string Cast(object obj) => base.Cast(obj).Trim();

        public override string Coerce(object obj) => base.Coerce(obj).Trim();

        public override string Normalize(string obj) => (obj is null) ? "" : obj.Trim();

        public override bool TryCast(object obj, out string result)
        {
            if (base.TryCast(obj, out result))
            {
                result = result.Trim();
                return true;
            }
            return false;
        }

        public override bool TryCoerce(object obj, out string result)
        {
            if (base.TryCast(obj, out result))
            {
                result = result.Trim();
                return true;
            }
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
