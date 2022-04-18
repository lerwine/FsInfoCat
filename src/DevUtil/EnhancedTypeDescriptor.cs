using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace DevUtil
{
    public class EnhancedTypeDescriptor
    {
        private static readonly Collection<EnhancedTypeDescriptor> _cache = new();

        public static readonly Type ValueTypeType = typeof(ValueType);

        public static readonly Type NullableType = typeof(Nullable<>);

        public static readonly Type IEnumerableType = typeof(IEnumerable<>);

        public static readonly Type IEquatableType = typeof(IEquatable<>);

        public Type Type { get; }

        public string TypeName { get; }

        public string BaseName { get; }

        public string Namespace { get; }

        public string ComponentName { get; }

        public TypeCategory Category { get; }

        public bool IsLocalEntityType { get; }

        public bool IsEntityType { get; }

        public EnhancedTypeDescriptor UnderlyingType { get; }

        public EnhancedTypeDescriptor ElementType { get; }

        public EnhancedTypeDescriptor BaseType { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> GenericArguments { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> EquatableTo { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> EnumerableInterfaces { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> ImplementedEntityTypes { get; }

        public ReadOnlyCollection<EnhancedTypeDescriptor> OtherInterfaceTypes { get; }

        public ReadOnlyCollection<EnhancedPropertyDescriptor> Properties { get; }

        public static EnhancedTypeDescriptor Get(Type type)
        {
            EnhancedTypeDescriptor td;
            Monitor.Enter(_cache);
            try
            {
                td = _cache.FirstOrDefault(t => t.Type.Equals(type));
                if (td is null)
                {
                    td = new(type);
                }
            }
            finally { Monitor.Exit(_cache); }
            return td;
        }

        protected EnhancedTypeDescriptor(Type type)
        {
            Type = type;
            _cache.Add(this);
            Collection<EnhancedTypeDescriptor> genericArguments = new();
            if (type.IsArray)
            {
                Category = TypeCategory.Array;
                ComponentName = BaseName = (UnderlyingType = Get(type.GetElementType())).BaseName;
                Namespace = UnderlyingType.Namespace;
                int rank = type.GetArrayRank();
                TypeName = (rank > 1) ? $"{UnderlyingType.TypeName}[{new string(',', rank - 1)}]" : $"{UnderlyingType.TypeName}[]";
            }
            else if (type.IsByRef)
            {
                Category = TypeCategory.ByRef;
                ComponentName = BaseName = (UnderlyingType = Get(type.GetElementType())).BaseName;
                Namespace = UnderlyingType.Namespace;
                TypeName = $"{UnderlyingType.TypeName}&";
            }
            else if (type.IsPointer)
            {
                Category = TypeCategory.Pointer;
                ComponentName = BaseName = (UnderlyingType = Get(type.GetElementType())).BaseName;
                Namespace = UnderlyingType.Namespace;
                TypeName = $"{UnderlyingType.TypeName}*";
            }
            else
            {
                if (type.IsValueType)
                {
                    if (type.IsEnum)
                    {
                        Category = TypeCategory.Enum;
                        BaseType = UnderlyingType = Get(Enum.GetUnderlyingType(type));
                        BaseName = type.Name;
                        Namespace = type.Namespace ?? "";
                    }
                    else if (type.IsPrimitive)
                    {
                        Category = TypeCategory.Primitive;
                        if (type == typeof(bool))
                            BaseName = "bool";
                        else if (type == typeof(short))
                            BaseName = "short";
                        else if (type == typeof(ushort))
                            BaseName = "ushort";
                        else if (type == typeof(int))
                            BaseName = "int";
                        else if (type == typeof(uint))
                            BaseName = "uint";
                        else if (type == typeof(long))
                            BaseName = "long";
                        else if (type == typeof(ulong))
                            BaseName = "ulong";
                        else if (type == typeof(float))
                            BaseName = "float";
                        else
                            BaseName = type.Name.ToLower();
                        Namespace = "";
                    }
                    else if (type.Equals(typeof(decimal)) || type.Equals(typeof(void)))
                    {
                        Category = TypeCategory.Struct;
                        BaseName = TypeName = type.Name.ToLower();
                        Namespace = "";
                    }
                    else
                    {
                        Namespace = type.Namespace ?? "";
                        bool isNullable;
                        if (type.IsGenericType)
                        {
                            if (type.IsGenericTypeDefinition)
                            {
                                isNullable = type.Equals(NullableType);
                                int i = type.Name.IndexOf("`");
                                BaseName = type.Name.Substring(0, i);
                            }
                            else if (type.GetGenericTypeDefinition().Equals(NullableType))
                            {
                                isNullable = true;
                                BaseName = (UnderlyingType = Get(Nullable.GetUnderlyingType(type))).BaseName;
                                BaseType = Get(NullableType);
                            }
                            else
                            {
                                isNullable = false;
                                int i = type.Name.IndexOf("`");
                                BaseName = type.Name.Substring(0, i);
                            }
                        }
                        else
                        {
                            isNullable = false;
                            BaseName = type.Name;
                        }
                        if (isNullable)
                        {
                            IsEntityType = UnderlyingType.IsEntityType;
                            IsLocalEntityType = UnderlyingType.IsLocalEntityType;
                            Category = TypeCategory.Nullable;
                        }
                        else
                        {
                            foreach (Type i in type.GetInterfaces())
                                if (i.Equals(EntityHelper.LocalDbEntityInterfaceType))
                                {
                                    IsLocalEntityType = IsEntityType = true;
                                    break;
                                }
                                else if (i.Equals(EntityHelper.DbEntityInterfaceType))
                                {
                                    IsEntityType = true;
                                    break;
                                }

                            Category = TypeCategory.Struct;
                        }
                    }
                    ComponentName = BaseName;
                }
                else
                {
                    if (type.IsInterface)
                    {
                        foreach (Type i in type.GetInterfaces())
                            if (i.Equals(EntityHelper.LocalDbEntityInterfaceType))
                            {
                                IsLocalEntityType = true;
                                break;
                            }
                            else if (i.Equals(EntityHelper.DbEntityInterfaceType))
                            {
                                IsEntityType = true;
                                break;
                            }
                        Category = TypeCategory.Interface;
                    }
                    else
                    {
                        Type t = type;
                        do
                        {
                            if (t.Equals(EntityHelper.LocalDbEntityClassType))
                            {
                                IsLocalEntityType = true;
                                break;
                            }
                            else if (t.Equals(EntityHelper.DbEntityClassType))
                            {
                                IsEntityType = true;
                                break;
                            }
                        } while ((t = t.BaseType) is not null && !t.Equals(EntityHelper.ObjectType));
                        Category = TypeCategory.Class;
                    }
                    string componentName = TypeDescriptor.GetComponentName(type);
                    if (type.IsGenericType)
                    {
                        if (type.IsGenericTypeDefinition)
                        {
                            if (type.IsClass && type.BaseType is not null && !type.BaseType.Equals(EntityHelper.ObjectType))
                                BaseType = Get(type.BaseType);
                        }
                        else
                            BaseType = Get(type.GetGenericTypeDefinition());
                        int i = type.Name.IndexOf("`");
                        BaseName = type.Name.Substring(0, i);
                        Namespace = type.Namespace ?? "";
                    }
                    else if (type.IsClass)
                    {
                        if (type.BaseType is not null && !type.BaseType.Equals(EntityHelper.ObjectType))
                        {
                            BaseType = Get(type.BaseType);
                            BaseName = type.Name;
                            Namespace = type.Namespace ?? "";
                        }
                        else if (type.Equals(typeof(string)))
                        {
                            BaseName = TypeName = "string";
                            Namespace = "";
                        }
                        else
                            Namespace = type.Namespace ?? "";
                    }
                    else
                    {
                        BaseName = type.Name;
                        Namespace = type.Namespace ?? "";
                    }
                    ComponentName = string.IsNullOrWhiteSpace(componentName) ? BaseName : componentName;
                }

                if (type.IsGenericType)
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        int n = type.GetGenericArguments().Length - 1;
                        switch (n)
                        {
                            case 0:
                                TypeName = $"{BaseName}<>";
                                break;
                            case 1:
                                TypeName = $"{BaseName}<,>";
                                break;
                            default:
                                TypeName = $"{BaseName}<{new string(',', n)}>";
                                break;
                        }
                    }
                    else
                    {
                        foreach (Type t in type.GetGenericArguments())
                            genericArguments.Add(Get(t));
                        if (Category == TypeCategory.Nullable)
                            TypeName = $"{BaseName}?";
                        else if (genericArguments.Count == 1)
                            TypeName = $"{BaseName}<{genericArguments[0].TypeName}>";
                        else
                            TypeName = $"{BaseName}<{string.Join(',', genericArguments.Select(a => a.TypeName))}>";
                    }
                }
                else
                    TypeName = BaseName;
            }
            Collection<EnhancedTypeDescriptor> equatableTo = new();
            Collection<EnhancedTypeDescriptor> enumerableInterfaces = new();
            Collection<EnhancedTypeDescriptor> implementedEntityTypes = new();
            Collection<EnhancedTypeDescriptor> otherInterfaceTypes = new();
            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsGenericType && !i.IsGenericTypeDefinition)
                {
                    Type g = i.GetGenericTypeDefinition();
                    if (g.Equals(IEnumerableType))
                    {
                        enumerableInterfaces.Add(Get(i));
                        continue;
                    }
                    if (g.Equals(IEquatableType))
                    {
                        equatableTo.Add(Get(i.GetGenericArguments()[0]));
                        continue;
                    }
                }
                EnhancedTypeDescriptor td = Get(i);
                if (td.IsEntityType)
                    implementedEntityTypes.Add(td);
                else
                    otherInterfaceTypes.Add(td);
            }
            GenericArguments = new(genericArguments);
            EquatableTo = new(equatableTo);
            EnumerableInterfaces = new(enumerableInterfaces);
            ImplementedEntityTypes = new(implementedEntityTypes);
            OtherInterfaceTypes = new(otherInterfaceTypes);
            Properties = new(EnhancedPropertyDescriptor.Create(this));
        }
    }
}
