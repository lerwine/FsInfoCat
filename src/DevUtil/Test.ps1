Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath 'bin/Debug/net6.0-windows/DevHelper') -ErrorAction Stop;

Function Test-EntityTypesXml {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$XDocument
    )
    $Namespace = $XDocument.Root.Name.Namespace;
    $MemberReferenceDictionary = Get-MemberReferenceDictionary -XDocument $XDocument;
    $RootSelector = [DevUtil.XElementSelector]::new($XDocument.Root);
    $Attributes = $RootSelector.Descendants($Namespace.GetName('seealso')).Attributes('cref');
    $Attributes = $Attributes.Concat($RootSelector.Descendants($Namespace.GetName('see')).Attributes('cref').GetItems());
    foreach ($XAttribute in $Attributes.GetItems()) {
        if (-not $MemberReferenceDictionary.ContainsKey($XAttribute.Value)) {
            Write-Warning -Message "$($XAttribute.Name) attribute references an unknown type `"$($XAttribute.Value)`": Line $($XAttribute.LineNumber), Column $($XAttribute.LinePosition)";
        }
    }
    foreach ($XAttribute in $RootSelector.Descendants($Namespace.GetName('Type')).Attributes('Ref').GetItems()) {
        if (-not $MemberReferenceDictionary.ContainsKey($XAttribute.Value)) {
            Write-Warning -Message "$($XAttribute.Name) attribute references an unknown type `"$($XAttribute.Value)`": Line $($XAttribute.LineNumber), Column $($XAttribute.LinePosition)";
        }
    }
    foreach ($XAttribute in $RootSelector.Descendants($Namespace.GetName('Argument')).Attributes('Ref').GetItems()) {
        $ModelElement = [DevUtil.XElementSelector]::new($XAttribute.Parent).Ancestors($Namespace.GetName('Model')).FirstOrDefault();
        if ($null -eq $ModelElement-or -not [DevUtil.XElementSelector]::new($ModelElement).ElementsWithAttribute($Namespace.GetName('TypeParameter'), 'Name', $XAttribute.Value).Any()) {
            Write-Warning -Message "$($XAttribute.Name) attribute references an generic argument `"$($XAttribute.Value)`": Line $($XAttribute.LineNumber), Column $($XAttribute.LinePos0ition)";
        }
    }
}
$EntityTypesPath = $PSScriptRoot | Join-Path -ChildPath '../FsInfoCat/Resources/EntityTypes.xml';

Function Test-ResourceNames {
    [CmdletBinding()]
    [OutputType([System.Xml.Linq.XDocument])]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.Linq.XDocument]$EntityTypesXml
    )
    $ResourcesPath = $PSScriptRoot | Join-Path -ChildPath '../FsInfoCat/Properties/Resources.resx';
    $ResourcesXml = [System.Xml.Linq.XDocument]::Load($ResourcesPath);
    $DataNames = @([DevUtil.XElementSelector]::new($ResourcesXml.Root).Descendants('data').Attributes('name').GetItems() | ForEach-Object { $_.Value });

    $MissingNames = @([DevUtil.XElementSelector]::new($EntityTypesXml.Root).Descendants('Display').Attributes('Label').GetItems() | ForEach-Object {
        $Text = $_.Value;
        if ($DataNames -cnotcontains $Text) { $Text | Write-Output }
        $Text = '' + $_.ShortName;
        $XElement = ([System.Xml.Linq.XAttribute]$_).Parent;
        $XAttribute = $XElement.Attribute([System.Xml.Linq.XNamespace]::None.GetName('ShortName'));
        if ($null -ne $XAttribute -and $DataNames -cnotcontains $XAttribute.Value) { $XAttribute.Value | Write-Output }
        $XAttribute = $XElement.Attribute([System.Xml.Linq.XNamespace]::None.GetName('Description'));
        if ($null -ne $XAttribute -and $DataNames -cnotcontains $XAttribute.Value) { $XAttribute.Value | Write-Output }
    } | Select-Object -Unique);

    if ($MissingNames.Count -gt 0) {
        $MissingNames | ForEach-Object {
            $ResourcesXml.Root.Add(([System.Xml.Linq.XElement]::new([System.Xml.Linq.XNamespace]::None.GetName('data'),
                [System.Xml.Linq.XAttribute]::new([System.Xml.Linq.XNamespace]::None.GetName('name'), $_),
                [System.Xml.Linq.XElement]::new([System.Xml.Linq.XNamespace]::None.GetName('value'), "TODO: Set value for resource `"$_`"")
            )));
        }
        [System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($ResourcesPath, [System.Xml.XmlWriterSettings]@{
            Indent = $true;
            Encoding = [System.Text.UTF8Encoding]::new($false, $false);
            IndentChars = '    ';
        });
        try {
            $ResourcesXml.Save($Writer);
            $Writer.Flush();
        } finally {
            $Writer.Close();
        }
    }
}

$EntityTypesXml = [System.Xml.Linq.XDocument]::Load($EntityTypesPath, ([System.Xml.Linq.LoadOptions]([System.Xml.Linq.LoadOptions]::PreserveWhitespace -bor [System.Xml.Linq.LoadOptions]::SetLineInfo)));
Test-ResourceNames -EntityTypesXml $EntityTypesXml;

[DevUtil.XElementSelector]::new($EntityTypesXml.DocumentElement).Elements('Local').Elements('AbstractEntity').Elements('View').Descendants('LeftJoin').GetItems() | ForEach-Object {

}

<#
[System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($EntityTypesPath);
#, [System.Xml.XmlWriterSettings]@{
#    #Indent = $true;
#    Encoding = [System.Text.UTF8Encoding]::new($false, $false);
#    #IndentChars = '    ';
#});
try {
    $EntityTypesDocument.WriteTo($Writer);
    $Writer.Flush();
} finally {
    $Writer.Close();
}
#>
