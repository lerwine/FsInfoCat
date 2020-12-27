namespace Dpcg
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Host;
    using System.Text.RegularExpressions;
    public class TypeChoice : IEquatable<TypeChoice>, IEquatable<ChoiceDescription>
    {
        public static readonly Regex GenericRegex = new Regex("`\\d+$", RegexOptions.Compiled);
        public static readonly TypeChoice OtherStruct = new TypeChoice("", "(other struct)", "", true, false);
        public static readonly TypeChoice OtherClass = new TypeChoice("", "(other class)", "", false, false);
        private readonly ChoiceDescription _choiceDescription;
        private readonly TypeChoice _elementType;
        private readonly TypeChoice _declaringType;
        private int _arrayRank;
        private ReadOnlyCollection<TypeChoice> _genericArguments;
        private readonly string _name;
        private readonly string _csName;
        private readonly string _fullName;
        private readonly bool _isEnum;
        private readonly bool _isNullableStruct;
        private readonly bool _isValueType;
        private readonly bool _isObservableCollection;
        private readonly bool _isBasicType;
        public string CsName { get { return _csName; } }
        public string FullName { get { return _fullName; } }
        public int GetArrayRank() { return _arrayRank; }
        public TypeChoice GetElementType() { return _elementType; }
        public TypeChoice GetDeclaringType() { return _declaringType; }
        public string GetName() { return _name; }
        public ChoiceDescription GetChoiceDescription() { return _choiceDescription; }
        public bool IsArray() { return _arrayRank < 1; }
        public bool IsEnum() { return _isEnum; }
        public bool IsValueType() { return _isValueType; }
        public bool IsObservableCollection() { return _isObservableCollection; }
        public bool IsNullableStruct() { return _isNullableStruct; }
        public ReadOnlyCollection<TypeChoice> GetGenericArguments() { return _genericArguments; }
        public bool IsOtherType() { return _name.Length == 0; }
        public bool IsString() { return _isBasicType && _csName == "string"; }
        public bool IsBoolean() { return _isBasicType && _csName == "bool"; }
        public bool IsBasicType() { return _isBasicType; }
        private TypeChoice(TypeChoice element, int arrayRank)
        {
            if (null == element)
                throw new ArgumentNullException("element");
            if (arrayRank < 1)
                throw new ArgumentOutOfRangeException("arrayRank");
            if (element.IsOtherType())
                throw new InvalidOperationException();
            _arrayRank = arrayRank;
            _isObservableCollection = _isBasicType = false;
            string idx = (arrayRank == 1) ? "[]" : "[" + (new String(',', arrayRank - 1)) + "]";
            _csName = (_name = (_elementType = element)._name) + idx;
            _fullName = _elementType._fullName + idx;
            _declaringType = null;
            _genericArguments = new ReadOnlyCollection<TypeChoice>(new TypeChoice[0]);
            _isEnum = _isNullableStruct = _isValueType = false;
            _choiceDescription = new ChoiceDescription(_csName, _fullName);
        }
        private TypeChoice(TypeChoice underlyingType)
        {
            _arrayRank = 0;
            _csName = (_name = (_elementType = underlyingType)._name) + "?";
            _fullName = "System.Nullable<" + _elementType._csName + ">";
            _declaringType = null;
            _genericArguments = new ReadOnlyCollection<TypeChoice>(new TypeChoice[0]);
            _isNullableStruct = true;
             _isObservableCollection = _isBasicType = _isEnum = _isValueType = false;
            _choiceDescription = new ChoiceDescription(_csName, _fullName);
        }
        private TypeChoice(TypeChoice declaringType, string name, bool isValueType, bool isEnum, params TypeChoice[] genericArgs)
        {
            _arrayRank = 0;
            _isObservableCollection = _isBasicType = _isNullableStruct = false;
            _elementType = null;
            _declaringType = declaringType;
            _genericArguments = new ReadOnlyCollection<TypeChoice>((null == genericArgs) ? new TypeChoice[0] : genericArgs);
            _isEnum = isEnum;
            _isValueType = isValueType;
            _name = name;
            if (_genericArguments.Count > 0)
                _csName = name + "<" + string.Join(", ", _genericArguments.Select(g => g._csName).ToArray()) + ">";
            else
                _csName = name;
            _fullName = declaringType._fullName + "." + _csName;
            _choiceDescription = new ChoiceDescription(_csName, _fullName);
        }
        private TypeChoice(string ns, string name, bool isValueType, bool isEnum, bool isNullableStruct, bool isObservableCollection, params TypeChoice[] genericArgs)
        {
            _arrayRank = 0;
            _isNullableStruct = isNullableStruct;
            _declaringType = null;
            _isEnum = isEnum;
            _isValueType = isValueType;
            _name = name;
            _isObservableCollection = isObservableCollection;
            _isBasicType = false;
            if (isEnum)
            {
                _elementType = genericArgs[0];
                _csName = name;
                _genericArguments = new ReadOnlyCollection<TypeChoice>(new TypeChoice[0]);
            }
            else
            {
                _elementType = null;
                _genericArguments = new ReadOnlyCollection<TypeChoice>((null == genericArgs) ? new TypeChoice[0] : genericArgs);
                if (_genericArguments.Count == 0)
                    _csName = name;
                else if (isNullableStruct)
                    _csName = name + "?";
                else
                    _csName = name + "<" + (string.Join(", ", _genericArguments.Select(g => g._csName).ToArray())) + ">";
            }
            _fullName = (string.IsNullOrEmpty(ns)) ? _csName : ns + "." + _csName;
            _choiceDescription = new ChoiceDescription(_csName, _fullName);
        }
        private TypeChoice(string ns, string csName, string name, bool isValueType, bool isString)
        {
            _arrayRank = 0;
            _isEnum = _isNullableStruct = _isObservableCollection = false;
            _isBasicType = isString;
            _isValueType = isValueType;
            _name = name;
            _csName = csName;
            if (name.Length == 0)
                _fullName = (isValueType) ? "System.Struct" : "System.Object";
            else
                _fullName = (string.IsNullOrEmpty(ns)) ? csName : ns + "." + csName;
            _elementType = _declaringType = null;
            _genericArguments = new ReadOnlyCollection<TypeChoice>(new TypeChoice[0]);
            _choiceDescription = new ChoiceDescription(_csName, _fullName);
        }
        public static TypeChoice CreateEnum(string ns, string name, Type underlyingType)
        {
            if (null == underlyingType)
                throw new ArgumentNullException("underlyingType");
            if (null == name)
                throw new ArgumentNullException("name");
            if (underlyingType.Equals(typeof(bool)) || !underlyingType.IsPrimitive)
                throw new ArgumentException("Invalid underlying type", "underlyingType");
            return new TypeChoice(ns, name, true, true, false, false, TypeChoice.Create(underlyingType));
        }
        public static TypeChoice CreateEnum(TypeChoice declaringType, string name, Type underlyingType)
        {
            if (null == declaringType)
                throw new ArgumentNullException("declaringType");
            if (null == underlyingType)
                throw new ArgumentNullException("underlyingType");
            if (null == name)
                throw new ArgumentNullException("name");
            if (underlyingType.Equals(typeof(bool)) || !underlyingType.IsPrimitive)
                throw new ArgumentException("Invalid underlying type", "underlyingType");
            return new TypeChoice(declaringType, name, true, true, TypeChoice.Create(underlyingType));
        }
        public static TypeChoice CreateType(string ns, string name, bool isValueType, params TypeChoice[] genericArgs)
        {
            if (null == name)
                throw new ArgumentNullException("name");
            return new TypeChoice(ns, name, isValueType, false, false, false, genericArgs);
        }
        public static TypeChoice CreateType(TypeChoice declaringType, string name, bool isValueType, params TypeChoice[] genericArgs)
        {
            if (null == declaringType)
                throw new ArgumentNullException("declaringType");
            if (null == name)
                throw new ArgumentNullException("name");
            return new TypeChoice(declaringType, name, isValueType, false, genericArgs);
        }
        public static TypeChoice CreateNullable(TypeChoice underlyingType)
        {
            if (null == underlyingType)
                throw new ArgumentNullException("underlyingType");
            if (underlyingType._isNullableStruct || underlyingType.IsOtherType() || !underlyingType._isValueType)
                throw new InvalidOperationException();
            return new TypeChoice(underlyingType);
        }
        public static TypeChoice Create(Type type)
        {
            if (type.IsArray)
                return CreateArray(TypeChoice.Create(type.GetElementType()), type.GetArrayRank());
            if (type.IsNested)
            {
                if (type.IsEnum)
                    return new TypeChoice(TypeChoice.Create(type.DeclaringType), type.Name, true, true);
                if (type.IsGenericType)
                    return new TypeChoice(TypeChoice.Create(type.DeclaringType), GenericRegex.Replace(type.Name, ""), type.IsValueType,
                    false, type.GetGenericArguments().Select(g => TypeChoice.Create(g)).ToArray());
                return new TypeChoice(TypeChoice.Create(type.DeclaringType), type.Name, type.IsValueType, false);
            }
            if (type.IsGenericParameter)
                return new TypeChoice("", type.Name, false, false, false, false);
            if (type.IsEnum)
                return new TypeChoice(type.Namespace, type.Name, true, true, false, false, TypeChoice.Create(Enum.GetUnderlyingType(type)));
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                    return new TypeChoice(type.Namespace, "Nullable", false, false, true, false,
                        TypeChoice.Create(Nullable.GetUnderlyingType(type)));
                return new TypeChoice(type.Namespace, GenericRegex.Replace(type.Name, ""), type.IsValueType, false, false,
                    type.GetGenericTypeDefinition().Equals(typeof(ObservableCollection<>)),
                    type.GetGenericArguments().Select(g => TypeChoice.Create(g)).ToArray());
            }
            if (type.Equals(typeof(bool)))
                return new TypeChoice(type.Namespace, "bool", type.Name, true, true);
            if (type.Equals(typeof(byte)))
                return new TypeChoice(type.Namespace, "byte", type.Name, true, true);
            if (type.Equals(typeof(sbyte)))
                return new TypeChoice(type.Namespace, "sbyte", type.Name, true, true);
            if (type.Equals(typeof(short)))
                return new TypeChoice(type.Namespace, "short", type.Name, true, true);
            if (type.Equals(typeof(ushort)))
                return new TypeChoice(type.Namespace, "ushort", type.Name, true, true);
            if (type.Equals(typeof(int)))
                return new TypeChoice(type.Namespace, "int", type.Name, true, true);
            if (type.Equals(typeof(uint)))
                return new TypeChoice(type.Namespace, "uint", type.Name, true, true);
            if (type.Equals(typeof(long)))
                return new TypeChoice(type.Namespace, "long", type.Name, true, true);
            if (type.Equals(typeof(ulong)))
                return new TypeChoice(type.Namespace, "ulong", type.Name, true, true);
            if (type.Equals(typeof(float)))
                return new TypeChoice(type.Namespace, "float", type.Name, true, true);
            if (type.Equals(typeof(double)))
                return new TypeChoice(type.Namespace, "double", type.Name, true, true);
            if (type.Equals(typeof(decimal)))
                return new TypeChoice(type.Namespace, "decimal", type.Name, true, true);
            if (type.Equals(typeof(char)))
                return new TypeChoice(type.Namespace, "char", type.Name, true, true);
            if (type.Equals(typeof(string)))
                return new TypeChoice(type.Namespace, "string", type.Name, true, true);
            return new TypeChoice(type.Namespace, type.Name, type.Name, type.IsValueType, false);
        }
        public static TypeChoice CreateArray(TypeChoice elementType, int arrayRank)
        {
            if (null == elementType)
                throw new ArgumentNullException("elementType");
            return (arrayRank < 1) ? elementType : new TypeChoice(elementType, arrayRank);
        }
        public bool Equals(TypeChoice other)
        {
            if (null == other)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (_arrayRank != other._arrayRank || _csName != other._csName || _fullName != other._fullName || _isEnum != other._isEnum ||
                    _isNullableStruct != other._isNullableStruct || _isValueType != other._isValueType ||
                    _genericArguments.Count != other._genericArguments.Count)
                return false;
            if (null == _elementType)
            {
                if (null != other._elementType)
                    return false;
            }
            else if (null == other._elementType || !_elementType.Equals(other._elementType))
                return false;
            if (null == _declaringType)
            {
                if (null != other._declaringType)
                    return false;
            }
            else if (null == other._declaringType || !_declaringType.Equals(other._declaringType))
                return false;
            for (int i = 0; i < _genericArguments.Count; i++)
            {
                if (!_genericArguments[i].Equals(other._genericArguments[i]))
                    return false;
            }
            return true;
        }
        public bool Equals(ChoiceDescription other)
        {
            if (null == other)
                return false;
            if (ReferenceEquals(other, _choiceDescription))
                return true;
            return _choiceDescription.Label == other.Label && _choiceDescription.HelpMessage == other.HelpMessage;
        }
        public override bool Equals(object obj)
        {
            return null != obj && obj is TypeChoice && Equals((TypeChoice) obj);
        }
        public override int GetHashCode()
        {
            return _fullName.GetHashCode();
        }
        public override string ToString()
        {
            return _fullName;
        }
        public int IndexOf(Collection<ChoiceDescription> choiceDescriptions, ChoiceDescription d)
        {
            if (null == d || null == choiceDescriptions)
                return -1;
            for (int i = 0; i < choiceDescriptions.Count; i++)
            {
                ChoiceDescription c = choiceDescriptions[i];
                if (ReferenceEquals(c, d) || (c.Label == d.Label && c.HelpMessage == d.HelpMessage))
                    return i;
            }
            return -1;
        }
        public object PromptForChoice(PSHost host, string caption, string message, IEnumerable choices, TypeChoice defaultChoice)
        {
            Collection<ChoiceDescription> choiceDescriptions = new Collection<ChoiceDescription>();
            Collection<TypeChoice> typeChoices = new Collection<TypeChoice>();
            int index;
            if (null != choices)
            {
                foreach (object o in choices)
                {
                    if (null != o)
                    {
                        object b = (o is PSObject) ? ((PSObject)o).BaseObject : o;
                        if (b is TypeChoice)
                        {
                            TypeChoice c = (TypeChoice)b;
                            if ((index = IndexOf(choiceDescriptions, c._choiceDescription)) < 0)
                            {
                                choiceDescriptions.Add(c._choiceDescription);
                                typeChoices.Add(c);
                            }
                            else
                            {
                                choiceDescriptions[index] = c._choiceDescription;
                                if ((index = typeChoices.IndexOf(c)) < 0)
                                    typeChoices.Add(c);
                                else
                                    typeChoices[index] = c;
                            }
                        }
                        else if (b is ChoiceDescription)
                        {
                            ChoiceDescription c = (ChoiceDescription)b;
                            if ((index = IndexOf(choiceDescriptions, c)) < 0)
                                choiceDescriptions.Add(c);
                        }
                    }
                }
            }
            if (null == defaultChoice)
            {
                if (choiceDescriptions.Count == 0)
                    return null;
                index = 0;
            }
            else if ((index = IndexOf(choiceDescriptions, defaultChoice._choiceDescription)) < 0)
            {
                index = 0;
                choiceDescriptions.Insert(0, defaultChoice._choiceDescription);
                typeChoices.Insert(0, defaultChoice);
            }
            else
            {
                choiceDescriptions[index] = defaultChoice._choiceDescription;
                int i = typeChoices.IndexOf(defaultChoice);
                if (i < 0)
                    typeChoices.Add(defaultChoice);
                else
                    typeChoices[i] = defaultChoice;
            }
            if ((index = host.UI.PromptForChoice(caption, message, choiceDescriptions, index)) < 0 || index >= choiceDescriptions.Count)
                return null;
            ChoiceDescription cd = choiceDescriptions[index];
            foreach (TypeChoice tc in typeChoices)
            {
                if (tc.Equals(cd))
                    return tc;
            }
            return cd;
        }
    }
}
namespace System.Management.Automation
{
    public class PSCustomObject
    {

    }
    public abstract class PSObject
    {
        public object BaseObject { get; }
    }
}

namespace System.Management.Automation.Host
{
    public sealed class ChoiceDescription
    {
        public string Label { get; }
        public string HelpMessage { get; }
        public ChoiceDescription(string label, string helpMessage)
        {
            Label = label;
            HelpMessage = helpMessage;
        }
    }
    public abstract class PSHostUserInterface
    {
        public abstract int PromptForChoice(string caption, string message, System.Collections.ObjectModel.Collection<ChoiceDescription> choices, int defaultChoice);
    }
    public abstract class PSHost
    {
        public abstract PSHostUserInterface UI { get; }
    }
}
