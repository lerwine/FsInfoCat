using System;
using System.Xml;

namespace DevUtil
{
    public interface ITypeMapper
    {
        bool IsMappedType(Type type);
        XmlQualifiedName ToXsdType(Type type);
    }
}
