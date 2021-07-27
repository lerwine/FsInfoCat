using System;

namespace CodeGeneration
{
    public sealed class NullCheckConstraint : CheckConstraint, IEquatable<NullCheckConstraint>
    {
        public NullCheckConstraint(SimpleColumnValueReference column, bool isNull)
        {
            Column = column;
            IsNull = isNull;
        }
        public SimpleColumnValueReference Column { get; }
        public bool IsNull { get; }
        public override bool IsCompound => false;
        public bool Equals(NullCheckConstraint other) => other is not null && (ReferenceEquals(this, other) || (IsNull == other.IsNull && Column.Equals(other.Column)));
        public override bool Equals(CheckConstraint other) => other is NullCheckConstraint o && Equals(o);
        public override bool Equals(object obj) => obj is NullCheckConstraint other && Equals(other);
        public override int GetHashCode() { unchecked { return (IsNull ? 0 : 3) ^ Column.GetHashCode(); } }
        public override string ToString() => ToSqlString();
        public override string ToSqlString() => IsNull ? $"{Column.ToSqlString()} IS NULL" : $"{Column.ToSqlString()} IS NOT NULL";
        public override string ToCsString() => IsNull ? $"{Column.ToCsString()} is null" : $"{Column.ToCsString()} is not null";
    }
}
