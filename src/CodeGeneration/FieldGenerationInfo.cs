using System;
using System.Data;
using System.Xml.Linq;
using static CodeGeneration.CgConstants;

namespace CodeGeneration
{
    public  class FieldGenerationInfo : IMemberGenerationInfo
    {
        internal FieldGenerationInfo(XElement fieldElement, EnumGenerationInfo declaringType, Func<string, IComparable> toRawValue)
        {
            Name = (Source = fieldElement).Attribute(XNAME_Name)?.Value;
            DeclaringType = declaringType;
            RawValue = toRawValue(fieldElement.Attribute(XNAME_Value)?.Value);
            Value = PropertyValueCode.OfEnumType(this).Value;
        }
        public string Name { get; }
        public PropertyValueCode Value { get; }
        public IComparable RawValue { get; }
        public DbType Type => DeclaringType.Type;
        public string CsTypeName => DeclaringType.CsTypeName;
        public XElement Source { get; }
        public EnumGenerationInfo DeclaringType { get; }
        ITypeGenerationInfo IMemberGenerationInfo.DeclaringType => DeclaringType;
        public string SqlTypeName => DeclaringType.SqlTypeName;
    }
}
