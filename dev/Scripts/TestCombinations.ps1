@(&{
    $SchemeList = @(
        [Uri]::UriSchemeFile,
        [Uri]::UriSchemeFtp,
        [Uri]::UriSchemeGopher,
        [Uri]::UriSchemeHttp,
        [Uri]::UriSchemeHttps,
        [Uri]::UriSchemeMailto,
        [Uri]::UriSchemeNews,
        [Uri]::UriSchemeNntp,
        [Uri]::UriSchemeNetTcp,
        [Uri]::UriSchemeNetPipe
        'urn'
    );
    $HostList = @($null, '', 'uuid', 'tempuri.org', 'mysite:8080', '[fe80::1dee:91b0:4872:1f9]');
    $UserInfoList = @($null, '', 'my%20User', 'myUser:myPw');
    $PathList = @('', '/', '/C:/Apps/7CVVR', ':5ec182f9-044f-40c8-b5f0-a45e78cb7fb2', '/test/file.html');
    $QueryList = @('', '?', '?My=Data');
    $FragmentList = @('', '#', '#myFragment');
    [UriComponents]$BaseComponents = ([UriComponents]::Scheme -bor [UriComponents]::UserInfo -bor [UriComponents]::Port -bor [UriComponents]::Host)

    foreach ($s in $SchemeList) {
        foreach ($h in $HostList) {
            foreach ($U in $UserInfoList) {
                foreach ($Path in $PathList) { foreach ($qry in $QueryList) { foreach ($frg in $FragmentList) {
                    $Data = [PsCustomObject]@{ Scheme = $s }
                    $ub = [UriBuilder]::new();
                    $Expected = @{ Scheme = $s };
                    $ub.Scheme = $s;
                    $HostValue = $h;
                    $HasError = $false;
                    if ($null -eq $h) {
                        $HostValue = '(not set)';
                    } else {
                        if ($h.Length -eq 0) { $HostValue = '(empty)'; }
                        try {
                            ($hn, $pn) = $h -split ':';
                            $ub.Host = $hn;
                            $Expected['Host'] = $hn;
                            if ($null -ne $pn) {
                                $ub.Port = [int]::Parse($pn);
                                $Expected['Port'] = $pn;
                            }
                        } catch {
                            $HasError = $true;
                            $HostValue = "$HostValue`nError: $_";
                        }
                    }
                    $Data | Add-Member -MemberType NoteProperty -Name 'Host' -Value $HostValue;
                    
                    $UserNameValue = $u;
                    if ($HasError) {
                        $UserNameValue = "$u`n(not applied)";
                    } else {
                        if ($null -eq $u) {
                            $UserNameValue = '(not set)';
                        } else {
                            if ($u.Length -eq 0) { $UserNameValue = '(empty)'; }
                            try {
                                $Expected['UserInfo'] = $u;
                                ($UserName, $pwd) = $u -split ':';
                                $ub.UserName = $UserName;
                                if ($null -ne $pwd) {
                                    $ub.Password = $pwd;
                                }
                            } catch {
                                $HasError = $true;
                                $UserNameValue = "$UserNameValue`nError: $_";
                            }
                        }
                    }
                    $Data | Add-Member -MemberType NoteProperty -Name 'UserInfo' -Value $UserNameValue;

                    if ($HasError) {
                        $Data | Add-Member -MemberType NoteProperty -Name 'Path' -Value '(not applied)';
                        $Data | Add-Member -MemberType NoteProperty -Name 'Query' -Value '(not applied)';
                        $Data | Add-Member -MemberType NoteProperty -Name 'Fragment' -Value '(not applied)';
                        $Data | Add-Member -MemberType NoteProperty -Name 'Validation' -Value '(not calculated)';
                        $Data | Add-Member -MemberType NoteProperty -Name 'AbsoluteUri' -Value '(not calculated)';
                    } else {
                        try {
                            $Uri = $ub.Uri;
                            if ($null -eq $Uri) {
                                $HasError = $false;
                                $Data | Add-Member -MemberType NoteProperty -Name 'Path' -Value '(not applied)';
                                $Data | Add-Member -MemberType NoteProperty -Name 'Query' -Value '(not applied)';
                                $Data | Add-Member -MemberType NoteProperty -Name 'Fragment' -Value '(not applied)';
                                $Data | Add-Member -MemberType NoteProperty -Name 'Validation' -Value '(not calculated)';
                                $Data | Add-Member -MemberType NoteProperty -Name 'AbsoluteUri' -Value '(Failed to build URI)';
                            } else {
                                $AbsoluteUri = $Uri.GetComponents($BaseComponents, [UriFormat]::UriEscaped);
                                if ($Path.Length -eq 0) {
                                    $Data | Add-Member -MemberType NoteProperty -Name 'Path' -Value $Path;
                                } else {
                                    $pp = $Path;
                                    $exp = $Path;
                                    if (-not $pp.StartsWith('/')) {
                                        if ($s -eq 'urn') {
                                            $exp = ":$Path";
                                        } else {
                                            $pp = $exp = "/$Path";
                                        }
                                    }
                                    try {
                                        $AbsoluteUri = "$AbsoluteUri$pp";
                                        $Uri = [Uri]::new($AbsoluteUri, [UriKind]::Absolute);
                                        $Expected['AbsolutePath'] = $exp;
                                        $Data | Add-Member -MemberType NoteProperty -Name 'Path' -Value $Path;
                                    } catch {
                                        $HasError = $true;
                                        $Data | Add-Member -MemberType NoteProperty -Name 'Path' -Value "$Path`n$_";
                                    }
                                }
                                            
                                $QueryValue = $qry;
                                if ($HasError) {
                                    $QueryValue = "$qry`n(not applied)";
                                } else {
                                    if ($null -eq $qry) {
                                        $QueryValue = '(not set)';
                                    } else {
                                        if ($qry.Length -eq 0) { $QueryValue = '(empty)'; }
                                        try {
                                            $AbsoluteUri = "$AbsoluteUri$qry";
                                            $Uri = [Uri]::new($AbsoluteUri, [UriKind]::Absolute);
                                            $Expected['Query'] = $qry;
                                        } catch {
                                            $HasError = $true;
                                            $QueryValue = "$QueryValue`nError: $_";
                                        }
                                    }
                                }
                                $Data | Add-Member -MemberType NoteProperty -Name 'Query' -Value $QueryValue;
                                            
                                $FragmentValue = $frg;
                                if ($HasError) {
                                    $FragmentValue = "$frg`n(not applied)";
                                } else {
                                    if ($null -eq $frg) {
                                        $FragmentValue = '(not set)';
                                    } else {
                                        if ($frg.Length -eq 0) { $FragmentValue = '(empty)'; }
                                        try {
                                            $AbsoluteUri = "$AbsoluteUri$frg";
                                            $Uri = [Uri]::new($AbsoluteUri, [UriKind]::Absolute);
                                            $Expected['Fragment'] = $frg;
                                        } catch {
                                            $HasError = $true;
                                            $FragmentValue = "$FragmentValue`nError: $_";
                                        }
                                    }
                                }

                                $Data | Add-Member -MemberType NoteProperty -Name 'Fragment' -Value $FragmentValue;
                                if ($HasError) {
                                    $Data | Add-Member -MemberType NoteProperty -Name 'AbsoluteUri' -Value '(not calculated)';
                                    $Data | Add-Member -MemberType NoteProperty -Name 'Validation' -Value '(not calculated)';
                                } else {
                                    $Data | Add-Member -MemberType NoteProperty -Name 'AbsoluteUri' -Value $Uri.AbsoluteUri;
                                    $Validation = @();
                                    if ($Expected.ContainsKey('Scheme') -and $Expected['Scheme'] -ne $Uri.Scheme) {
                                        $Validation += @("Scheme => Expected: '$($Expected['Scheme'])'; Actual: '$($Uri.Scheme)'");
                                    }
                                    if ($Expected.ContainsKey('UserInfo') -and $Expected['UserInfo'] -ne $Uri.UserInfo) {
                                        $Validation += @("UserInfo => Expected: '$($Expected['UserInfo'])'; Actual: '$($Uri.UserInfo)'");
                                    }
                                    if ($Expected.ContainsKey('Host')) {
                                        $hp = $Uri.Host;
                                        if (-not $Uri.IsDefaultPort) { $hp = "$hp`:$($Uri.Port)" }
                                        if ($hp -ne $Expected['Host']) {
                                            $Validation += @("Scheme => Expected: '$($Expected['Scheme'])'; Actual: '$($Uri.Scheme)'");
                                        }
                                    }
                                    if ($Expected.ContainsKey('AbsolutePath') -and $Expected['AbsolutePath'] -ne $Uri.AbsolutePath) {
                                        $Validation += @("AbsolutePath => Expected: '$($Expected['AbsolutePath'])'; Actual: '$($Uri.AbsolutePath)'");
                                    }
                                    if ($Expected.ContainsKey('Query') -and $Expected['Query'] -ne $Uri.Query) {
                                        $Validation += @("Query => Expected: '$($Expected['Query'])'; Actual: '$($Uri.Query)'");
                                    }
                                    if ($Expected.ContainsKey('Fragment') -and $Expected['Fragment'] -ne $Uri.Fragment) {
                                        $Validation += @("Fragment => Expected: '$($Expected['Fragment'])'; Actual: '$($Uri.Fragment)'");
                                    }
                                    $Data | Add-Member -MemberType NoteProperty -Name 'Validation' -Value ($Validation -join "`n");
                                }
                            }
                        } catch {
                            $HasError = $true;
                            $Data | Add-Member -MemberType NoteProperty -Name 'Path' -Value '(not applied)';
                            $Data | Add-Member -MemberType NoteProperty -Name 'Query' -Value '(not applied)';
                            $Data | Add-Member -MemberType NoteProperty -Name 'Fragment' -Value '(not applied)';
                            $Data | Add-Member -MemberType NoteProperty -Name 'Validation' -Value '(not calculated)';
                            $Data | Add-Member -MemberType NoteProperty -Name 'AbsoluteUri' -Value ('' + $_);
                        }
                    }
                    $Data | Add-Member -MemberType NoteProperty -Name 'HasError' -Value $HasError -PassThru;
                } } }
            }
        }
    }
}) | Out-GridView -Title 'Combinations';