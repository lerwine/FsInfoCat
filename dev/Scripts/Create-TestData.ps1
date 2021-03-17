$FilePathTestDataPath = 'c:\users\lerwi\git\fsinfocat\src\FsInfoCat.Test\Resources\FilePathTestData.xml';
$XmlDocument = [System.Xml.XmlDocument]::new();
$XmlDocument.Load($FilePathTestDataPath);
$startNameChars = -join @(&{
    for ($i = 32; $i -lt 126; $i++) {
        [char]$c = $i;
        if ("$c" -match '\w') { $c }
    }
});
$nameChars = -join @(&{
    for ($i = 32; $i -lt 126; $i++) {
        [char]$c = $i;
        if ("$c" -match '[\w-]') { $c }
    }
});
$WsChars = "`r`n`t ";
$Random = [Random]::new();

foreach ($TestDataElement in $XmlDocument.SelectNodes('//FileSystem')) {
    $OriginalString = '' + $TestDataElement.Match;
    $uriString = ('' + $TestDataElement.Match).Replace('\', '/');
    $Uri = $null;
    if ([Uri]::TryCreate($uriString.Replace('#', '%23').Replace('#', '%23').Replace('?', '%3F'), [UriKind]::Absolute, [ref]$Uri)) {
        $uriString = $Uri.AbsoluteUri;
    } else {
        if ($OriginalString.Contains('\') -and $uriString.StartsWith('/') -and [Uri]::TryCreate("file://$($uriString.Replace('#', '%23').Replace('#', '%23').Replace('?', '%3F'))", [UriKind]::Absolute, [ref]$Uri)) {
            $uriString = $Uri.AbsoluteUri;
        } else {
            if (-not [uri]::IsWellFormedUriString($uriString, [UriKind]::Relative)) {
                $uriString = [Uri]::EscapeUriString($uriString).Replace('#', '%23').Replace('#', '%23').Replace('?', '%3F');
            }
        }
    }
    $TestDataElement.AppendChild($XmlDocument.CreateElement('URI')).InnerText = $uriString;
}
<#
foreach ($TargetElement in @($XmlDocument.DocumentElement.SelectNodes('//Host[@IsIPV2="false" and @IsDns="false"]'))) {
<#
    [System.Xml.XmlElement]$UriElement = $TargetElement.ParentNode;
    $UriElement.RemoveChild($TargetElement) | Out-Null;
  
    $XmlElement = $UriElement.AppendChild($XmlDocument.CreateElement('HostNameWUncRegex'));
    $success = '' + $TargetElement.Success;
    $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Success')).Value = '' + $success;
    if ($success -eq 'true') {
        @($TargetElement.SelectNodes('Group')) | ForEach-Object {
            $XmlElement.AppendChild($_.CloneNode($true)) | Out-Null;
        }
        $TargetElement.RemoveChild($TargetElement.SelectSingleNode('Group[@Name="unc"]')) | Out-Null;
    }  
# >
    
    $Name = '' + $TargetElement.Name;
    if ($Name.Contains(':')) {
        $Name = ((('' + $TargetElement.Name) -split ':') | ForEach-Object {
            $n = $_;
            $len = $_.Length;
            if ($len -gt 0) {
                $Random.Next(0, 65536).ToString('x');
            }
            else
            {
                $n;
            }
        }) -join ':';
        $TargetElement.SetAttribute('Name', $Name);
        $Name = ((('' + $TargetElement.Name) -split ':') | ForEach-Object {
            $n = $_;
            $len = $_.Length;
            if ($len -gt 0) {
                $Random.Next(0, 65536).ToString('x');
            }
            else
            {
                $n;
            }
        }) -join ':';
        $XmlElement = $XmlDocument.DocumentElement.AppendChild($TargetElement.CloneNode($true));
        $XmlElement.SetAttribute('Name', "[$Name]");
        if ($XmlElement.IPV6 -eq "Normal") {
            $XmlElement.SetAttribute('IPV6', "Bracketed");
        }
        $Name = ((('' + $TargetElement.Name) -split ':') | ForEach-Object {
            $n = $_;
            $len = $_.Length;
            if ($len -gt 0) {
                $Random.Next(0, 65536).ToString('x');
            }
            else
            {
                $n;
            }
        }) -join '-';
        $XmlElement = $XmlDocument.DocumentElement.AppendChild($TargetElement.CloneNode($true));
        $XmlElement.SetAttribute('Name', "$Name.ipv6-literal.net");
        if ($XmlElement.IPV6 -eq "Normal") {
            $XmlElement.SetAttribute('IPV6', "UNC");
        }
        if (-not $Name.StartsWith('-')) {
            $XmlElement.SetAttribute('IsDns', "true");
        }
        $Name = ((('' + $TargetElement.Name) -split ':') | ForEach-Object {
            $n = $_;
            $len = $_.Length;
            if ($len -gt 0) {
                $Random.Next(0, 65536).ToString('x');
            }
            else
            {
                $n;
            }
        }) -join '-';
        $XmlElement = $XmlDocument.DocumentElement.AppendChild($TargetElement.CloneNode($true));
        $XmlElement.SetAttribute('Name', $Name);
        $XmlElement.SetAttribute('IPV6', "None");
    }
}
#>

<#
@('0.0.0.0', '00.00.00.00', '000.000.000.000', '192.168.1.2', '10.255.51.3', '010.255.051.03', '010.255.051.003', '255.255.255.255') | ForEach-Object {
    $XmlElement = $XmlDocument.DocumentElement.AppendChild($XmlDocument.CreateElement('HostName'));
    $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Value')).Value = $_;
    $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Description')).Value = "IPV2 Address $_";
    $RegexElement = $XmlElement.AppendChild($XmlDocument.CreateElement('Ipv2AddressRegex'));
    $RegexElement.Attributes.Append($XmlDocument.CreateAttribute('Success')).Value = 'true';
    $RegexElement.InnerText = $_;
    $RegexElement = $XmlElement.AppendChild($XmlDocument.CreateElement('Ipv6AddressRegex'));
    $RegexElement.Attributes.Append($XmlDocument.CreateAttribute('Success')).Value = 'false';
    $RegexElement = $XmlElement.AppendChild($XmlDocument.CreateElement('DnsNameRegex'));
    $RegexElement.Attributes.Append($XmlDocument.CreateAttribute('Success')).Value = 'true';
    $RegexElement.InnerText = $_;
}
#>
#<#
$XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
$XmlWriterSettings.IndentChars = "    ";
$XmlWriterSettings.Indent = $true;
$XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $true);
$XmlWriterSettings.OmitXmlDeclaration = $true;
$XmlWriter = [System.Xml.XmlWriter]::Create($FilePathTestDataPath, $XmlWriterSettings);
try {
    $XmlDocument.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.close() }
#>