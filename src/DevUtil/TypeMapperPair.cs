using System;
using System.Xml;

namespace DevUtil
{
    public class TypeMapperPair<T, U> : ITypeMapper
        where T : ITypeMapper
        where U : ITypeMapper
    {
        public T PrimaryMapper { get; }

        public T SecondaryMapper { get; }

        public TypeMapperPair(T primaryMapper, T secondaryMapper)
        {
            PrimaryMapper = primaryMapper ?? throw new ArgumentNullException(nameof(primaryMapper));
            SecondaryMapper = secondaryMapper ?? throw new ArgumentNullException(nameof(secondaryMapper));
        }

        public bool IsMappedType(Type type) => type is not null && PrimaryMapper.IsMappedType(type) ||SecondaryMapper.IsMappedType(type);

        public XmlQualifiedName ToXsdType(Type type)
        {
            if (type is null) return null;
            return PrimaryMapper.ToXsdType(type) ?? SecondaryMapper.ToXsdType(type);
        }
    }
}
