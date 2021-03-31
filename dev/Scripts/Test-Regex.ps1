$URI_HOST_AND_PATH_STRICT_REGEX = [System.Text.RegularExpressions.Regex]::new(@'
^
(
    (?![^:/]*:)
|
    (?<file>
        file://
        (
            (?i)
            (?<host>
                (?<ipv4>((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.))
            |
                (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}(?=[/:?#]|$))\[?(?<ipv6>[a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
            |
                (?=[\w-.]{1,255}(?![\w-.]))
                (?<dns>[a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?)
            )
            (?=/|$)
        |
            /(?=[a-z]:)
        |
            (?=/|$)
        )
    )
)
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
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)))

$URI_VALIDATION_REGEX = [System.Text.RegularExpressions.Regex]::new(@'
(?<=^\s*)
(
    (?<file>file)://
    (?<root>
        ((?<!\d)(0(?=\d))*(?!25[6-9]|([3-9]\d|1\d\d)\d)\d{1,3}\.?){4}(?<!\.)
    |
        (?=\[[a-f\d]*(:[a-f\d]*){2,7}\]|[a-f\d]*(:[a-f\d]*){2,7}$(?=[/:?#]|$)(?=[/:?#]|$))\[?([a-f\d]{1,4}(:[a-f\d]{1,4}){7}|(([a-f\d]{1,4}:)+|:)(:[a-f\d]{1,4})+|::)\]?
    |
        (?=[\w-.]{1,255}(?![\w-.]))
        [a-z\d][\w-]*(\.[a-z\d][\w-]*)*\.?
    |
        /+[a-z]:
    )?
    (?=/+|\s*$)
|
    (?!file:)
)
(?<p>
([^\u0000-\u0019""<>|:;*?\\%]+|%((?![A-F\d]{2})|2[013-9B-E]|3[\dD]|[57][\dABDEF]|[4689]))*
)(?=/*\s*$)
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]::Compiled -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)))

$ABS_URI_STRING_NORMALIZE_REGEX = [System.Text.RegularExpressions.Regex]::new(@'

^
    \s*
    (
        (?<scheme>(?!file:)(?i)FILE(?=:))
    |
        (\.\.?/+)+
    |
        \s+
    )
|
    (?<esc>
        (
            %
            (
                a-f[\dA-Fa-f]
            |
                [\dA-F][a-f]
            )
        )+
    )
|
    (?<=file:///)
    /+
|
    (?<!file:/*)
    (/(?=/))+
|
    /\.(?=/|$)
|
    (/\s*|\s+)
$
'@, ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor [System.Text.RegularExpressions.RegexOptions]::IgnorePatternWhitespace)))


$InputText = "\"
$Match = $URI_HOST_AND_PATH_STRICT_REGEX.Match($InputText);
"URI_HOST_AND_PATH_STRICT_REGEX: $($Match.Success)";
if ($Match.Success) {
    "`t: $($Match.Value)";
    @('file', 'host', 'ipv4', 'ipv6', 'dns', 'path', 'root') | ForEach-Object {
        $g = $Match.Groups[$_];
        "`t$_`: $($g.Success)";
        if ($g.Success) { "`t`t$($g.Value)" }
    }
}

$Match = $URI_VALIDATION_REGEX.Match($InputText);
"URI_VALIDATION_REGEX: $($Match.Success)";
if ($Match.Success) {
    "`t$($Match.Value)";
    $g = $Match.Groups['root'];
    "`troot: $($g.Success)";
    if ($g.Success) { "`t`t$($g.Value)" }
    $g = $Match.Groups['p'];
    "`tp: $($g.Success)";
    if ($g.Success) { "`t`t$($g.Value)" }
}

"'$($ABS_URI_STRING_NORMALIZE_REGEX.Replace($InputText, ''))'"