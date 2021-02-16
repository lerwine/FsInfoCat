Param(
    [string[]]$CName = @('maml:navigationLinkType'),

    # XML Schema files or folders containing Xml Schema files (*.xsd).
    [string[]]$Schemas = @(
        'C:\Users\lerwi\Git\FsInfoCat\dev\src\DevHelper',
        'C:\Users\lerwi\Git\FsInfoCat\Resources\Build',
        'C:\Users\lerwi\Git\FsInfoCat\Resources\PSMaml'
    ),

    [Hashtable]$PrefixMap = @{
        exeBlock = 'http://schemas.microsoft.com/automatedTaskExecutionBlock/2004/10'
        maml = 'http://schemas.microsoft.com/maml/2004/10'
        legacy = 'http://schemas.microsoft.com/maml/2004/4';
        dev = 'http://schemas.microsoft.com/maml/dev/2004/10';
        command = 'http://schemas.microsoft.com/maml/dev/command/2004/10';
        dscResource = 'http://schemas.microsoft.com/maml/dev/dscResource/2004/10';
        managed = 'http://schemas.microsoft.com/maml/dev/managed/2004/10';
        xaml = 'http://schemas.microsoft.com/maml/dev/managed/xaml/2004/10';
        doc = 'http://schemas.microsoft.com/maml/internal';
        provider = 'http://schemas.microsoft.com/powershell/provider/2008/09';
        xsi = 'http://www.w3.org/2001/XMLSchema-instance';
        int = 'urn:Erwine:Leonard:T:PsHelpIntermediate.xsd';
        xsl = 'http://www.w3.org/1999/XSL/Transform';
        msdata = 'urn:schemas-microsoft-com:xml-msdata';
        msxsl = 'urn:schemas-microsoft-com:xslt';
        msb = 'http://schemas.microsoft.com/developer/msbuild/2003';
        xs = 'http://www.w3.org/2001/XMLSchema';
        wpf="http://schemas.microsoft.com/winfx/2006/xaml/presentation";
        mc="http://schemas.openxmlformats.org/markup-compatibility/2006";
        x="http://schemas.microsoft.com/winfx/2006/xaml";
        d="http://schemas.microsoft.com/expression/blend/2008";
        vs='http://schemas.microsoft.com/Visual-Studio-Intellisense';
        wsdl='http://schemas.xmlsoap.org/wsdl/';
        'soap-envelope'='http://www.w3.org/2003/05/soap-envelope';
        'soap-encoding'='http://www.w3.org/2001/06/soap-encoding';
        xhtml='http://www.w3.org/1999/xhtml';
        type='http://www.w3.org/2001/03/XMLSchema/TypeLibrary';
        synth='http://www.w3.org/2001/10/synthesis';
        MSHelp = 'http://msdn.microsoft.com/mshelp';
        fs='http://schemas.microsoft.com/appv/2010/FilesystemMetadata';
        appv='http://schemas.microsoft.com/appv/2010/manifest';
        'appv1.1'='http://schemas.microsoft.com/appv/2013/manifest';
        'appv1.2'='http://schemas.microsoft.com/appv/2014/manifest';
        ar='http://schemas.microsoft.com/VisualStudio/CodeAnalysis/AnalysisResults/General';
    }
)

Function Get-XmlNamespace {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        # XML Schema files or folders containing Xml Schema files (*.xsd).
        [string[]]$Schemas,

        [switch]$Raw
    )

    Begin { $AllItems = @(); }
    Process {
        foreach ($Path in $Schemas) {
            if ($Path | Test-Path -PathType Container) {
                if ($Raw.IsPresent) {
                    ((Get-ChildItem -Path $Path -Filter '*.xsd' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw;
                    ((Get-ChildItem -Path $Path -Filter '*.xml' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw;
                    ((Get-ChildItem -Path $Path -Filter '*.xslt' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw;
                    ((Get-ChildItem -Path $Path -Filter '*.config' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw;
                } else {
                    $AllItems += @(((Get-ChildItem -Path $Path -Filter '*.xml' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw);
                    $AllItems += @(((Get-ChildItem -Path $Path -Filter '*.xsd' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw);
                    $AllItems += @(((Get-ChildItem -Path $Path -Filter '*.xslt' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw);
                    $AllItems += @(((Get-ChildItem -Path $Path -Filter '*.config' -File) | Select-Object -ExpandProperty 'FullName') | Get-XmlNamespace -Raw);
                }
            } else {
                if ($Path | Test-Path -PathType Leaf) {
                    $XmlDocument = [System.Xml.XmlDocument]::new();
                    $XmlDocument.Load($Path);
                    if ($Raw.IsPresent) {
                        @($XmlDocument.SelectNodes('//namespace::*[not(. = ../../namespace::*)]')) | ForEach-Object {
                            [PSCustomObject]@{
                                Prefix = $_.LocalName;
                                Namespace = $_.Value;
                                Path = $Path
                            };
                        }
                    } else {
                        $AllItems += @(@($XmlDocument.SelectNodes('//namespace::*[not(. = ../../namespace::*)]')) | ForEach-Object {
                            [PSCustomObject]@{
                                Prefix = $_.LocalName;
                                Namespace = $_.Value;
                                Path = $Path
                            };
                        });
                    }
                } else {
                    Write-Warning -Message "Path '$Path' not found.";
                }
            }
        }
    }

    End {
        if (-not $Raw) {
            ($AllItems | Group-Object -Property 'Namespace') | ForEach-Object {
                $ns = $_.Name;
                if (@($_.Group | Where-Object { -not [string]::IsNullOrEmpty($_.Prefix) }).Count -eq 0) {
                    $_.Group | ForEach-Object {
                        Write-Warning -Message "Namespace `"$ns`" has no prefix in `"$($_.Path)`"";
                    }
                } else {
                    ($_.Group | Group-Object -Property 'Prefix') | ForEach-Object {
                        [PSCustomObject]@{
                            Namespace = $ns;
                            Prefix = $_.Name;
                            Path = ($_.Group | Select-Object -ExpandProperty 'Path') -join "`n";
                        }
                    }
                }
            }
        }
    }
}

class SchemaSource {
    [System.Xml.XmlDocument]$Document;
    [string]$BaseName;
    [string]$FullName;
    [string]$TargetNamespace;
    [System.Xml.XmlNamespaceManager]$Nsmgr;
    [System.Collections.Generic.Dictionary[string, string]]$Map = [System.Collections.Generic.Dictionary[string, string]]::new();
    SchemaSource([System.Xml.XmlDocument]$Document, [string]$Path) {
        $this.Document = $Document;
        $this.FullName = $Path;
        $this.BaseName = [System.IO.Path]::GetFileNameWithoutExtension(($Path | Split-Path -Leaf));
        $this.TargetNamespace = '' + $Document.DocumentElement.TargetNamespace;
        $this.Nsmgr = [System.Xml.XmlNamespaceManager]::new($Document.NameTable);
        $this.Nsmgr.AddNamespace('xs', 'http://www.w3.org/2001/XMLSchema');
        $this.Map = [System.Collections.Generic.Dictionary[string, string]]::new();
        @($Document.SelectNodes('//namespace::*[not(. = ../../namespace::*)]')) | ForEach-Object {
            if (-not [string]::IsNullOrEmpty($_.LocalName) -and -not $this.Map.ContainsKey($_.LocalName)) {
                $this.Map.Add($_.LocalName, $_.Value);
            }
        }
    }

    [string] ToString() {
        return "$($this.TargetNamespace);$($this.FullName)";
    }
}

Add-Type -TypeDefinition @'
public class QualifiedXmlName : System.Xml.XmlQualifiedName, System.IEquatable<QualifiedXmlName>
{
    private const string XML_SCHEMA_XSD = "http://www.w3.org/2001/XMLSchema";
    private readonly string _prefix;
    private readonly string _localName;
    private readonly bool _isValid;
    private readonly bool _isAnyNamespace;
    private readonly bool _isAnyPrefix;
    public string Prefix { get { return _prefix; } }
    public string LocalName { get { return _localName; } }
    public bool IsValid { get { return _isValid; } }
    public bool IsAnyNamespace { get { return _isAnyNamespace; } }
    public bool IsAnyPrefix { get { return _isAnyPrefix; } }
    public QualifiedXmlName(string prefix, string localName, string ns)
        : base((null == prefix) ? ((null == localName) ? "" : localName) : prefix + ((string.IsNullOrEmpty(localName)) ? ":" : ":" + localName), ns)
    {
        _isAnyPrefix = null == prefix;
        _isAnyNamespace = null == ns;
        if (IsEmpty)
        {
            _localName = "";
            _prefix = "";
            _isValid = false;
        }
        else
        {
            bool isValid;
            _localName = AsValidNCName(localName, out isValid);
            if (!(string.IsNullOrEmpty(ns) || System.Uri.IsWellFormedUriString(ns, System.UriKind.Absolute)))
                isValid = false;

            if (string.IsNullOrEmpty(prefix))
            {
                _prefix = "";
                _isValid = isValid && ((_isAnyPrefix) ? _localName : ":" + _localName).Equals(Name);
            }
            else if (isValid)
            {
                _prefix = AsValidNCName(prefix, out isValid);
                _isValid = isValid && (_prefix + ":" + _localName).Equals(Name);
            }
            else
            {
                _prefix = AsValidNCName(prefix, out isValid);
                _isValid = false;
            }
        }
    }
    public bool IsEqualTo(System.Xml.XmlNode node)
    {
        return null != node && !IsEmpty && node.LocalName.Equals(_localName) && ((_isAnyNamespace) ? (_isAnyPrefix || _prefix.Equals(node.Prefix)) :
            Namespace.Equals(node.NamespaceURI));
    }
    public bool IsSameNamespace(System.Xml.XmlNode node, bool allowNsLookup)
    {
        if (null == node || IsEmpty)
            return false;
        if (_isAnyNamespace)
        {
            if (!allowNsLookup)
                return false;
            if (_isAnyPrefix)
                return true;
            if (node is System.Xml.XmlDocument)
            {
                System.Xml.XmlElement element = ((System.Xml.XmlDocument)node).DocumentElement;
                if (element.NamespaceURI.Equals(XML_SCHEMA_XSD))
                {
                    string p = element.GetPrefixOfNamespace(XML_SCHEMA_XSD);
                    if (_prefix.Equals((string.IsNullOrEmpty(p)) ? "xs" : p))
                        return true;
                }
                node = element;
            }
            string ns = node.GetNamespaceOfPrefix(_prefix);
            if (string.IsNullOrEmpty(ns))
                return node.NamespaceURI.Length == 0 && node.GetPrefixOfNamespace(ns).Equals(_prefix);
            return ns.Equals(node.NamespaceURI);
        }
        if (node is System.Xml.XmlDocument)
        {
            System.Xml.XmlElement element = ((System.Xml.XmlDocument)node).DocumentElement;
            if (element.NamespaceURI.Equals(XML_SCHEMA_XSD))
            {
                string a = element.GetAttribute("targetNamespace");
                return (string.IsNullOrEmpty(a)) ? Namespace.Length == 0 : Namespace.Equals(a);
            }
            node = element;
        }
        return node.LocalName.Equals(_localName) && ((_isAnyNamespace) ? (_isAnyPrefix || _prefix.Equals(node.Prefix)) :
            Namespace.Equals(node.NamespaceURI));
    }
    public bool IsSameNamespace(System.Xml.XmlNode node) { return IsSameNamespace(node, false); }
    public bool Equals(QualifiedXmlName other)
    {
        return null != other && (ReferenceEquals(this, other) || _isValid == other._isValid && _isAnyPrefix == other._isAnyPrefix &&
            _isAnyNamespace == other._isAnyNamespace && Namespace.Equals(other.Namespace) && Name.Equals(other.Name));
    }
    public override bool Equals(object other) { return Equals(other as QualifiedXmlName); }
    public override int GetHashCode() { return ToString().GetHashCode(); }
    public override string ToString()
    {
        if (_isAnyNamespace)
            return (_isAnyPrefix) ? _localName : _prefix + ":" + _localName;
        return ((_isAnyPrefix) ? _localName : _prefix + ":" + _localName) + ";" + Namespace;
    }
    public static string AsValidNCName(string value, out bool isValid)
    {
        if (string.IsNullOrEmpty(value))
        {
            isValid = false;
            return "";
        }
        isValid = System.Xml.XmlConvert.IsStartNCNameChar(value[0]);
        for (int i = 1; isValid && i < value.Length; i++)
            isValid = System.Xml.XmlConvert.IsNCNameChar(value[i]);
        if (isValid)
            return value;
        return System.Xml.XmlConvert.EncodeLocalName(value);
    }
    public static QualifiedXmlName Create(string name)
    {
        return Create(name, null);
    }
    public static QualifiedXmlName Create(string name, System.Collections.IDictionary nsLookupMap)
    {
        if (null == name)
            return null;
        int i = name.IndexOf(';');
        string ns;
        if (i < 0)
            ns = null;
        else
        {
            ns = name.Substring(i + 1);
            name = name.Substring(0, i);
        }

        i = name.IndexOf(':');
        if (i > -1)
        {
            string prefix = name.Substring(0, i);
            if (null == ns && null != nsLookupMap && nsLookupMap.Contains(prefix))
            {
                object obj = nsLookupMap[prefix];
                ns = (null == obj || obj is string) ? (string)obj : obj.ToString();
            }
            return new QualifiedXmlName(prefix, name.Substring(i + 1), ns);
        }
        return new QualifiedXmlName(null, name, ns);
    }
}
'@ -ReferencedAssemblies 'System.Xml' -ErrorAction Stop;

Function Test-XmlName {
    [CmdletBinding(DefaultParameterSetName = 'Any')]
    Param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Local')]
        [switch]$Local,

        [switch]$Encode,

        [Parameter(ParameterSetName = 'Any')]
        [switch]$AsQName
    )

    Process {
        if ([string]::IsNullOrEmpty($Name)) {
            if (-not ($Encode.IsPresent -or $AsQName.IsPresent)) {
                $false | Write-Output;
            }
        } else {
            if ($Local.IsPresent) {
                $Success = [System.Xml.XmlConvert]::IsStartNCNameChar($Name[0]);
                for ($i = 1; $i -lt $Name.Length -and $Success; $i++) {
                    $Success = [System.Xml.XmlConvert]::IsNCNameChar($Name[$i]);
                }
                if ($Encode.IsPresent) {
                    if ($Success) {
                        $Name | Write-Output;
                    } else {
                        [System.Xml.XmlConvert]::EncodeLocalName($Name) | Write-Output;
                    }
                } else {
                    $Success | Write-Output;
                }
            } else {
                if ($Encode.IsPresent) {
                    $LocalName = $null;
                    ($Prefix, $LocalName) = $Name.Split(':', 2);
                    if ($AsQName.IsPresent) {
                        if ($null -eq $LocalName) {
                            if ($Prefix.Length -eq 0) {
                                [PSCustomObject]@{
                                    Prefix = $null;
                                    LocalName = Test-XmlName -Name $Name -Local -Encode;
                                } | Write-Output;
                            } else {
                                [PSCustomObject]@{
                                    Prefix = $null;
                                    LocalName = Test-XmlName -Name $Prefix -Local -Encode;
                                } | Write-Output;
                            }
                        } else {
                            if ($LocalName.Length -eq 0) {
                                [PSCustomObject]@{
                                    Prefix = $null;
                                    LocalName = Test-XmlName -Name $Name -Local -Encode;
                                } | Write-Output;
                            } else {
                                if ($Prefix.Length -eq 0) {
                                    [PSCustomObject]@{
                                        Prefix = '';
                                        LocalName = Test-XmlName -Name $LocalName -Local -Encode;
                                    } | Write-Output;
                                } else {
                                    [PSCustomObject]@{
                                        Prefix = Test-XmlName -Name $Prefix -Local -Encode;
                                        LocalName = Test-XmlName -Name $LocalName -Local -Encode;
                                    } | Write-Output;
                                }
                            }
                        }
                    } else {
                        if ($null -eq $LocalName) {
                            if ($Prefix.Length -eq 0) {
                                Test-XmlName -Name $Name -Local -Encode;
                            } else {
                                Test-XmlName -Name $Prefix -Local -Encode;
                            }
                        } else {
                            if ($LocalName.Length -eq 0) {
                                Test-XmlName -Name $Name -Local -Encode;
                            } else {
                                if ($Prefix.Length -eq 0) {
                                    ":$(Test-XmlName -Name $LocalName -Local -Encode)";
                                } else {
                                    "$(Test-XmlName -Name $Prefix -Local -Encode):$(Test-XmlName -Name $LocalName -Local -Encode)";
                                }
                            }
                        }
                    }
                } else {
                    $Segments = $Name.Split(':');
                    switch ($Segments.Length) {
                        1 {
                            if ($AsQName.IsPresent) {
                                if (Test-XmlName -Name $Name -Local) {
                                    [PSCustomObject]@{
                                        Prefix = $null;
                                        LocalName = Test-XmlName -Name $Name -Local -Encode;
                                    } | Write-Output;
                                }
                            } else {
                                Test-XmlName -Name $Name -Local;
                            }
                            break;
                        }
                        2 {
                            if (($segments[1] | Test-XmlName -Local) -and ($segments[0].Length -eq 0 -or ($Segments[0] | Test-XmlName -Local))) {
                                if ($AsQName.IsPresent) {
                                    [PSCustomObject]@{
                                        Prefix = Test-XmlName -Name $segments[0] -Local -Encode;
                                        LocalName = Test-XmlName -Name $segments[1] -Local -Encode;
                                    } | Write-Output;
                                } else {
                                    $true | Write-Output;
                                }
                            } else {
                                if (-not $AsQName.IsPresent) {
                                    $false | Write-Output;
                                }
                            }
                            break;
                        }
                        default {
                            if (-not $AsQName.IsPresent) {
                                $false | Write-Output;
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}

Function Get-SchemaNodeByCName {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$CName,

        [Parameter(Mandatory = $true)]
        [SchemaSource[]]$Schemas
    )

    Process {
        [QualifiedXmlName]$QName = [QualifiedXmlName]::Create($CName, $Script:PrefixMap);
        foreach ($Source in $Schemas) {
            $qn = $QName;
            if ($qn.IsAnyNamespace -and -not $qn.IsAnyPrefix) {
                $qn = [QualifiedXmlName]::Create($CName, $Source:Map);
            }
            $XmlElement = $null;
            if ($qn.IsAnyNamespace) {
                $XmlElement = $Source.Document.DocumentElement.SelectSingleNode("xs:*[@name='$($qn.LocalName)']", $Source.Nsmgr);
                if ($null -ne $XmlElement -and -not $qn.IsEqualTo($XmlElement)) { $XmlElement = $null }
            } else {
                if ($qn.Namespace -ceq $Source.TargetNamespace) {
                    $XmlElement = $Source.Document.DocumentElement.SelectSingleNode("xs:*[@name='$($qn.LocalName)']", $Source.Nsmgr);
                }
            }
            if ($null -ne $XmlElement) {
                $Prefix = $qn.Prefix;
                if ($qn.IsAnyPrefix) { $Prefix = $XmlElement.GetPrefixOfNamespace($qn.Namespace) }
                $ElementName = $XmlElement.LocalName;
                $XmlReader = [System.Xml.XmlReader]::Create($Source.FullName);
                try {
                    while ($XmlReader.Read()) {
                        #([System.Xml.XmlReader]$XmlReader).Depth
                        if ($XmlReader.Depth -eq 1) {
                            if ($XmlReader.NodeType -eq [System.Xml.XmlNodeType]::Element -and $XmlReader.NamespaceURI -ceq 'http://www.w3.org/2001/XMLSchema' -and `
                                    $XmlReader.LocalName -ceq $ElementName -and $XmlReader.AttributeCount -gt 0 -and $XmlReader.GetAttribute('name') -ceq $qn.LocalName) {
                                [PSCustomObject]@{
                                    BaseName = $Source.BaseName;
                                    FullName = $Source.FullName;
                                    LineNumber = $XmlReader.LineNumber;
                                    LinePosition = $XmlReader.LinePosition;
                                    Prefix = $Prefix;
                                    Type = $ElementName;
                                    LocalName = $qn.LocalName;
                                    OuterXml = $XmlReader.ReadOuterXml();
                                } | Write-Output;
                                break;
                            }
                        } else {
                            if ($XmlReader.Depth -gt 1) {
                                ([System.Xml.XmlReader]$XmlReader).ReadInnerXml() | Out-Null;
                            }
                        }
                    }
                } finally {
                    $XmlReader.Close();
                }
            }
        }
    }
}

Function Get-SchemaNodeByReferenceUrl {
    [CmdletBinding()]
    Param (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Url,

        [Parameter(Mandatory = $true)]
        [SchemaSource[]]$Schemas
    )

    Process {
        [System.Uri]$Uri = $null;
        if ([System.Uri]::TryCreate($Url, [System.UriKind]::Absolute, [ref]$Uri) -and $Uri.Scheme -eq 'urn' -and $Uri.AbsolutePath.StartsWith('xsd:')) {
            $segments = $Uri.AbsolutePath.Substring(4).Split(':', 3);
            if ($segments.Length -eq 4 -and $segments[2].Length -gt 5 -and $segments[2].StartsWith('doc(') -and $segments[2].ToLower().EndsWith(')')) {
                $BaseName = $segments[1].Substring(4, $segments[1].Length - 5);
                $Schemas | Where-Object { $_.BaseName -ceq $BaseName } | ForEach-Object {
                    $segments[1]
                }
            }
        }
        ($Prefix, $LocalName) = $CName.Split(':', 2);
        if ([string]::IsNullOrEmpty($LocalName)) {
            $Prefix = $null;
            $LocalName = $CName;
        }
        if ($LocalName.Length -eq 0) {
            $Prefix = $null;
            $LocalName = [System.Xml.XmlConvert]::EncodeLocalName($CName);
        } else {
            $IsValid = [System.Xml.XmlConvert]::IsStartNCNameChar($LocalName[0]);
            for ($i = 1; $IsValid -and $i -lt $LocalName.Length; $i++) {
                $IsValid = [System.Xml.XmlConvert]::IsNCNameChar($LocalName[$i]);
            }
            if (-not $IsValid) { $LocalName = [System.Xml.XmlConvert]::EncodeLocalName($LocalName) }
        }
        if (-not [string]::IsNullOrEmpty($Prefix)) {
            $IsValid = [System.Xml.XmlConvert]::IsStartNCNameChar($Prefix[0]);
            for ($i = 1; $IsValid -and $i -lt $Prefix.Length; $i++) {
                $IsValid = [System.Xml.XmlConvert]::IsNCNameChar($Prefix[$i]);
            }
            if (-not $IsValid) { $Prefix = [System.Xml.XmlConvert]::EncodeLocalName($Prefix) }
        }
        foreach ($Source in $Schemas) {
            $XmlElement = $null;
            if ($null -ne $Prefix) {
                if ($Prefix.Length -eq 0) {
                    if ($Source.TargetNamespace.Length -gt 0) { return $null }
                } else {
                    $ns = $null;
                    if ($Source.Map.ContainsKey($Prefix)) {
                        $ns = $Source.Map[$Prefix];
                    } else {
                        if ($Script:PrefixMap.ContainsKey($Prefix)) {
                            $ns = $Script:PrefixMap[$Prefix];
                        } else {
                            return $null;
                        }
                    }
                    if ($ns -cne $Source.TargetNamespace) { return $null; }
                }
            }
            $XmlElement = $Source.Document.DocumentElement.SelectSingleNode("xs:*[@name='$LocalName']", $Source.Nsmgr);
            if ($null -eq $XmlElement) { return $null }
            $ElementName = $XmlElement.LocalName;
            $XmlReader = [System.Xml.XmlReader]::Create($Source.Path);
            try {
                while ($XmlReader.Read()) {
                    if ($XmlReader.NodeType -eq [System.Xml.XmlNodeType]::Element -and $XmlReader.NamespaceURI -ceq 'http://www.w3.org/2001/XMLSchema' -and `
                            $XmlReader.LocalName -ceq $ElementName -and $XmlReader.AttributeCount -gt 0 -and $XmlReader.GetAttribute('name') -ceq $LocalName) {
                        [PSCustomObject]@{
                            Path = $Source.Path;
                            LineNumber = $XmlReader.LineNumber;
                            LinePosition = $XmlReader.LinePosition;
                            Prefix = $Prefix;
                            Type = $ElementName;
                            Name = $LocalName;
                            OuterXml = $XmlReader.ReadOuterXml();
                        } | Write-Output;
                        break;
                    }
                }
            } finally {
                $XmlReader.Close();
            }
        }
    }
}

$SchemaSources = @($Schemas | ForEach-Object {
    if ($_ | Test-Path -PathType Leaf) {
        $XmlDocument = [System.Xml.XmlDocument]::new();
        $XmlDocument.Load($_);
        if ($null -ne $XmlDocument.DocumentElement) { Write-Output -InputObject ([SchemaSource]::new($XmlDocument, $_)) -NoEnumerate }
    } else {
        if ($_ | Test-Path -PathType Container) {
            ((Get-ChildItem -Path $_ -Filter '*.xsd') | Select-Object -ExpandProperty 'FullName') | ForEach-Object {
                $XmlDocument = [System.Xml.XmlDocument]::new();
                $XmlDocument.Load($_);
                if ($null -ne $XmlDocument.DocumentElement) { Write-Output -InputObject ([SchemaSource]::new($XmlDocument, $_)) -NoEnumerate }
            }
        } else {
            Write-Warning -Message "Path '$_' not found.";
        }
    }
});

if ($SchemaSources.Count -eq 0) {
    Write-Warning -Message 'No schemas loaded';
    return;
}
#('C:\Windows\Microsoft.NET\Framework64\v4.0.30319\ASP.NETWebAdminFiles\App_Data' | Get-XmlNamespace) | Out-GridView;
# $CName | Get-SchemaNodeByCName -Schemas $SchemaSources
$ResultCode = ((($CName | Get-SchemaNodeByCName -Schemas $SchemaSources) | ForEach-Object {
    "<!--";
    "    urn:xsd:$($_.Prefix):doc($($_.BaseName)):/xs:schema/xs:$($_.Type)[@name='$($_.LocalName)']#L$($_.LineNumber)";
    ($_.OuterXml -split '\r\n?|\n') | ForEach-Object { "    $_".TrimEnd() }
    "-->"
}) | Out-String).TrimEnd();
if ($ResultCode.Length -gt 0) {
    $ResultCode | Write-Output;
    [System.Windows.Clipboard]::SetText($ResultCode);
} else {
    Write-Warning -Message 'No matches found';
}
# $CName | ForEach-Object {
#     [QualifiedXmlName]$QName = [QualifiedXmlName]::Create($_, $Script:PrefixMap);
#     $SchemaSources | ForEach-Object {
#         $Result = $_.Get($Prefix, $LocalName);
#         if ($null -ne $Result) { $Result | Write-Output }
#     }
# } | ForEach-Object {""
#     "# Line $($_.LineNumber), Position $($_.LinePosition)";
#     "doc($($_.Path | Split-Path -Leaf)/xs:schema/xs:$($_.Type)[@name='$($_.Name)']#$($_.Prefix)";
#     "urn:xsd:$($_.Prefix):doc($($_.Path | Split-Path -Leaf)):/xs:schema/xs:$($_.Type)[@name='$($_.Name)']#";
#     $_.OuterXml;
# }

# urn:xsd:maml:doc(inlineCommon.xsd):/xs:schema/xs:element[@name='acronym']
