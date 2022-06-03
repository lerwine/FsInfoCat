enum ConversionType {
    String;
    Boolean;
    Value;
    HexBinary;
    Base64Binary;
}

Function Get-FieldMappings {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Type')]
        [System.Xml.Schema.XmlSchemaComplexType]$Type,

        [Parameter(Mandatory = $true, ParameterSetName = 'Group')]
        [System.Xml.Schema.XmlSchemaAttributeGroup]$Group,

        [Parameter(Mandatory = $true)]
        [hashtable]$Hashtable
    )

    $AttributeCollection = $null;
    if ($PSBoundParameters.ContainsKey('Group')) {
        $AttributeCollection = $Group.Attributes;
    } else {
        $AttributeCollection = $Type.Attributes;
    }
    foreach ($a in @($AttributeCollection)) {
        if ($a -is [System.Xml.Schema.XmlSchemaAttributeGroupRef]) {
            Get-FieldMappings -Group $Type.Parent.AttributeGroups.Item($a.RefName) -Hashtable $Hashtable;
        } else {
            if (-not $Hashtable.ContainsKey($a.Name)) {
                switch ($a.AttributeType.TypeCode) {
                    Boolean {
                        $Hashtable[$a.Name] = [ConversionType]::Boolean;
                        break;
                    }
                    Decimal {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Float {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Double {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Integer {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    NonPositiveInteger {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    NegativeInteger {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Long {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Int {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Short {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    Byte {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    NonNegativeInteger {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    UnsignedLong {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    UnsignedInt {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    UnsignedShort {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    UnsignedByte {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    PositiveInteger {
                        $Hashtable[$a.Name] = [ConversionType]::Value;
                        break;
                    }
                    HexBinary {
                        $Hashtable[$a.Name] = [ConversionType]::HexBinary;
                        break;
                    }
                    Base64Binary {
                        $Hashtable[$a.Name] = [ConversionType]::Base64Binary;
                        break;
                    }
                    default {
                        $Hashtable[$a.Name] = [ConversionType]::String;
                        break;
                    }
                }
            }
        }
    }
}

Function Import-AudioPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'AudioPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-DocumentPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'DocumentPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-DRMPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'DRMPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-GPSPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'GPSPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-ImagePropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'ImagePropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-MediaPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'MediaPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-MusicPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'MusicPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-PhotoPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'PhotoPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-RecordedTVPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'RecordedTVPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-SummaryPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'SummaryPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-VideoPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'VideoPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-PersonalTagDefinition {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'PersonalTagDefinitionType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-SharedTagDefinition {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'SharedTagDefinitionType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-BinaryPropertySet {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'BinaryPropertySetType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {

    }
}

Function Import-FileSystem {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$Element,

        [Parameter(Mandatory = $true)]
        [System.Xml.Schema.XmlSchemaSet]$SchemaSet,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNameSpacemanager]$Nsmgr,

        [Parameter(Mandatory = $true)]
        [System.IO.StringWriter]$Writer
    )

    Begin {
        $ElementType = $SchemaSet.GlobalTypes.Item(($SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq 'FileSystemType' }));
        $AttributeMap = @{};
        Get-FieldMappings -Type $ElementType -Hashtable $AttributeMap;
    }

    Process {
        $Writer.WriteLine("INSERT INTO `"FileSystems`" (`"$(((@($Element.Attributes) | ForEach-Object { $_.Name }) -join '", "'))`")");
        $Writer.Write("    VALUES(");
        $a = @($Element.Attributes)[0];
        switch ($AttributeMap[$a.LocalName]) {
            String {
                $Writer.Write("'$($a.Value.Replace("'", "''"))'");
                break;
            }
            HexBinary {
                $Writer.Write("X'$($a.Value)'");
                break;
            }
            Base64Binary {
                $Writer.Write("X'$((-join ([Convert]::FromBase64String($a.Value) | ForEach-Object { $_.ToString('x2') })))'");
                break;
            }
            Boolean {
                if ([System.Xml.XmlConvert]::ToBoolean($a.Value)) {
                    $Writer.Write("1");
                } else {
                    $Writer.Write("0");
                }
                break;
            }
            default {
                $Writer.Write($a.Value);
                break;
            }
        }
        foreach ($a in @(@($Element.Attributes) | Select-Object -Skip 1)) {
            switch ($AttributeMap[$a.LocalName]) {
                String {
                    $Writer.Write(", '$($a.Value.Replace("'", "''"))'");
                    break;
                }
                HexBinary {
                    $Writer.Write(", X'$($a.Value)'");
                    break;
                }
                Base64Binary {
                    $Writer.Write(", X'$((-join ([Convert]::FromBase64String($a.Value) | ForEach-Object { $_.ToString('x2') })))'");
                    break;
                }
                Boolean {
                    if ([System.Xml.XmlConvert]::ToBoolean($a.Value)) {
                        $Writer.Write(", 1");
                    } else {
                        $Writer.Write(", 0");
                    }
                    break;
                }
                default {
                    $Writer.Write(', ');
                    $Writer.Write($a.Value);
                    break;
                }
            }
        }
        $Writer.WriteLine(");");
    }
}

$XmlPath = $PSScriptRoot | Join-Path -ChildPath '..\FsInfoCat.UnitTests\Resources\SampleData.xml';
$SchemaPath = $PSScriptRoot | Join-Path -ChildPath '..\FsInfoCat.UnitTests\Resources\SampleData.xsd';
$OutputPath = $PSScriptRoot | Join-Path -ChildPath '..\FsInfoCat.Local\Resources\SampleData.sql';
$SchemaSet = [System.Xml.Schema.XmlSchemaSet]::new();
$Reader = [System.Xml.XmlReader]::Create($SchemaPath);
$XmlSchema = $null;
try {
    $XmlSchema = [System.Xml.Schema.XmlSchema]::Read($Reader, $null);
} finally { $Reader.Close() }
$SchemaSet.Add($XmlSchema) | Out-Null;
$SchemaSet.Compile();
<#
$Root = $SchemaSet.GlobalElements.Item(@($SchemaSet.GlobalElements.Names)[0]);
foreach ($Element in @($Root.ElementType.Particle.Items)) {
    "        `$ElementType = `$SchemaSet.GlobalTypes.Item((`$SchemaSet.GlobalTypes.Names | Where-Object { $_.Name -eq '$($Element.Name)Type' }));"
}
#>
$Xml = [Xml]::new();
$Xml.Load($XmlPath);
$Nsmgr = [System.Xml.XmlNamespaceManager]::new($Xml.NameTable);
$Nsmgr.AddNamespace('data', 'https://raw.githubusercontent.com/lerwine/FsInfoCat/V1/src/FsInfoCat.UnitTests/Resources/SampleData.xsd');
if ($null -ne $Xml.DocumentElement) {
    #$Writer = [System.IO.StreamWriter]::new($OutputPath, $false, [System.Text.UTF8Encoding]::new($false, $false));
    $Writer = [System.IO.StringWriter]::new();
    try {
        <#
        $Xml.DocumentElement.SelectNodes('AudioPropertySet') | Import-AudioPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('DocumentPropertySet') | Import-DocumentPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('DRMPropertySet') | Import-DRMPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('GPSPropertySet') | Import-GPSPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('ImagePropertySet') | Import-ImagePropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('MediaPropertySet') | Import-MediaPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('MusicPropertySet') | Import-MusicPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('PhotoPropertySet') | Import-PhotoPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('RecordedTVPropertySet') | Import-RecordedTVPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('SummaryPropertySet') | Import-SummaryPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('VideoPropertySet') | Import-VideoPropertySet -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('PersonalTagDefinition') | Import-PersonalTagDefinition -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('SharedTagDefinition') | Import-SharedTagDefinition -Writer $Writer -ErrorAction Stop;
        $Xml.DocumentElement.SelectNodes('BinaryPropertySet') | Import-BinaryPropertySet -Writer $Writer -ErrorAction Stop;
        #>
        $Xml.DocumentElement.SelectNodes('data:FileSystem', $Nsmgr) | Import-FileSystem -SchemaSet $SchemaSet -Writer $Writer -Nsmgr $Nsmgr -ErrorAction Stop;
        $Writer.Flush();
        $Writer.ToString();
    } finally {
        $Writer.Close();
    }
}
