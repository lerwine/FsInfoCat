using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public abstract class TypeMapper : ITypeMapper
    {
        private readonly string _namespace;

        public XNamespace Namespace { get; }

        public string Prefix { get; }

        protected TypeMapper(string prefix, string @namespace)
        {
            Prefix = string.IsNullOrEmpty(prefix) ? "" : XmlConvert.VerifyNCName(prefix);
            if (string.IsNullOrEmpty(@namespace))
            {
                _namespace = string.Empty;
                Namespace = XNamespace.None;
            }
            else if (Uri.IsWellFormedUriString(@namespace, UriKind.Absolute))
            {
                _namespace = @namespace;
                Namespace = XNamespace.Get(@namespace);
            }
            else
                throw new ArgumentException(Uri.IsWellFormedUriString(@namespace, UriKind.Relative) ? "Namespace cannot be relative" : "Namespace is not a well-formed URI string");
        }

        public abstract bool IsMappedType(Type type);

        public virtual bool CanMapToXsdType(Type type, string ncName)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            return (ncName is not null) && GetNCNameOrNull(type) == ncName;
        }

        public abstract string GetNCNameOrNull(Type type);

        public virtual XmlQualifiedName ToXsdType(Type type)
        {
            string ncName = GetNCNameOrNull(type);
            return (ncName is null) ? null : new XmlQualifiedName(ncName, _namespace);
        }

        public static bool IsMappedType(Type type, IEnumerable<TypeMapper> mappers)
        {
            if (mappers is null) throw new ArgumentNullException(nameof(mappers));
            foreach (TypeMapper m in mappers)
                if (m is not null && m.IsMappedType(type)) return true;
            return false;
        }

        public static bool IsMappedType(Type type, params TypeMapper[] mappers) => IsMappedType(type, (IEnumerable<TypeMapper>)mappers);

        public static XmlQualifiedName ToXsdType(Type type, IEnumerable<TypeMapper> mappers)
        {
            if (mappers is null) throw new ArgumentNullException(nameof(mappers));
            foreach (TypeMapper m in mappers)
            {
                if (m is null) continue;
                XmlQualifiedName n = m.ToXsdType(type);
                if (n is not null) return n;
            }
            return null;
        }

        public static XmlQualifiedName ToXsdType(Type type, params TypeMapper[] mappers) => ToXsdType(type, (IEnumerable<TypeMapper>)mappers);

        public static bool CanMapToXsdType(Type type, string ncName, IEnumerable<TypeMapper> mappers)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            if (mappers is null) throw new ArgumentNullException(nameof(mappers));
            if (ncName is null) return false;
            foreach (TypeMapper m in mappers)
                if (m is not null && m.CanMapToXsdType(type, ncName)) return true;
            return false;
        }

        public static bool CanMapToXsdType(Type type, string ncName, params TypeMapper[] mappers) => CanMapToXsdType(type, ncName, (IEnumerable<TypeMapper>)mappers);
    }
}
