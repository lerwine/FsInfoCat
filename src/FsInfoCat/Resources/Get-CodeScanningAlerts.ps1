Function Get-CodeScanningAlerts {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Repository,
        [Parameter(Mandatory = $true)]
        [PSCredential]$AccessToken,
        [string]$Owner
    )

    if ([string]::IsNullOrEmpty($Owner)) { $Owner = $Credentials.UserName }
    
    $Page = 1;
    
    $BaseWebUri = "https://github.com/$([Uri]::EscapeDataString($Owner))/$([Uri]::EscapeDataString($Repository))/blob/"
    $BaseApiUri = "https://api.github.com/repos/$([Uri]::EscapeDataString($Owner))/$([Uri]::EscapeDataString($Repository))/code-scanning/alerts?state=open&per_page=100&page=";
    $Uri = "$BaseApiUri$Page";
    Write-Progress -Activity 'Getting Code Scan Alerts' -Status "Getting page $Page" -CurrentOperation $Uri -Id 1;
    $Result = Invoke-WebRequest -Uri $Uri -Method Get -Headers @{
        Authorization = "token $($Credentials.GetNetworkCredential().Password)";
        Accept = 'application/sarif+json';
    };
    while ($null -ne $Result -and $null -ne $Result.Content) {
        $Alerts = $Result.Content | ConvertFrom-Json;
        if ($null -eq $Alerts -or $Alerts.Count -eq 0) { break }
        $Alerts | ForEach-Object {
            $Instance = $_.most_recent_instance;
            if ($null -ne $Instance.message) {
                $Properties = @{};
                ($Instance.message | Get-Member -MemberType Properties) | ForEach-Object {
                    $Properties[$_.Name] = $Instance.message.($_.Name);
                }
                if ($Properties.Count -eq 1) {
                    $_ | Add-Member -MemberType NoteProperty -Name 'message' -Value "$($Properties[($Properties.Keys | Select-Object -First 1)])".Trim();
                } else {
                    if ($Properties.Count -gt 1) {
                        $_ | Add-Member -MemberType NoteProperty -Name 'message' -Value ($Properties.Keys | Select-Object { "$_`: $($Properties[$_])" } | Out-String).Trim();
                    } else {
                        $_ | Add-Member -MemberType NoteProperty -Name 'message' -Value "$($Instance.message)".Trim();
                    }
                }
            } else {
                $_ | Add-Member -MemberType NoteProperty -Name 'message' -Value '';
            }
            
            $_ | Add-Member -MemberType NoteProperty -Name 'source_url' -Value "$BaseWebUri$($Instance.commit_sha)/$($Instance.location.path)#L$($Instance.location.start_line)" -PassThru;
        } | Write-Output;
        if ($Alerts.Count -lt 100) { break }
        $Page++;
        $Uri = "$BaseApiUri$Page";
        Write-Progress -Activity 'Getting Code Scan Alerts' -Status "Getting page $Page" -CurrentOperation $Uri -Id 1;
        $Result = Invoke-WebRequest -Uri $Uri -Method Get -Headers @{
            Authorization = "token $($Credentials.GetNetworkCredential().Password)";
            Accept = 'application/sarif+json';
        };
    }
    Write-Progress -Activity 'Getting Code Scan Alerts' -Status "$Page pages retrieved" -Id 1 -Completed;
}

$AccessToken = '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';
while ($null -eq $Credentials) {
    $Credentials = Get-Credential -Message 'Enter personal access token' -UserName 'lerwine';
}
((Get-CodeScanningAlerts -Repository 'FsInfoCat' -AccessToken $Credentials) | Group-Object -Property @{ Expression = { $_.rule.id } }) | ForEach-Object {
    "" | Write-Output;
    "----------------" | Write-Output;
    "" | Write-Output;
    $Id = $_.Name;
    "$Id; $($_.Group[0].rule.severity): Fix code scanning alert - $($_.Group[0].rule.description)" | Write-Output;
    $MessageGrouped = @($_.Group | Group-Object -Property 'message');
    if ($Id -eq 'cs/compilation-message') { $MessageGrouped = @($MessageGrouped | Where-Object { -not $_.Name.StartsWith('Warning CS0618') }) }
    if ($MessageGrouped.Count -gt 1) {
        $Alerts = @($MessageGrouped | Where-Object { $_.Group.Count -eq 1 -or [string]::IsNullOrWhiteSpace($_.Name) } | ForEach-Object { $_.Group | Write-Output });
        if ($Alerts.Count -gt 0) {
            "Tracking issue for:" | Write-Output;
            ($Alerts | Sort-Object -Property 'number') | ForEach-Object {
                "- [ ] $($_.html_url)" | Write-Output;
                "      $($_.source_url)" | Write-Output;
                if (-not [string]::IsNullOrWhiteSpace($_.message)) {
                    ($_.message -split '\r\n?|\n') | Where-Object { -not [string]::IsNullOrWhiteSpace($_) } | ForEach-Object {
                        "" | Write-Output;
                        "      $_".TrimEnd() | Write-Output;
                    }
                }
            }
        }
        $MessageGrouped = @($MessageGrouped | Where-Object { $_.Group.Count -gt 1 -and -not [string]::IsNullOrWhiteSpace($_.Name) });
        if ($MessageGrouped.Count -gt 0) {
            if ($Alerts.Count -gt 0) { "" | Write-Output }
            $MessageGrouped[0].Name.Trim() | Write-Output;
            "" | Write-Output;
            "Tracking issue for:" | Write-Output;
            ($MessageGrouped[0].Group | Sort-Object -Property 'number') | ForEach-Object {
                "- [ ] $($_.html_url)" | Write-Output;
                "      $($_.source_url)" | Write-Output;
            }
            if ($MessageGrouped.Count -gt 1) {
                ($MessageGrouped | Select-Object -Skip 1) | ForEach-Object {
                    "" | Write-Output;
                    $_.Name.Trim() | Write-Output;
                    "" | Write-Output;
                    "Tracking issue for:" | Write-Output;
                    ($_.Group | Sort-Object -Property 'number') | ForEach-Object {
                        "- [ ] $($_.html_url)" | Write-Output;
                        "      $($_.source_url)" | Write-Output;
                    }
                }
            }
        }
        
    } else {
        if ($MessageGrouped.Count -eq 1) {
            if (-not [string]::IsNullOrWhiteSpace($_.Group[0].message)) {
                $_.Group[0].message | Write-Output;
                "" | Write-Output;
            }
            "Tracking issue for:" | Write-Output;
            ($_.Group | Sort-Object -Property 'number') | ForEach-Object {
                "- [ ] $($_.html_url)" | Write-Output;
                "      $($_.source_url)" | Write-Output;
            }
        }
    }
} | Out-File -FilePath ($PSScriptRoot | Join-Path -ChildPath 'CodeScanResults.txt');
# nJK3v-rn$avaJript

