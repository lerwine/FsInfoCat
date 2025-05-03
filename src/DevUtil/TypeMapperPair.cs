using System;
using System.Xml;

namespace DevUtil
{
    public class TypeMapperPair<T, U>(T primaryMapper, T secondaryMapper) : ITypeMapper
        where T : ITypeMapper
        where U : ITypeMapper
    {
        public T PrimaryMapper { get; } = primaryMapper ?? throw new ArgumentNullException(nameof(primaryMapper));

        public T SecondaryMapper { get; } = secondaryMapper ?? throw new ArgumentNullException(nameof(secondaryMapper));

        public bool IsMappedType(Type type) => type is not null && PrimaryMapper.IsMappedType(type) ||SecondaryMapper.IsMappedType(type);

        public XmlQualifiedName ToXsdType(Type type)
        {
            if (type is null) return null;
            return PrimaryMapper.ToXsdType(type) ?? SecondaryMapper.ToXsdType(type);
        }
    }
}
