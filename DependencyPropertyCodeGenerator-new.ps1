Add-Type -TypeDefinition @'
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
'@ -ReferencedAssemblies 'System.Management.Automation';

Function Convert-TypeToCsName {
    [CmdletBinding(DefaultParameterSetName = 'Optimal')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Optimal')]
        [Type[]]$Type,
        [switch]$Full
    )
    Process {
        if ($Full.IsPresent) {
            foreach ($t in $Type) {
                if ($t.IsArray) {
                    $n = (Convert-TypeToCsName -Type $t.GetElementType() -Full) + "[";
                    $r = $t.GetArrayRank();
                    for ($i = 1; $i -lt $r; $i++) { $n += ',' }
                    $n + ']';
                } else {
                    if ($t.IsNested) {
                        if ($t.IsGenericTypeDefinition) {
                            (Convert-TypeToCsName -Type $t.DeclaringType -Full) + '.' + ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | ForEach-Object { $_.name }) -join ', ') + ">";
                        } else {
                            if ($t.IsGenericType) {
                                (Convert-TypeToCsName -Type $t.DeclaringType -Full) + '.' + ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | Convert-TypeToCsName) -join ', ') + ">";
                            } else {
                                (Convert-TypeToCsName -Type $t.DeclaringType -Full) + '.' + $t.Name;
                            }
                        }
                    } else {
                        if ($t.IsGenericTypeDefinition) {
                            ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | ForEach-Object { $_.Name }) -join ', ') + ">";
                        } else {
                            if ($t.IsGenericType) {
                                if ($t.GetGenericTypeDefinition() -eq [Nullable``1]) {
                                    (Convert-TypeToCsName -Type ([Nullable]::GetUnderlyingType($t)) -Full) + '?';    
                                } else {
                                    if ([string]::IsNullOrEmpty($t.Namespace)) {
                                        ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | Convert-TypeToCsName) -join ', ') + ">";
                                    } else {
                                        $t.Namespace + '.' + ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | Convert-TypeToCsName) -join ', ') + ">";
                                    }
                                }
                            } else {
                                if ([string]::IsNullOrEmpty($t.Namespace)) {
                                    $t.Name;
                                } else {
                                    $t.Namespace + '.' + $t.Name;
                                }
                            }
                        }
                    }
                }
            }
        } else {
            foreach ($t in $Type) {
                if ($t.IsArray) {
                    $n = (Convert-TypeToCsName -Type $t.GetElementType()) + "[";
                    $r = $t.GetArrayRank();
                    for ($i = 1; $i -lt $r; $i++) { $n += ',' }
                    $n + ']';
                } else {
                    if ($t.IsNested) {
                        if ($t.IsGenericTypeDefinition) {
                            (Convert-TypeToCsName -Type $t.DeclaringType) + '.' + ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | ForEach-Object { $_.name }) -join ', ') + ">";
                        } else {
                            if ($t.IsGenericType) {
                                (Convert-TypeToCsName -Type $t.DeclaringType) + '.' + ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | Convert-TypeToCsName) -join ', ') + ">";
                            } else {
                                (Convert-TypeToCsName -Type $t.DeclaringType) + '.' + $t.Name;
                            }
                        }
                    } else {
                        if ($t.IsGenericTypeDefinition) {
                            ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | ForEach-Object { $_.Name }) -join ', ') + ">";
                        } else {
                            if ($t.IsGenericType) {
                                if ($t.GetGenericTypeDefinition() -eq [Nullable``1]) {
                                    (Convert-TypeToCsName -Type ([Nullable]::GetUnderlyingType($t))) + '?';    
                                } else {
                                    ($t.Name -replace '`\d+$', '') + "<" + (($t.GetGenericArguments() | Convert-TypeToCsName) -join ', ') + ">";
                                }
                            } else {
                                switch ($t.FullName) {
                                    'System.Boolean' { 'bool'; break; }
                                    'System.Byte' { 'byte'; break; }
                                    'System.SByte' { 'sbyte'; break; }
                                    'System.Int16' { 'short'; break; }
                                    'System.UInt16' { 'ushort'; break; }
                                    'System.Int32' { 'int'; break; }
                                    'System.UInt32' { 'uint'; break; }
                                    'System.Int64' { 'long'; break; }
                                    'System.UInt64' { 'ulong'; break; }
                                    'System.Single' { 'float'; break; }
                                    'System.Double' { 'double'; break; }
                                    'System.Char' { 'char'; break; }
                                    'System.String' { 'string'; break; }
                                    default {
                                        $t.Name;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

Function New-TypeChoice {
    [CmdletBinding(DefaultParameterSetName = 'Pipelined')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Pipelined')]
        [Type[]]$InputType,
        [Parameter(Mandatory = $true, ParameterSetName = 'Type')]
        [Type]$Type,
        [Parameter(Mandatory = $true, ParameterSetName = 'ObservableCollection')]
        [Parameter(Mandatory = $true, ParameterSetName = 'Nullable')]
        [Dpcg.TypeChoice]$Choice,
        [Parameter(Mandatory = $true, ParameterSetName = 'ObservableCollection')]
        [switch]$ObservableCollection,
        [Parameter(Mandatory = $true, ParameterSetName = 'Nullable')]
        [switch]$Nullable,
        [Parameter(Mandatory = $true, ParameterSetName = 'Class')]
        [switch]$OtherClass,
        [Parameter(Mandatory = $true, ParameterSetName = 'Struct')]
        [switch]$OtherStruct,
        [string]$Namespace,
        [string]$Name,
    )

    Process {
        switch ($PSCmdlet.ParameterSetName) {
            'Pipelined' {
                $InputType | ForEach-Object { [Dpcg.TypeChoice]::Create($_) }
                break;
            }
            'Type' {
                [Dpcg.TypeChoice]::Create($Type);
                break;
            }
            'ObservableCollection' {
                # TODO: Implement this
                break;
            }
            'Nullable' {
                [Dpcg.TypeChoice]::CreateNullable($Choice.Type);
                break;
            }
            default {
                if ($OtherStruct.IsPresent) {
                    [Dpcg.TypeChoice]::OtherStruct;
                } else {
                    [Dpcg.TypeChoice]::OtherClass;
                }
            }
        }
    }
}

Function Convert-ToCommentLines {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [xml]$Xml,
        [switch]$Contents
    )
    $Sw = [System.IO.StringWriter]::new();
    $XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
    $XmlWriterSettings.OmitXmlDeclaration = $true;
    if ($Contents.IsPresent) {
        $XmlWriterSettings.ConformanceLevel = [System.Xml.ConformanceLevel]::Fragment;
    } else {
        $XmlWriterSettings.ConformanceLevel = [System.Xml.ConformanceLevel]::Document;
    }
    $XmlWriter = [System.Xml.XmlWriter]::Create($Sw, $XmlWriterSettings);
    if ($Contents.IsPresent) {
        $Elements = @($Xml.DocumentElement.SelectNodes('*'));
        $Elements[0].WriteTo($XmlWriter);
        ($Elements | Select-Object -Skip 1) | ForEach-Object {
            $XmlWriter.Flush();
            $Sw.WriteLine();
            $_.WriteTo($XmlWriter);
        }
    } else {
        $Xml.DocumentElement.WriteTo($XmlWriter);
    }
    $XmlWriter.Flush();
    (($Sw.ToString() -split '\r\n?|\n') | ForEach-Object { '        /// ' + $_.Trim() }) -join ([System.Environment]::NewLine);
    $XmlWriter.Close();
}

Function Test-IsCollection {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Dpcg.TypeChoice]$TypeChoice,
        [switch]$Observable
    )
    if ($Observable.IsPresent) {
        $TypeChoice.Type.IsGenericType() -and [System.Collections.ObjectModel.ObservableCollection``1].IsAssignableFrom($TypeChoice.Type);
    } else {
        [System.Collections.ICollection].IsAssignableFrom($TypeChoice.Type);
    }
}

Function Test-IsNullable {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Dpcg.TypeChoice]$TypeChoice,
        [switch]$Struct
    )
    if ($Struct.IsPresent) {
        -not ($TypeChoice.Type.IsGenericType() -and [System.Nullable``1].IsAssignableFrom($TypeChoice.Type));
    } else {
        if ($TypeChoice.Type.IsValueType) {
            $TypeChoice.Type.IsGenericType() -and [System.Nullable``1].IsAssignableFrom($TypeChoice.Type);
        } else {
            $true;
        }
    }
}

Function Test-IsValueType {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Dpcg.TypeChoice]$TypeChoice,
        [switch]$IncludeNullable
    )
    if ($TypeChoice.Type.IsValueType) {
        $IncludeNullable.IsPresent -or -not ($TypeChoice.Type.IsGenericType() -and [System.Nullable``1].IsAssignableFrom($TypeChoice.Type));
    } else {
        $false;
    }
}

$TypeChoices = ([bool], [byte], [sbyte], [System.Int16], [System.UInt16], [int], [System.UInt32], [long], [System.UInt64], [float], [double], [char], [Guid], [string], [Uri], [System.DateTime], [System.TimeSpan]) | New-TypeChoice;

$YesNoChoices = @(
    [System.Management.Automation.Host.ChoiceDescription]::new('Yes'),
    [System.Management.Automation.Host.ChoiceDescription]::new('No')
);

$PropertyLabel = Read-Host -Prompt 'Property Name';
if ($null -eq $PropertyLabel -or ($PropertyLabel = $PropertyLabel.Trim()).Length -eq 0) { return }

$Elements = @($PropertyLabel -split '\s+');
$PropertyName = $FieldName = $null;
if ($Elements.Count -eq 1) {
    $PropertyName = [System.Xml.XmlConvert]::EncodeLocalName($PropertyLabel);
    if ($PropertyName.Length -eq 1) {
        $PropertyName = $PropertyName.ToUpper();
    } else {
        $PropertyName = $PropertyName.Substring(0, 1).ToUpper() + $PropertyName.Substring(1).ToLower();
    }
} else {
    $PropertyName = -join ($Elements | ForEach-Object {
        $n = [System.Xml.XmlConvert]::EncodeLocalName($_);
        if ($n.Length -eq 1) {
            $n.ToUpper();
        } else {
            $n.Substring(0, 1).ToUpper() + $n.Substring(1).ToLower();
        }
    });
}
[Dpcg.TypeChoice]$PropertyType = [Dpcg.TypeChoice]::PromptForChoice($Host, 'Property Type', 'Select property type', $TypeChoices);
$i = $Host.UI.PromptForChoice('Property Type', 'Select property type', $TypeChoices, 0);
if ($null -eq $PropertyType) { return }
if ($PropertyType.IsOtherType()) {
    $ns = [Guid]::NewGuid().ToString('n');
    $TypeName = '';
    if ($PropertyType.CsName -eq 'struct') {
        $TypeName = Read-Host -Prompt 'Struct Name';
        if ($null -eq $TypeName -or ($TypeName = $TypeName.Trim()).Length -eq 0) { return }
        $i = 1;
        if (-not $TypeName.Contains('<')) {
            $i = $Host.UI.PromptForChoice('Enum', 'Is this an enum?', $YesNoChoices, 0);
        }
        if ($i -ne 0) {
            Add-Type -TypeDefinition @"
namespace $ns
{
    public enum $TypeName
    {
        First,
        Second
    }
}
"@
        } else {
            if ($i -ne 1) { return }
                Add-Type -TypeDefinition @"
namespace $ns
{
    public struct $TypeName
    {
        private int _first;
        private int _second;
        public int First { get { return _first; } }
        public int Second { get { return _second; } }
        public $TypeName(int first, int second)
        {
            _first = first;
            _second = second;
        }
    }
}
"@
        }
    } else {
        $TypeName = Read-Host -Prompt 'Class Name';
        if ($null -eq $TypeName -or ($TypeName = $TypeName.Trim()).Length -eq 0) { return }
            Add-Type -TypeDefinition @"
namespace $ns
{
    public class $TypeName
    {
        private int _first;
        private int _second;
        public int First { get { return _first; } }
        public int Second { get { return _second; } }
        public $TypeName(int first, int second)
        {
            _first = first;
            _second = second;
        }
    }
}
"@
    }
    $t = $null;
    $t = [scriptblock]::Create("[$ns.$TypeName]").Invoke();
    if ($null -eq $t) { return }
    $PropertyType = New-TypeChoice -Type $t;
}

if (Test-IsCollection -TypeChoice $PropertyType) {
    $i = $Host.UI.PromptForChoice('Collection', 'Is this an observable collection?', $YesNoChoices, 0);
    if ($i -eq 0) {
        $PropertyType = New-TypeChoice -Choice $PropertyType -ObservableCollection;
    } else {
        if ($i -ne 1) { return }
    }
}
if (Test-IsValueType -TypeChoice $PropertyType) {
    $i = $Host.UI.PromptForChoice('Collection', 'Is this nullable?', $YesNoChoices, 0);
    if ($i -eq 0) {
        $PropertyType = New-TypeChoice -Choice $PropertyType.Type -Nullable;
    } else {
        if ($i -ne 1) { return }
    }
    
}

$i = $Host.UI.PromptForChoice('Read-Only', 'Is this read-only?', $YesNoChoices, 0);
$IsReadOnly = $false;
if ($i -eq 0) {
    $IsReadOnly = $true;
} else {
    if ($i -ne 1) { return }
}
$HandlePropertyChanged = $false;
$RelayPropertyChangedEvent = $false;
$i = $Host.UI.PromptForChoice('Property Change', 'Relay Property Changed event?', $YesNoChoices, 0);
if ($i -eq 0) {
    $HandlePropertyChanged = $RelayPropertyChangedEvent = $true;
} else {
    if ($i -ne 1) { return }
}
if (-not $HandlePropertyChanged) {
    $i = $Host.UI.PromptForChoice('Property Change', 'Handle Property Changed event?', $YesNoChoices, 0);
    if ($i -eq 0) {
        $HandlePropertyChanged = $true;
    } else {
        if ($i -ne 1) { return }
    }
}
$DefaultValue = 'null';
switch ($PropertyType.Type.FullName) {
    'System.String' {
        $DefaultValue = '""';
        $HandleCoerceValue = $true;
        break;
    }
    'System.Byte' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'System.SByte' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'System.UInt16' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'System.Int32' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'System.UInt32' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'System.Int64' {
        $DefaultValue = '0L';
        $HandleCoerceValue = $true;
        break;
    }
    'System.UInt64' {
        $DefaultValue = '0L';
        $HandleCoerceValue = $true;
        break;
    }
    'System.Boolean' {
        $DefaultValue = 'false';
        $HandleCoerceValue = $true;
        break;
    }
    default {
        if (Test-IsCollection -TypeChoice $PropertyType -Observable) {
            $HandleCoerceValue = $true;
        } else {
            if (Test-IsValueType -TypeChoice $PropertyType) {
                $DefaultValue = "default($($PropertyType.CsName))";
                $HandleCoerceValue = $true;
            }
        }
        break;
    }
}

if (-not $HandleCoerceValue) {
    $i = $Host.UI.PromptForChoice('Coerce Value', 'Handle Coerce Value event?', $YesNoChoices, 0);
    $HandleCoerceValue = $false;
    if ($i -eq 0) {
        $HandleCoerceValue = $true;
    } else {
        if ($i -ne 1) { return }
    }
}
$VmType = Read-Host -Prompt 'View Model Type Name';
if ($null -eq $VmType -or ($VmType = $VmType.Trim()).Length -eq 0) { return }

$sb = [System.Text.StringBuilder]::new();
$sb.AppendLine().Append('        #region ');
if ($PropertyLabel -eq $PropertyName) {
    $sb.Append($PropertyName) | Out-Null;
} else {
    $sb.Append('"').Append($PropertyLabel).Append('" (').Append($PropertyName).Append(')') | Out-Null;
}
[xml]$Xml = '<summary />';
$Xml.PreserveWhitespace = $true;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nDefines the name for the ")) | Out-Null;
$Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $PropertyName;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;
$sb.AppendLine(' Property Members').AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public const string PropertyName_').Append($PropertyName).Append(' = "').Append($PropertyName).AppendLine('";').AppendLine() | Out-Null;
[xml]$Xml = '<summary />';
$Xml.PreserveWhitespace = $true;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nIdentifies the ")) | Out-Null;
$Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $PropertyName;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;

if ($IsReadOnly) {
    $sb.Append('        private static readonly DependencyPropertyKey ').Append($PropertyName).Append('PropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_').Append($PropertyName) | Out-Null;
} else {
    $sb.AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($PropertyName).Append('Property = DependencyProperty.Register(PropertyName_').Append($PropertyName) | Out-Null;
}

$sb.Append(', typeof(').Append($PropertyType.CsName).Append('), typeof(').Append($VmType).AppendLine('),').Append('                new PropertyMetadata(').Append($DefaultValue) | Out-Null;
if ($HandlePropertyChanged -or $HandleCoerceValue) {
    if ($HandlePropertyChanged) {
        $OldType = $PropertyType;
        if (Test-IsValueType -TypeChoice $PropertyType) {
            $OldType = New-TypeChoice -Choice $PropertyType -Nullable;
        }
        $sb.AppendLine(',').Append('                    (d, e) => (d as ').Append($VmType).Append(').On').Append($PropertyName).Append('PropertyChanged(e.OldValue as ').Append($OldType.CsName) | Out-Null;
        if (Test-IsNullable -TypeChoice $PropertyType) {
            $sb.Append(', e.NewValue as ').Append($PropertyType.CsName) | Out-Null;
        } else {
            $sb.Append(', (').Append($PropertyType.CsName).Append(')(e.NewValue)') | Out-Null;
        }
        $sb.Append(')') | Out-Null;
    } else {
        if ($HandleCoerceValue) { $sb.Append(', null') | Out-Null; }
    }
    if ($HandleCoerceValue) {
        $sb.AppendLine(',').Append('                    (d, baseValue) => Coerce').Append($PropertyName).Append('Value(baseValue as ').Append($PropertyType.CsName) | Out-Null;
        $sb.Append(')') | Out-Null;
    }






    $sb.AppendLine().AppendLine('            )').AppendLine('        );') | Out-Null;
    if ($IsReadOnly) {
        $sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($PropertyName).Append('Property = ').Append($PropertyName).AppendLine('PropertyKey.DependencyProperty;') | Out-Null;
    }
    if ($HandlePropertyChanged) {
        $sb.AppendLine().Append('        private void On').Append($PropertyName).Append('PropertyChanged(').Append($PropertyType.CsName) | Out-Null;
        $sb.Append(' oldValue, ').Append($PropertyType.CsName).AppendLine(' newValue)').AppendLine('        {') | Out-Null;
        if (Test-IsCollection -TypeChoice $PropertyType -Observable) {
            $sb.AppendLine('            if (null != oldValue)').Append('                oldValue.CollectionChanged -= On').Append($PropertyName).AppendLine('CollectionChanged;') | Out-Null;
            $sb.Append('            newValue.CollectionChanged += On').Append($PropertyName).AppendLine('CollectionChanged;') | Out-Null;
            $sb.Append('            On').Append($PropertyName).AppendLine('CollectionChanged(newValue, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));') | Out-Null;
            $sb.AppendLine('        }').AppendLine().Append('        private void On').Append($PropertyName).AppendLine('CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)').AppendLine('        {') | Out-Null;
            $sb.Append('            // TODO: Implement On').Append($PropertyName).AppendLine('CollectionChanged') | Out-Null;
        } else {
            $sb.Append('            // TODO: Implement On').Append($PropertyName).AppendLine('PropertyChanged') | Out-Null;
        }
        $sb.AppendLine('            throw new NotImplementedException();').AppendLine('        }') | Out-Null;
    }
    if ($HandleCoerceValue) {
        $sb.AppendLine().Append('        public static ').Append($PropertyType.CsName).Append(' Coerce').Append($PropertyName).Append('Value(').Append($PropertyType.CsName) | Out-Null;
        $sb.AppendLine(' value)').AppendLine('        {');
        if ($DefaultValue -eq 'null') {
            if (Test-IsCollection -TypeChoice $PropertyType -Observable) {
                $sb.AppendLine('            if (null == value)').Append('                return new ').Append($PropertyType.CsName).AppendLine('();').AppendLine('            return value;') | Out-Null;
            } else {
                $sb.Append('            // TODO: Implement On').Append($PropertyName).AppendLine('PropertyChanged').AppendLine('            throw new NotImplementedException();') | Out-Null;
            }
        } else {
            if ($Nullable) {
                $sb.AppendLine('            if (null == value)').Append('                return ').Append($DefaultValue).AppendLine(';').AppendLine('            return value;') | Out-Null;
            } else {
                $sb.AppendLine('            if (value.HasValue)').AppendLine('                return value.Value;').Append('            return ').Append($DefaultValue).AppendLine(';') | Out-Null;
            }
        }
        $sb.AppendLine('        }') | Out-Null;
    }
} else {
    $sb.AppendLine('));') | Out-Null;
}
[xml]$Xml = '<summary />';
$Xml.PreserveWhitespace = $true;
$Xml.DocumentElement.InnerText = "`n$PropertyLabel`n";
$sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public ').Append($PropertyType.CsName).Append(' ').AppendLine($PropertyName).AppendLine('        {') | Out-Null;
$sb.AppendLine('            get').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    return (').Append($PropertyType.CsName).Append(')(GetValue(').Append($PropertyName).AppendLine('Property));') | Out-Null;
$sb.Append('                return Dispatcher.Invoke(() => (').Append($PropertyType.CsName).Append(')(GetValue(').Append($PropertyName).AppendLine('Property)));').AppendLine('            }').Append('            ') | Out-Null ;
if ($IsReadOnly) { $sb.Append('private ') | Out-Null }
$sb.AppendLine('set').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    SetValue(').Append($PropertyName).Append('Property') | Out-Null;
if ($IsReadOnly) { $sb.Append('Key') | Out-Null }
$sb.AppendLine(', value);').AppendLine('                else').Append('                    Dispatcher.Invoke(() => SetValue(').Append($PropertyName).Append('Property') | Out-Null;
if ($IsReadOnly) { $sb.Append('Key') | Out-Null }
$sb.AppendLine(', value));').AppendLine('            }').AppendLine('        }').AppendLine().AppendLine('        #endregion') | Out-Null;
[System.Windows.Clipboard]::SetText($sb.ToString());