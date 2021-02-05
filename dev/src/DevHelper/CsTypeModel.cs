using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevHelper
{
    public class CsTypeModel
    {
        public static Regex GenericNameRegex = new Regex(@"`\d+$", RegexOptions.Compiled);
        private readonly bool _isArray;
        private readonly int _rank;
        private readonly bool _isEnum;
        private readonly CsTypeModel _elementType;
        private readonly string _name;
        private readonly string _shortHand;
        private readonly string _namespace;
        private readonly bool _isValueType;
        private readonly bool _isNullableStruct;
        private ReadOnlyCollection<CsTypeModel> _genericArguments;
        private readonly CsTypeModel _declaringType;
        private bool _isString;
        private bool _isBoolean;
        private bool _isNumeric;
        private bool _isPrimitive;
        private string _defaultValue;
        public bool IsArray { get { return _isArray; } }
        public int Rank { get { return _rank; } }
        public bool IsEnum { get { return _isEnum; } }
        public CsTypeModel ElementType { get { return _elementType; } }
        public string Name { get { return _name; } }
        public string ShortHand { get { return _shortHand; } }
        public string Namespace { get { return _namespace; } }
        public bool IsValueType { get { return _isValueType; } }
        public bool IsNullableStruct { get { return _isNullableStruct; } }
        public bool IsString { get { return _isString; } }
        public bool IsBoolean { get { return _isBoolean; } }
        public bool IsNumeric { get { return _isNumeric; } }
        public bool IsPrimitive { get { return _isPrimitive; } }
        public string DefaultValue { get { return _defaultValue; } }
        public ReadOnlyCollection<CsTypeModel> GenericArguments { get { return _genericArguments; } }
        public CsTypeModel DeclaringType { get { return _declaringType; } }
        public CsTypeModel(string ns, string name, bool isValueType, IEnumerable<CsTypeModel> genericArguments)
        {
            _isArray = _isNullableStruct = _isEnum = _isString = _isBoolean = _isNumeric = _isPrimitive = false;
            _rank = 0;
            _elementType = _declaringType = null;
            _shortHand = _name = name;
            _namespace = (ns is null) ? "" : ns;
            _isValueType = isValueType;
            _defaultValue = (isValueType) ? "default(" + _shortHand + ")" : "null";
            _genericArguments = new ReadOnlyCollection<CsTypeModel>((genericArguments is null) ? new CsTypeModel[0] : genericArguments.Where(a => null != a).ToArray());
        }
        private CsTypeModel(CsTypeModel elementType, int rank)
        {
            _isValueType = _isEnum = _isString = _isBoolean = _isNumeric = _isPrimitive = false;
            _isArray = rank > 0;
            _isNullableStruct = !_isArray;
            _elementType = elementType;
            _defaultValue = "null";
            if (_isArray)
            {
                _rank = rank;
                string idx = ((_rank == 1) ? "[]" : "[" + (new String(',', _rank - 1))) + "]";
                _declaringType = null;
                _name = _elementType._name + idx;
                _shortHand = _elementType._shortHand + idx;
                _namespace = _elementType._namespace;
                _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[0]);
            }
            else
            {
                _rank = 0;
                _name = "Nullable<" + _elementType._shortHand + ">";
                _shortHand = _elementType._shortHand + "?";
                _namespace = _elementType._namespace;
                Type t = typeof(Nullable<>);
                _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[] { new CsTypeModel(t.Namespace, GenericNameRegex.Replace(t.Name, ""), false, null) });
            }
        }
        public CsTypeModel(CsTypeModel declaringType, string name, bool isValueType, IEnumerable<CsTypeModel> genericArguments)
        {
            _isArray = _isNullableStruct = _isEnum = _isString = _isBoolean = _isNumeric = _isPrimitive = false;
            _rank = 0;
            _elementType = null;
            _declaringType = declaringType;
            _shortHand = declaringType._shortHand + "." + name;
            _namespace = "";
            _isValueType = isValueType;
            _defaultValue = (isValueType) ? "default(" + _shortHand + ")" : "null";
            _genericArguments = new ReadOnlyCollection<CsTypeModel>((genericArguments is null) ? new CsTypeModel[0] : genericArguments.Where(a => null != a).ToArray());
        }
        public CsTypeModel(Type type)
        {
            if (type is null)
                throw new ArgumentNullException("type");
            _isArray = type.IsArray;
            _isEnum = type.IsEnum;
            _isPrimitive = type.IsPrimitive;
            if (_isArray)
            {
                _rank = type.GetArrayRank();
                string idx = ((_rank == 1) ? "[]" : "[" + (new String(',', _rank - 1))) + "]";
                _elementType = new CsTypeModel(type.GetElementType());
                _declaringType = null;
                _name = _elementType._name + idx;
                _shortHand = _elementType._shortHand + idx;
                _namespace = _elementType._namespace;
                _isValueType = _isNullableStruct = _isString = _isBoolean = _isNumeric = false;
                _defaultValue = "null";
                _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[0]);
                return;
            }
            _rank = 0;
            if (type.IsNested)
            {
                _declaringType = new CsTypeModel(type.DeclaringType);
                _namespace = "";
                _isValueType = type.IsValueType;
                _isNullableStruct = _isString = _isBoolean = _isNumeric = false;
                if (_isEnum)
                {
                    _elementType = new CsTypeModel(Enum.GetUnderlyingType(type));
                    _shortHand = _name = type.Name;
                    _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[0]);
                    _defaultValue = "default(" + _shortHand + ")";
                    return;
                }
                if (type.IsGenericType)
                {
                    _genericArguments = new ReadOnlyCollection<CsTypeModel>(type.GetGenericArguments().Select(a => new CsTypeModel(a)).ToArray());
                    _shortHand = _name = GenericNameRegex.Replace(type.Name, "") + "<" + string.Join(", ", _genericArguments.Select(a => a._shortHand).ToArray()) + ">";
                }
                else
                {
                    _shortHand = _name = type.Name;
                    _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[0]);
                }
                _defaultValue = (_isValueType) ? "default(" + _shortHand + ")" : "null";
                return;
            }
            _declaringType = null;
            if (type.IsGenericParameter)
            {
                _elementType = null;
                _namespace = "";
                _shortHand = _name = type.Name;
                _isValueType = _isNullableStruct = _isString = _isBoolean = _isNumeric = false;
                _defaultValue = "null";
                _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[0]);
                return;
            }
            if (type.IsGenericType)
            {
                _isString = _isBoolean = _isNumeric = false;
                Type t = typeof(Nullable<>);
                if (t.IsAssignableFrom(type.GetGenericTypeDefinition()))
                {
                    _elementType = new CsTypeModel(Nullable.GetUnderlyingType(type));
                    _name = "Nullable<" + _elementType._shortHand + ">";
                    _shortHand = _elementType._shortHand + "?";
                    _namespace = _elementType._namespace;
                    _isValueType = false;
                    _isNullableStruct = true;
                    _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[] { new CsTypeModel(t.Namespace, GenericNameRegex.Replace(t.Name, ""), false, null) });
                    _defaultValue = "null";
                    return;
                }
                _genericArguments = new ReadOnlyCollection<CsTypeModel>(type.GetGenericArguments().Select(a => new CsTypeModel(a)).ToArray());
                _namespace = (type.Namespace is null) ? "" : type.Namespace;
                _shortHand = _name = GenericNameRegex.Replace(type.Name, "") + "<" + string.Join(", ", _genericArguments.Select(a => a._shortHand).ToArray()) + ">";
                _isNullableStruct = false;
                _isValueType = type.IsValueType;
                _namespace = (type.Namespace is null) ? "" : type.Namespace;
                _defaultValue = (_isValueType) ? "default(" + _shortHand + ")" : "null";
                return;
            }
            _namespace = (type.Namespace is null) ? "" : type.Namespace;
            _genericArguments = new ReadOnlyCollection<CsTypeModel>(new CsTypeModel[0]);
            _name = type.Name;
            _isNullableStruct = false;
            _isValueType = type.IsValueType;
            _isString = type.Equals(typeof(string));
            if (_isString)
            {
                _shortHand = "string";
                _isBoolean = _isNumeric = false;
                _defaultValue = "\"\"";
                return;
            }
            _isBoolean = type.Equals(typeof(bool));
            if (_isBoolean)
            {
                _shortHand = "bool";
                _isNumeric = false;
                _defaultValue = "false";
                return;
            }
            if (type.Equals(typeof(char)))
            {
                _shortHand = "char";
                _isNumeric = false;
                _defaultValue = "default(" + _shortHand + ")";
                return;
            }
            if (type.Equals(typeof(byte)))
            {
                _defaultValue = "(byte)0";
                _shortHand = "byte";
            }
            else if (type.Equals(typeof(sbyte)))
            {
                _defaultValue = "(sbyte)0";
                _shortHand = "sbyte";
            }
            else if (type.Equals(typeof(short)))
            {
                _defaultValue = "(short)0";
                _shortHand = "short";
            }
            else if (type.Equals(typeof(ushort)))
            {
                _defaultValue = "(ushort)0";
                _shortHand = "ushort";
            }
            else if (type.Equals(typeof(int)))
            {
                _defaultValue = "0";
                _shortHand = "int";
            }
            else if (type.Equals(typeof(uint)))
            {
                _defaultValue = "0U";
                _shortHand = "uint";
            }
            else if (type.Equals(typeof(long)))
            {
                _defaultValue = "0L";
                _shortHand = "long";
            }
            else if (type.Equals(typeof(ulong)))
            {
                _defaultValue = "0UL";
                _shortHand = "ulong";
            }
            else if (type.Equals(typeof(float)))
            {
                _defaultValue = "0.0f";
                _shortHand = "float";
            }
            else if (type.Equals(typeof(double)))
            {
                _defaultValue = "0.0";
                _shortHand = "double";
            }
            else if (type.Equals(typeof(decimal)))
            {
                _defaultValue = "0.0m";
                _shortHand = "decimal";
            }
            else
            {
                _shortHand = _name;
                _isNumeric = false;
                _defaultValue = (_isValueType) ? "default(" + _shortHand + ")" : "null";
                return;
            }
            _isNumeric = true;
        }
        public override string ToString()
        {
            if (null != _declaringType)
                return _declaringType.ToString() + "." + _name;
            if (_namespace.Length == 0)
                return _name;
            return _namespace + "." + _name;
        }
        public CsTypeModel AsNullable()
        {
            return (_isValueType) ? new CsTypeModel(this, 0) : this;
        }
        public CsTypeModel ToArray(int rank)
        {
            return (rank < 1) ? this : new CsTypeModel(this, rank);
        }
    }
}
