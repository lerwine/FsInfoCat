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
$ResourcesPath = $PSScriptRoot | Join-Path -ChildPath '../FsInfoCat/Properties/Resources.resx';

$EntityTypesXml = [Xml]::new();
$EntityTypesXml.PreserveWhitespace = $true;
$EntityTypesXml.Load($EntityTypesPath);
$ResourcesXml = [Xml]::new();
$ResourcesXml.Load($ResourcesPath);
$MissingNames = @(@($EntityTypesXml.SelectNodes('//Display[not(count(@Label)=0)]')) | ForEach-Object {
    $Text = '' + $_.Label;
    if ($null -eq $ResourcesXml.DocumentElement.SelectSingleNode("data[@name='$($Text)']")) { $Text | Write-Output }
    $Text = '' + $_.ShortName;
    if ($Text.Lenth -gt 0 -and $null -eq $ResourcesXml.DocumentElement.SelectSingleNode("data[@name='$($Text)']")) { $Text | Write-Output }
    $Text = '' + $_.Description;
    if ($Text.Lenth -gt 0 -and $null -eq $ResourcesXml.DocumentElement.SelectSingleNode("data[@name='$($Text)']")) { $Text | Write-Output }
} | Select-Object -Unique);

if ($MissingNames.Count -gt 0) {
    $MissingNames | ForEach-Object {
        $XmlElement = $ResourcesXml.DocumentElement.AppendChild($ResourcesXml.CreateElement('data'));
        $XmlElement.Attributes.Append($ResourcesXml.CreateAttribute('name')).Value = $_;
        $XmlElement.AppendChild($ResourcesXml.CreateElement('value')).InnerText = "TODO: Define resource";
    }
    [System.Xml.XmlWriter]$Writer = [System.Xml.XmlWriter]::Create($ResourcesPath, [System.Xml.XmlWriterSettings]@{
        Indent = $true;
        Encoding = [System.Text.UTF8Encoding]::new($false, $false);
        IndentChars = '    ';
    });
    try {
        $ResourcesXml.WriteTo($Writer);
        $Writer.Flush();
    } finally {
        $Writer.Close();
    }
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
