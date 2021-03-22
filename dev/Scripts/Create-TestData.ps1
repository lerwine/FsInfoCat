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
$SegmentsRegex = [System.Text.RegularExpressions.Regex]::new('(?:^|\G)(?:/(?<n>[^/]*)|(?<n>[^/]+))');
$RecodeRegex = [System.Text.RegularExpressions.Regex]::new('(^|\G)((?<r>%([a-f][\dA-Fa-f]|[\dA-F][a-f]))|([^%]+|%(?![\dA-F]))+)');
$WindowsRegex = [System.Text.RegularExpressions.Regex]::new(@"
^
(
    (?<file>
        (?i)
        (?<scheme>file):[\\/]{2}
        (
            (?<host>[^\\/]+)
            (?<path>[\\/].*)?
        |
            [\\/](?<path>[a-z]:([\\/].*)?)
        )
    )
|
    (?<unc>
        [\\/]{2}
        (?<host>[^\\/]+)
        (?<path>([\\/](?![\\/]).*)?)
    )
|
    (?<path>[a-z]:([\\/].*)?)
|
    (?!file:|((?i)FILE:/))
    (?<scheme>[a-zA-Z][\w-.]+):
    (//?)?
    (?<host>
        (
            [^?#/@:]*
            (:[^?#/@:]*)?
            @
        )?
        [^?#/@:]+
        (:\d+)?
    )?
    (?<path>([/:].*)?)
)$
"@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));
$LinuxRegex = [System.Text.RegularExpressions.Regex]::new(@"
^
(
    (?<file>
        (?i)
        (?<scheme>file):[\\/]{2}
        (/(?<host>[^/]+))?
        (?<path>/.*)?
    )
|
    (?<unc>
        //
        (?<host>[^/]+)
        (?<path>(/.*)?)
    )
|
    (?<path>/(?!/).*)
|
    (?!file:|((?i)FILE:/))
    (?<scheme>[a-zA-Z][\w-.]+):
    (//?)?
    (?<host>
        (
            [^?#/@:]*
            (:[^?#/@:]*)?
            @
        )?
        [^?#/@:]+
        (:\d+)?
    )?
    (?<path>([/:].*)?)
)$
"@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));
<#
foreach ($TestDataElement in @($XmlDocument.SelectNodes('//WellFormed'))) {
    $TestDataElement.ParentNode.RemoveChild($TestDataElement) | Out-Null;
}#>

class LocalPathType {
}
class PathSegmentsType {
}
class UriAuthorityType {
}
class BaseUri {
    [bool]$IsWellFormed;
    [PathSegmentsType]$Path;
    [string]$Query;
    [string]$Fragment;
    [LocalPathType]$LocalPath;
}
class AbsoluteUrl : BaseUri {
    [UriAuthorityType]$Authority;
}
class TranslatedAbsoluteUrl : AbsoluteUrl {
}
class WellformedAbsoluteUrl : AbsoluteUrl {
}
class AbsoluteOsPathUrl : AbsoluteUrl{
    [string]$Match;
    [TranslatedAbsoluteUrl]$Translated;
    [WellformedAbsoluteUrl]$WellFormed;
}
class RelativeUri : BaseUri {
}
class TranslatedRelativeUri : RelativeUri {
}
class WellformedRelativeUri : RelativeUri {
}
class RelativeOsPathUrl : RelativeUri {
    [string]$Match;
    [TranslatedRelativeUri]$Translated;
    [WellformedRelativeUri]$WellFormed;
}
class UncHostInfo {
    [string]$Match;
    [bool]$IsAbsolute;
}
class FileSystemPath {
    [UncHostInfo]$Host;
    [PathSegmentsType]$Path;
    [Uri]$URI;
}
class TranslatedFileSystemPath : FileSystemPath {
    [bool]$IsAbsolute;
}
class OSFileSystemPath : FileSystemPath {
    [UncHostInfo]$Host;
    [PathSegmentsType]$Path;
    [Uri]$URI;
    [FileSystemPath]$Translated;
}
class OSPathType {
    hidden [AbsoluteOsPathUrl]$AbsoluteUrl;
    hidden [RelativeOsPathUrl]$RelativeUrl;
    hidden [FileSystemPath]$FileSystem;
}    
class TestDataItem {
    [string]$InputString;
    hidden [OSPathType]$Windows;
    hidden [OSPathType]$Linux;
}
class FilePathTestData {
    hidden [System.Collections.ObjectModel.Collection[TestDataItem]]$Items;
    TestDataNode() {
    }
}
foreach ($TestDataElement in $XmlDocument.SelectNodes('//TestData')) {
    $TestData = [TestDataNode]::new($TestDataElement);
    
    $TestData.Linux.AbsoluteUrl = [AbsoluteUrlNode]@{
        XmlElement = $TestData.Linux.XmlElement.SelectSingleNode('AbsoluteUrl');
    };
    $TestData.Linux.RelativeUrl = [RelativeUrlNode]@{
        XmlElement = $TestData.Linux.XmlElement.SelectSingleNode('RelativeUrl');
    };
    $TestData.Linux.FileSystem = [FileSystemNode]@{
        XmlElement = $TestData.Linux.XmlElement.SelectSingleNode('FileSystem');
    };

    $Match = $WindowsRegex.Match($InputString);
    if ($Match.Groups["file"].Success) {
        
    }
    if ($Match.Groups["host"].Success) {
    }
    if ($Match.Groups["unc"].Success) {
    }
    if ($Match.Groups["path"].Success) {
    }
    if ($Match.Groups["scheme"].Success) {
    }
    #$OriginalString = $WellFormedString = $OriginalString -replace '^([a-z]):', 'tMpX$1:';
    $WellFormedString = $OriginalString;
    $Uri = $null;
    $SchemeAuth = $Path = $Query = $Fragment = '';
    if (-not [Uri]::IsWellFormedUriString($WellFormedString, [UriKind]::Relative)) {
        $WellFormedString = $WellFormedString;
        if (-not [Uri]::IsWellFormedUriString($WellFormedString, [UriKind]::Relative)) {
            $WellFormedString = $RecodeRegex.Replace($WellFormedString, {
                if ($args[0].Groups['r'].Success) {
                    return $args[0].Value.ToUpper();
                } else {
                    return [Uri]::EscapeUriString($args[0].Value)
                }
            });
            if (-not [Uri]::TryCreate($WellFormedString, [UriKind]::Relative, [ref]$Uri)) {
                Write-Warning -Message "$OriginalString -> $WellFormedString";
                $Uri = $null;
            }
        }
    }
    Write-Information -Message "$OriginalString -> $WellFormedString" -InformationAction Continue;
    $Segments = @();
    $Path = $Query = $Fragment = '';
    if ($WellFormedString.Contains('#') -or $WellFormedString.Contains('?')) {
        $WellFormedString = $WellFormedString.Replace('#', [Uri]::HexEscape('#')).Replace('?', [Uri]::HexEscape('?'));
    #if ($WellFormedString.Contains('#')) {
    #    $WellFormedString = $WellFormedString.Replace('#', [Uri]::HexEscape('#'));
    }
    $Path  = $WellFormedString;
    if ($WellFormedString -match '^(<p>[^#?]*)(<q>[^#]*)(<f>.*)') {
        $Path = $Matches['p'];
        $Query = $Matches['q'];
        $Fragment = $Matches['f'];
    }
    $Segments = $SegmentsRegex.Matches($Path) | Select-Object -ExcludeProperty 'Value';
    $XmlElement = $TestDataElement.AppendChild($XmlDocument.CreateElement('WellFormed'));
    $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Value')).Value = $WellFormedString;
    $XmlElement = $XmlElement.AppendChild($XmlDocument.CreateElement('Path'));
    if ($Uri.Segments.Length -lt $MinSegmentCount) {
        $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Match')).Value = $Path;
        $XmlElement.AppendChild($XmlDocument.CreateElement('Directory')).InnerText = -join $Segments;
        $XmlElement.AppendChild($XmlDocument.CreateElement('FileName')).Attributes.Append($XmlDocument.CreateAttribute('xsi', 'nil', 'http://www.w3.org/2001/XMLSchema-instance')).Value = 'true';
    } else {
        $FileName = ($Segments | Select-Object -Last 1) -replace '/$', '';
        $Directory = -join ($Segments | Select-Object -SkipLast 1);
        if ([string]::IsNullOrEmpty($FileName) -or [string]::IsNullOrEmpty($Directory) -or $Directory.EndsWith('/')) {
            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Match')).Value = "$Directory$FileName";
        } else {
            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Match')).Value = "$Directory/$FileName";
        }
        if ($Uri.Segments.Count -gt 3) { $Directory = $Directory -replace '/$', '' }
        $XmlElement.AppendChild($XmlDocument.CreateElement('Directory')).InnerText = $Directory;
        $XmlElement.AppendChild($XmlDocument.CreateElement('FileName')).InnerText = $FileName;
    }
}
<#
        $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Match')).Value = 
        $Name = $Uri.Segments | ForEach-Object { $_.Replace('/', '') } | Where-Object { $_.Length -gt 0 } | Select-Object -Last 1;
        if ($null -eq $Name) {
            $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('Value')).Value = 
            
        $Seg = $Uri.Segments | Select-Object -First 1;

    $TestDataElement.AppendChild($XmlDocument.CreateElement('URI')).InnerText = $uriString;
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