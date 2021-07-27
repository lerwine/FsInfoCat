using System;

namespace CodeGeneration
{
    public sealed class SimpleColumnValueReference : ColumnValueReference, IEquatable<SimpleColumnValueReference>
    {
        private readonly string _name;
        public SimpleColumnValueReference(string name) { _name = name ?? ""; }
        public override string Name => _name;
        public bool Equals(SimpleColumnValueReference other) => other is not null && (ReferenceEquals(this, other) || _name == other._name);
        public override bool Equals(ColumnValueReference other) => other is SimpleColumnValueReference o && Equals(o);
        public override bool Equals(ValueReference other) => other is SimpleColumnValueReference o && Equals(o);
        public override bool Equals(object obj) => obj is SimpleColumnValueReference other && Equals(other);
        public override int GetHashCode() => _name.GetHashCode();
        public override string ToString() => ToSqlString();
        public override string ToCsString() => _name;
        public override string ToSqlString() => $"\"{_name}\"";
    }
}
