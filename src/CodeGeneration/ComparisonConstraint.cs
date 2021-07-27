using System;

namespace CodeGeneration
{
    public sealed class ComparisonConstraint : CheckConstraint, IEquatable<ComparisonConstraint>
    {
        private ComparisonConstraint(ColumnValueReference lValue, string sqlOp, string csOp, ValueReference rValue)
        {
            LValue = lValue;
            SqlOp = sqlOp;
            CsOp = csOp;
            RValue = rValue;
        }
        public ColumnValueReference LValue { get; }
        public string SqlOp { get; }
        public string CsOp { get; }
        public ValueReference RValue { get; }
        public override bool IsCompound => false;
        public static ComparisonConstraint AreEqual(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "=", "==", rValue);
        public static ComparisonConstraint NotEqual(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<>", "!=", rValue);
        public static ComparisonConstraint LessThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<", "<", rValue);
        public static ComparisonConstraint NotGreaterThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, "<=", "<=", rValue);
        public static ComparisonConstraint GreaterThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, ">", ">", rValue);
        public static ComparisonConstraint NotLessThan(ColumnValueReference lValue, ValueReference rValue) => new(lValue, ">=", ">=", rValue);
        public bool Equals(ComparisonConstraint other) => other is not null && (ReferenceEquals(this, other) || (LValue.Equals(other.LValue) && SqlOp == other.SqlOp && RValue.Equals(other.RValue)));
        public override bool Equals(CheckConstraint other) => other is ComparisonConstraint o && Equals(o);
        public override bool Equals(object obj) => obj is ComparisonConstraint other && Equals(other);
        public override int GetHashCode() { unchecked { return (((LValue.GetHashCode() * 3) ^ SqlOp.GetHashCode()) * 5) ^ RValue.GetHashCode(); } }
        public override string ToString() => ToSqlString();
        public override string ToSqlString() => $"{LValue.ToSqlString()}{SqlOp}{RValue.ToSqlString()}";
        public override string ToCsString() => $"{LValue.ToCsString()} {CsOp} {RValue.ToCsString()}";
    }
}
