using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace DevUtil
{
    public abstract class EnhancedTypeDescriptor : IEquatable<EnhancedTypeDescriptor>
    {
        private static readonly Collection<EnhancedTypeDescriptor> _cache = new();

        public Type Type { get; }

        public abstract string BaseName { get; }

        public abstract string Namespace { get; }

        public abstract string ComponentName { get; }

        public TypeCategory Category { get; }

        public abstract bool IsLocalEntityType { get; }

        public abstract bool ImplementsIDbEntity { get; }

        public abstract EnhancedTypeDescriptor BaseType { get; }

        public abstract EnhancedTypeDescriptor UnderlyingType { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> EquatableTo { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> EnumerableInterfaces { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> ImplementedEntityTypes { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> OtherInterfaceTypes { get; }

        public ReadOnlyCollection<EnhancedPropertyDescriptor> Properties { get; }
        public EnhancedTypeDescriptor DeclaringType { get; }

        public static IEnumerable<EnhancedTypeDescriptor> GetFsInfoCatTypes() => ReflectionExtensions.LocalAssembly.GetTypes().Concat(ReflectionExtensions.BaseAssembly.GetTypes()).Select(t => EnhancedDefinedTypeDescriptor.Get(t));

        public static IEnumerable<EnhancedTypeDescriptor> GetBaseAssemblyTypes() => ReflectionExtensions.BaseAssembly.GetTypes().Select(t => EnhancedDefinedTypeDescriptor.Get(t));

        public static IEnumerable<EnhancedTypeDescriptor> GetLocalAssemblyTypes() => ReflectionExtensions.LocalAssembly.GetTypes().Select(t => EnhancedDefinedTypeDescriptor.Get(t));

        public static IEnumerable<EnhancedTypeDescriptor> GetDbEntityTypes() => ReflectionExtensions.LocalAssembly.GetTypes().Concat(ReflectionExtensions.BaseAssembly.GetTypes()).Where(t => t.ImplementsIDbEntity()).Select(t => EnhancedDefinedTypeDescriptor.Get(t));

        public static IEnumerable<EnhancedTypeDescriptor> GetBaseDbEntityTypes() => ReflectionExtensions.BaseAssembly.GetTypes().Where(t => t.ImplementsIDbEntity()).Select(t => EnhancedDefinedTypeDescriptor.Get(t));

        public static IEnumerable<EnhancedTypeDescriptor> GetLocalDbEntityTypes() => ReflectionExtensions.LocalAssembly.GetTypes().Concat(ReflectionExtensions.BaseAssembly.GetTypes()).Where(t => t.ImplementsILocalDbEntity()).Select(t => EnhancedDefinedTypeDescriptor.Get(t));

        protected EnhancedTypeDescriptor([DisallowNull] Type type)
        {
            Type = type;
            _cache.Add(this);
            Collection<EnhancedTypeDescriptor> equatableTo = new();
            Collection<EnhancedTypeDescriptor> enumerableInterfaces = new();
            Collection<EnhancedTypeDescriptor> implementedEntityTypes = new();
            Collection<EnhancedTypeDescriptor> otherInterfaceTypes = new();
            Collection<EnhancedPropertyDescriptor> properties = new();
            EquatableTo = new(equatableTo);
            EnumerableInterfaces = new(enumerableInterfaces);
            ImplementedEntityTypes = new(implementedEntityTypes);
            OtherInterfaceTypes = new(otherInterfaceTypes);
            Properties = new(properties);
            if (type.IsArray)
                Category = TypeCategory.Array;
            else if (type.IsByRef)
                Category = TypeCategory.ByRef;
            else if (type.IsPointer)
                Category = TypeCategory.Pointer;
            else if (type.IsEnum)
                Category = TypeCategory.Enum;
            else if (type.IsPrimitive)
                Category = TypeCategory.Primitive;
            else if (type.IsConstructedNullableType())
                Category = TypeCategory.Nullable;
            else if (type.IsValueType)
                Category = TypeCategory.Struct;
            else if (type.IsInterface)
                Category = TypeCategory.Interface;
            else
                Category = TypeCategory.Class;
            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsConstructedGenericType)
                {
                    Type d = i.GetGenericTypeDefinition();
                    if (d.Equals(typeof(IEnumerable<>)))
                    {
                        enumerableInterfaces.Add(new EnhancedConstructedTypeDescriptor(i));
                        continue;
                    }
                    if (d.Equals(typeof(IEquatable<>)))
                    {
                        equatableTo.Add(EnhancedDefinedTypeDescriptor.Get(i.GetGenericArguments()[0]));
                        continue;
                    }
                }
                EnhancedTypeDescriptor td = EnhancedDefinedTypeDescriptor.Get(i);
                if (td.ImplementsIDbEntity)
                    implementedEntityTypes.Add(td);
                else
                    otherInterfaceTypes.Add(td);
            }
            if (type.IsNested)
                DeclaringType = EnhancedDefinedTypeDescriptor.Get(type.DeclaringType);
            EnhancedPropertyDescriptor.Create(this, properties);
        }

        protected static EnhancedTypeDescriptor Get(Type type, Func<EnhancedDefinedTypeDescriptor> createFunc)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));
            Monitor.Enter(_cache);
            try { return _cache.FirstOrDefault(t => t.Type.Equals(type)) ?? ((type.IsArray || type.IsByRef || type.IsPointer || type.IsConstructedGenericType) ? (EnhancedTypeDescriptor)new EnhancedConstructedTypeDescriptor(type) : createFunc()); }
            finally { Monitor.Exit(_cache); }
        }

        public override bool Equals(object obj) => Equals(obj as EnhancedTypeDescriptor);

        public bool Equals(EnhancedTypeDescriptor other) => other is not null && Type.Equals(other.Type);

        public override int GetHashCode() => Type.AssemblyQualifiedName.GetHashCode();
    }

    public sealed class EnhancedDefinedTypeDescriptor : EnhancedTypeDescriptor
    {
        private readonly string _baseName;
        private readonly string _componentName;
        private readonly string _namespace;
        private readonly EnhancedTypeDescriptor _underlyingType;
        private readonly EnhancedTypeDescriptor _baseType;
        private readonly bool _implementsIDbEntity;
        private readonly bool _isLocalEntityType;

        public override string BaseName => _baseName;

        public override string Namespace => _namespace;

        public override string ComponentName => _componentName;

        public override EnhancedTypeDescriptor BaseType => _baseType;

        public override EnhancedTypeDescriptor UnderlyingType => _underlyingType;

        public override bool IsLocalEntityType => _implementsIDbEntity;

        public override bool ImplementsIDbEntity => _isLocalEntityType;

        private EnhancedDefinedTypeDescriptor([DisallowNull] Type type) : base(type)
        {
            switch (Category)
            {
                case TypeCategory.Enum:
                    _baseType = _underlyingType = Get(Enum.GetUnderlyingType(type));
                    _componentName = _baseName = type.Name;
                    _namespace = type.Namespace ?? "";
                    break;
                case TypeCategory.Primitive:
                    if (type == typeof(bool))
                        _baseName = "bool";
                    else if (type == typeof(short))
                        _baseName = "short";
                    else if (type == typeof(ushort))
                        _baseName = "ushort";
                    else if (type == typeof(int))
                        _baseName = "int";
                    else if (type == typeof(uint))
                        _baseName = "uint";
                    else if (type == typeof(long))
                        _baseName = "long";
                    else if (type == typeof(ulong))
                        _baseName = "ulong";
                    else if (type == typeof(float))
                        _baseName = "float";
                    else
                        _baseName = type.Name.ToLower();
                    _namespace = "";
                    _componentName = _baseName;
                    break;
                case TypeCategory.Array:
                case TypeCategory.ByRef:
                case TypeCategory.Pointer:
                case TypeCategory.Nullable:
                    throw new ArgumentException("Nullable, array, ByRef, and pointer types not supported", nameof(type));
                default:
                    if (type.IsConstructedGenericType)
                        throw new ArgumentException("Constructed generic types not supported", nameof(type));
                    if (type.Equals(typeof(decimal)) || type.Equals(typeof(void)))
                    {
                        _componentName = _baseName = type.Name.ToLower();
                        _namespace = "";
                    }
                    else
                    {
                        if (type.IsGenericType)
                        {
                            int i = type.Name.IndexOf("`");
                            _baseName = (i < 0) ? type.Name : type.Name.Substring(0, i);
                        }
                        else
                            _baseName = type.Name;
                        _namespace = type.Namespace ?? "";
                        string componentName = TypeDescriptor.GetComponentName(type);
                        _componentName = string.IsNullOrWhiteSpace(componentName) ? BaseName : componentName;
                        if (!(type.IsValueType || type.IsInterface || type.BaseType is null || type.BaseType == typeof(object)))
                            _baseType = Get(type.BaseType);
                        if (type.ImplementsIDbEntity())
                        {
                            _implementsIDbEntity = true;
                            _isLocalEntityType = type.ImplementsILocalDbEntity();
                        }
                    }
                    break;
            }
        }

        public static EnhancedTypeDescriptor Get([DisallowNull] Type type) => Get(type, () => new EnhancedDefinedTypeDescriptor(type));
    }

    public sealed class EnhancedConstructedTypeDescriptor : EnhancedTypeDescriptor
    {
        private readonly string _baseName;
        private readonly string _componentName;
        private readonly string _namespace;
        private readonly EnhancedTypeDescriptor _underlyingType;
        private readonly EnhancedTypeDescriptor _baseType;
        private readonly bool _implementsIDbEntity;
        private readonly bool _isLocalEntityType;

        public override string BaseName => _baseName;

        public override string Namespace => _namespace;

        public override string ComponentName => _componentName;

        public override EnhancedTypeDescriptor BaseType => _baseType;

        public override EnhancedTypeDescriptor UnderlyingType => _underlyingType;

        public EnhancedTypeDescriptor ElementType { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> GenericArguments { get; }

        public override bool IsLocalEntityType => _implementsIDbEntity;

        public override bool ImplementsIDbEntity => _isLocalEntityType;

        public EnhancedConstructedTypeDescriptor([DisallowNull] Type type) : base(type)
        {
            Collection<EnhancedTypeDescriptor> genericArguments = new();
            GenericArguments = new(genericArguments);
            switch (Category)
            {
                case TypeCategory.Array:
                case TypeCategory.ByRef:
                case TypeCategory.Pointer:
                    _baseName = (_underlyingType = EnhancedDefinedTypeDescriptor.Get(type.GetElementType())).BaseName;
                    _componentName = UnderlyingType.ComponentName;
                    _namespace = UnderlyingType.Namespace;
                    break;
                case TypeCategory.Nullable:
                    _baseType = EnhancedDefinedTypeDescriptor.Get(typeof(Nullable<>));
                    _baseName = (_underlyingType = EnhancedDefinedTypeDescriptor.Get(Nullable.GetUnderlyingType(type))).BaseName;
                    _implementsIDbEntity = UnderlyingType.ImplementsIDbEntity;
                    _isLocalEntityType = UnderlyingType.IsLocalEntityType;
                    _componentName = UnderlyingType.ComponentName;
                    _namespace = UnderlyingType.Namespace;
                    break;
                default:
                    if (type.IsConstructedGenericType)
                    {
                        int i = type.Name.IndexOf("`");
                        _baseName = (i < 0) ? type.Name : type.Name.Substring(0, i);
                        _namespace = type.Namespace ?? "";
                        string componentName = TypeDescriptor.GetComponentName(type);
                        _componentName = string.IsNullOrWhiteSpace(componentName) ? BaseName : componentName;
                        foreach (Type t in type.GetGenericArguments())
                            genericArguments.Add(EnhancedDefinedTypeDescriptor.Get(t));
                        if (type.ImplementsIDbEntity())
                        {
                            _implementsIDbEntity = true;
                            _isLocalEntityType = type.ImplementsILocalDbEntity();
                        }
                    }
                    else
                    {
                        if (type.IsGenericType)
                            throw new ArgumentException("Unconstructed generic types not supported", nameof(type));
                        if (Category == TypeCategory.Struct || Category == TypeCategory.Interface)
                            throw new ArgumentException("Struct and Interface types not supported unless constructed generic type", nameof(type));
                        if (type.Equals(typeof(string)))
                        {
                            _componentName = _baseName = "string";
                            _namespace = "";
                        }
                        else
                        {
                            if (!(type.IsValueType || type.BaseType is null || type.BaseType == typeof(object)))
                                _baseType = EnhancedDefinedTypeDescriptor.Get(type.BaseType);
                            if (type.ImplementsIDbEntity())
                            {
                                _implementsIDbEntity = true;
                                _isLocalEntityType = type.ImplementsILocalDbEntity();
                            }
                        }
                    }
                    break;
            }
        }
    }
}
