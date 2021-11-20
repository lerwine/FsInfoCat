Function Get-CodeScanningAlertsOld {
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

Function Convert-CodeScanningAlertsToText {
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
}

class CodeLocation {
    [string]$path;
    [int]$start_line;
    [int]$start_column;
    [int]$end_line;
    [int]$end_column;
    CodeLocation([PSCustomObject]$obj) {
        $this.path = $obj.path;
        $this.start_line = $obj.start_line;
        $this.start_column = $obj.start_column;
        $this.end_line = $obj.end_line;
        $this.end_column = $obj.end_column;
    }
}
class CodeScanResult {
    [string]$analysis_key;
    [string]$category;
    [string[]]$classifications;
    [string]$commit_sha;
    [string]$environment;
    [CodeLocation]$location;
    [string]$message;
    [string]$ref;
    [string]$state;
    CodeScanResult([PSCustomObject]$obj) {
        $this.analysis_key = $obj.analysis_key;
        $this.category = $obj.category;
        if ($null -eq $obj.classifications) { $this.classifications = ([string[]]@()) } else { $this.classifications = ([string[]]$obj.classifications) }
        $this.commit_sha = $obj.commit_sha;
        $this.environment = $obj.environment;
        if ($null -eq $obj.location) {
            $this.location = $null;
        } else {
            $this.location = [CodeLocation]::new($obj.location);
        }
        if ($null -eq $obj.message) {
            $this.message = $null;
        } else {
            if ($null -eq $obj.message.text) {
                $this.message = ($obj.message | Out-String).TrimEnd();
            } else {
                $this.message = '' + $obj.message.text;
            }
        }
        $this.ref = $obj.ref;
        $this.state = $obj.state;
    }
}
class CodeScanRule {
    [string]$id;
    [string]$severity;
    [string]$name;
    [string]$description;
    CodeScanRule([PSCustomObject]$obj) {
        $this.id = $obj.id;
        $this.severity = $obj.severity;
        $this.name = $obj.name;
        $this.description = $obj.description;
    }
}
class CodeScanTool {
    [string]$guid;
    [string]$name;
    [string]$version;
    CodeScanTool([PSCustomObject]$obj) {
        $this.guid = $obj.guid;
        $this.name = $obj.name;
        $this.version = $obj.version;
    }
}
class CodeScanningAlert {
    [string]$created_at;
    [string]$dismissed_at;
    [string]$dismissed_by;
    [string]$dismissed_reason;
    [string]$source_url;
    [string]$html_url;
    [string]$instances_url;
    [CodeScanResult]$most_recent_instance;
    [int]$number;
    [CodeScanRule]$rule;
    [string]$state;
    [CodeScanTool]$tool;
    [string]$url;
    CodeScanningAlert([PSCustomObject]$obj) {
        $UriBuilder = [UriBuilder]::new($obj.html_url)
        $this.created_at = $obj.created_at;
        $this.dismissed_at = $obj.dismissed_at;
        $this.dismissed_by = $obj.dismissed_by;
        $this.dismissed_reason = $obj.dismissed_reason;
        $this.html_url = $obj.html_url;
        $this.instances_url = $obj.instances_url;
        if ($null -eq $obj.most_recent_instance) {
            $this.most_recent_instance = $null;
            $this.source_url = '';
        } else {
            $this.most_recent_instance = [CodeScanResult]::new($obj.most_recent_instance);
            $Uri = $null;
            if ($null -ne $this.most_recent_instance.location -and (-not ([string]::IsNullOrWhiteSpace($this.most_recent_instance.commit_sha) -or [string]::IsNullOrWhiteSpace($this.most_recent_instance.location.path))) -and [Uri]::TryCreate(('' + $obj.html_url), [UriKind]::Absolute, [ref]$Uri)) {
                $UriBuilder = [UriBuilder]::new($Uri);
                $UriBuilder.Path = "$(-join ($Uri.Segments | Select-Object -First 3))blob/$($this.most_recent_instance.commit_sha)/$($this.most_recent_instance.location.path)";
                if ($null -eq $this.most_recent_instance.location.start_line) {
                    $UriBuilder.Fragment = '';
                } else {
                    if ($null -eq $this.most_recent_instance.location.end_line -or $this.most_recent_instance.location.start_line -eq $this.most_recent_instance.location.end_line) {
                        $UriBuilder.Fragment = "L$($this.most_recent_instance.location.start_line)";
                    } else {
                        $UriBuilder.Fragment = "L$($this.most_recent_instance.location.start_line)-L$($this.most_recent_instance.location.end_line)";
                    }
                }
                $UriBuilder.Query = '';
                $this.source_url = $UriBuilder.Uri.AbsoluteUri;
            } else {
                $this.source_url = '';
            }
        }
        $this.number = $obj.number;
        if ($null -eq $obj.rule) {
            $this.rule = $null;
        } else {
            $this.rule = [CodeScanRule]::new($obj.rule);
        }
        $this.state = $obj.state;
        if ($null -eq $obj.tool) {
            $this.tool = $null;
        } else {
            $this.tool = [CodeScanTool]::new($obj.tool);
        }
        $this.url = $obj.url;
    }
}
# 6 = source url; 5 = rule description
Function Get-CodeScanningAlerts {
    [CmdletBinding()]
    [OutputType([CodeScanningAlert[]])]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Repository,
        [Parameter(Mandatory = $true)]
        [PSCredential]$AccessToken,
        [string]$Owner,
        [switch]$Raw
    )

    if ([string]::IsNullOrEmpty($Owner)) { $Owner = $Credentials.UserName }
    
    $Page = 1;
    
    $BaseWebUri = "https://github.com/$([Uri]::EscapeDataString($Owner))/$([Uri]::EscapeDataString($Repository))/blob/"
    $BaseApiUri = "https://api.github.com/repos/$([Uri]::EscapeDataString($Owner))/$([Uri]::EscapeDataString($Repository))/code-scanning/alerts?state=open&per_page=100&page=";
    $Uri = "$BaseApiUri$Page";
    Write-Progress -Activity 'Getting Code Scan Alerts' -Status "Getting page $Page" -CurrentOperation $Uri;
    $OldPreference = $ProgressPreference;
    $ProgressPreference = [System.Management.Automation.ActionPreference]::SilentlyContinue;
    $Result = Invoke-WebRequest -Uri $Uri -Method Get -Headers @{
        Authorization = "token $($Credentials.GetNetworkCredential().Password)";
        Accept = 'application/sarif+json';
    };
    $ProgressPreference = $OldPreference;
    while ($null -ne $Result -and $null -ne $Result.Content) {
        $Alerts = $Result.Content | ConvertFrom-Json;
        if ($null -eq $Alerts -or $Alerts.Count -eq 0) { break }
        if ($Raw.IsPresent) { $Alerts | Write-Output } else { $Alerts | ForEach-Object { [CodeScanningAlert]::new($_) } }
        if ($Alerts.Count -lt 100) { break }
        $Page++;
        $Uri = "$BaseApiUri$Page";
        Write-Progress -Activity 'Getting Code Scan Alerts' -Status "Getting page $Page" -CurrentOperation $Uri -Id 1;
        $ProgressPreference = [System.Management.Automation.ActionPreference]::SilentlyContinue;
        $Result = Invoke-WebRequest -Uri $Uri -Method Get -Headers @{
            Authorization = "token $($Credentials.GetNetworkCredential().Password)";
            Accept = 'application/sarif+json';
        };
        $ProgressPreference = $OldPreference;
    }
    Write-Progress -Activity 'Getting Code Scan Alerts' -Status "$Page pages retrieved" -Id 1 -Completed;
}

Function Export-CodeScanningAlerts {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [CodeScanningAlert]$Alert,
        
        [Parameter(Mandatory = $true)]
        [string]$Path,

        [Parameter(Mandatory = $true)]
        [int]$ExpectedRowCount
    )
    Begin {
        Write-Progress -Activity 'Exporting Data' -Status 'Initializing' -CurrentOperation 'Starting Excel';
        $ExcelApp = New-Object -ComObject 'Excel.Application';
        $ExcelApp.visible = $True;
        Write-Progress -Activity 'Exporting Data' -Status 'Initializing' -CurrentOperation 'Creating new workbook';
        $Workbook = $ExcelApp.Workbooks.Add();
        $Worksheet = $workbook.Worksheets.Item(1);
        $Worksheet.Name = 'Code Scanning Alerts';
        <#

35.86



8.43

8.43
        #>
        $ColumnInfo = @(
            @{ Header = 'Number'; Type = [CodeScanningAlert]; Field = 'number'; Format = '0'; IsDateTime = $false; ColumnWidth = 8.43; HyperlinkUrl = @{ Type = [CodeScanningAlert]; Field = 'html_url' } },
            @{ Header = 'Rule Severity'; Type = [CodeScanRule]; Field = 'severity'; Format = '@'; IsDateTime = $false; ColumnWidth = 12.0 },
            @{ Header = 'Message'; Type = [CodeScanResult]; Field = 'message'; Format = '@'; IsDateTime = $false; ColumnWidth = 35.86 },
            @{ Header = 'Rule Description'; Type = [CodeScanRule]; Field = 'description'; Format = '@'; IsDateTime = $false; ColumnWidth = 35.86 },
            @{ Header = 'Path'; Type = [CodeLocation]; Field = 'path'; Format = '@'; IsDateTime = $false; ColumnWidth = 35.86; HyperlinkUrl = @{ Type = [CodeScanningAlert]; Field = 'source_url' } },
            @{ Header = 'Start Line'; Type = [CodeLocation]; Field = 'start_line'; Format = '0'; IsDateTime = $false; ColumnWidth = 8.57 },
            @{ Header = 'Start Column'; Type = [CodeLocation]; Field = 'start_column'; Format = '0'; IsDateTime = $false; ColumnWidth = 11.71 },
            @{ Header = 'End Line'; Type = [CodeLocation]; Field = 'end_line'; Format = '0'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'End Column'; Type = [CodeLocation]; Field = 'end_column'; Format = '0'; IsDateTime = $false; ColumnWidth = 10.86 },
            @{ Header = 'Commit SHA'; Type = [CodeScanResult]; Field = 'commit_sha'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Instance State'; Type = [CodeScanResult]; Field = 'state'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Alert State'; Type = [CodeScanningAlert]; Field = 'state'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Rule ID'; Type = [CodeScanRule]; Field = 'id'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Rule Name'; Type = [CodeScanRule]; Field = 'name'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'URL'; Type = [CodeScanningAlert]; Field = 'url'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Instances URL'; Type = [CodeScanningAlert]; Field = 'instances_url'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Created At'; Type = [CodeScanningAlert]; Field = 'created_at'; Format = 'm/d/yy h:mm;@'; IsDateTime = $true; ColumnWidth = 8.43 },
            @{ Header = 'Dismissed At'; Type = [CodeScanningAlert]; Field = 'dismissed_at'; Format = 'm/d/yy h:mm;@'; IsDateTime = $true; ColumnWidth = 8.43 },
            @{ Header = 'Dismissed By'; Type = [CodeScanningAlert]; Field = 'dismissed_by'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Dismissed Reason'; Type = [CodeScanningAlert]; Field = 'dismissed_reason'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Classifications'; Type = [CodeScanResult]; Field = 'classifications'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Analysis Key'; Type = [CodeScanningAlert]; Field = 'analysis_key'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Environment'; Type = [CodeScanResult]; Field = 'environment'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Ref'; Type = [CodeScanResult]; Field = 'ref'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Tool Guid'; Type = [CodeScanTool]; Field = 'guid'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Tool Name'; Type = [CodeScanTool]; Field = 'name'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Tool Version'; Type = [CodeScanTool]; Field = 'version'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 },
            @{ Header = 'Category'; Type = [CodeScanResult]; Field = 'category'; Format = '@'; IsDateTime = $false; ColumnWidth = 8.43 }
        );
        Write-Progress -Activity 'Exporting Data' -Status 'Writing Data' -CurrentOperation 'Creating Headers';
        $cn = 1;
        foreach ($ci in $ColumnInfo) {
            $Worksheet.Columns($cn).ColumnWidth = $ci.ColumnWidth;
            $Worksheet.Cells.Item(1, $cn).Font.Bold = $True;
            $Worksheet.Cells.Item(1, $cn).NumberFormat = '@';
            $Worksheet.Cells.Item(1, $cn++) = $ci.Header;
        }
        $RowNumber = 1;
        $ActualRowCount = 0;
    }
    Process {
        if ($RowNumber -le $ExpectedRowCount) {
            $RowNumber++;
            Write-Progress -Activity 'Exporting Data' -Status 'Writing Data' -CurrentOperation "Exporting row # $RowNumber" -PercentComplete ((($RowNumber - 1) * 100) / $ExpectedRowCount);
        } else {
            $RowNumber++;
            Write-Progress -Activity 'Exporting Data' -Status 'Writing Data' -CurrentOperation "Exporting row # $RowNumber";
        }
        [CodeScanResult]$Result = $Alert.most_recent_instance;
        if ($null -eq $Result) { $Result = [CodeScanResult]::new($null) }
        $classifications = '[]';
        if ($null -ne $Result.classifications -and $Result.classifications.Length -gt 0) { $classifications = $Result.Classifications | ConvertTo-Json  -Compress }
        [CodeLocation]$Location = $Result.location;
        if ($null -eq $Location) { $Location = [CodeLocation]::new($null) }
        [CodeScanRule]$Rule = $Alert.rule;
        if ($null -eq $Rule) { $Rule = [CodeScanRule]::new($null) }
        [CodeScanTool]$Tool = $Alert.tool;
        if ($null -eq $Tool) { $Tool = [CodeScanTool]::new($null) }
        
        $Values = @($Rule.id, $Rule.name, $Rule.severity, $Result.message, $Location.path, $Location.start_line, $Location.start_column, $Location.end_line, $Location.end_column, $Result.category, $classifications,
            $Result.commit_sha, $Result.state, $Alert.state, $Alert.number, $Alert.url, $Alert.html_url, $Alert.instances_url, $Alert.created_at, $Alert.dismissed_at, $Alert.dismissed_by, $Alert.dismissed_reason,
            $Alert.analysis_key, $Result.environment, $Result.ref, $Tool.guid, $Tool.name, $Tool.version, $Rule.description);
        $cn = 1;
        $DateTime = [DateTime]::Now;
        foreach ($ci in $ColumnInfo) {
            $Worksheet.Cells.Item($RowNumber, $cn).NumberFormat = $ci.Format;
            $Text = '';
            $HyperlinkUrl = '';
            if ($null -ne $ci.HyperlinkUrl) {
                if ($ci.HyperlinkUrl.Type -eq [CodeScanningAlert]) {
                    $HyperlinkUrl = '' + $Alert.($ci.HyperlinkUrl.Field);
                } else {
                    if ($ci.HyperlinkUrl.Type -eq [CodeScanResult]) {
                        if ($null -ne $Result) {
                            $HyperlinkUrl = '' + $Result.($ci.HyperlinkUrl.Field);
                        }
                    } else {
                        if ($ci.HyperlinkUrl.Type -eq [CodeLocation]) {
                            if ($null -ne $Location) { $HyperlinkUrl = '' + $Location.($ci.HyperlinkUrl.Field) }
                        } else {
                            if ($ci.HyperlinkUrl.Type -eq [CodeScanRule]) {
                                if ($null -ne $Rule) { $HyperlinkUrl = '' + $Rule.($ci.HyperlinkUrl.Field) }
                            } else {
                                if ($ci.HyperlinkUrl.Type -eq [CodeScanTool] -and $null -ne $Tool) { $HyperlinkUrl = '' + $Tool.($ci.HyperlinkUrl.Field) }
                            }
                        }
                    }
                }
            }
            if ($ci.Type -eq [CodeScanningAlert]) {
                $Text = '' + $Alert.($ci.Field);
            } else {
                if ($ci.Type -eq [CodeScanResult]) {
                    if ($null -ne $Result) {
                        if ($ci.Field -eq 'classifications') {
                            if ($null -ne $Result.classifications) {
                                if ($Result.classifications.Count -eq 1) {
                                    $Text = $Result.classifications[0];
                                } else {
                                    $Text = ($Result.classifications | Out-String).TrimEnd();
                                }
                            }
                        } else {
                            $Text = '' + $Result.($ci.Field);
                        }
                    }
                } else {
                    if ($ci.Type -eq [CodeLocation]) {
                        if ($null -ne $Location) { $Text = '' + $Location.($ci.Field) }
                    } else {
                        if ($ci.Type -eq [CodeScanRule]) {
                            if ($null -ne $Rule) { $Text = '' + $Rule.($ci.Field) }
                        } else {
                            if ($ci.Type -eq [CodeScanTool] -and $null -ne $Tool) { $Text = '' + $Tool.($ci.Field) }
                        }
                    }
                }
            }
            if ($ci.IsDateTime -and [DateTime]::TryParse($Text, [ref]$DateTime)) { $Text = $DateTime.ToString() }
            $Worksheet.Cells.Item($RowNumber, $cn) = $Text;
            if (-not [string]::IsNullOrWhiteSpace($HyperlinkUrl)) {
                $Worksheet.Hyperlinks.Add($Worksheet.Cells.Item($RowNumber, $cn), $HyperlinkUrl, '', $Text) | Out-Null;
            }
            $cn++;
        }
    }
    End {
        Write-Progress -Activity 'Exporting Data' -Status 'Writing Data' -CurrentOperation "Saving changes to $Path";
        if ($Path | Test-Path) { Remove-Item -Path $Path -Force }
        $Workbook.SaveAs($Path);
        $ExcelApp.Quit();
        Write-Progress -Activity 'Exporting Data' -Status 'Export completed' -Completed;
    }
}

Function Convert-CodeScanningAlertsToCsv {
    [CmdletBinding(DefaultParameterSetName = 'Literal')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [CodeScanningAlert]$Alert,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Path')]
        [string]$Path,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Literal')]
        [string]$LiteralPath,

        [ValidateSet('Unicode', 'UTF7', 'UTF8', 'ASCII', 'UTF32', 'BigEndianUnicode', 'Default', 'OEM')]
        [string]$Encoding = 'ASCII',

        [char]$Delimiter,

        [switch]$Append,

        [switch]$Confirm,

        [switch]$Force,

        [switch]$NoClobber
    )
    Begin { $FlatObjects = [System.Collections.ObjectModel.Collection[PSCustomObject]]::new() }
    Process {
        [CodeScanResult]$Result = $Alert.most_recent_instance;
        if ($null -eq $Result) { $Result = [CodeScanResult]::new($null) }
        $classifications = '[]';
        if ($null -ne $Result.classifications -and $Result.classifications.Length -gt 0) { $classifications = $Result.Classifications | ConvertTo-Json  -Compress }
        [CodeLocation]$Location = $Result.location;
        if ($null -eq $Location) { $Location = [CodeLocation]::new($null) }
        [CodeScanRule]$Rule = $Alert.rule;
        if ($null -eq $Rule) { $Rule = [CodeScanRule]::new($null) }
        [CodeScanTool]$Tool = $Alert.tool;
        if ($null -eq $Tool) { $Tool = [CodeScanTool]::new($null) }
        $FlatObjects.Add(([PSCustomObject]@{
            'Rule ID' = '' + $Rule.id;
            'Rule Name' = '' + $Rule.name;
            'Rule Severity' = '' + $Rule.severity;
            'Message' = $Result.message;
            'Path' = '' + $Location.path;
            'Start Line' = '' + $Location.start_line;
            'Start Column' = '' + $Location.start_column;
            'End Line' = '' + $Location.end_line;
            'End Column' = '' + $Location.end_column;
            'Category' = '' + $Result.category;
            'Classifications' = $classifications;
            'Commit SHA' = '' + $Result.commit_sha;
            'Instance State' = '' + $Result.state;
            'Alert State' = '' + $Alert.state;
            'Number' = '' + $Alert.number;
            'URL' = '' + $Alert.url;
            'Html URL' = '' + $Alert.html_url;
            'Instances URL' = '' + $Alert.instances_url;
            'Created At' = '' + $Alert.created_at;
            'Dismissed At' = '' + $Alert.dismissed_at;
            'Dismissed By' = '' + $Alert.dismissed_by;
            'Dismissed Reason' = '' + $Alert.dismissed_reason;
            'Analysis Key' = '' + $Alert.analysis_key;
            'Environment' = '' + $Result.environment;
            'Ref' = '' + $Result.ref;
            'Tool Guid' = '' + $Tool.guid;
            'Tool Name' = '' + $Tool.name;
            'Tool Version' = '' + $Tool.version;
            'Rule Description' = '' + $Rule.description;
        }));
    }
    End {
        $Splat = @{};
        if ($PSBoundParameters.ContainsKey('Delimiter')) { $Splat['Delimiter'] = $Delimiter }
        if ($Append.IsPresent) { $Splat['Append'] = $Append }
        if ($Confirm.IsPresent) { $Splat['Confirm'] = $Confirm }
        if ($Force.IsPresent) { $Splat['Force'] = $Force }
        if ($NoClobber.IsPresent) { $Splat['NoClobber'] = $NoClobber }
        if ($PSBoundParameters.ContainsKey('LiteralPath')) {
            $FlatObjects | Export-Csv @splat -NoTypeInformation -Encoding $Encoding -LiteralPath $LiteralPath;
        } else {
            $FlatObjects | Export-Csv @splat -NoTypeInformation -Encoding $Encoding -Path $Path;
        }
    }
}
while ($null -eq $Credentials) {
    $Credentials = Get-Credential -Message 'Enter personal access token' -UserName 'lerwine';
}

#$csa = Get-CodeScanningAlerts -Repository 'FsInfoCat' -AccessToken $Credentials -Raw;

$CodeScanningAlerts = Get-CodeScanningAlerts -Repository 'FsInfoCat' -AccessToken $Credentials;
#$CodeScanningAlerts | Convert-CodeScanningAlertsToCsv -LiteralPath ($PSScriptRoot | Join-Path -ChildPath 'CodeScanResults.csv') -Delimiter "`t" -Force;
$CodeScanningAlerts | Export-CodeScanningAlerts -Path ($PSScriptRoot | Join-Path -ChildPath 'CodeScanResults.xlsx') -ExpectedRowCount $CodeScanningAlerts.Count;

<#
$CodeScanningAlert = $CodeScanningAlerts[0];
$most_recent_instance = $CodeScanningAlert.most_recent_instance;
$location = $most_recent_instance.location;
$message = $most_recent_instance.message;
$rule = $CodeScanningAlert.rule;
$tool = $CodeScanningAlert.tool;

$CodeScanningAlert | Get-Member -MemberType Properties

   TypeName: System.Management.Automation.PSCustomObject

Name                 MemberType   Definition
----                 ----------   ----------
created_at           NoteProperty string created_at=2021-11-18T09:52:30Z
dismissed_at         NoteProperty object dismissed_at=null
dismissed_by         NoteProperty object dismissed_by=null
dismissed_reason     NoteProperty object dismissed_reason=null
html_url             NoteProperty string html_url=https://github.com/lerwine/FsInfoCat/security/code-scanning/1606
instances_url        NoteProperty string instances_url=https://api.github.com/repos/lerwine/FsInfoCat/code-scanning/alerts/1606/instances
most_recent_instance NoteProperty System.Management.Automation.PSCustomObject most_recent_instance=@{ref=refs/heads/main; analysis_key=.github/workflows/codeql-analysis.yml:analyze; environment={"language":"csharp"}; category=.github/workflows/codeql-analysis.yml:analyze/language:csharp; state=open; commit_sha=c4c9a18059c9e658a4a24d3d81457e142f4837d6; message=; location=; classifications=System.Object[]}
number               NoteProperty int number=1606
rule                 NoteProperty System.Management.Automation.PSCustomObject rule=@{id=cs/missed-ternary-operator; severity=note; description=Missed ternary opportunity; name=cs/missed-ternary-operator}
state                NoteProperty string state=open
tool                 NoteProperty System.Management.Automation.PSCustomObject tool=@{name=CodeQL; guid=; version=2.7.0}
url                  NoteProperty string url=https://api.github.com/repos/lerwine/FsInfoCat/code-scanning/alerts/1606


$CodeScanningAlert.most_recent_instance | Get-Member -MemberType Properties;

   TypeName: System.Management.Automation.PSCustomObject

Name            MemberType   Definition
----            ----------   ----------
analysis_key    NoteProperty string analysis_key=.github/workflows/codeql-analysis.yml:analyze
category        NoteProperty string category=.github/workflows/codeql-analysis.yml:analyze/language:csharp
classifications NoteProperty Object[] classifications=System.Object[]
commit_sha      NoteProperty string commit_sha=c4c9a18059c9e658a4a24d3d81457e142f4837d6
environment     NoteProperty string environment={"language":"csharp"}
location        NoteProperty System.Management.Automation.PSCustomObject location=@{path=src/FsInfoCat/Collections/WeakReferenceList.cs; start_line=120; end_line=123; start_column=21; end_column=65}
message         NoteProperty System.Management.Automation.PSCustomObject message=@{text=Both branches of this 'if' statement write to the same variable - consider using '?' to express intent better.}
ref             NoteProperty string ref=refs/heads/main
state           NoteProperty string state=open

$CodeScanningAlert.most_recent_instance.classifications[0].GetType()

IsPublic IsSerial Name                                     BaseType
-------- -------- ----                                     --------
True     True     String                                   System.Object


$CodeScanningAlert.most_recent_instance.location | Get-Member -MemberType Properties;

   TypeName: System.Management.Automation.PSCustomObject

Name         MemberType   Definition
----         ----------   ----------
end_column   NoteProperty int end_column=65
end_line     NoteProperty int end_line=123
path         NoteProperty string path=src/FsInfoCat/Collections/WeakReferenceList.cs
start_column NoteProperty int start_column=21
start_line   NoteProperty int start_line=120


$CodeScanningAlert.most_recent_instance.message | Get-Member -MemberType Properties;

   TypeName: System.Management.Automation.PSCustomObject

Name MemberType   Definition
---- ----------   ----------
text NoteProperty string text=Both branches of this 'if' statement write to the same variable - consider using '?' to express intent better.


$CodeScanningAlert.rule | Get-Member -MemberType Properties;

   TypeName: System.Management.Automation.PSCustomObject
   
Name        MemberType   Definition
----        ----------   ----------
description NoteProperty string description=Extraction message
id          NoteProperty string id=cs/extraction-message
name        NoteProperty string name=cs/extraction-message
severity    NoteProperty string severity=note


$CodeScanningAlert.tool | Get-Member -MemberType Properties;

   TypeName: System.Management.Automation.PSCustomObject

Name    MemberType   Definition
----    ----------   ----------
guid    NoteProperty object guid=null
name    NoteProperty string name=CodeQL
version NoteProperty string version=2.7.0



$CodeScanningAlerts | Where-Object {
    $members = @($_.tool | Get-Member -MemberType Properties);
    $members.Count -ne 3 } | Select-Object -First 1;

[System.Windows.Clipboard]::SetText(((($tool | Get-Member -MemberType Properties) | Out-String -Width 8192) -replace ' +(?=[\r\n])', ''));
#>