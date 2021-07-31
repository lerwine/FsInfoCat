using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using static CodeGeneration.CgConstants;

namespace CodeGeneration
{
    public abstract class CheckConstraint : IEquatable<CheckConstraint>
    {
        public abstract bool IsCompound { get; }
        public static CheckConstraint Import(XElement checkElement)
        {
            if (checkElement is null)
                return null;
            if (checkElement.Name == XNAME_Check || checkElement.Name == XNAME_And)
            {
                CheckConstraint[] constraints = checkElement.Elements().Select(e => Import(e)).Where(c => c is not null).ToArray();
                if (constraints.Length == 1)
                    return constraints[0];
                if (constraints.Length > 1)
                    return new ComparisonGroup(false, constraints);
                return null;
            }
            if (checkElement.Name == XNAME_Or)
            {
                CheckConstraint[] constraints = checkElement.Elements().Select(e => Import(e)).Where(c => c is not null).ToArray();
                if (constraints.Length == 1)
                    return constraints[0];
                if (constraints.Length > 1)
                    return new ComparisonGroup(true, constraints);
                return null;
            }
            SimpleColumnValueReference name = new(checkElement.Attribute(XNAME_Name)?.Value);
            if (checkElement.Name == XNAME_IsNull)
                return new NullCheckConstraint(name, true);
            if (checkElement.Name == XNAME_NotNull)
                return new NullCheckConstraint(name, false);

            ColumnValueReference lValue = (checkElement.AttributeToBoolean(XNAME_Trimmed) ?? false) ? ColumnValueMethodResultReference.Trim(name) : name;

            if (checkElement.AttributeToBoolean(XNAME_Length) ?? false)
                lValue = ColumnValueMethodResultReference.Length(lValue);
            XElement other = checkElement.Elements().First();
            ValueReference rValue;
            if (other.Name == XNAME_OtherProperty)
            {
                name = new SimpleColumnValueReference(other.Attribute(XNAME_Name)?.Value);
                ColumnValueReference r = (other.AttributeToBoolean(XNAME_Trimmed) ?? false) ? ColumnValueMethodResultReference.Trim(name) : name;
                rValue = (other.AttributeToBoolean(XNAME_Length) ?? false) ? ColumnValueMethodResultReference.Length(r) : r;
            }
            else if (other.Name == XNAME_True)
                rValue = ConstantValueReference.Of(true);
            else if (other.Name == XNAME_False)
                rValue = ConstantValueReference.Of(false);
            else if (other.Name == XNAME_Now)
                rValue = ConstantValueReference.Now();
            else
            {
                string t = other.Attribute(XNAME_Value)?.Value;
                if (other.Name == XNAME_Byte)
                    rValue = ConstantValueReference.Byte(t);
                else if (other.Name == XNAME_SByte)
                    rValue = ConstantValueReference.SByte(t);
                else if (other.Name == XNAME_Short)
                    rValue = ConstantValueReference.Short(t);
                else if (other.Name == XNAME_UShort)
                    rValue = ConstantValueReference.UShort(t);
                else if (other.Name == XNAME_Int)
                    rValue = ConstantValueReference.Int(t);
                else if (other.Name == XNAME_UInt)
                    rValue = ConstantValueReference.UInt(t);
                else if (other.Name == XNAME_Long)
                    rValue = ConstantValueReference.Long(t);
                else if (other.Name == XNAME_ULong)
                    rValue = ConstantValueReference.ULong(t);
                else if (other.Name == XNAME_Double)
                    rValue = ConstantValueReference.Double(t);
                else if (other.Name == XNAME_Float)
                    rValue = ConstantValueReference.Float(t);
                else if (other.Name == XNAME_Decimal)
                    rValue = ConstantValueReference.Decimal(t);
                else if (other.Name == XNAME_DateTime)
                    rValue = ConstantValueReference.DateTime(t);
                else
                    rValue = ConstantValueReference.String(t);
            }
            if (checkElement.Name == XNAME_LessThan)
                return ComparisonConstraint.LessThan(lValue, rValue);
            if (checkElement.Name == XNAME_NotGreaterThan)
                return ComparisonConstraint.NotGreaterThan(lValue, rValue);
            if (checkElement.Name == XNAME_NotEquals)
                return ComparisonConstraint.NotEqual(lValue, rValue);
            if (checkElement.Name == XNAME_NotLessThan)
                return ComparisonConstraint.NotLessThan(lValue, rValue);
            if (checkElement.Name == XNAME_GreaterThan)
                return ComparisonConstraint.GreaterThan(lValue, rValue);
            return ComparisonConstraint.AreEqual(lValue, rValue);
        }
        public abstract bool Equals(CheckConstraint other);
        public abstract string ToSqlString();
        public abstract string ToCsString();
        public virtual CheckConstraint And(CheckConstraint cc)
        {
            if (cc is null || Equals(cc))
                return this;
            return new ComparisonGroup(false, this, cc);
        }
        public virtual CheckConstraint Or(CheckConstraint cc)
        {
            if (cc is null || Equals(cc))
                return this;
            return new ComparisonGroup(true, this, cc);
        }
    }
}
