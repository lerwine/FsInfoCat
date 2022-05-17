<#
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
#>
$Pattern = @'
(^|\G)(?<n>[^<>,\s]+)
(
    <
    (
        (?<d>(?<a>[\s,]+)?>)
    |
        (?<a>
            [^<>]+
            (
                (?>
                    (?>
                        (?<o><)
                        [^<>]*
                    )+
                    (?>
                        (?<-o>>)
                        [^<>]*
                    )+
                )+
                (?(o)(?!))
            )?
        )
        >
    )
)?
\s*(,\s*)?
'@ -replace '[\s\r\n]+', '';

#$Regex = [System.Text.RegularExpressions.Regex]::new('^[^<>]*(?>(?>(?<open><)[^<>]*)+(?>(?<-open>>)[^<>]*)+)+(?(open)(?!))', [System.Text.RegularExpressions.RegexOptions]::Compiled);
$Pattern
$Script:NestedAngleBracketRegex = [System.Text.RegularExpressions.Regex]::new($Pattern, [System.Text.RegularExpressions.RegexOptions]::Compiled);

Function Add-SeeAlsoComponents {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Text,
        
        [Parameter(Mandatory = $true)]
        [Xml]$Xml
    )

    Process {
        $MatchCollection = $Script:NestedAngleBracketRegex.Matches($Text);
        foreach ($Match in $MatchCollection) {
            if ($Match.Groups['d'].Success) {
                if ($Match.Groups['a'].Success) {
                    $Xml.DocumentElement.AppendChild($Xml.CreateElement('seealso')).Attributes.Append($Xml.CreateAttribute('cref')).Value = "$($Match.Groups['n'].Value){}";
                } else {
                    $Xml.DocumentElement.AppendChild($Xml.CreateElement('seealso')).Attributes.Append($Xml.CreateAttribute('cref')).Value = "$($Match.Groups['n'].Value){$($Match.Groups['a'].Value)}";
                }
            } else {
                if ($Match.Groups['a'].Success) {
                    $ArgCount = Add-SeeAlsoComponents -Text $Match.Groups['a'].Value.Trim() -Xml $Xml;
                    if ($ArgCount -lt 2) {
                        $Xml.DocumentElement.AppendChild($Xml.CreateElement('seealso')).Attributes.Append($Xml.CreateAttribute('cref')).Value = "$($Match.Groups['n'].Value){T}";
                    } else {
                        $Cref = [System.Text.StringBuilder]::new($Match.Groups['n'].Value).Append('{T1');
                        for ($i = 2; $i -le $ArgCount; $i++) { $Cref.Append(', T').Append($i) | Out-Null }
                        $Xml.DocumentElement.AppendChild($Xml.CreateElement('seealso')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $Cref.Append('}').ToString();
                    }
                } else {
                    $Xml.DocumentElement.AppendChild($Xml.CreateElement('seealso')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $Match.Groups['n'].Value;
                }
            }
        }
        $MatchCollection.Count | Write-Output;
    }
}

$Text = [System.Windows.Clipboard]::GetText();
[Xml]$Xml = '<doc />';
Add-SeeAlsoComponents -Text $Text -Xml $Xml;
[System.Windows.Clipboard]::SetText(((@('') + @($Xml.DocumentElement.SelectNodes('*') | % { "    /// $($_.OuterXml)" })) | Out-String).TrimEnd());