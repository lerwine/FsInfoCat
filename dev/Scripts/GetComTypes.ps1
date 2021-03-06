﻿$Hash = @{};
$Items = @(Get-ChildItem HKLM:\Software\Classes -ErrorAction SilentlyContinue | Where-Object {
	(Test-Path -Path "$($_.PSPath)\CLSID")
} | ForEach-Object {
    $sk = $_.OpenSubKey('CLSID');
    $Hash[$sk.GetValue('')] = $_.PSChildName;
});

[Xml]$Xml = '<Properties/>';
$Xml.Load('c:\users\lerwi\git\fsinfocat\src\FsInfoCat\Resources\PropertySystem.xml');
@($Xml.DocumentElement.SelectNodes('Format/@ID')) | ForEach-Object {
    if ($Hash.ContainsKey($_.Value)) { "$($_.Value) = $($Hash[$_.Value])" }
}
<#

(Get-ChildItem HKLM:\Software\Classes -ErrorAction SilentlyContinue | Where-Object {
	$_.PSChildName -match '^\w+\.\w+$' -and (Test-Path -Path "$($_.PSPath)\CLSID")
} | Select-Object -ExpandProperty PSChildName);


$Path = 'C:\Users\lerwi\Git\FsInfoCat\FsInfoCat\FsInfoCat.UnitTests\Resources\Local\Users\gwash';
$Name = 'Pictures';
$WshShell = New-Object -ComObject WScript.Shell;
$ShellApp = New-Object -ComObject Shell.Application;
Get-Member -InputObject $ShellApp;
$Folder = $ShellApp.NameSpace($Path);
$Folder | Get-Member
$Item = $Folder.Items().Item($Name);
Get-Member -InputObject $Item;
for ($i = 0; $i -lt 0xffff; $i++) {
    $n = $Folder.GetDetailsOf($null, $i);
    if (-not [string]::IsNullOrEmpty($n)) {
        $v = $Folder.GetDetailsOf($Item, $i);
        if (-not [string]::IsNullOrEmpty($v)) {
            "$i. $n = $v"
        }
    }
}

$Folder.GetDetailsOf($Item, 'PerceivedType')
$Item.ExtendedProperty('Kind');

$Item2 = $Folder.ParseName($Name);
#>