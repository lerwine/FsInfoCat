using System;

namespace CodeGeneration
{
    public sealed class ColumnValueMethodResultReference : ColumnValueReference, IEquatable<ColumnValueMethodResultReference>
    {
        public const string SqlMethodName_trim = "trim";
        public const string CsNemberName_trim = "Trim()";
        public const string SqlMethodName_length = "length";
        public const string CsNemberName_length = "Length";
        private ColumnValueMethodResultReference(ColumnValueReference column, string methodName, string csMemberName)
        {
            Column = column;
            SqlMethodName = methodName;
        }
        public ColumnValueReference Column { get; }
        public string SqlMethodName { get; }
        public string CsMemberName { get; }
        public override string Name => Column.Name;
        public static ColumnValueMethodResultReference LengthTrimmed(ColumnValueReference column) => (column is ColumnValueMethodResultReference m) ?
            ((m.SqlMethodName == SqlMethodName_length) ? m : Length((m.SqlMethodName == SqlMethodName_trim) ? m : Trim(column))) : Length(Trim(column));
        public static ColumnValueMethodResultReference Trim(ColumnValueReference column)
        {
            if (column is ColumnValueMethodResultReference m)
            {
                if (m.SqlMethodName == SqlMethodName_trim)
                    return m;
                if (m.SqlMethodName == SqlMethodName_length)
                {
                    if (m.Column is ColumnValueMethodResultReference c && c.SqlMethodName == SqlMethodName_trim)
                        return m;
                    return Length(Trim(m.Column));
                }
            }
            return new ColumnValueMethodResultReference(column, SqlMethodName_trim, CsNemberName_trim);
        }
        public static ColumnValueMethodResultReference Length(ColumnValueReference column) => (column is ColumnValueMethodResultReference m && m.SqlMethodName == SqlMethodName_length) ? m :
            new ColumnValueMethodResultReference(column, SqlMethodName_length, CsNemberName_length);
        public bool Equals(ColumnValueMethodResultReference other) => other is not null && (ReferenceEquals(this, other) || (SqlMethodName == other.SqlMethodName && Column.Equals(other.Column)));
        public override bool Equals(ColumnValueReference other) => other is ColumnValueMethodResultReference o && Equals(o);
        public override bool Equals(ValueReference other) => other is ColumnValueMethodResultReference o && Equals(o);
        public override bool Equals(object obj) => obj is ColumnValueMethodResultReference other && Equals(other);
        public override int GetHashCode() { unchecked { return (SqlMethodName.GetHashCode() * 3) ^ Column.GetHashCode(); } }
        public override string ToString() => ToSqlString();
        public override string ToSqlString() => $"{SqlMethodName}({Column.ToSqlString()})";
        public override string ToCsString() => $"{Column.ToCsString()}.{CsMemberName}";
    }
}
