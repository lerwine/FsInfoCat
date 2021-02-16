Param(
    #[Parameter(Mandatory = $true)]
    # Modifed XPath query which starts with a XQuery doc function, followed by an XPath expression.
    [string]$Query = 'doc("blockCommon.xsd"|"inline.xsd")/schema/element[@name="para"]',
    [string[]]$SearchPath = @('$PSScriptRoot$', '$PWD$', 'C:\Users\lerwi\Git\FsInfoCat\Resources\Build', 'C:\Users\lerwi\Git\FsInfoCat\Resources\PSMaml')
)

$Script:XmlNamespaces = @{
    msh = "http://msh";
    maml = "http://schemas.microsoft.com/maml/2004/10";
    command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
    dev = "http://schemas.microsoft.com/maml/dev/2004/10";
    MSHelp = "http://msdn.microsoft.com/mshelp";
    xsl="http://www.w3.org/1999/XSL/Transform";
    xhtml="http://www.w3.org/1999/xhtml";
    msxsl="urn:schemas-microsoft-com:xslt";
    xmlns="http://www.w3.org/2000/xmlns/";
    xsi="http://www.w3.org/2001/XMLSchema-instance";
    xs="http://www.w3.org/2001/XMLSchema";
    msdata="urn:schemas-microsoft-com:xml-msdata";
};

$SearchDirectories = @($SearchPath | ForEach-Object {
    switch ($_) {
        '$PSScriptRoot$' {
            $PSScriptRoot | Write-Output;
            break;
        }
        '$PWD$' {
            (Get-Location).Path | Write-Output;
            break;
        }
        default {
            $PWD | Join-Path -ChildPath $_;
            break;
        }
    }
} | Where-Object { $_ | Test-Path -PathType Container });
if ($SearchDirectories.Count -eq 0) {
    Write-Warning -Message 'No search directories found';
    return;
}
$DocNames = @();
$XPath = '';
if ($Query -match '^doc\(\s*(?<doc>("[^"]*"|''[^'']*'')(\s*\|\s*("[^"]*"|''[^'']*''))*)\s*\)(?<xpath>.+)$') {
    $doc = $Matches['doc'];
    $XPath = $Matches['xpath'];
    do {
        if ($doc -match '^("(?<f1>[^"]+)?"|''(?<f2>[^'']+)?'')(\s*\|\s*(?<n>.+))?$') {
            $f = $Matches['f1'];
            if ($null -eq $f) { $f = '' + $Matches['f2'] }
            $f = $f.Trim();
            if ($f.Length -eq 0) {
                Write-Warning -Message 'File name cannot be empty.';
            } else {
                if ($DocNames -inotcontains $f) { $DocNames += $f }
            }
            $doc = $Matches['n'];
        } else {
            Write-Warning -Message 'Unexpected doc parse error';
            break;
        }
    } while ($null -ne $doc);
    if ($DocNames.Count -eq 0) {
        Write-Warning -Message 'No document names';
        return;
    }
} else {
    Write-Warning -Message 'Could not parse query';
    return;
}

$DocNames | ForEach-Object {
    $n = $_;
    $SearchDirectories | ForEach-Object { $_ | Join-Path $n } | Where-Object { $_ | Test-Path -PathType Leaf } | ForEach-Object {
        $XmlDocument = [System.Xml.XmlDocument]::new();
        $XmlDocument.Load($_);
        if ($null -ne $XmlDocument.DocumentElement) {
            $Nsmgr = [System.Xml.XmlNamespaceManager]::new($XmlDocument.NameTable);
            @($Script:XmlNamespaces.Keys) | ForEach-Object {
                $Nsmgr.AddNamespace($_, $Script:XmlNamespaces[$_]);
            }
            @($XmlDocument.DocumentElement.SelectNodes($XPath, $Nsmgr)) | ForEach-Object {
                
            }
        }
    }
}
