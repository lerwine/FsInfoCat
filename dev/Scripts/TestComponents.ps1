$Uri = [Uri]::new('https://User%23%3F%2F%3A%40%25Name:pass%23%3F%2F%3A%40%25word@host/My%20%23%3F%2F%3A%40%25Path/?q=v#tag#?/:@%aha', [UriKind]::Absolute);


[UriComponents[]]$Components = @(
    [UriComponents]::Scheme,
    [UriComponents]::UserInfo,
    [UriComponents]::Host,
    [UriComponents]::NormalizedHost,
    [UriComponents]::Port,
    [UriComponents]::StrongPort,
    [UriComponents]::HostAndPort,
    [UriComponents]::SchemeAndServer,
    [UriComponents]::StrongAuthority,
    [UriComponents]::Path,
    [UriComponents]::Query,
    [UriComponents]::PathAndQuery,
    [UriComponents]::Fragment,
    [UriComponents]::AbsoluteUri,
    [UriComponents]::HttpRequestUrl,
    [UriComponents]::SerializationInfoString
);
$UriFormat = [UriFormat]::UriEscaped;
$KeepDelimiter = $true;
$Components | ForEach-Object {
    [UriComponents]$c = $_;
    if ($KeepDelimiter) { [UriComponents]$c = $_ -bor [UriComponents]::KeepDelimiter; }
    $e = '';
    $v = '';
    try { $v = $Uri.GetComponents($c, $UriFormat) }
    catch { $e = '' + $_ }
    [PsCustomObject]@{
        Component = $_.ToString('F');
        Value = $v;
        Error = $e;
    }
} | Out-GridView -Title 'Components'

[UriComponents]$cv = ([UriComponents]::Scheme -bor [UriComponents]::UserInfo -bor [UriComponents]::Port -bor [UriComponents]::Host)
$Uri.GetComponents($cv, $UriFormat)