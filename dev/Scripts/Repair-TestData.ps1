<#$Script:FilePathTestDataPath = 'c:\users\lerwi\git\fsinfocat\src\FsInfoCat.Test\Resources\FilePathTestData.xml';
$Script:FilePathTestDataDocument = [System.Xml.XmlDocument]::new();
$Script:FilePathTestDataDocument.Load($Script:FilePathTestDataPath);
#>

Function Save-FilePathTestData {
    <#
    .SYNOPSIS
        Saves changes to the FilePathTestData XML.
    .DESCRIPTION
        Saves changes to the FilePathTestData XML document to the location Specified by the FilePathTestDataPath script variable.
    .INPUTS
        Inputs None.
    .OUTPUTS
        Output None.
    #>
    [CmdletBinding()]
    Param()
    $XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
    $XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $true);
    $XmlWriterSettings.Indent = $true;
    $XmlWriterSettings.IndentChars = '    ';
    $XmlWriter = [System.Xml.XmlWriter]::Create($Script:FilePathTestDataPath, $XmlWriterSettings);
    try {
        $Script:FilePathTestDataDocument.WriteTo($XmlWriter);
        $XmlWriter.Flush();
    }
    finally { $XmlWriter.Close() }
}

Function Test-XmlNodeContains {
    <#
    .SYNOPSIS
        Tests whether a potential ancestor node contains the referenced node.
    .DESCRIPTION
        Iterates through the ancestors of the referenced node to see if it is contained by the ancestor node.
    .INPUTS
        Inputs System.Xml.XmlNode. The node to test to see whether it is a descendant node.
    .OUTPUTS
        Output Boolean. True if the referenced node is a descendant of the ancestor node; otherwise, False.
    #>
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlNode]$ReferenceNode,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$AncestorNode
    )

    Process {
        if ([Object]::ReferenceEquals($TargetNode, $ReferenceNode) -or $ReferenceNode -is [System.Xml.XmlDocument] -or $null -eq $ReferenceNode.ParentNode -or [Object]::ReferenceEquals($TargetNode.ParentNode, $ReferenceNode.ParentNode)) {
            $false | Write-Output;
        } else {
            $Result = $false;
            for ($p = $ReferenceNode.ParentNode; $null -ne $p; $p = $p.ParentNode) {
                if ([Object]::ReferenceEquals($p, $TargetNode)) {
                    $Result = $true;
                    break;
                }
            }
            $Result | Write-Output;
        }
    }
}

Function Test-XmlNodeFollows {
    <#
    .SYNOPSIS
        Tests whether a one follows another as a sibling node..
    .DESCRIPTION
        Iterates through the preceding siblings of the target node to see if it follows the candidate precedent node..
    .INPUTS
        Inputs System.Xml.XmlNode. The node to test whether it follows another node.
    .OUTPUTS
        Output Boolean. True if the target node follows the precedent node; otherwise, False.
    #>
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        # The node to test whether it follows the precedent node.
        [System.Xml.XmlNode]$TargetNode,

        [Parameter(Mandatory = $true)]
        # The node to test against as the potential preceding node.
        [System.Xml.XmlNode]$Precedent,

        # Tests whether the target node immediately follows the precedent node.
        [switch]$Immediate
    )

    Process {
        if ([Object]::ReferenceEquals($TargetNode, $Precedent) -or $null -eq $TargetNode.ParentNode -or $null -eq $Precedent.ParentNode -or -not [Object]::ReferenceEquals($TargetNode.ParentNode, $Precedent.ParentNode)) {
            $false | Write-Output;
        } else {
            $Result = $false;
            if ($Immediate.IsPresent) {
                $NodeType = $TargetNode.NodeType;
                if ($NodeType -eq $Precedent.NodeType) {
                    for ($p = $TargetNode.PreviousSibling; $null -ne $p; $p = $p.PreviousSibling) {
                        if ([Object]::ReferenceEquals($p, $Precedent)) {
                            $Result = $true;
                            break;
                        }
                        if ($p.NodeType -eq $NodeType) { break }
                    }
                } else {
                    $OtherType = $Precedent.NodeType;
                    for ($p = $TargetNode.PreviousSibling; $null -ne $p; $p = $p.PreviousSibling) {
                        if ([Object]::ReferenceEquals($p, $Precedent)) {
                            $Result = $true;
                            break;
                        }
                        if ($p.NodeType -eq $NodeType -or $p.NodeType -eq $OtherType) { break }
                    }
                }
            } else {
                for ($p = $TargetNode.PreviousSibling; $null -ne $p; $p = $p.PreviousSibling) {
                    if ([Object]::ReferenceEquals($p, $Precedent)) {
                        $Result = $true;
                        break;
                    }
                }
            }
            $Result | Write-Output;
        }
    }
}

Function Test-XmlNodePrecedes {
    <#
    .SYNOPSIS
        Tests whether a one precedes another as a sibling node..
    .DESCRIPTION
        Iterates through the following siblings of the target node to see if it precedes the candidate antecedent node..
    .INPUTS
        Inputs System.Xml.XmlNode. The node to test whether it follows another node.
    .OUTPUTS
        Output Boolean. True if the target node precedes the antecedent node; otherwise, False.
    #>
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        # The node to test whether it precedes the antecedent node.
        [System.Xml.XmlNode]$TargetNode,

        [Parameter(Mandatory = $true)]
        # The node to test against as the potential following node.
        [System.Xml.XmlNode]$Antecedent,

        [switch]$Immediate
    )

    Process {
        if ([Object]::ReferenceEquals($TargetNode, $Antecedent) -or $null -eq $TargetNode.ParentNode -or $null -eq $Antecedent.ParentNode -or -not ([Object]::ReferenceEquals($TargetNode.ParentNode, $Antecedent.ParentNode))) {
            $false | Write-Output;
        } else {
            $Result = $false;
            if ($Immediate.IsPresent) {
                $NodeType = $TargetNode.NodeType;
                if ($NodeType -eq $Antecedent.NodeType) {
                    for ($n = $TargetNode.NextSibling; $null -ne $n; $n = $n.NextSibling) {
                        if ([Object]::ReferenceEquals($n, $Antecedent)) {
                            $Result = $true;
                            break;
                        }
                        if ($n.NodeType -eq $NodeType) { break }
                    }
                } else {
                    $OtherType = $Antecedent.NodeType;
                    for ($n = $TargetNode.NextSibling; $null -ne $n; $n = $n.NextSibling) {
                        if ([Object]::ReferenceEquals($n, $Antecedent)) {
                            $Result = $true;
                            break;
                        }
                        if ($n.NodeType -eq $NodeType -or $n.NodeType -eq $OtherType) { break }
                    }
                }
            } else {
                for ($n = $TargetNode.NextSibling; $null -ne $n; $n = $n.NextSibling) {
                    if ([Object]::ReferenceEquals($n, $Antecedent)) {
                        $Result = $true;
                        break;
                    }
                }
            }
            $Result | Write-Output;
        }
    }
}

Function Set-XmlChildElementSequence {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidateScript({ $_ | ForEach-Object { [System.Xml.XmlConvert]::VerifyNCName($_) } })]
        [string[]]$ElementNames,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement
    )

    Begin {
        $OrderedElementNames = [System.Collections.Generic.LinkedList[string]]::new();
    }

    Process {
        foreach ($n in $ElementNames) {
            if (-not $OrderedElementNames.Contains($n)) { $OrderedElementNames.AddLast($n) | Out-Null }
        }
    }

    End {
        if ($OrderedElementNames.Count -lt 2) {
            Write-Warning -Message "Not enough different element names!";
            return;
        }
        $TargetSequence = [System.Collections.Generic.LinkedList[System.Xml.XmlElement]]::new();
        foreach ($n in $OrderedElementNames) {
            foreach ($e in $ParentElement.SelectNodes($n)) { $TargetSequence.AddLast($e) | Out-Null }
        }
        if ($TargetSequence.Count -lt 2) {
            Write-Information -MessageData "Nothing to do - Fewer than 2 elements matched any of the provided element names.";
            return;
        }
        $SequentialNode = $TargetSequence.First;
        $TargetElement = $SequentialNode.Value;
        $FirstChild = $ParentElement.FirstChild;
        while ($FirstChild -isnot [System.Xml.XmlElement]) { $FirstChild = $FirstChild.NextSibling }
        if (-not [Object]::ReferenceEquals($FirstChild, $TargetElement)) {
            $ParentElement.InsertBefore($ParentElement.RemoveChild($TargetElement), $FirstChild) | Out-Null;
        }
        do {
            $ExpectedNext = $SequentialNode.Next.Value;
            if (-not (Test-XmlNodePrecedes -TargetNode $TargetElement -Antecedent $ExpectedNext -Immediate)) {
                $ParentElement.InsertAfter($ParentElement.RemoveChild($ExpectedNext), $TargetElement) | Out-Null;
            }
            $SequentialNode = $SequentialNode.Next;
            $TargetElement = $SequentialNode.Value;
        } while ($null -ne $SequentialNode.Next);
    }
}

$Script:WindowsFormatDetectionRegex = [System.Text.RegularExpressions.Regex]::new(@'
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
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:LinuxFormatDetectionRegex = [System.Text.RegularExpressions.Regex]::new(@'
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
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:WindowUriHostAndPathStrictRegex = [System.Text.RegularExpressions.Regex]::new(@'
^
(?<file>
    file://
    (
        (?i)
        (?<host>
            (?=(\d+\.){3}\d+)
            (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
        |
            (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$))\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
        |
            (?=[\w-.]{1,255}(?![\w-.]))
            (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
        )
        (?=/|$)
        |
        /(?=[a-z]:)
    )
)?
(?<path>
    (
        (?<root>[a-zA-Z]):(/|(?=$))
    |
        /
    |
        (?![a-zA-Z]:)
    )
    (
        ([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+
        (
            /
            ([!$&-)+-.=@[\]\w]+|%(2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689][\dA-F]))+
        )*
    )?
)
(?=/?$)
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:LinuxUriHostAndPathStrictRegex = [System.Text.RegularExpressions.Regex]::new(@'
^
(
    (?!file://)
|
    (?<file>
 
        file://
        (
            (?i)
            (?<host>
                (?=(\d+\.){3}\d+)
                (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
            |
                (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$))\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
            |
                (?=[\w-.]{1,255}(?![\w-.]))
                (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
            )
            (?=/|$)
            |
            (?=/)
        )
    )
)
(?<path>
    (?<root>/)?
    ([!$=&-.:;=@[\]\w]+|%(0[1-9A-F]|2[\dA-E]|[2-9A-F][\dA-F]))+
    (
        /
        ([!$=&-.:;=@[\]\w]+|%(0[1-9A-F]|2[\dA-E]|[2-9A-F][\dA-F]))+
    )*
)
(?=/?$)
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));
        
$Script:WindowsUriValidationRegex = [System.Text.RegularExpressions.Regex]::new(@'
(?<=^\s*)
(
    file://
    (?<root>
        (?=(\d+\.){3}\d+)
        ((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$(?=[/:?#]|$)(?=[/:?#]|$))\[?([a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        [a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
    |
        /*[a-z]:
    )
    (/|(?=\s*$))
|
    (?=[^:]*$)
)
(
    ([^\u0000-\u0019""<>|:;*?\\/%]+|%((?![A-F\d]{2})|2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689]))+
    (
        /+
        ([^\u0000-\u0019""<>|:;*?\\/%]+|%((?![A-F\d]{2})|2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689]))+
    )*
)?
(?=/*\s*$)
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:LinuxUriValidationRegex = [System.Text.RegularExpressions.Regex]::new(@'
(?<=^\s*)
(
    file://
    (?<root>
        (
            (?=(\d+\.){3}\d+)
            ((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4}
        |
            (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$))\[?([a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
        |
            (?=[\w-.]{1,255}(?![\w-.]))
            [a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
        )
        (/|(?=\s*$))
    |
        /
    |
        \s*$
    )
|
    (?!/|file:)
)
(
    ([^\u0000-\u0019/%]+|%((?![A-F\d]{2})|0[1-9A-F]|2[\dA-E]|[2-9A-F]))+
    (
        /+
        ([^\u0000-\u0019/%]+|%((?![A-F\d]{2})|0[1-9A-F]|2[\dA-E]|[2-9A-F]))+
    )*
)?
(?=/*\s*$)
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:WindowsFsHostAndPathRegex = [System.Text.RegularExpressions.Regex]::new(@'
^
(
    (//|\\\\)
    (?<host>
        (?=(\d+\.){3}\d+)
        (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$(?=[/:?#]|$)(?=[/:?#]|$)|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)
        \[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+|::)(\]|(?<unc>\.ipv6-literal\.net))?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
    )
    (?<path>
        [/\\]?(?=$)
    |
        (
            [/\\]
            [^\u0000-\u0019""<>|:;*?\\/]+
        )*
    )
|
    (?<path>
        ((?<root>[a-z]):(?=[/\\]|$))?
        (
            [^\u0000-\u0019""<>|:;*?\\/]+
            (
                [/\\]
                [^\u0000-\u0019""<>|:;*?\\/]+
            )*
        )?
    )
)
[\\/]?$
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:LinuxFsHostAndPathRegex = [System.Text.RegularExpressions.Regex]::new(@'
^
(
    //
    (?<host>
        (?=(\d+\.){3}\d+)
        (?<ipv4>((2(5[0-5|[0-4]?\d?)?|[01]?\d\d?)(\.|(?![.\d]))){4})
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$)|[a-f\d]*(-[a-f\d]*){2,7}\.ipv6-literal\.net$)
        \[?(?<ipv6>[a-f\d]{1,4}([:-][a-f\d]{1,4}){7}|(([a-f\d]{1,4}[:-])+|[:-])([:-][a-f\d]{1,4})+|::)(\]|(?<unc>\.ipv6-literal\.net))?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
    )
    (?<path>
        /(?=$)
    |
        (
            /
            [^\u0000/]+
        )*
    )
|
    (?<path>
        (?<root>/)?
        (
            [^\u0000/]+
            (
                /
                [^\u0000/]+
            )*
        )?
    )
)
/?$
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor `
        [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)));

$Script:AbsoluteUrlNodeOrder = @('Path', 'Query', 'Fragment', 'LocalPath', 'Authority', 'Translated', 'WellFormed');
$Script:RelativeUrlNodeOrder = @('Path', 'Query', 'Fragment', 'LocalPath', 'Authority', 'Translated', 'WellFormed');

Function Repair-LinuxAbsoluteUrlNodes {
    [CmdletBinding()]
    Param()
    
    $XmlNodeList = $Script:FilePathTestDataDocument.SelectNodes('//Linux/AbsoluteUrl');
    $TotalCount = $XmlNodeList.Count;
    $ItemNumber = 0;
    $PercentComplete = -1;
    foreach ($AbsoluteUrlElement in @($XmlNodeList)) {
        [int]$Pct = ($number++ * 100) / $TotalCount;
        if ($Pct -ne $PercentComplete) {
            $PercentComplete = $Pct;
            Write-Progress -Activity 'Fix Linux AbsoluteUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete $PercentComplete;
        }

    }
    Write-Progress -Activity 'Fix Linux AbsoluteUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete 100 -Completed;
}

Function Repair-LinuxRelativeUrlNodes {
    [CmdletBinding()]
    Param()
    
    $XmlNodeList = $Script:FilePathTestDataDocument.SelectNodes('//Linux/RelativeUrl');
    $TotalCount = $XmlNodeList.Count;
    $ItemNumber = 0;
    $PercentComplete = -1;
    foreach ($AbsoluteUrlElement in @($XmlNodeList)) {
        [int]$Pct = ($number++ * 100) / $TotalCount;
        if ($Pct -ne $PercentComplete) {
            $PercentComplete = $Pct;
            Write-Progress -Activity 'Fix Linux RelativeUrl elements' -Status "RelativeUrl $ItemNumber of $TotalCount" -PercentComplete $PercentComplete;
        }

    }
    Write-Progress -Activity 'Fix Linux RelativeUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete 100 -Completed;
}

Function Repair-LinuxFileSystemNodes {
    [CmdletBinding()]
    Param()
    
    $XmlNodeList = $Script:FilePathTestDataDocument.SelectNodes('//Linux/FileSystem');
    $TotalCount = $XmlNodeList.Count;
    $ItemNumber = 0;
    $PercentComplete = -1;
    foreach ($AbsoluteUrlElement in @($XmlNodeList)) {
        [int]$Pct = ($number++ * 100) / $TotalCount;
        if ($Pct -ne $PercentComplete) {
            $PercentComplete = $Pct;
            Write-Progress -Activity 'Fix Linux RelativeUrl elements' -Status "RelativeUrl $ItemNumber of $TotalCount" -PercentComplete $PercentComplete;
        }

    }
    Write-Progress -Activity 'Fix Linux RelativeUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete 100 -Completed;
}

Function Repair-WindowsAbsoluteUrlNodes {
    [CmdletBinding()]
    Param()
    
    $XmlNodeList = $Script:FilePathTestDataDocument.SelectNodes('//Windows/AbsoluteUrl');
    $TotalCount = $XmlNodeList.Count;
    $ItemNumber = 0;
    $PercentComplete = -1;
    foreach ($AbsoluteUrlElement in @($XmlNodeList)) {
        [int]$Pct = ($number++ * 100) / $TotalCount;
        if ($Pct -ne $PercentComplete) {
            $PercentComplete = $Pct;
            Write-Progress -Activity 'Fix Windows AbsoluteUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete $PercentComplete;
        }

    }
    Write-Progress -Activity 'Fix Windows AbsoluteUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete 100 -Completed;
}

Function Repair-WindowsRelativeUrlNodes {
    [CmdletBinding()]
    Param()
    
    $XmlNodeList = $Script:FilePathTestDataDocument.SelectNodes('//Windows/RelativeUrl');
    $TotalCount = $XmlNodeList.Count;
    $ItemNumber = 0;
    $PercentComplete = -1;
    foreach ($AbsoluteUrlElement in @($XmlNodeList)) {
        [int]$Pct = ($number++ * 100) / $TotalCount;
        if ($Pct -ne $PercentComplete) {
            $PercentComplete = $Pct;
            Write-Progress -Activity 'Fix Windows RelativeUrl elements' -Status "RelativeUrl $ItemNumber of $TotalCount" -PercentComplete $PercentComplete;
        }

    }
    Write-Progress -Activity 'Fix Windows RelativeUrl elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete 100 -Completed;
}

Function Repair-WindowsFileSystemNodes {
    [CmdletBinding()]
    Param()
    
    $XmlNodeList = $Script:FilePathTestDataDocument.SelectNodes('//Windows/FileSystem');
    $TotalCount = $XmlNodeList.Count;
    $ItemNumber = 0;
    $PercentComplete = -1;
    foreach ($AbsoluteUrlElement in @($XmlNodeList)) {
        [int]$Pct = ($number++ * 100) / $TotalCount;
        if ($Pct -ne $PercentComplete) {
            $PercentComplete = $Pct;
            Write-Progress -Activity 'Fix Windows FileSystem elements' -Status "RelativeUrl $ItemNumber of $TotalCount" -PercentComplete $PercentComplete;
        }

    }
    Write-Progress -Activity 'Fix Windows FileSystem elements' -Status "AbsoluteUrl $ItemNumber of $TotalCount" -PercentComplete 100 -Completed;
}

Function Test-UriHostAndPathStrictRegex {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateSet("Windows", "Linux")]
        [string]$Target,

        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.Xml.XmlElement]$TestDataElement
    )
    
    $InputString = '' + $TestDataElement.InputString;
    $Match = $null;
    if ($Target -eq 'Windows') {
        $Match = $Script:WindowUriHostAndPathStrictRegex.Match($InputString);
    } else {
        $Match = $Script:LinuxUriHostAndPathStrictRegex.Match($InputString);
    }
    if ($Match.Success) {
        if ($Match.Groups['file'].Success) {
            $Uri = $null;
            if (-not [Uri]::TryCreate($InputString, [UriKind]::Absolute, [ref]$Uri)) {
                $Uri = $null;
                Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a well-formed absolute URL for '$InputString', it was not able to be parsed as a NET Uri";
            }
            $AbsoluteUrlElement = $TestDataElement.SelectSingleNode("$Target/AbsoluteUrl");
            if ($null -eq $AbsoluteUrlElement) {
                Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a well-formed absolute URL for '$InputString', but there is no AbsoluteUrl element";
            } else {
                $m = '' + $AbsoluteUrlElement.Match;
                if ($m -cne $Match.Value) {
                    Write-Warning -Message "Unexpected $Target/AbsoluteUrl/@Match value. Expected:`n'$($Match.Value)'`n:'$m'";
                }
                $m = '' + $AbsoluteUrlElement.IsWellFormed;
                if ($m -cne 'true') {
                    Write-Warning -Message "Unexpected $Target/AbsoluteUrl/@IsWellFormed value. Expected: 'true'; Actual: '$m'";
                }
                $AuthorityElement = $AbsoluteUrlElement.SelectSingleNode('Authority');
                if ($null -eq $AuthorityElement) {
                    Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a well-formed absolute URL for '$InputString', but there is no Authority element";
                } else {
                    $SchemeElement = $AuthorityElement.SelectSingleNode('Scheme');
                    if ($null -eq $SchemeElement) {
                        Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a well-formed absolute URL for '$InputString', but there is no Scheme element";
                    } else {
                        $m = '' + $SchemeElement.Name;
                        if ($m -cne 'file') {
                            Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Authority/Scheme/@Name value. Expected: 'file'; Actual: '$m'";
                        }
                        $m = '' + $SchemeElement.Delimiter;
                        if ($m -cne '://') {
                            Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Authority/Scheme/@Delimiter value. Expected: '://'; Actual: '$m'";
                        }
                    }
                    $HostElement = $AuthorityElement.SelectSingleNode('Host');
                    if ($Match.Groups['host'].Success) {
                        if ($null -ne $Uri) {
                            if ([string]::IsNullOrEmpty($Uri.Host)) {
                                Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a host name for '$InputString', but System.Uri did not";
                            } else {
                                if ($Match.Groups['host'].Value -cne $Uri.Host) {
                                    Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted '$($Match.Groups['host'].Value)' as the host name for '$InputString', but System.Uri detected '$($Uri.Host)'";
                                }
                            }
                        }
                        if ($null -eq $HostElement) {
                            Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a host name for '$InputString', but there is no Host element";
                        } else {
                            $t = '' + $HostElement.Type;
                            if ($Match.Groups['ipv4'].Success) {
                                if ($t -cne 'IPV4') {
                                    Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Authority/Scheme/Host/@Type value. Expected: 'IPV4'; Actual: '$t'";
                                }
                            } else {
                                if ($Match.Groups['ipv6'].Success) {
                                    if ($t -cne 'IPV6') {
                                        Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Authority/Scheme/Host/@Type value. Expected: 'IPV6'; Actual: '$t'";
                                    }
                                } else {
                                    if ($t -cne 'DNS') {
                                        Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Authority/Scheme/Host/@Type value. Expected: 'DNS'; Actual: '$t'";
                                    }
                                }
                            }
                        }
                    } else {
                        if ($null -ne $HostElement) {
                            Write-Warning -Message "$($Target)UriHostAndPathStrictRegex did not detect a host name for '$InputString', but there is a Host element";
                        }
                    }
                }
            }
        }
        $PathElement = $AbsoluteUrlElement.SelectSingleNode('Path');
        if ($null -eq $PathElement) {
            Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a file URL for '$InputString', but there is no Path element";
        } else {
            $LocalPathElement = $AbsoluteUrlElement.SelectSingleNode('LocalPath');
            if ($null -eq $LocalPathElement) {
                Write-Warning -Message "$($Target)UriHostAndPathStrictRegex deteted a file URL for '$InputString', but there is no LocalPathElement element";
            }
            $PathString = '';
            if ($Match.Groups['path'].Success) {
                $PathString = $Match.Groups['path'].Value;
                if ($null -ne $LocalPathElement) {
                    $m = '' + $LocalPathElement.IsAbsolute;
                    if ($Match.Groups['root'].Success) {
                        if ($m -cne 'true') {
                            Write-Warning -Message "Unexpected $Target/AbsoluteUrl/LocalPath/@IsAbsolute value. Expected: 'true'; Actual: '$m'";
                        }
                    } else {
                        if ($m -cne 'true') {
                            Write-Warning -Message "Unexpected $Target/AbsoluteUrl/LocalPath/@IsAbsolute value. Expected: 'false'; Actual: '$m'";
                        }
                    }
                }
            }
            $Directory = $PathString;
            $FileName = '';
            if ($PathString -match '(^|(?<=[^/])/)?([^/]+)/?$') {
                $len = $Matches[0].Length;
                if ($len -eq $PathString.Length) {
                    $FileName = $PathString;
                    $Directory = '';
                } else {
                    $FileName = $Matches[2];
                    $Directory = $PathString.Substring(0, $PathString.Length - $len);
                }
            }
            $e = $PathElement.SelectSingleNode('Directory');
            if ($null -eq $e) {
                Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Path exists but does not have a Directory attribute";
            } else {
                if ($e.IsEmpty) {
                    Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Path/Directory value. Expected: '$Directory' but element was nil";
                } else {
                    if ($e.InnerText -cne $Directory) {
                        Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Path/Directory value. Expected: '$Directory' but element was '$($e.InnerText)'";
                    }
                }
            }
            $e = $PathElement.SelectSingleNode('FileName');
            if ($null -eq $e) {
                Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Path exists but does not have a FileName attribute";
            } else {
                if ($e.IsEmpty) {
                    if ($FileName.Length -gt 0) {
                        Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Path/FileName value. Expected: '$FileName' but element was nil";
                    }
                } else {
                    if ($e.InnerText -cne $FileName) {
                        Write-Warning -Message "Unexpected $Target/AbsoluteUrl/Path/FileName value. Expected: '$FileName' but element was '$($e.InnerText)'";
                    }
                }
            }
        }
    }
}

<#
foreach ($TestDataElement in @($Script:FilePathTestDataDocument.SelectNodes('//TestData'))) {
    Test-UriHostAndPathStrictRegex -Target Linux -TestDataElement $TestDataElement;
}

Repair-LinuxAbsoluteUrlNodes -ErrorAction Stop -InformationAction Continue;
Repair-LinuxRelativeUrlNodes -ErrorAction Stop -InformationAction Continue;
Repair-WindowsAbsoluteUrlNodes -ErrorAction Stop -InformationAction Continue;
Repair-WindowsRelativeUrlNodes -ErrorAction Stop -InformationAction Continue;
Save-FilePathTestData -ErrorAction Stop -InformationAction Continue;
$Match = $Script:WindowsFormatDetectionRegex.Match('file:///')
#>

($PSScriptRoot | Join-Path -ChildPath '../../src/FsInfoCat.Test') | Resolve-Path