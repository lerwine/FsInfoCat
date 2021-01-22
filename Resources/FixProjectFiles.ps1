Param(
    [switch]$AllowAnyGit,
    [string]$GitExePath,
    [string]$RepositoryPath
)

$DebugPreference = [System.Management.Automation.ActionPreference]::Continue;
$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$ErrorActionPreference = [System.Management.Automation.ActionPreference]::Stop;
$Script:ProjectFileExtensions = @('.csproj', '.props', '.targets');
$Script:IgnoreFolderNames = @('bin', 'obj', 'wwwroot');

Function Get-AllProjectFiles {
    [CmdletBinding()]
    Param(
        [string]$Path = '.',

        [switch]$Literal,

        [int]$Depth = 32
    )

    Write-Debug -Message ("Enter Get-AllProjectFiles " + (($PSBoundParameters.Keys | ForEach-Object { "-$_ $($PSBoundParameters[$_])" }) -join ' '));
    if ($Literal.IsPresent) {
        Write-Debug -Message "Calling: Get-ChildItem -LiteralPath `"$Path`" -File";
        (Get-ChildItem -LiteralPath $Path -File) | Where-Object {
            $Script:ProjectFileExtensions -icontains $_.Extension
        } | Select-Object -ExpandProperty 'FullName';
        if ($Depth -gt 0) {
            $Depth--;
            Write-Debug -Message "Calling: Get-ChildItem -LiteralPath `"$Path`" -Directory";
            (Get-ChildItem -LiteralPath $Path -Directory) | Where-Object {
                $Script:IgnoreFolderNames -inotcontains $_.Name
            } | ForEach-Object {
                Write-Debug -Message "Calling: Get-AllProjectFiles -Path `"$($_.FullName)`" -Depth $Depth -Literal";
                Get-AllProjectFiles -Path $_.FullName -Depth $Depth -Literal;
            }
        }
    } else {
        Write-Debug -Message "Calling: Get-ChildItem -Path `"$Path`" -File";
        (Get-ChildItem -Path $Path -File) | Where-Object {
            $Script:ProjectFileExtensions -icontains $_.Extension
        } | Select-Object -ExpandProperty 'FullName';
        if ($Depth -gt 0) {
            $Depth--;
            Write-Debug -Message "Calling: Get-ChildItem -Path `"$Path`" -Exclude 'bin', 'obj', 'wwwroot' -Directory";
            (Get-ChildItem -Path $Path -Directory) | Where-Object {
                $Script:IgnoreFolderNames -inotcontains $_.Name
            } | ForEach-Object {
                Write-Debug -Message "Calling: Get-AllProjectFiles -Path `"$($_.FullName)`" -Depth $Depth -Literal";
                Get-AllProjectFiles -Path $_.FullName -Depth $Depth -Literal;
            }
        }
    }
}

Function Test-GitRepositoryStatus {
    [CmdletBinding()]
    Param(
        [string]$GitExePath,
        [string]$TargetDirectory,
        [switch]$AllowAnyGit
    )

    if ($PSBoundParameters.ContainsKey('TargetDirectory')) {
        Write-Debug -Message "Testing TargetDirectory: $TargetDirectory";
        if (-not ($TargetDirectory | Test-Path -PathType Container)) {
            if ($TargetDirectory | Test-Path) {
                Write-Warning -Message ("The specified target path is not a subdirectory.`n" + `
                    "Aborting.");
            } else {
                Write-Warning -Message ("The specified target subdirectory does not exist.`n" + `
                    "Aborting.");
            }
            return $false;
        }
    } else {
        $TargetDirectory = Get-Location;
        Write-Debug -Message "Target directory not specified, using path from Get-Location: $TargetDirectory";
    }

    $GitCommand = $null;
    if ($PSBoundParameters.ContainsKey('GitExePath')) {
        Write-Debug -Message "Calling: Get-Command -Name `"$GitExePath`" -ErrorAction Continue";
        $GitCommand = Get-Command -Name $GitExePath -ErrorAction Continue;
        if ($null -eq $GitCommand) {
            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                "Unable to locate the git command at the location specified by the -GitExePath parameter.`n" + `
                "Aborting.");
            return $false;
        }
    } else {
        $GitCommand = Get-Command -Name 'git.exe' -ErrorAction Continue;
        if ($null -eq $GitCommand) {
            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                "Unable to locate git.exe.`n" + `
                "You can try using the -GitExePath parameter if you know the path.`n" + `
                "Aborting.");
            return $false;
        }
        if ($GitCommand.CommandType -ne [System.Management.Automation.CommandTypes]::Application -and -not $AllowAnyGit.IsPresent) {
            Write-Debug -Message "Using git CommandType is $($GitCommand.CommandType)";
            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                "`"git.exe`" refers to a command, but it is not an executable file.`n" + `
                "You can use the the -AllowAnyGit switch to bypass this check if this is intentional.`n" + `
                "Aborting.");
            return $false;
        }
    }
    Write-Debug -Message "Using git path: $($GitCommand.Path)";
    $StandardOutput = [System.IO.Path]::GetTempFileName();
    Write-Debug -Message "Redirecting git command standard output to $StandardOutput";
    try {
        try {
            $StandardError = [System.IO.Path]::GetTempFileName();
            Write-Debug -Message "Redirecting git command standard error to $StandardError";
            $Proc = $null;
            Write-Information -MessageData "git status -z -u";
            $Proc = Start-Process -FilePath $GitCommand.Path -ArgumentList 'status', '-z', '-u' -WorkingDirectory $TargetDirectory `
                -RedirectStandardOutput $StandardOutput -RedirectStandardError $StandardError `
                -ErrorAction Continue -NoNewWindow -Wait -PassThru;
            if ($null -eq $Proc) {
                Write-Warning -Message ("Unable to verify status of respository.`n" + `
                    "Failed to execute `"git.exe`".`nAborting.");
                return $false;
            }
            Write-Debug -Message "git returned exit code $($Proc.ExitCode)";

            $Items = @();
            Write-Debug -Message "Reading standard output result from $StandardOutput";
            $OutputMessage = Get-Content -LiteralPath $StandardOutput;
            if ($OutputMessage.Length -eq 0) {
                Write-Debug -Message 'Nothing was written to standard output';
            } else {
                if ($OutputMessage.Trim().Length -eq 0) {
                    Write-Debug -Message "$($OutputMessage.Length) whitespace characters were written to standard output";
                } else {
                    Write-Debug -Message "$($OutputMessage.Length) characters were written to standard error";
                    $Items = @($OutputMessage.Split([char]0) | Where-Object { $_.Trim().Length -gt 0 });
                    Write-Debug -Message "Parsed $($Items.Count) null-separated lines from standard output:`n`t$(($Items -join "`n`t"))";
                }
            }
            Write-Debug -Message "Reading standard error result from $StandardError";
            $ErrorMessage = Get-Content -LiteralPath $StandardError;
            if ($ErrorMessage.Length -eq 0) {
                Write-Debug -Message 'Nothing was written to standard error';
            } else {
                if ($ErrorMessage.Trim().Length -eq 0) {
                    Write-Debug -Message "$($ErrorMessage.Length) whitespace characters were written to standard error";
                } else {
                    Write-Debug -Message "$($ErrorMessage.Length) characters were written to standard error:`n$ErrorMessage";
                }
            }

            switch ($Proc.ExitCode) {
                0 { break; }
                128 {
                    $ErrorMessage = (@($OutputMessage, $ErrorMessage) | ForEach-Object {
                        if (-not [string]::IsNullOrWhiteSpace($_)) { ($_ -split '\r\n?|\n') | ForEach-Object { "`t$_".TrimEnd() } }
                    } | Out-String).TrimEnd();
                    if ($PSBoundParameters.ContainsKey('TargetDirectory')) {
                        if ($ErrorMessage.Length -gt 0) {
                            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                                "It doesn't appear that the specified target subdirectory is part of a git repository.`n" + `
                                "Actual response from git.exe:`n$ErrorMessage`nAborting.");
                        } else {
                            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                                "It doesn't appear that the specified target subdirectory is part of a git repository.`n" + `
                                "Aborting.");
                        }
                    } else {
                        if ($ErrorMessage.Length -gt 0) {
                            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                                "It doesn't appear that the current working directory ($TargetDirectory) is part of a git repository.`n" + `
                                "Actual response from git.exe:`n$ErrorMessage`nAborting.");
                        } else {
                            Write-Warning -Message ("Unable to verify status of respository.`n" + `
                                "It doesn't appear that the current working directory ($TargetDirectory) is part of a git repository.`n" + `
                                "Aborting.");
                        }
                    }
                    return $false;
                }
                default {
                    $ErrorMessage = (@($OutputMessage, $ErrorMessage) | ForEach-Object {
                        if (-not [string]::IsNullOrWhiteSpace($_)) { ($_ -split '\r\n?|\n') | ForEach-Object { "`t$_".TrimEnd() } }
                    } | Out-String).TrimEnd();
                    if ($ErrorMessage.Length -gt 0) {
                        Write-Warning -Message ("Unable to verify status of respository.`n" + `
                            "The git command returned error code $($Proc.ExitCode).`n" + `
                            "Actual response from git.exe:`n$ErrorMessage`nAborting.");
                    } else {
                        Write-Warning -Message ("Unable to verify status of respository.`n" + `
                            "The git command returned error code $($Proc.ExitCode).`n" + `
                            "Aborting.");
                    }
                    return $false;
                }
            }
            if (-not [string]::IsNullOrWhiteSpace($ErrorMessage)) {
                if (-not [string]::IsNullOrWhiteSpace($OutputMessage)) { $ErrorMessage = "$OutputMessage`n$ErrorMessage"; }
                Write-Warning -Message ("Unable to verify status of respository.`n" + `
                    "The git command returned error code $($Proc.ExitCode).`n" + `
                    "Actual response from git.exe:`n$ErrorMessage`nAborting.");
            }
            if ($Items.Count -eq 0) { return $true }
            $Unsaved = @();
            $Changed = @();
            if ($null -eq $Script:__Test_GitRepositoryStatusRegex) {
                $Script:__Test_GitRepositoryStatusRegex = [System.Text.RegularExpressions.Regex]::new('^((?=[\s?]?[MADRCU!])(?<c>.[MADRCU?!\s])|(?=\s?\?)(?<u>.[\s?])|\s\s)\s(?<f>\S.*)$', [System.Text.RegularExpressions.RegexOptions]::Compiled);
            }
            foreach ($Status in $Items) {
                if ($Status.Trim().Length -gt 0) {
                    Write-Debug -Message "Testing $Status against regular expression $($Script:__Test_GitRepositoryStatusRegex)";
                    $m = $Script:__Test_GitRepositoryStatusRegex.Match($Status);
                    if (-not $m.Success) {
                        if (-not [string]::IsNullOrWhiteSpace($ErrorMessage)) { $OutputMessage += "`n$ErrorMessage"; }
                        Write-Warning -Message ("Unable to verify status of respository.`n" + `
                            "The git command returned error code $($Proc.ExitCode).`n" + `
                            "Actual response from git.exe:`n$OutputMessage`nAborting.");
                        return $false;
                    }
                    Write-Debug -Message "Matched status characters `"$($m.Groups[1])`"";
                    if ($m.Groups['c'].Success) {
                        $Changed += @($m.Groups["f"].Value);
                    } else {
                        if ($m.Groups['u'].Success) {
                            $Unsaved += @($m.Groups["f"].Value);
                        }
                    }
                }
            }
            Write-Information -MessageData $OutputMessage;
            if ($Changed.Count -gt 0) {
                if ($Changed.Count -eq 1) {
                    Write-Warning -Message ("The following file has been changed and needs to be checked in or stashed before running this script:`n" + `
                        "`t$($Changed[0])`nAborting.");
                } else {
                    Write-Warning -Message ("The following files have been changed and need to be checked in or stashed before running this script:`n" + `
                        "`t$(($Changed -join "`n`t"))`nAborting.");
                }
                return $false;
            }
            if ($Unsaved.Count -eq 0) { return $true }
            if ($Unsaved.Count -eq 1) {
                Write-Warning -Message ("The following file has unsaved changes and needs to be reverted, checked in or stashed before running this script:`n" + `
                    "`t$($Unsaved[0])`nAborting.");
            } else {
                Write-Warning -Message ("The following files have been changed and need to be reverted, checked in or stashed before running this script:`n" + `
                    "`t$(($Unsaved -join "`n`t"))`nAborting.");
            }
            return $false;
        } finally {
            Write-Debug -Message "Deleting temp standard error file $StandardError";
            Remove-Item -LiteralPath $StandardError -Force;
        }
    } finally {
        Write-Debug -Message "Deleting temp standard output file $StandardOutput";
        Remove-Item -LiteralPath $StandardOutput -Force;
    }
    Write-Debug -Message "Returning false after failure";
    return $false;
}

$GitRepoOkay = $false;
Write-Progress -Activity 'Fixing project files' -Status 'Verifying git repostory status';
if ($PSBoundParameters.ContainsKey('GitExePath')) {
    if ($PSBoundParameters.ContainsKey('TargetDirectory')) {
        if ($AllowAnyGit.IsPresent) {
            $GitRepoOkay = Test-GitRepositoryStatus -GitExePath $GitExePath -TargetDirectory $TargetDirectory -AllowAnyGit;
        } else {
            $GitRepoOkay = Test-GitRepositoryStatus -GitExePath $GitExePath -TargetDirectory $TargetDirectory;
        }
    } else {
        if ($AllowAnyGit.IsPresent) {
            $GitRepoOkay = Test-GitRepositoryStatus -GitExePath $GitExePath -AllowAnyGit;
        } else {
            $GitRepoOkay = Test-GitRepositoryStatus -GitExePath $GitExePath;
        }
    }
} else {
    if ($PSBoundParameters.ContainsKey('TargetDirectory')) {
        if ($AllowAnyGit.IsPresent) {
            $GitRepoOkay = Test-GitRepositoryStatus -TargetDirectory $TargetDirectory -AllowAnyGit;
        } else {
            $GitRepoOkay = Test-GitRepositoryStatus -TargetDirectory $TargetDirectory;
        }
    } else {
        if ($AllowAnyGit.IsPresent) {
            $GitRepoOkay = Test-GitRepositoryStatus -AllowAnyGit;
        } else {
            $GitRepoOkay = Test-GitRepositoryStatus;
        }
    }
}
if (-not $GitRepoOkay) {
    Write-Progress -Activity 'Fixing project files' -Status 'Safe git repository status not confirmed' -Completed;
    return;
}

$TopText = @'
$(FsInfoCatTrace)
$(MSBuildThisFile) Top:
    MSBuildProjectDirectory = $(MSBuildProjectDirectory)
            TargetFramework = $(TargetFramework); AssemblyVersion = $(AssemblyVersion)
              Configuration = $(Configuration); DefineConstants = $(DefineConstants)
                 OutputPath = $(OutputPath); OsPlatform = $(OsPlatform)
          RepositoryRootDir = $(RepositoryRootDir);
'@;
$BottomText = @'
$(FsInfoCatTrace)
$(MSBuildThisFile) Bottom:
MSBuildProjectDirectory = $(MSBuildProjectDirectory)
TargetFramework = $(TargetFramework); AssemblyVersion = $(AssemblyVersion)
  Configuration = $(Configuration); DefineConstants = $(DefineConstants)
     OutputPath = $(OutputPath); OsPlatform = $(OsPlatform)
RepositoryRootDir = $(RepositoryRootDir);
'@;
Write-Debug -Message "Calling: Get-AllProjectFiles";
$ChangedProjectFiles = @(@(Get-AllProjectFiles | ForEach-Object {
    Write-Debug -Message "Loading project file: `"$_`"";
    Write-Progress -Activity 'Fixing project files' -Status 'Loading project files' -CurrentOperation $_;
    [xml]$Xml = [System.Xml.XmlDocument]::new();
    $Xml.Load($_);
    if ($null -eq $Xml.DocumentElement) {
        Write-Error -Message "Failed to load project file $_" -Category OpenError -ErrorId 'LoadError' -TargetObject $_ -ErrorAction Stop;
    }
    [PSCustomObject]@{
        Path = $_;
        Document = $Xml
    };
}) | ForEach-Object {
    Write-Debug -Message "Searching $($_.Path) by XPath /Project/PropertyGroup/FsInfoCatTrace";
    [System.Xml.XmlElement[]]$Elements = @($_.Document.SelectNodes('/Project/PropertyGroup/FsInfoCatTrace'));
    Write-Debug -Message "$($_.Path) contained $($Elements.Length) matches";
    $e = $Elements.Length - 1;
    if ($e -lt 0) {
        Write-Warning -Message "$($_.Path) did not contain any FsInfoCatTrace elements.";
    } else {
        $HasChanges = $false;
        $HasChanges = ($Elements[0].InnerText -cne $TopText);
        if ($HasChanges) {
            $Elements[0].InnerText = $TopText;
        }
        if ($e -gt 0) {
            if ($HasChanges -or $Elements[$e].InnerText -cne $BottomText) {
                $Elements[$e].InnerText = $BottomText;
                $HasChanges = $true;
            }
        } else {
            Write-Warning -Message "$($_.Path) only contained one FsInfoCatTrace element.";
        }
        if ($HasChanges) { $_ | Write-Output }
    }
});

if ($ChangedProjectFiles.Count -eq 0) {
    Write-Host -Object "No changes were made.";
} else {
    $XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
    $XmlWriterSettings.Indent = $true;
    $XmlWriterSettings.OmitXmlDeclaration = $true;
    $XmlWriterSettings.Encoding = [System.Text.UTF8Encoding]::new($false, $false);

    $ChangedProjectFiles | ForEach-Object {
        Write-Progress -Activity 'Fixing project files' -Status 'Saving project files' -CurrentOperation $_.Path;
        $XmlWriter = [System.Xml.XmlWriter]::Create($_.Path, $XmlWriterSettings);
        try {
            $_.Document.WriteTo($XmlWriter);
            $XmlWriter.Flush();
        } finally { $XmlWriter.Close() }
    }
    if ($ChangedProjectFiles.Count -eq 1) {
        Write-Host -Object "1 file changed.";
    } else {
        Write-Host -Object "$($ChangedProjectFiles.Count) files changed.";
    }
}
