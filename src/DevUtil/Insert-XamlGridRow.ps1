
Param(
    [string]$LiteralPath = 'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Desktop\LocalData\SummaryPropertySets\ListItemControl.xaml',
    [int]$RowIndex = 2,
    [int]$RowCount = 2,
    [string]$DefaultPrefix = 'p',
    [string]$XPath = '/p:UserControl/p:Grid[1]',
    [string]$Height = 'Auto'
)

Import-Module -Name './bin/Debug/net6.0/DevHelper' -ErrorAction Stop;

$Xml = [Xml]::new();
$Xml.PreserveWhitespace = $true;
$Xml.Load($LiteralPath);
if ($null -eq $Xml.DocumentElement) {
    Write-Warning -Message 'No document element';
} else {
    $nsmgr = [System.Xml.XmlNamespaceManager]::new($Xml.NameTable);
    $nsmgr.AddNamespace($DefaultPrefix, $Xml.DocumentElement.NamespaceURI);
    $GridElement = $Xml.SelectSingleNode($XPath, $nsmgr);
    if ($null -eq $GridElement) {
        Write-Warning -Message "Element not found at XPath $XPath";
    } else {
        Add-XamlGridRow -GridElement $GridElement -RowIndex $RowIndex -RowCount $RowCount -Height $Height;
        $GridElement.OuterXml;
    }
}
