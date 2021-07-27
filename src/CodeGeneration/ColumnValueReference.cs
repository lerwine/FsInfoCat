using System;

namespace CodeGeneration
{
    public abstract class ColumnValueReference : ValueReference, IEquatable<ColumnValueReference>
    {
        public abstract string Name { get; }
        public abstract bool Equals(ColumnValueReference other);
        internal ComparisonConstraint LessThanLiteral(int value) => ComparisonConstraint.LessThan(this, ConstantValueReference.Of(value));
        internal ComparisonConstraint NotLessThanLiteral(int minValue) => ComparisonConstraint.NotLessThan(this, ConstantValueReference.Of(minValue));
        internal ComparisonConstraint GreaterThanLiteral(int value) => ComparisonConstraint.GreaterThan(this, ConstantValueReference.Of(value));
        internal ComparisonConstraint IsEqualTo(ValueReference value) => ComparisonConstraint.AreEqual(this, value);
        internal ColumnValueReference Length() => ColumnValueMethodResultReference.Length(this);
        internal ColumnValueReference Trimmed() => ColumnValueMethodResultReference.Trim(this);
    }
}
