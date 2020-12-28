Add-Type -TypeDefinition @'
namespace CodeHelper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Management.Automation;
    using System.Management.Automation.Host;
    using System.Text.RegularExpressions;
    public class ChoiceValueDescription : PSObject, IEquatable<ChoiceValueDescription>, IComparable<ChoiceValueDescription>
    {
        private readonly PSObject _value;
        private readonly ChoiceDescription _choiceDescription;
        public string Label { get { return _choiceDescription.Label; } }
        public string HelpMessage { get { return _choiceDescription.HelpMessage; } }
        public ChoiceValueDescription(object value, ChoiceDescription description) : base(description)
        {
            _choiceDescription = description;
            _value = (null == value || value is PSObject) ? value as PSObject : PSObject.AsPSObject(value);
        }
        public bool Equals(ChoiceValueDescription other)
        {
            return null != other && (ReferenceEquals(this, other) || (((null == other._value) ? null == _value : other._value.Equals(_value)) && base.Equals(this)));
        }
        public PSObject GetValue() { return _value; }
        public int CompareTo(ChoiceValueDescription other)
        {
            if (null == other)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            if (null == other._value)
                return (null == _value) ? 0 : 1;
            if (null == _value)
                return -1;
            int result = _value.CompareTo(other._value);
            return (result == 0) ? base.CompareTo(other) : result;
        }
        public static ChoiceValueDescription Create(object obj)
        {
            if (null == obj || obj is ChoiceValueDescription)
                return obj as ChoiceValueDescription;
            for (PSObject o = obj as PSObject; null != o; o = o.ImmediateBaseObject as PSObject)
            {
                if (o is ChoiceValueDescription)
                    return o as ChoiceValueDescription;
            }
            PSObject psObject;
            if (obj is PSObject)
                obj = (psObject = (PSObject)obj).BaseObject;
            else
                psObject = PSObject.AsPSObject(obj);
            if (obj is ChoiceDescription)
            {
                ChoiceDescription choiceDescription = (ChoiceDescription)obj;
                IEnumerable<PSPropertyInfo> properties = psObject.Properties.Match("Value", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance);
                PSPropertyInfo pi = properties.FirstOrDefault(p => null != p.Value);
                return new ChoiceValueDescription((null != pi) ? pi.Value : properties.Select(p => p.Value).DefaultIfEmpty(choiceDescription.Label), choiceDescription);
            }
            string label = null, helpMessage = null;
            object value = null;
            bool valueIsNull = false;
            if (obj is IDictionary)
            {
                IDictionary dictionary = (IDictionary)obj;
                if (dictionary.Contains("Label"))
                {
                    object a = dictionary["Label"];
                    if (null != a)
                    {
                        object o = (a is PSObject) ? ((PSObject)a).BaseObject : a;
                        label = ((o is string) ? (string)o : a.ToString()).Trim();
                    }
                }
                if (dictionary.Contains("HelpMessage"))
                {
                    object a = dictionary["HelpMessage"];
                    if (null != a)
                    {
                        object o = (a is PSObject) ? ((PSObject)a).BaseObject : a;
                        helpMessage = ((o is string) ? (string)o : a.ToString()).Trim();
                    }
                }
                if (dictionary.Contains("Value"))
                    valueIsNull = null == (value = dictionary["Value"]);
            }
            if (string.IsNullOrWhiteSpace(label))
            {
                IEnumerable<PSPropertyInfo> properties = psObject.Properties.Match("Label", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance);
                IEnumerable<string> sv = properties.Select(p =>
                {
                    if (null == p.Value)
                        return null;
                    object o = (p.Value is PSObject) ? ((PSObject)p.Value).BaseObject : p.Value;
                    return (o is string) ? (string)o : o.ToString();
                });
                label = sv.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            }
            if (null == value && !valueIsNull)
                value = psObject.Properties.Match("Value", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance && null != p.Value).Select(p => p.Value).DefaultIfEmpty(label).First();
            if (string.IsNullOrWhiteSpace(label) && string.IsNullOrWhiteSpace(label = (null == value) ? null : value.ToString()) && string.IsNullOrWhiteSpace(label = obj.ToString()))
                throw new ArgumentException("Could not calculate a label string", "obj");
            if (string.IsNullOrWhiteSpace(helpMessage))
                helpMessage = psObject.Properties.Match("HelpMessage", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance && null != p.Value)
                    .Select(p => (p.Value is string) ? (string)p.Value : p.Value.ToString()).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            if (null == value && !valueIsNull)
                value = label;
            return new ChoiceValueDescription((null != value || valueIsNull) ? value : label, (string.IsNullOrWhiteSpace(helpMessage)) ? new ChoiceDescription(label) : new ChoiceDescription(label.Trim(), helpMessage.Trim()));
        }
        public static Collection<ChoiceValueDescription> ToCollection(IEnumerable source)
        {
            Collection<ChoiceValueDescription> result = new Collection<ChoiceValueDescription>();
            if (null != source)
            {
                if (source is string)
                    result.Add(Create(source));
                else
                    foreach (object obj in source)
                    {
                        ChoiceValueDescription d = Create(obj);
                        if (null != d && !result.Contains(d))
                            result.Add(d);
                    }
            }
            return result;
        }
    }
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
            _namespace = (null == ns) ? "" : ns;
            _isValueType = isValueType;
            _defaultValue = (isValueType) ? "default(" + _shortHand + ")" : "null";
            _genericArguments = new ReadOnlyCollection<CsTypeModel>((null == genericArguments) ? new CsTypeModel[0] : genericArguments.Where(a => null != a).ToArray());
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
            _genericArguments = new ReadOnlyCollection<CsTypeModel>((null == genericArguments) ? new CsTypeModel[0] : genericArguments.Where(a => null != a).ToArray());
        }
        public CsTypeModel(Type type)
        {
            if (null == type)
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
                _namespace = (null == type.Namespace) ? "" : type.Namespace;
                _shortHand = _name = GenericNameRegex.Replace(type.Name, "") + "<" + string.Join(", ", _genericArguments.Select(a => a._shortHand).ToArray()) + ">";
                _isNullableStruct = false;
                _isValueType = type.IsValueType;
                _namespace = (null == type.Namespace) ? "" : type.Namespace;
                _defaultValue = (_isValueType) ? "default(" + _shortHand + ")" : "null";
                return;
            }
            _namespace = (null == type.Namespace) ? "" : type.Namespace;
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
'@ -ReferencedAssemblies 'System.Management.Automation' -ErrorAction Stop;

Function Test-CsClassName {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Name
    )

    Begin {
        if ($null -eq $Script:__Test_CsClassName) { $Script:__Test_CsClassName = [System.Text.RegularExpressions.Regex]::new('^[A-Z][A-Za-z\d]*$', [System.Text.RegularExpressions.RegexOptions]::Compiled) }
        $Result = $true;
    }
    Process {
        if ($Result -and -not $Script:__Test_CsClassName.IsMatch($Name)) {
            if ($DisplayWarning.IsPresent) {
                Write-Warning -Message "$Name is an invalid class name"
            } else {
                Write-Information -MessageData "$Name is valid";
            }
            $Result = $false;
        }
    }
    End {
        $Result | Write-Output
    }
}

Function Test-FileInfo {
    [CmdletBinding(DefaultParameterSetName = 'Exists')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Path,
        [Parameter(Mandatory = $true, ParameterSetName = 'Created')]
        [switch]$AsCreated,
        [Parameter(ParameterSetName = 'NotExists')]
        [switch]$AssertNotExists,
        [Parameter(Mandatory = $true, ParameterSetName = 'Exists')]
        [switch]$AssertExists
    )

    Begin {
        $Result = $true;
    }
    Process {
        $FileInfo = $null;
        $FileInfo = [System.IO.FileInfo]::new($Path);
        if ($null -eq $FileInfo) {
            $Result = $false;
            Write-Warning -Message "Failed to validate $Path";
        } else {
            if ($FileInfo.Exists) {
                if ($AsCreated.IsPresent) {
                    Write-Information -MessageData "$($FileInfo.Length) bytes written to $($FileInfo.FullName).";
                } else {
                    if ($AssertExists.IsPresent) {
                        Write-Information -MessageData "$($FileInfo.FullName) contains $($FileInfo.Length) bytes.";
                    } else {
                        if ($AssertNotExists.IsPresent) {
                            Write-Warning -Message "$($FileInfo.FullName) contains $($FileInfo.Length) bytes.";
                        }
                    }
                }
            } else {
                $Result = $false;
                if ($AsCreated.IsPresent) {
                    Write-Warning -Message "Failed to create $($FileInfo.FullName).";
                } else {
                    if ($AssertExists.IsPresent) {
                        Write-Warning -Message "$($FileInfo.FullName) does not exist.";
                    } else {
                        if ($AssertNotExists.IsPresent) {
                            Write-Information -MessageData "$($FileInfo.FullName) does not exist.";
                        }
                    }
                }
            }
        }
    }
    End {
        $Result | Write-Output
    }
}

Function Read-CsClassName {
    [CmdletBinding()]
    Param(
        [string]$Prompt = 'Enter class name'
    )
    
    $Name = '';
    do {
        $Name = Read-Host -Prompt $Prompt;
        if ($null -eq $Name -or ($Name = $Name.Trim()).Length -eq 0) { return }
    } while (-not ($Name | Test-CsClassName));
    $Name | Write-Output;
}

Function Read-Choice {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [object[]]$Options,
        [Parameter(Mandatory = $true)]
        [string]$Caption,
        [Parameter(Mandatory = $true)]
        [string]$Message,
        [object]$DefaultChoice,
        [switch]$GridView
    )

    Begin {
        $AllOptions = @();
    }

    Process {
        $AllOptions += @($Options);
    }

    End {
        $Collection = [CodeHelper.ChoiceValueDescription]::ToCollection($AllOptions);
        $d = $null;
        $i = 0;
        if ($PSBoundParameters.ContainsKey($DefaultChoice)) {
            $d = [CodeHelper.ChoiceValueDescription]::Create($DefaultChoice);
            if (($i = $Collection.IndexOf($d)) -lt 0) {
                $Collection.Insert(0, $d);
                $i = 0;
            }
        }
        if ($Collection.Count -gt 0) {
            $Result = $null;
            if ($GridView.IsPresent) {
                $Result = $Collection | Out-GridView -Title "$Caption`: $Message" -OutputMode Single;
            } else {
                $i = $Host.UI.PromptForChoice($Caption, $Message, $Collection, $i);
                if ($i -ge 0 -and $i -lt $Collection.Count) { $Result = $Collection[$i] }
            }
            if ($null -ne $Result) { return $Result.GetValue() }
        }
    }
}

Function Read-YesOrNo {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Caption,
        [Parameter(Mandatory = $true)]
        [string]$Message,
        [bool]$Default = $false
    )

    if ($null -eq $Script:__Read_YesOrNo_Choices) {
        $Script:__Read_YesOrNo_Choices = @(
            [System.Management.Automation.Host.ChoiceDescription]::new('Yes'),
            [System.Management.Automation.Host.ChoiceDescription]::new('No')
        );
    }
    $DefaultChoice = 1;
    if ($Default) { $DefaultChoice = 0 }
    $Host.UI.PromptForChoice($Caption, $Message, $Script:__Read_YesOrNo_Choices, $DefaultChoice)
}

Function Convert-ToCommentLines {
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

Function New-WpfWindowScaffold {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidatePattern('^[A-Z][A-Za-z\d]*$')]
        [string]$Name
    )
    
    $WindowXamlPath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\$($Name)Window.xaml";
    $WindowCsPath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\$($Name)Window.xaml.cs";
    $ViewModelPath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\ViewModels\$($Name)ViewModel.cs";
    
    if ($WindowXamlPath | Test-FileInfo -InformationAction Ignore -WarningAction Continue) {
        Write-Warning -Message "Aborted.";
        return;
    }
    
    if ($WindowCsPath | Test-FileInfo -AssertNotExists -InformationAction Ignore -WarningAction Continue) {
        Write-Warning -Message "Aborted.";
        return;
    }

    if ($ViewModelPath | Test-FileInfo -InformationAction Ignore -WarningAction Continue) {
        Write-Warning -Message "Aborted.";
        return;
    }

    $Encoding = [System.Text.UTF8Encoding]::new($false, $false);
    [System.IO.File]::WriteAllText($WindowXamlPath, @"
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:local="clr-namespace:FsInfoCat.Desktop"
    xmlns:vm="clr-namespace:FsInfoCat.Desktop.ViewModels" mc:Ignorable="d" x:Class="FsInfoCat.Desktop.$($Name)Window"
    Title="$Name" Height="450" Width="800">
    <Window.DataContext>
        <vm:$($Name)ViewModel />
    </Window.DataContext>
</Window>
"@, $Encoding);
    if (-not ($WindowXamlPath | Test-FileInfo -AsCreated)) {
        Write-Warning -Message "Failed to create $Name WPF window scaffold.";
        return;
    }

    [System.IO.File]::WriteAllText($WindowCsPath, @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsInfoCat.Desktop.ViewModels;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for $($Name)Window.xaml
    /// </summary>
    public partial class $($Name)Window : Window
    {
        public $($Name)ViewModel ViewModel { get { return DataContext as $($Name)ViewModel; } }
        
        public $($Name)Window()
        {
            InitializeComponent();
        }
    }
}
"@, $Encoding);
    if (-not ($WindowCsPath | Test-FileInfo -AsCreated)) {
        Write-Information -MessageData "Deleting $WindowXamlPath";
        Write-Warning -Message "Failed to create $Name WPF window scaffold.";
        return;
    }

    [System.IO.File]::WriteAllText($Path, @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsInfoCat.Desktop.Commands;

namespace FsInfoCat.Desktop.ViewModels
{
    /// <summary>
    /// View model for <see cref="$($Name)Window" />.xaml
    /// </summary>
    public class $($Name)ViewModel : DependencyObject
    {
        public $($Name)ViewModel()
        {
        }
    }
}
"@, $Encoding);
    if ($ViewModelPath | Test-FileInfo -AsCreated) {
        Write-Information -MessageData "$Name WPF window scaffold created.";
    } else {
        Write-Information -MessageData "Deleting $WindowXamlPath";
        Write-Information -MessageData "Deleting $WindowCsPath";
        Write-Warning -Message "Failed to create $Name WPF window scaffold.";
    }
}

Function New-MvcScaffold {
    Param(
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][A-Z\da-z]*([A-Z\d][A-Z\da-z]*)*$')]
        [string]$Name
    )

    $FilePath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat\Models\$Name.cs";
    [System.IO.File]::WriteAllText($FilePath, @"
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class $Name
    {
        private string _displayName = "";
        private string _notes = "";

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid $($Name)ID { get; set; }

        [MaxLength(256)]
        [Display(Name = "Display Name")]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public $($Name)() { }

        public $($Name)(string displayName, Guid createdBy)
        {
            $($Name)ID = Guid.NewGuid();
            DisplayName = displayName;
            CreatedOn = ModifiedOn = DateTime.Now;
            CreatedBy = ModifiedBy = createdBy;
        }

    }
}
"@, [System.Text.UTF8Encoding]::new($false, $false));
    $FilePath = (Get-Command -Name 'dotnet').Path;
    Push-Location;
    try {
        Set-Location -Path ($PSScriptRoot | Join-Path -ChildPath 'src\FsInfoCat.Web');
        . dotnet build
        . dotnet aspnet-codegenerator controller -name "$($Name)Controller" -m $Name -dc FsInfoDataContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
    } finally {
        Pop-Location;
    }
}

Function Read-CsTypeModel {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Prompt
    )
    
    if ($null -eq $Script:__Read_CsTypeModel_Choices) {
        $Script:__Read_CsTypeModel_Choices = [System.Collections.ObjectModel.Collection[CodeHelper.ChoiceValueDescription]]::new();
        ([string], [bool], [byte], [sbyte], [System.Int16], [System.UInt16], [int], [System.UInt32], [long], [System.UInt64], [float], [double], [decimal], [Guid], [Uri], [DateTime]) | ForEach-Object {
            $t = [CodeHelper.CsTypeModel]::new($_);
            $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($t, [System.Management.Automation.Host.ChoiceDescription]::new($t.ShortHand, $t.ToString())));
        }
        $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($false, [System.Management.Automation.Host.ChoiceDescription]::new("(other class)", "Other class type")));
        $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($true, [System.Management.Automation.Host.ChoiceDescription]::new("(other struct)", "Other struct type")));
    }

    $Result = $Script:__Read_CsTypeModel_Choices | Out-GridView -Title $Prompt -OutputMode Single;
    [CodeHelper.CsTypeModel]$CsTypeModel = $null;
    if ($null -ne $Result) {
        if ($Result -is [CodeHelper.CsTypeModel]) {
            $CsTypeModel = $Result;
        } else {
            $IsValueType = $Result;
            $Name = Read-CsClassName -Prompt 'Class name';
            if ([string]::IsNullOrWhiteSpace($Name)) { return }
            $CsTypeModel = [CodeHelper.CsTypeModel]::new("", $Name, $IsValueType, $null);
        }
        if (Read-YesOrNo -Caption 'Collection', 'Make this a collection?') {
            $mc = [System.Collections.ObjectModel.Collection[CodeHelper.CsTypeModel]]::new();
            $mc.Add($CsTypeModel);
            $CsTypeModel = [CodeHelper.CsTypeModel]::new('System.Collections.ObjectModel', 'Collection', $false, $mc);
        }
        if ($CsTypeModel.IsValueType -and (Read-YesOrNo -Caption 'Nullability', 'Make this nullable?')) {
            $CsTypeModel = $CsTypeModel.AsNullable();
        }
    }
}

Function New-DependencyProperty {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][a-zA-Z\d]*$')]
        [string]$Name,
        [Parameter(Mandatory = $true)]
        [CodeHelper.CsTypeModel]$PropertyType,
        [Parameter(Mandatory = $true)]
        [string]$ModelType,
        [switch]$ReadOnly,
        [switch]$HandlePropertyChanged,
        [switch]$HandleCoerceValue,
        [switch]$ExposeChangedEvent
    )

    $sb = [System.Text.StringBuilder]::new();
    $sb.AppendLine().Append('        #region ').Append($Name) | Out-Null;
    [xml]$Xml = '<summary />';
    $Xml.PreserveWhitespace = $true;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nDefines the name for the ")) | Out-Null;
    $Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $Name;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;
    $sb.AppendLine(' Property Members').AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public const string PropertyName_').Append($Name).Append(' = "').Append($Name).AppendLine('";').AppendLine() | Out-Null;
    [xml]$Xml = '<summary />';
    $Xml.PreserveWhitespace = $true;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nIdentifies the ")) | Out-Null;
    $Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $Name;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;

    if ($ReadOnly.IsPresent) {
        $sb.Append('        private static readonly DependencyPropertyKey ').Append($Name).Append('PropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_').Append($Name) | Out-Null;
    } else {
        $sb.AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($Name).Append('Property = DependencyProperty.Register(PropertyName_').Append($Name) | Out-Null;
    }
    $sb.Append(', typeof(').Append($PropertyType.ShortHand).Append('), typeof(').Append($ModelType).AppendLine('),').Append('                new PropertyMetadata(').Append($PropertyType.DefaultValue) | Out-Null;
    if ($HandlePropertyChanged.IsPresent -or $HandleCoerceValue.IsPresent) {
        if ($HandlePropertyChanged.IsPresent) {
            $sb.AppendLine(',').Append('                    (d, e) => (d as ').Append($ModelType).Append(').On').Append($Name).Append('PropertyChanged(e.OldValue as ').Append($PropertyType.AsNullable().ShortHand) | Out-Null;
            if ($PropertyType.IsValueType) {
                $sb.Append(', (').Append($PropertyType.ShortHand).Append(')(e.NewValue)') | Out-Null;
            } else {
                $sb.Append(', e.NewValue as ').Append($PropertyType.ShortHand) | Out-Null;
            }
            $sb.Append(')') | Out-Null;
        } else {
            if ($HandleCoerceValue.IsPresent) { $sb.Append(', null') | Out-Null; }
        }
        if ($HandleCoerceValue.IsPresent) {
            $sb.AppendLine(',').Append('                    (d, baseValue) => Coerce').Append($Name).Append('Value(baseValue as ').Append($PropertyType.ShortHand).Append(')') | Out-Null;
        }
        $sb.AppendLine().AppendLine('            )').AppendLine('        );') | Out-Null;
        if ($ReadOnly.IsPresent) {
            $sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($Name).Append('Property = ').Append($Name).AppendLine('PropertyKey.DependencyProperty;') | Out-Null;
        }
        if ($HandlePropertyChanged.IsPresent) {
            $sb.AppendLine().Append('        private void On').Append($Name).Append('PropertyChanged(').Append($PropertyType.ShortHand) | Out-Null;
            $sb.Append(' oldValue, ').Append($PropertyType.ShortHand).AppendLine(' newValue)').AppendLine('        {') | Out-Null;
            if ($IsObservableCollection) {
                $sb.AppendLine('            if (null != oldValue)').Append('                oldValue.CollectionChanged -= On').Append($Name).AppendLine('CollectionChanged;') | Out-Null;
                $sb.Append('            newValue.CollectionChanged += On').Append($Name).AppendLine('CollectionChanged;') | Out-Null;
                $sb.Append('            On').Append($Name).AppendLine('CollectionChanged(newValue, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));') | Out-Null;
                $sb.AppendLine('        }').AppendLine().Append('        private void On').Append($Name).AppendLine('CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)').AppendLine('        {') | Out-Null;
                $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('CollectionChanged') | Out-Null;
            } else {
                $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('PropertyChanged') | Out-Null;
            }
            $sb.AppendLine('            throw new NotImplementedException();').AppendLine('        }') | Out-Null;
        }
        if ($HandleCoerceValue.IsPresent) {
            $sb.AppendLine().Append('        public static ').Append($PropertyType.ShortHand).Append(' Coerce').Append($Name).Append('Value(').Append($PropertyType.ShortHand) | Out-Null;
            $sb.AppendLine(' value)').AppendLine('        {') | Out-Null;
            if ($PropertyType.DefaultValue -eq 'null') {
                if ($IsObservableCollection) {
                    $sb.AppendLine('            if (null == value)').Append('                return new ').Append($PropertyType.ShortHand).AppendLine('();').AppendLine('            return value;') | Out-Null;
                } else {
                    $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('PropertyChanged').AppendLine('            throw new NotImplementedException();') | Out-Null;
                }
            } else {
                if ($PropertyType.IsValueType) {
                    $sb.AppendLine('            if (value.HasValue)').AppendLine('                return value.Value;').Append('            return ').Append($PropertyType.DefaultValue).AppendLine(';') | Out-Null;
                } else {
                    $sb.AppendLine('            if (null == value)').Append('                return ').Append($PropertyType.DefaultValue).AppendLine(';').AppendLine('            return value;') | Out-Null;
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
    $sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public ').Append($PropertyType.ShortHand).Append(' ').AppendLine($Name).AppendLine('        {') | Out-Null;
    $sb.AppendLine('            get').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    return (').Append($PropertyType.ShortHand).Append(')(GetValue(').Append($Name).AppendLine('Property));') | Out-Null;
    $sb.Append('                return Dispatcher.Invoke(() => (').Append($PropertyType.ShortHand).Append(')(GetValue(').Append($Name).AppendLine('Property)));').AppendLine('            }').Append('            ') | Out-Null;
    if ($ReadOnly.IsPresent) { $sb.Append('private ') | Out-Null }
    $sb.AppendLine('set').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    SetValue(').Append($Name).Append('Property') | Out-Null;
    if ($ReadOnly.IsPresent) { $sb.Append('Key') | Out-Null }
    $sb.AppendLine(', value);').AppendLine('                else').Append('                    Dispatcher.Invoke(() => SetValue(').Append($Name).Append('Property') | Out-Null;
    if ($ReadOnly.IsPresent) { $sb.Append('Key') | Out-Null }
    $sb.AppendLine(', value));').AppendLine('            }').AppendLine('        }').AppendLine().AppendLine('        #endregion') | Out-Null;
    $sb.ToString();
}

Function Read-DependencyProperty {
    [CmdletBinding()]
    Param()
    $Name = Read-CsClassName -Prompt 'Property name';
    if ([string]::IsNullOrWhiteSpace($Name)) { return }
    $CsTypeModel = Read-CsTypeModel -Prompt 'Property type';
    if ($null -eq $CsTypeModel) { return }
    $ModelType = Read-Host -Prompt 'View model type';
    if ([string]::IsNullOrWhiteSpace($ModelType)) { return }
    $ReadOnly = Read-YesOrNo -Caption 'Read-Only' -Message 'Is this a read-only property?';
    $HandlePropertyChanged = $ExposeChangedEvent = Read-YesOrNo -Caption 'Property Change' -Message 'Expose property changed event?';
    if (-not $HandlePropertyChanged) {
        $HandlePropertyChanged = Read-YesOrNo -Caption 'Property Change' -Message 'Handle property changed event?';
    }
    if ($CsTypeModel.IsValueType -or (Read-YesOrNo -Caption 'Coerce value' -Message 'Handle coerce value?')) {
        if ($ReadOnly) {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -ExposeChangedEvent -HandlePropertyChanged -HandleCoerceValue;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -HandlePropertyChanged -HandleCoerceValue;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -HandleCoerceValue;
                }
            }
        } else {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ExposeChangedEvent -HandlePropertyChanged -HandleCoerceValue;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -HandlePropertyChanged -HandleCoerceValue;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -HandleCoerceValue;
                }
            }
        }
    } else {
        if ($ReadOnly) {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -ExposeChangedEvent -HandlePropertyChanged;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -HandlePropertyChanged;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly;
                }
            }
        } else {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ExposeChangedEvent -HandlePropertyChanged;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -HandlePropertyChanged;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType;
                }
            }
        }
    }
}

$Text = Read-DependencyProperty;
$Text;
if (-not [string]::IsNullOrWhiteSpace($Text)) { [System.Windows.Clipboard]::SetText($Text) }