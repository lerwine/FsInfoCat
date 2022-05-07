if ($null -eq $Script:TypeDefinitions) {
    Set-Variable -Name 'TypeDefinitions' -Option Constant -Scope Script -Value ([Xml]::new());
    $Script:TypeDefinitions.Load(($PSScriptRoot | Join-Path -ChildPath 'TypeDefinitions.xml'));
    Set-Variable -Name 'PrimeNumbers' -Option Constant -Scope Script -Value ([System.Collections.ObjectModel.Collection[int]]::new());
    (2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97) | ForEach-Object { $Script:PrimeNumbers.Add($_) }
}

Add-Type -TypeDefinition @'
namespace TypeDefinitions
{
    using System;
    public enum IntegerType
    {
        byte;
        sbyte;
        short;
        ushort;
        int;
        uint;
        long;
        ulong;
    }

    public enum BasicType
    {
        byte;
        sbyte;
        short;
        ushort;
        int;
        uint;
        long;
        ulong;
        bool;
        char;
        float;
        double;
        decimal;
        string;
        DateTime;
        Guid;
        float;
    }

    public enum PropertyAccessType
    {
        GetSet;
        Get;
        Set;
    }

    public interface IPropertyType : IEquatable<IPropertyType>, IComparable<IPropertyType>
    {
        string Name { get; }
        PropertyAccessType Access { get; }
        string BackingField { get; }
        bool AllowNull { get; }
        string TypeName { get; }
        bool IsNormalized { get; }
    }
    public abstract class AbstractPropertyType, IEquatable<AbstractPropertyType>, IComparable<AbstractPropertyType>
    {
        public const string ElementName_Struct = "Struct";
        public const string ElementName_Class = "Class";
        public const string ElementName_Entity = "Entity";
        public const string ElementName_Interface = "Interface";
        public const string ElementName_Enum = "Enum";
        public const string ElementName_Boolean = "Boolean";
        public const string ElementName_Byte = "Byte";
        public const string ElementName_SByte = "SByte";
        public const string ElementName_Char = "Char";
        public const string ElementName_UShort = "UShort";
        public const string ElementName_Short = "Short";
        public const string ElementName_Int = "Int";
        public const string ElementName_UInt = "UInt";
        public const string ElementName_Long = "Long";
        public const string ElementName_ULong = "ULong";
        public const string ElementName_Float = "Float";
        public const string ElementName_Double = "Double";
        public const string ElementName_Decimal = "Decimal";
        public const string ElementName_DateTime = "DateTime";
        public const string ElementName_Guid = "Guid";
        public const string ElementName_String = "String";
        public const string ElementName_Value = "Value";
        public const string ElementName_Class = "Class";
        public const string ElementName_ObjArray = "ObjArray";
        public const string ElementName_Array = "Array";
        public const string ElementName_PrimaryKey = "PrimaryKey";
        public const string ElementName_Navigation = "Navigation";
        public const string ElementName_HashSet = "HashSet";
        public const string ElementName_IEnumerable = "IEnumerable";
        private readonly string _name;
        public string Name { get { return _name; } }
        private readonly PropertyAccessType _access;
        public PropertyAccessType Access { get { return _access; } }
        protected AbstractPropertyType(string name, PropertyAccessType access)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be null, empty or white-space only", "name");
            _name = name;
            _access = access;
        }
        public int CompareTo(IPropertyType other)
        {
            return (other == null || other.Name == null) ? -1 : _name.CompareTo(other.Name);
        }
        public bool Equals(IPropertyType other)
        {
            return other != null && (ReferenceEquals(this, other) || _name.Equals(other.Name));
        }
        int IComparable<AbstractPropertyType>.CompareTo(AbstractPropertyType other) { return CompareTo(other) }
        bool IEquatable<AbstractPropertyType>.Equals(AbstractPropertyType other) { return Equals(other) }
        public override bool Equals(object obj) { return Equals(obj as IPropertyType); }
        public override GetHashCode() { return _name.GetHashCode(); }
        private static GetStructProperties(XmlElement element, Collection<IPropertyType> collection)
        {
            foreach (XmlElement e in element.SelectNodes("Properties/*"))
            {
                XmlAttribute attribute = (XmlAttribute)e.SelectSingleNode("@Name);
                if (attribute == null) continue;
                string name = attribute.Value;
                if (collection.Any(p => p.Name == name)) continue;
                bool allowsNull;
                switch (e.LocalName)
                {
                    case ElementName_Boolean:
                        break;
                    case ElementName_Byte:
                        break;
                    case ElementName_SByte:
                        break;
                    case ElementName_Char:
                        break;
                    case ElementName_UShort:
                        break;
                    case ElementName_Short:
                        break;
                    case ElementName_Int:
                        break;
                    case ElementName_UInt:
                        break;
                    case ElementName_Long:
                        break;
                    case ElementName_ULong:
                        break;
                    case ElementName_Float:
                        break;
                    case ElementName_Double:
                        break;
                    case ElementName_Decimal:
                        break;
                    case ElementName_DateTime:
                        break;
                    case ElementName_Guid:
                        break;
                    case ElementName_String:
                        break;
                    case ElementName_Value:
                        break;
                    case ElementName_ObjArray:
                        break;
                    case ElementName_Array:
                        break;
                    default: // ElementName_Class
                        break;
                }
            }
        }
        private static GetClassProperties(XmlElement element, Collection<IPropertyType> collection)
        {
            foreach (XmlElement e in element.SelectNodes("Properties/*"))
            {
                XmlAttribute attribute = (XmlAttribute)e.SelectSingleNode("@Name);
                if (attribute == null) continue;
                string name = attribute.Value;
                if (collection.Any(p => p.Name == name)) continue;
                bool allowsNull;
                switch (e.LocalName)
                {
                    case ElementName_Boolean:
                        break;
                    case ElementName_Byte:
                        break;
                    case ElementName_SByte:
                        break;
                    case ElementName_Char:
                        break;
                    case ElementName_UShort:
                        break;
                    case ElementName_Short:
                        break;
                    case ElementName_Int:
                        break;
                    case ElementName_UInt:
                        break;
                    case ElementName_Long:
                        break;
                    case ElementName_ULong:
                        break;
                    case ElementName_Float:
                        break;
                    case ElementName_Double:
                        break;
                    case ElementName_Decimal:
                        break;
                    case ElementName_DateTime:
                        break;
                    case ElementName_Guid:
                        break;
                    case ElementName_String:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        bool isNormalized = GetAttributeBoolean(element, AttributeSelector_IsNormalized);
                        break;
                    case ElementName_Value:
                        break;
                    case ElementName_ObjArray:
                        break;
                    case ElementName_Array:
                        break;
                    case ElementName_IEnumerable:
                        break;
                    default: // ElementName_Class
                        break;
                }
            }
        }
        private static GetEntityProperties(XmlElement element, Collection<IPropertyType> collection)
        {
            foreach (XmlElement e in element.SelectNodes("Properties/*"))
            {
                XmlAttribute attribute = (XmlAttribute)e.SelectSingleNode("@Name);
                if (attribute == null) continue;
                string name = attribute.Value;
                if (collection.Any(p => p.Name == name)) continue;
                bool allowsNull;
                switch (e.LocalName)
                {
                    case ElementName_Boolean:
                        break;
                    case ElementName_Byte:
                        break;
                    case ElementName_SByte:
                        break;
                    case ElementName_Char:
                        break;
                    case ElementName_UShort:
                        break;
                    case ElementName_Short:
                    3 wq
                        break;
                    case ElementName_Int:
                        break;
                    case ElementName_UInt:
                        break;
                    case ElementName_Long:
                        break;
                    case ElementName_ULong:
                        break;
                    case ElementName_Float:
                        break;
                    case ElementName_Double:
                        break;
                    case ElementName_Decimal:
                        break;
                    case ElementName_DateTime:
                        break;
                    case ElementName_Guid:
                        break;
                    case ElementName_String:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        bool isNormalized = GetAttributeBoolean(element, AttributeSelector_IsNormalized);
                        break;
                    case ElementName_Value:
                        break;
                    case ElementName_ObjArray:
                        break;
                    case ElementName_Array:
                        break;
                    case ElementName_PrimaryKey:
                        break;
                    case ElementName_Navigation:
                        break;
                    case ElementName_HashSet:
                        break;
                    default: // ElementName_Class
                        break;
                }
            }
        }
        private static bool GetAttributeBoolean(XmlElement element, string selector, bool defaultValue)
        {
            XmlAttribute attribute = (XmlAttribute)e.SelectSingleNode(selector);
            if (attribute == null) return defaultValue;
            try { return XmlConvert.ToBoolean(attribute.Value.Trim()); }
            catch { return defaultValue; }

        }
        public const string AttributeSelector_Name = "@IsNullable";
        public const string AttributeSelector_IsNullable = "@IsNullable";
        public const string AttributeSelector_AllowNull = "@AllowNull";
        public const string AttributeSelector_IsNormalized = "@IsNormalized";
        private static GetInterfaceProperties(XmlElement element, Collection<IPropertyType> collection)
        {
            foreach (XmlElement e in element.SelectNodes("Properties/*"))
            {
                XmlAttribute attribute = (XmlAttribute)e.SelectSingleNode(AttributeSelector_Name);
                if (attribute == null) continue;
                string name = attribute.Value;
                if (collection.Any(p => p.Name == name)) continue;
                bool allowsNull;
                switch (e.LocalName)
                {
                    case ElementName_Boolean:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Byte:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_SByte:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Char:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_UShort:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Short:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Int:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_UInt:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Long:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_ULong:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Float:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Double:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Decimal:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_DateTime:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_Guid:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_String:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        bool isNormalized = GetAttributeBoolean(element, AttributeSelector_IsNormalized);
                        break;
                    case ElementName_Value:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_IsNullable);
                        break;
                    case ElementName_ObjArray:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        break;
                    case ElementName_Array:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        break;
                    case ElementName_IEnumerable:
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        break;
                    default: // ElementName_Class
                        allowsNull = GetAttributeBoolean(element, AttributeSelector_AllowNull);
                        break;
                }
            }
        }
        public static Collection<IPropertyType> GetProperties(XmlDocument xml, string typeName)
        {
            XmlElement element = xml.DocumentElement.SelectSingleNode("//Namespace/*[@Name=\"" + typeName + "\"]") as XmlElement;
            if (element == null) return null;
            XmlAttribute attribute = element.SelectSingleNode("@Name");
            if (attribute == null) return null;
            Collection<IPropertyType> result = new Collection<IPropertyType>();
            switch (attribute.Value)
            {
                case ElementName_Struct:
                    GetStructProperties(element, result);
                    break;
                case ElementName_Class:
                    GetClassProperties(element, result);
                    break;
                case ElementName_Entity:
                    GetEntityProperties(element, result);
                    break;
                case ElementName_Interface:
                    GetInterfaceProperties(element, result);
                    break;
            }
            return result;
        }
    }

    public abstract class ConcretePropertyType : AbstractPropertyType
    {
        private readonly string _backingField;
        public string BackingField { get { return _backingField; } }
        protected ConcretePropertyType(string name, PropertyAccessType access, string backingField) : base(name, access)
        {
            _backingField = string.IsNullOrWhiteSpace(backingField) ? null : backingField;
        }
    }

    public sealed class AbstractValuePropertyType : AbstractPropertyType, IEquatable<AbstractValuePropertyType>, IComparable<AbstractValuePropertyType>
    {
        private readonly bool _isNullable;
        private readonly string _typeName;
        public bool IsNullable { get { return _isNullable; } }
        public string TypeName { get { return _typeName; } }
        bool IPropertyType.AllowNull { get { return _isNullable; } }
        bool IPropertyType.IsNormalized { get { return false; } }
        public AbstractValuePropertyType(string name, string typeName, bool isNullable, PropertyAccessType access) : base(name, access)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("TypeName cannot be null, empty or white-space only", "typeName");
            _isNullable = isNullable;
            _typeName = typeName;
        }
        public AbstractValuePropertyType(string name, string typeName, bool isNullable) : this(name, typeName, isNullable, PropertyAccessType.GetSet) { }
        public AbstractValuePropertyType(string name, string typeName) : base(name, PropertyAccessType.GetSet) : this(name, typeName, false, PropertyAccessType.GetSet) { }
        int IComparable<AbstractValuePropertyType>.CompareTo(AbstractValuePropertyType other) { return CompareTo(other) }
        bool IEquatable<AbstractValuePropertyType>.Equals(AbstractValuePropertyType other) { return Equals(other) }
    }

    public sealed class ConcreteValuePropertyType : ConcretePropertyType, IEquatable<ConcreteValuePropertyType>, IComparable<ConcreteValuePropertyType>
    {
        private readonly bool _isNullable;
        private readonly string _typeName;
        public bool IsNullable { get { return _isNullable; } }
        public string TypeName { get { return _typeName; } }
        bool IPropertyType.AllowNull { get { return _isNullable; } }
        bool IPropertyType.IsNormalized { get { return false; } }
        public ConcreteValuePropertyType(string name, string typeName, bool isNullable, PropertyAccessType access, string backingField) : base(name, access, backingField)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("TypeName cannot be null, empty or white-space only", "typeName");
            _isNullable = isNullable;
            _typeName = typeName;
        }
        public ConcreteValuePropertyType(string name, string typeName, bool isNullable, PropertyAccessType access) : this(name, typeName, isNullable, access, null) { }
        public ConcreteValuePropertyType(string name, string typeName, bool isNullable) : this(name, typeName, isNullable, PropertyAccessType.GetSet, null) { }
        public ConcreteValuePropertyType(string name, string typeName) : this(name, typeName, false, PropertyAccessType.GetSet, null) { }
        int IComparable<ConcreteValuePropertyType>.CompareTo(ConcreteValuePropertyType other) { return CompareTo(other) }
        bool IEquatable<ConcreteValuePropertyType>.Equals(ConcreteValuePropertyType other) { return Equals(other) }
    }

    public abstract class AbstractRefPropertyType : AbstractPropertyType
    {
        private readonly bool _allowNull;
        private readonly string _typeName;
        public bool AllowNull { get { return _allowNull; } }
        public string TypeName { get { return _typeName; } }
        protected AbstractRefPropertyType(string name, string typeName, bool allowNull, PropertyAccessType access) : base(name, access)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("TypeName cannot be null, empty or white-space only", "typeName");
            _allowNull = allowNull;
            _typeName = typeName;
        }
        public AbstractRefPropertyType(string name, string typeName, bool allowNull) : this(name, typeName, allowNull, PropertyAccessType.GetSet) { }
        public AbstractRefPropertyType(string name, string typeName) : base(name, PropertyAccessType.GetSet) : this(name, typeName, false, PropertyAccessType.GetSet) { }
    }
    

    public sealed class AbstractClassPropertyType : AbstractRefPropertyType, IEquatable<AbstractClassPropertyType>, IComparable<AbstractClassPropertyType>
    {
        bool IPropertyType.IsNormalized { get { return false; } }
        public AbstractClassPropertyType(string name, string typeName, bool allowNull, PropertyAccessType access) : base(name, typeName, allowNull, access) { }
        public AbstractClassPropertyType(string name, string typeName, bool allowNull) : base(name, typeName, allowNull) { }
        public AbstractClassPropertyType(string name, string typeName) : base(name, typeName) { }
        int IComparable<AbstractClassPropertyType>.CompareTo(AbstractClassPropertyType other) { return CompareTo(other) }
        bool IEquatable<AbstractClassPropertyType>.Equals(AbstractClassPropertyType other) { return Equals(other) }
    }

    public abstract class ConcreteRefPropertyType : ConcretePropertyType
    {
        private readonly bool _allowNull;
        private readonly string _typeName;
        public bool AllowNull { get { return _allowNull; } }
        public string TypeName { get { return _typeName; } }
        protected ConcreteRefPropertyType(string name, string typeName, bool allowNull, PropertyAccessType access, string backingField) : base(name, access, backingField)
        {
            if (string.IsNullOrWhiteSpace(typeName)) throw new ArgumentException("TypeName cannot be null, empty or white-space only", "typeName");
            _allowNull = allowNull;
            _typeName = typeName;
        }
        public ConcreteRefPropertyType(string name, string typeName, bool allowNull, PropertyAccessType access) : this(name, typeName, allowNull, access, null) { }
        public ConcreteRefPropertyType(string name, string typeName, bool allowNull) : this(name, typeName, allowNull, PropertyAccessType.GetSet, null) { }
        public ConcreteRefPropertyType(string name, string typeName) : this(name, typeName, false, PropertyAccessType.GetSet, null) { }
    }
    
    public sealed class ConcreteClassPropertyType : ConcreteRefPropertyType, IEquatable<ConcreteClassPropertyType>, IComparable<ConcreteClassPropertyType>
    {
        bool IPropertyType.IsNormalized { get { return false; } }
        public ConcreteClassPropertyType(string name, string typeName, bool allowNull, PropertyAccessType access, string backingField) : base(name, typeName, allowNull, access, backingField) { }
        public ConcreteClassPropertyType(string name, string typeName, bool allowNull, PropertyAccessType access) : base(name, typeName, allowNull, access) { }
        public ConcreteClassPropertyType(string name, string typeName, bool allowNull) : base(name, typeName, allowNull) { }
        public ConcreteClassPropertyType(string name, string typeName) : base(name, typeName) { }
        int IComparable<ConcreteClassPropertyType>.CompareTo(ConcreteClassPropertyType other) { return CompareTo(other) }
        bool IEquatable<ConcreteClassPropertyType>.Equals(ConcreteClassPropertyType other) { return Equals(other) }
    }

    public sealed class AbstractStringPropertyType : AbstractRefPropertyType, IEquatable<AbstractStringPropertyType>, IComparable<AbstractStringPropertyType>
    {
        private readonly bool _isNormalized;
        public bool IsNormalized { get { return _isNormalized; } }
        public AbstractStringPropertyType(string name, bool isNormalized, bool allowNull, PropertyAccessType access) : base(name, "string", allowNull, access)
        {
            _isNormalized = isNormalized;
        }
        public AbstractStringPropertyType(string name, bool isNormalized, bool allowNull) : this(name, isNormalized, allowNull, PropertyAccessType.GetSet) { }
        public AbstractStringPropertyType(string name, bool isNormalized) : this(name, isNormalized, false, PropertyAccessType.GetSet) { }
        public AbstractStringPropertyType(string name) : this(name, false, false, PropertyAccessType.GetSet) { }
        int IComparable<AbstractStringPropertyType>.CompareTo(AbstractStringPropertyType other) { return CompareTo(other) }
        bool IEquatable<AbstractStringPropertyType>.Equals(AbstractStringPropertyType other) { return Equals(other) }
    }

    public sealed class ConcreteStringPropertyType : ConcreteRefPropertyType : IPropertyType, IEquatable<ConcreteStringPropertyType>, IComparable<ConcreteStringPropertyType>
    {
        private readonly bool _isNormalized;
        public bool IsNormalized { get { return _isNormalized; } }
        public ConcreteStringPropertyType(string name, bool isNormalized, bool allowNull, PropertyAccessType access, string backingField) : base(name, "string", allowNull, access, backingField)
        {
            _isNormalized = isNormalized;
        }
        public ConcreteStringPropertyType(string name, bool isNormalized, bool allowNull, PropertyAccessType access) : this(name, isNormalized, allowNull, access) { }
        public ConcreteStringPropertyType(string name, bool isNormalized, bool allowNull) : this(name, isNormalized, allowNull, PropertyAccessType.GetSet) { }
        public ConcreteStringPropertyType(string name, bool isNormalized) : this(name, isNormalized, false, PropertyAccessType.GetSet) { }
        public ConcreteStringPropertyType(string name) : this(name, false, false, PropertyAccessType.GetSet) { }
        int IComparable<ConcreteStringPropertyType>.CompareTo(ConcreteStringPropertyType other) { return CompareTo(other) }
        bool IEquatable<ConcreteStringPropertyType>.Equals(ConcreteStringPropertyType other) { return Equals(other) }
    }

}
'@ -ReferencedAssemblies 'System.Xml';

Function Test-IsPrimeNumber {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateRange(0, [int]::MaxValue)]
        [int]$Value
    )

    Process {
        if ($Value -lt 2 -or $Value % 2 -eq 0) {
            $false | Write-Output;
        } else {
            $v = $Script:PrimeNumbers[$Script:PrimeNumbers.Count - 1];
            if ($Value -eq $v) {
                $true | Write-Output;
            } else {
                if ($Value -lt $v) {
                    $Script:PrimeNumbers.Contains($Value) | Write-Output;
                } else {
                    $e = $Value -shr 1;
                    $IsPrime = $true;
                    for ($d = 2; $d -le $e; $d++) {
                        if ($Value % $d -eq 0) {
                            $IsPrime = $false;
                            break;
                        }
                    }
                    $IsPrime | Write-Output;
                }
            }
        }
    }
}

Function Get-PrimeNumber {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateRange(0, 1024)]
        [int]$Position
    )

    Process {
        if ($Position -lt $Script:PrimeNumbers.Count) {
            $Script:PrimeNumbers[$Position] | Write-Output;
        } else {
            $i = $Script:PrimeNumbers.Count - 1;
            $n = $Script:PrimeNumbers[$i] + 2;
            while (-not ($n | Test-IsPrimeNumber)) { $n += 2 }
            $Script:PrimeNumbers.Add($n);
            while (++$i -lt $Position) {
                $n += 2;
                while (-not ($n | Test-IsPrimeNumber)) { $n += 2 }
                $Script:PrimeNumbers.Add($n);
            }
            $n | Write-Output;
        }
    }
}

Function Get-CodeForGetHashCode {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$TypeName
    )
}
Get-PrimeNumber -Position 27