using System;

namespace CodeGeneration
{
    public abstract class ValueReference : IEquatable<ValueReference>
    {
        public abstract bool Equals(ValueReference other);
        public abstract string ToSqlString();
        public abstract string ToCsString();
    }
}
