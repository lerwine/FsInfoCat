Param(
    [string]$ProjectPath = 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\FsInfoCat.PsDesktop.csproj'
)

$ProjectFile = [System.IO.FileInfo]::new($ProjectPath);
[xml]$ProjectXml = '<Project ToolsVersion="14.0" DefaultTargets="Build" xmlns="http://schemas.microsoft.com/developer/msbuild/2003" />';
$ProjectXml.Load($ProjectFile.FullName);
$ProjectDirectory = $ProjectFile.Directory;
$SubIdx = $ProjectDirectory.FullName.Length + 1;
$XmlNamespaceManager = [System.Xml.XmlNamespaceManager]::new($ProjectXml.NameTable);
$XmlNamespaceManager.AddNamespace('msb', 'http://schemas.microsoft.com/developer/msbuild/2003');
$FileElements = @((@($ProjectXml.DocumentElement.SelectNodes('msb:ItemGroup/msb:*', $XmlNamespaceManager)) | Select-Object -Property @{ Label = 'Name'; Expression = { [System.IO.Path]::GetFileName($_.Include) } }, @{ Label = 'DirectoryName'; Expression = {
    $d = [System.IO.Path]::GetDirectoryName($_.Include);
    if ($null -ne $d) { $d } else { '' }
} }, @{ Label = 'Type'; Expression = { $_.PSBase.LocalName } }, @{ Label = 'Element'; Expression = { $_ } }) | Where-Object { $_.Type -ne 'Reference' -and $_.Type -ne 'ProjectReference' });
$ActualFiles = @(($ProjectDirectory.GetFiles('*', [System.IO.SearchOption]::AllDirectories) | Select-Object -Property 'Name', @{ Label = 'DirectoryName'; Expression = {
    if ($_.DirectoryName.Length -gt $SubIdx) { $_.DirectoryName.Substring($SubIdx) } else { '' }
} }, @{ Label = 'Type'; Expression = {
    switch ($_.Extension) {
        '.cs' { 'Compile'; break; }
        '.md' { 'None'; break; }
        '' { 'None'; break; }
        default { 'Content'; break; }
    }
} }) | Where-Object { $_.DirectoryName -ne 'obj' -and $_.DirectoryName -ne 'bin' -and -not ($_.DirectoryName.StartsWith('obj\') -or $_.DirectoryName.StartsWith('bin\') -or $_.Name.EndsWith('.csproj')) });

$FileElements | ForEach-Object {
    [System.Xml.XmlElement]$ParentNode = $_.Element.ParentNode;
    $ParentNode.RemoveChild($_.Element) | Out-Null;
    if ($ParentNode.IsEmpty -or $ParentNode.InnerXml.Trim().Length -eq 0) { $ParentNode.ParentNode.RemoveChild($ParentNode) | Out-Null }
}
$ActualFiles = @($ActualFiles | ForEach-Object {
    $e = $_;
    $r = $FileElements | Where-Object { $_.Name -ieq $e.Name -and $_.Name -ieq $e.Name } | Select-Object -First 1;
    if ($null -eq $r) {
        $x = $ProjectXml.CreateElement($_.Type, 'http://schemas.microsoft.com/developer/msbuild/2003');
        if ([string]::IsNullOrEmpty($e.DirectoryName)) {
            $x.Attributes.Append($ProjectXml.CreateAttribute('Include')).Value = $e.Name;
        } else {
            $x.Attributes.Append($ProjectXml.CreateAttribute('Include')).Value = $e.DirectoryName + '\' + $e.Name;
        }
        $e | Add-Member -MemberType NoteProperty -Name 'Element' -Value $x -PassThru;
    } else {
        if ([string]::IsNullOrEmpty($e.DirectoryName)) {
            $r.Element.Include = $e.Name;
        } else {
            $r.Element.Include = $e.DirectoryName + '\' + $e.Name;
        }
        $r
    }
})
$RefElement = @($ProjectXml.DocumentElement.SelectNodes('msb:ItemGroup', $XmlNamespaceManager)) | Select-Object -Last 1;
if ($null -eq $RefElement) { $RefElement = @($ProjectXml.DocumentElement.SelectNodes('msb:PropertyGroup', $XmlNamespaceManager)) | Select-Object -Last 1 }
foreach ($Group in (($ActualFiles | Group-Object -Property 'Type') | Sort-Object -Property 'Name')) {
    $ItemGroup = $ProjectXml.CreateElement('ItemGroup', 'http://schemas.microsoft.com/developer/msbuild/2003');
    $ProjectXml.DocumentElement.InsertAfter($ItemGroup, $RefElement) | Out-Null;
    $RefElement = $ItemGroup;
    ($Group.Group | Sort-Object -Property 'DirectoryName', 'Name') | ForEach-Object { $ItemGroup.AppendChild($_.Element) | Out-Null }
} 
$XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
$XmlWriterSettings.Indent = $true;
$XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $false);
$XmlWriter = [System.Xml.XmlWriter]::Create($ProjectFile.FullName, $XmlWriterSettings);
try {
    $ProjectXml.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
<#
(Get-ChildItem -Path 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat' -Recurse -Filter '*.cs') | Where-Object { -not ($_.StartsWith('obj\') -or $_.StartsWith('bin\')) } | ForEach-Object { $_.FullName.Substring(43) } | Sort-Object | ForEach-Object {
"        <Compile Include=`"$_`" />"
}
#>
