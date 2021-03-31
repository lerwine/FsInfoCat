if ($null -eq $Script:IsWindows) {
    Set-Variable -Name 'IsWindows' -Option Constant -Scope 'Script' -Value ($PSVersionTable.PSEdition -eq 'Desktop' -or $PSVersionTable.Platform -eq 'Win32NT');
}

Write-Verbose -Message "Performing $(if ($PSVersionTable.PSEdition -eq 'Desktop') { 'Desktop edition' } else { "$($PSVersionTable.Platform) platform" })-specific initialization";
&{
    # Calculate constant variable "LocalMachineIdentifier" within encapsulated script block so variables of the rest of the module aren't polluted.
    if ($null -ne $Script:LocalMachineIdentifier) { return }
    $Identifier = '';
    $OldErrorAction = $ErrorActionPreference;
    $ErrorActionPreference = [System.Management.Automation.ActionPreference]::Stop;
    try {
        if ($Script:IsWindows) {
            Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ResourceUnavailable) -Scope 0;
            Set-Variable -Name 'TargetObject' -Value 'Microsoft.PowerShell.Management' -Scope 0;
            Set-Variable -Name 'ErrorId' -Value 'RequiredModuleLoadError' -Scope 0;
            Set-Variable -Name 'Reason' -Value "Failed to load module '$TargetObject'" -Scope 0;
            if ($null -eq (Get-Module -Name $TargetObject)) {
                Write-Verbose -Message "Loading '$TargetObject' module";
                Import-Module -Name $TargetObject -ErrorAction Stop;
            }
            Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ReadError) -Scope 0;
            Set-Variable -Name 'TargetObject' -Value 'SELECT * from Win32_UserAccount WHERE Name="Administrator"' -Scope 0;
            Set-Variable -Name 'ErrorId' -Value 'CimQueryException' -Scope 0;
            Set-Variable -Name 'Reason' -Value 'Cim query throw an exception' -Scope 0;
            $CimInstance = $null;
            Write-Verbose -Message "'Microsoft.PowerShell.Management' module is loaded. Looking up machine SID";
            Write-Debug -Message "Invoking CIM query '$TargetObject'";
            $CimInstance = Get-CimInstance -Query $TargetObject -ErrorAction Stop;
            if ($null -eq $CimInstance) {
                Set-Variable -Name 'Reason' -Value 'CIM query returned null' -Scope 0;
                Write-Debug -Message $Reason;
                Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ObjectNotFound) -Scope 0;
                Set-Variable -Name 'ErrorId' -Value 'CimQueryNullResult' -Scope 0;
            } else {
                Write-Debug -Message "CIM query returned $CimInstance";
                Remove-Variable -Name 'TargetObject' -Scope 0;
                Set-Variable -Name 'TargetObject' -Value $CimInstance -Scope 0;
                Set-Variable -Name 'Reason' -Value 'Failed to read SID of Administrator principle' -Scope 0;
                Set-Variable -Name 'ErrorId' -Value 'AccountSIDAccessError' -Scope 0;
                Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ResourceUnavailable) -Scope 0;
                if ([string]::IsNullOrWhiteSpace($CimInstance.SID)) {
                    Set-Variable -Name 'Reason' -Value 'SID of Administrator principle is null or empty' -Scope 0;
                    Write-Debug -Message $Reason;
                    Set-Variable -Name 'ErrorId' -Value 'NoAccountSID' -Scope 0;
                    Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::InvalidResult) -Scope 0;
                } else {
                    Set-Variable -Name 'Reason' -Value 'Failed to parse SID of Administrator principle' -Scope 0;
                    Set-Variable -Name 'ErrorId' -Value 'AccountSIDParseError' -Scope 0;
                    Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ParserError) -Scope 0;
                    Remove-Variable -Name 'TargetObject' -Scope 0;
                    Set-Variable -Name 'TargetObject' -Value $CimInstance.SID -Scope 0;
                    $sid = $null;
                    Write-Debug -Message "Parsing '$TargetObject' as SID string";
                    $sid = [System.Security.Principal.SecurityIdentifier]::new($TargetObject);
                    if ($null -ne $sid) {
                        Set-Variable -Name 'Reason' -Value 'Cannot convert SID of Administrator principle to machine SID' -Scope 0;
                        Set-Variable -Name 'ErrorId' -Value 'AccountDomainSidAccessError' -Scope 0;
                        Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::InvalidType) -Scope 0;
                        Remove-Variable -Name 'TargetObject' -Scope 0;
                        Set-Variable -Name 'TargetObject' -Value $sid -Scope 0;
                        Write-Debug -Message "Getting AccountDomainSid from '$TargetObject'";
                        $sid = $sid.AccountDomainSid;
                        if ($null -eq $sid) {
                            Set-Variable -Name 'Reason' -Value 'AccountDomainSid of Administrator principle is null' -Scope 0;
                            Write-Debug -Message $Reason;
                            Set-Variable -Name 'ErrorId' -Value 'NoMachineSID' -Scope 0;
                            Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::InvalidResult) -Scope 0;
                        } else {
                            Set-Variable -Name 'Identifier' -Value $sid.ToString() -Scope 0;
                            Remove-Variable -Name 'Reason' -Scope 0;
                        }
                    } else {
                        Write-Debug -Message $Reason;
                    }
                }
            }
        } else {
            Set-Variable -Name 'Reason' -Value 'Failed to read content of system file contents' -Scope 0;
            Set-Variable -Name 'ErrorId' -Value 'MachineIdFileAccessError' -Scope 0;
            Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ReadError) -Scope 0;
            Set-Variable -Name 'TargetObject' -Value '/etc/machine-id' -Scope 0;
            Write-Debug -Message "Checking path '$TargetObject'";
            if ($TargetObject | Test-Path -PathType Leaf) {
                Write-Debug -Message "Getting content of '$TargetObject'";
                Set-Variable -Name 'Identifier' -Value ('' + (Get-Content -LiteralPath $TargetObject -Force)).Trim() -Scope 0;
                Write-Debug -Message "Loaded contents '$TargetObject': `"$Identifier`"";
                if ($Identifier.Length -eq 0) {
                    Set-Variable -Name 'Reason' -Value 'Content of machine-id is empty or could not be loaded' -Scope 0;
                    Write-Debug -Message $Reason;
                    Set-Variable -Name 'ErrorId' -Value 'NoMachineIdContent' -Scope 0;
                    Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::InvalidResult) -Scope 0;
                } else {
                    Remove-Variable -Name 'TargetObject' -Scope 0;
                    Set-Variable -Name 'TargetObject' -Value $Identifier -Scope 0;
                    $Guid = [Guid]::Empty;
                    Write-Debug -Message "Parsing '$TargetObject' as Guid";
                    if ([Guid]::TryParse($Identifier, [ref]$Guid)) {
                        Set-Variable -Name 'Identifier' -Value $Guid.Tostring('d').ToLower() -Scope 0;
                        Remove-Variable -Name 'Reason' -Scope 0;
                    } else {
                        Set-Variable -Name 'Reason' -Value 'Failed to validate/parse content of /etc/machine-id' -Scope 0;
                        Write-Debug -Message $Reason;
                        Set-Variable -Name 'ErrorId' -Value 'MachineIdParseError' -Scope 0;
                        Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ParserError) -Scope 0;
                        Set-Variable -Name 'Identifier' -Value '' -Scope 0;
                    }
                }
            } else {
                Set-Variable -Name 'Reason' -Value 'File /etc/machine-id not found' -Scope 0;
                Write-Debug -Message $Reason;
                Set-Variable -Name 'ErrorId' -Value 'MachineIdFileNotFound' -Scope 0;
                Set-Variable -Name 'Category' -Value ([System.Management.Automation.ErrorCategory]::ObjectNotFound) -Scope 0;
            }
        }
    } catch {
        $ErrorActionPreference = $OldErrorAction;
        $ErrorRecord = $null;
        if ([FsInfoCat.PS.CoersionHelper]::TryGetErrorRecord($_, [ref]$ErrorRecord)) {
            Write-Error -ErrorRecord $ErrorRecord -CategoryReason $Reason;
        } else {
            $Exception = $null;
            if ([FsInfoCat.PS.CoersionHelper]::TryGetException($_, [ref]$Exception)) {
                Write-Error -Exception $Exception -Category $Category -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
            } else {
                Write-Error -Message ('' + $_) -Category $Category -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
            }
        }
    } finally {
        Write-Debug -Message "Setting '$Identifier' as LocalMachineIdentifier";
        Set-Variable -Name 'LocalMachineIdentifier' -Value $Identifier -Description 'Current host machine identifier' -Option Constant -Scope 'Script';
        if ([string]::IsNullOrEmpty($Identifier) -and -not [string]::IsNullOrWhiteSpace($Reason)) {
            Write-Error -Message "Failed to detect current machine identifier: $Reason" -Category $Category -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
        }
    }
}
if ([string]::IsNullOrWhiteSpace($Script:LocalMachineIdentifier)) {
    Write-Warning -Message 'Intialization failed.';
} else {
    Write-Information -MessageData "Initialization successful";
}

Function ConvertTo-PasswordHash {
    <#
    .SYNOPSIS
        Creates a password hash code.

    .DESCRIPTION
        Creates an object that contains a SHA512 password hash.
        
    .PARAMETER RawPwd
        Raw password to be converted to a SHA512 has.

    .PARAMETER Salt
        Optional value whose bits reprsent the encryption salt to use. If this is not specified, a cryptographically random salt value will be generated.

    .EXAMPLE
        PS C:\> $PwHash = ConvertTo-PasswordHash -RawPwd 'myPassword';
        Generates a cryptographically random salt and hashes the raw password.

    .INPUTS
        Raw password to be converted to a SHA512 hash.

    .OUTPUTS
        A FsInfoCat.Util.PwHash object representing the password hash.
    #>
    [CmdletBinding()]
    [OutputType([FsInfoCat.Util.PwHash])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptyString]
        [AllowNull]
        [string]$RawPwd,
        [System.UInt64]$Salt
    )
    Process {
        $Result = $null;
        if ($PSBoundParameters.ContainsKey('Salt')) {
            Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Invoking FsInfoCat.Util.PwHash.Create(rawPw: `"$RawPwd`", saltBits: 0x$($Salt.ToString('x8')))";
            $Result = [FsInfoCat.Util.PwHash]::Create($RawPwd, $Salt);
        } else {
            Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Invoking FsInfoCat.Util.PwHash.Create(rawPw: `"$RawPwd`")";
            $Result = [FsInfoCat.Util.PwHash]::Create($RawPwd);
        }
        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Returning $Result";
        $Result | Write-Output;
    }
}

Function Test-PasswordHash {
    <#
    .SYNOPSIS
        Checks to see if a raw password matches a password hash.

    .DESCRIPTION
        Tests whether the provided raw password is a match of the same password that was used to create the SHA512 password hash.
        
    .PARAMETER RawPwd
        The raw password to test.
        
    .PARAMETER PwHash
        FsInfoCat.Util.PwHash value to test the password against.

    .PARAMETER EncodedPwHash
        A base-64 string containing the password hash to test the password against.

    .EXAMPLE
        PS C:\> $Success = $myPw | Test-PasswordHash -PwHash $PwHash;

    .INPUTS
        The raw password to test.

    .OUTPUTS
        true if the raw password is a match; otherwise, false.
    #>
    [CmdletBinding(DefaultParameterSetName = "String")]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = "String")]
        [AllowEmptyString]
        [AllowNull]
        [string]$RawPwd,
        [Parameter(Mandatory = $true, ParameterSetName = "PwHash")]
        [AllowNull]
        [Nullable[FsInfoCat.Util.PwHash]]$PwHash,
        [Parameter(Mandatory = $true, ParameterSetName = "String")]
        [AllowEmptyString]
        [AllowNull]
        [string]$EncodedPwHash
    )

    Begin {
        $PwHashObj = $null;
        if ($PSBoundParameters.ContainsKey('String')) {
            Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Invoking FsInfoCat.Util.PwHash.Import(base64EncodedHash: `"$RawPwd`")";
            $PwHashObj = [FsInfoCat.Util.PwHash]::Import($EncodedPwHash);
        } else {
            $PwHashObj = $PwHash;
        }
        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Using $PwHashObj as the comparison SHA512 hash";
    }
    Process {
        $result = $null;
        if ([string]::IsNullOrEmpty($RawPwd)) {
            Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Testing $(if ($null -eq $RawPwd) { 'null value' } else { 'empty string' })";
            $result = ($null -eq $PwHashObj);
        } else {
            Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Testing string: `"$RawPwd`"";
            $result = ($null -ne $PwHashObj -and $PwHashObj.Test($RawPwd));
        }
        if ($null -ne $result) {
            Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Returning $result";
            $result | Write-Output;
        }
    }
}

class FsLogicalVolume {
    [string]$RootPath;
    [string]$VolumeName;
    [string]$VolumeId;
    [string]$FsName;
    [string]$RemotePath;
    [bool]$IsReadOnly;
    [bool]$IsFixed;
    [bool]$IsNetwork;
    [bool]$CaseSensitive;
}

Function Get-FsLogicalVolume {
    <#
    .SYNOPSIS
        Gets logical volumes for local host.

    .DESCRIPTION
        Detects logical volumes on the current local host machine.
        
    .INPUTS
        None

    .OUTPUTS
        FsLogicalVolume objects representing logical volumes detected on the local host.
    #>
    [CmdletBinding()]
    [OutputType([FsLogicalVolume])]
    Param()
    
    $OldErrorAction = $ErrorActionPreference;
    $ErrorActionPreference = [System.Management.Automation.ActionPreference]::Stop;
    try {
        if ($Script:IsWindows) {
            Set-Variable -Name 'TargetObject' -Value 'CIM_LogicalDisk' -Scope 0;
            Set-Variable -Name 'ErrorId' -Value 'NewCimInstanceFail' -Scope 0;
            Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ReadError) -Scope 0;
            Set-Variable -Name 'Reason' -Value "Failed to invoke Get-CimInstance -ClassName '$TargetObject'" -Scope 0;
            Write-Verbose -Message "$($PSCmdlet.MyInvocation.InvocationName): Getting logical volume listing";
            (((Get-CimInstance -ClassName $TargetObject) | ForEach-Object {
                Remove-Variable -Name 'TargetObject' -Scope 0;
                Set-Variable -Name 'TargetObject' -Value $_ -Scope 0;
                Set-Variable -Name 'ErrorId' -Value 'CimAssociatedInstanceFail' -Scope 0;
                Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ObjectNotFound) -Scope 0;
                Set-Variable -Name 'Reason' -Value "Failed to invoke Get-CimAssociatedInstance -ResultClassName 'CIM_Directory'" -Scope 0;
                Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Invoking `"Get-CimAssociatedInstance -ResultClassName 'CIM_Directory'`" on $TargetObject";
                [PsCustomObject]@{ ID = $_.DeviceID; Disk = $_; Directory = $_ | Get-CimAssociatedInstance -ResultClassName 'CIM_Directory' }
            } | Group-Object -Property 'ID') | ForEach-Object { $_.Group | Where-Object { $null -ne $_.Directory } | Select-Object -First 1 }) | ForEach-Object {
                Remove-Variable -Name 'TargetObject' -Scope 0;
                Set-Variable -Name 'TargetObject' -Value $_.Disk -Scope 0;
                Set-Variable -Name 'ErrorId' -Value 'CimAssociatedInstanceFail' -Scope 0;
                Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ObjectNotFound) -Scope 0;
                Set-Variable -Name 'Reason' -Value "Failed to invoke Get-CimAssociatedInstance -ResultClassName 'CIM_Directory'" -Scope 0;
                Write-Verbose -Message "$($PSCmdlet.MyInvocation.InvocationName): Creating new FsLogicalVolume from LogicalDisk $TargetObject and Directory $($_.Directory)";
                $LogicalDisk = $TargetObject;
                $FsLogicalVolume = [FsLogicalVolume]@{
                    RootPath = '' + $_.Directory.Name;
                    VolumeName = '' + $LogicalDisk.VolumeName;
                    VolumeId = '' + $LogicalDisk.VolumeSerialNumber;
                    FsName = '' + $LogicalDisk.FileSystem;
                    IsReadOnly = ($LogicalDisk.Access -ne 0 -and ($LogicalDisk.Access -band 2) -eq 0);
                    CaseSensitive = $false;
                };
                switch ($LogicalDisk.DriveType) {
                    1 { # No Root
                        Write-Verbose -Message "$($PSCmdlet.MyInvocation.InvocationName): Ignoring device with no root ($($LogicalDisk.ToString()))";
                        break;
                    }
                    2 { # Removable
                        $FsLogicalVolume.IsFixed = $false;
                        $FsLogicalVolume.IsNetwork = $false;
                        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Created $FsLogicalVolume";
                        $FsLogicalVolume | Write-Output;
                        break;
                    }
                    3 { # Local Disk
                        $FsLogicalVolume.IsFixed = $true;
                        $FsLogicalVolume.IsNetwork = $false;
                        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Created $FsLogicalVolume";
                        $FsLogicalVolume | Write-Output;
                        break;
                    }
                    4 { # Network Drive
                        $FsLogicalVolume.IsFixed = $false;
                        $FsLogicalVolume.IsNetwork = $true;
                        $FsLogicalVolume.RemotePath = '' + $LogicalDisk.ProviderName;
                        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Created $FsLogicalVolume";
                        $FsLogicalVolume | Write-Output;
                        break;
                    }
                    5 { # Compact Disc
                        $FsLogicalVolume.IsFixed = $false;
                        $FsLogicalVolume.IsNetwork = $false;
                        if ($LogicalDisk.Access -eq 0) { $FsLogicalVolume.IsReadOnly = $true }
                        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Created $FsLogicalVolume";
                        $FsLogicalVolume | Write-Output;
                        break;
                    }
                    6 { # Ram DisK
                        $FsLogicalVolume.IsFixed = $false;
                        $FsLogicalVolume.IsNetwork = $false;
                        Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Created $FsLogicalVolume";
                        $FsLogicalVolume | Write-Output;
                        break;
                    }
                    default { # Unknown
                        Write-Warning -Message "$($PSCmdlet.MyInvocation.InvocationName): Unable to determine drive type for $($_.ToString())";
                        break;
                    }
                }
                Remove-Variable -Name 'Reason' -Scope 0;
            }
        } else {
            if ($null -eq $Script:OptionsParseRegex) {
                $Script:OptionsParseRegex = [System.Text.RegularExpressions.Regex]::new('(^|\G,)(?<k>"[^"]*"|[^",]*)(?<v>=("[^"]*"|[^",]*))?(?=,|$)', ([System.Text.RegularExpressions.RegexOptions]::Compiled));
            }
            if ($null -eq $Script:FsTypes) {
                Set-Variable -Name 'TargetObject' -Value ($PSScriptRoot | Join-Path -ChildPath 'fstypes.json') -Scope 0;
                Set-Variable -Name 'ErrorId' -Value 'FsTypesLoadFail' -Scope 0;
                Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ReadError) -Scope 0;
                Set-Variable -Name 'Reason' -Value "Failed to read data from '$TargetObject'" -Scope 0;
                Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Loading file system type information from $TargetObject";
                $JsonText = [System.IO.File]::ReadAllText($TargetObject);
                if ([string]::IsNullOrWhiteSpace($JsonText)) {
                    Set-Variable -Name 'ErrorId' -Value 'NoFsTypesData' -Scope 0;
                    Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::InvalidData) -Scope 0;
                    Set-Variable -Name 'Reason' -Value "Failed to read data from '$TargetObject'" -Scope 0;
                } else {
                    Set-Variable -Name 'ErrorId' -Value 'FsTypesParseFail' -Scope 0;
                    Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ParserError) -Scope 0;
                    Set-Variable -Name 'Reason' -Value "Failed to parse data from '$TargetObject'" -Scope 0;
                    Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Parsing data loaded from $TargetObject`: '$JsonText'";
                    Set-Variable -Name 'TargetObject' -Value $JsonText -Scope 0;
                    $Script:FsTypes = $JsonText | ConvertFrom-Json;
                    #$Script:FsTypes = [System.IO.File]::ReadAllText("C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.PS\fstypes.json") | ConvertFrom-Json;
                }
            }

            if ($null -ne $Script:FsTypes) {
                Set-Variable -Name 'TargetObject' -Value 'findmnt -A -b -J -l -R -o SOURCE,TARGET,FSTYPE,OPTIONS,LABEL,UUID,HOTPLUG' -Scope 0;
                Set-Variable -Name 'ErrorId' -Value 'FindMntInvocationFail' -Scope 0;
                Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::OpenError) -Scope 0;
                Set-Variable -Name 'Reason' -Value "Invocation failed: '$TargetObject'" -Scope 0;
                Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Invoking '$TargetObject'";
                #$JsonText = [System.IO.File]::ReadAllText('C:\Users\lerwi\Git\FsInfoCat\findmnt.json');
                $JsonText = (findmnt -A -b -J -l -R -o SOURCE,TARGET,FSTYPE,OPTIONS,LABEL,UUID,HOTPLUG) | Out-String;
                if ([string]::IsNullOrWhiteSpace($JsonText)) {
                    Set-Variable -Name 'ErrorId' -Value 'NoFindMntOutput' -Scope 0;
                    Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ObjectNotFound) -Scope 0;
                    Set-Variable -Name 'Reason' -Value "Failed to read results of '$TargetObject'" -Scope 0;
                } else {
                    Set-Variable -Name 'ErrorId' -Value 'FindMntResultParseFail' -Scope 0;
                    Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ParserError) -Scope 0;
                    Set-Variable -Name 'Reason' -Value "Failed to parse results from '$TargetObject'" -Scope 0;
                    Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Parsing data returned from '$TargetObject': '$JsonText'";
                    Remove-Variable -Name 'TargetObject' -Scope 0;
                    Set-Variable -Name 'TargetObject' -Value $JsonText -Scope 0;
                    $FindMnt = $JsonText | ConvertFrom-Json;
                    if ($null -eq $FindMnt -or $null -eq $FindMnt.filesystems -or @($FindMnt.filesystems).Count -eq 0) {
                        Set-Variable -Name 'ErrorId' -Value 'NoMountPoints' -Scope 0;
                        Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::ObjectNotFound) -Scope 0;
                        Set-Variable -Name 'Reason' -Value "No mount points found in invocation output" -Scope 0;
                    } else {
                        $FindMnt.filesystems | Where-Object { $null -ne $_.fstype } | ForEach-Object {
                            Remove-Variable -Name 'TargetObject' -Scope 0;
                            Set-Variable -Name 'TargetObject' -Value $_ -Scope 0;
                            Set-Variable -Name 'ErrorId' -Value 'FindMntItemParseFail' -Scope 0;
                            Set-Variable -Name 'ErrorCategory' -Value ([System.Management.Automation.ErrorCategory]::InvalidResult) -Scope 0;
                            Set-Variable -Name 'Reason' -Value "Failed to read item results from '$TargetObject'" -Scope 0;
                            Write-Verbose -Message $($PSCmdlet.MyInvocation.InvocationName): "Creating new FsLogicalVolume from findmnt output $TargetObject";
                            $fstype = $_.fstype;
                            $i = $fstype.IndexOf('.');
                            if ($i -gt 0) { $fstype = $fstype.Substring(0, $i) }
                            $characteristics = $Script:FsTypes.($fstype);
                            if ($null -eq $characteristics) {
                                Write-Warning -Message "Skipping unknown file system type `"$($_.fstype)`" ($($_.target))";
                            } else {
                                if ($characteristics.isSpecial) {
                                    Write-Verbose -Message "Skipping special file system $($_.target)";
                                } else {
                                    $mc = $Script:OptionsParseRegex.Matches($_.options);
                                    $ExplicitRO = @($mc | Where-Object { $_.Groups['k'].Value -eq 'ro' -and -not $_.Groups['v'].Success}).Count -gt 0;
                                    $ExplicitRW = @($mc | Where-Object { $_.Groups['k'].Value -eq 'rw' -and -not $_.Groups['v'].Success}).Count -gt 0;
                                    if (-not ($ExplicitRO -or $ExplicitRW)) { $ExplicitRO = $characteristics.isRemovable }
                                    $FsLogicalVolume = [FsLogicalVolume]@{
                                        RootPath = '' + $_.target;
                                        VolumeName = '' + $_.label;
                                        FsName = $_.fstype;
                                        IsReadOnly = $ExplicitRO -and -not $ExplicitRW;
                                        IsFixed = -not ($_.hotplug -ne '0' -or $characteristics.isRemovable);
                                        CaseSensitive = $true;
                                    };
                                    if ($characteristics.isNetwork) {
                                        $FsLogicalVolume.RemotePath = '' + $_.source;
                                    } else {
                                        if (-not $characteristics.isRemovable) {
                                            $FsLogicalVolume.VolumeId = '' + $_.uuid;
                                        }
                                    }
                                    Write-Debug -Message "$($PSCmdlet.MyInvocation.InvocationName): Created $FsLogicalVolume";
                                    $FsLogicalVolume | Write-Output;
                                }
                            }
                            Remove-Variable -Name 'Reason' -Scope 0;
                        }
                    }
                }
            }
        }
    } catch {
        $ErrorActionPreference = $OldErrorAction;
        $ErrorRecord = $null;
        if ([FsInfoCat.PS.CoersionHelper]::TryGetErrorRecord($_, [ref]$ErrorRecord)) {
            Write-Error -ErrorRecord $ErrorRecord -CategoryReason $Reason;
        } else {
            $Exception = $null;
            if ([FsInfoCat.PS.CoersionHelper]::TryGetException($_, [ref]$Exception)) {
                Write-Error -Exception $Exception -Category $ErrorCategory -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
            } else {
                Write-Error -Message ('' + $_) -Category $Category -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
            }
        }
    } finally {
        Write-Debug -Message "Setting '$Identifier' as LocalMachineIdentifier";
        if (-not [string]::IsNullOrWhiteSpace($Reason)) {
            Write-Error -Message "Failed to detect current machine identifier: $Reason" -Category $Category -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
        }
    }
}

Function Register-FsLogicalVolume {
    <#
    .SYNOPSIS
        Registers a logical volume for crawl commands.

    .DESCRIPTION
        Utilizes output from Get-FsLogicalVolume to invoke the Register-FsVolumeInfo commmand.
        
    .PARAMETER LogicalVolume
        The logical volme to register.
        
    .PARAMETER CaseSensitive
        Forces file name lookups in the specified volume to be case-sensitive.
        
    .PARAMETER IgnoreCase
        Forces case file name lookups in the specified volume to ignore letter casing.
        
    .PARAMETER Force
        Allows existing registration to be updated even if it has already been registered. Also allows non-existant root directories to be registered.

    .PARAMETER PassThru
        Returns the IVolumeInfo object for the registered volume.

    .EXAMPLE
        PS C:\> Get-FsLogicalVolume | Register-FsLogicalVolume
        Registers all logical volumes.

    .EXAMPLE
        PS C:\> $Lv = Get-FsLogicalVolume | Where-Object { $_.RemotePath -eq '\\MyRemoteMachine\MyShare' }
        PS C:\> $Volume = Register-FsLogicalVolume -LogicalVolume -CaseSensitive -PassThru;
        Registers a specific logical volume where file name lookups are case-sensitive and returns the IVolume object.

    .OUTPUTS
        An IVolumeInfo if the Passthru switch is used.

    .NOTES
        By default, on Windows systems, volumes are assumed to ignore letter casing for file name lookups and non-windows systems are assumed to be case-senstive.
    #>
    [CmdletBinding(DefaultParameterSetName = 'IgnoreCaseOpt')]
    [OutputType([FsInfoCat.Models.Volumes.IVolumeInfo])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [FsLogicalVolume]$LogicalVolume,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'ForceCaseSensitive')]
        [switch]$CaseSensitive,

        [Parameter(ParameterSetName = 'IgnoreCaseOpt')]
        [switch]$IgnoreCase,

        [switch]$Force,

        [switch]$PassThru
    )

    Begin {
        if ($null -eq $Script:__Register_FsLogicalVolume_IdRegex) {
            Set-Variable -Name '__Register_FsLogicalVolume_IdRegex' -Value (
                [System.Text.RegularExpressions.Regex]::new('^((?<vid>[a-f\d]{8})(-(<ord>[a-f\d][a-f\d]?))?|(?<vh>[a-f\d]{4})-(?<vl>[a-f\d]{4}))$',
                ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::Compiled -bor [System.Text.RegularExpressions.RegexOptions]::IgnoreCase)))
            ) -Option Constant -Scope 'Script';
        }
    }

    Process {
        $Splat = @{ RootPathName = $LogicalVolume.RootPath; DriveFormat = $LogicalVolume.FsName; VolumeName = $LogicalVolume.VolumeName }
        if ($LogicalVolume.IsNetwork) {
            if ([string]::IsNullOrWhiteSpace($LogicalVolume.RemotePath)) {
                Write-Warning -Message "Cannot register logical volume - RemotePath missing for $LogicalVolume";
            } else {
                $Uri = $null;
                if ([Uri]::TryCreate($LogicalVolume.RemotePath, [UriKind]::Absolute, [ref]$Uri)) {
                    $Splat['Identifier'] = $Uri;
                } else {
                    Write-Warning -Message "Cannot register logical volume - Cannot create absolute URI from RemotePath for $LogicalVolume";
                }
            }
        } else {
            if ([string]::IsNullOrWhiteSpace($LogicalVolume.VolumeId)) {
                Write-Warning -Message "Cannot register logical volume - VolumeId missing or not supported for $LogicalVolume";
            } else {
                $m = $Script:__Register_FsLogicalVolume_IdRegex.Match($LogicalVolume.VolumeId);
                if ($m.Success) {
                    if ($m.Groups['ord'].Success) {
                        $Splat['Identifier'] = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new([System.UInt32]::Parse($m.Groups['vid'].Value, [System.Globalization.NumberStyles]::HexNumber),
                            [System.UInt32]::Parse($m.Groups['ord'].Value, [System.Globalization.NumberStyles]::HexNumber));
                    } else {
                        if ($m.Groups['vid'].Success) {
                            $Splat['Identifier'] = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new([System.UInt32]::Parse($m.Groups['vid'].Value, [System.Globalization.NumberStyles]::HexNumber));
                        } else {
                            $Splat['Identifier'] = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new(((
                                [System.UInt32]::Parse($m.Groups['vh'].Value, [System.Globalization.NumberStyles]::HexNumber) -shl 8
                            ) -bor [System.UInt32]::Parse($m.Groups['vl'].Value, [System.Globalization.NumberStyles]::HexNumber)));
                        }
                    }
                } else {
                    $uuid = $null;
                    if ([Guid]::TryParse($LogicalVolume.VolumeId, [ref]$uuid)) {
                        $Splat['Identifier'] = $uuid;
                    } else {
                        Write-Warning -Message "Cannot register logical volume - Unable to parse VolumeId for $LogicalVolume";
                    }
                }
            }
        }
        if ($null -ne $Splat['Identifier']) {
            if ([string]::IsNullOrWhiteSpace($Splat['VolumeName'])) { $Splat['VolumeName'] = '' + $Splat['Identifier'] }
            if ($CaseSensitive.IsPresent) {
                $Splat['CaseSensitive'] = $CaseSensitive;
            } else {
                if ($LogicalVolume.CaseSensitive -and -not $IgnoreCase.IsPresent) {
                    $Splat['CaseSensitive'] = [System.Management.Automation.SwitchParameter]::Present;
                }
            }
            if ($Force.IsPresent) { $Splat['Force'] = $Force }
            if ($PassThru.IsPresent) { $Splat['PassThru'] = $PassThru }
            Register-FsVolumeInfo @Splat;
        }
    }
}

Function Start-FileSystemCrawl {
    <#
    .SYNOPSIS
        Starts the crawl of one or more subdirectories to gather information about the files and directory structure contained within.

    .DESCRIPTION
        Starts a background job to the specified subdirectories to gather information about the files and directory structure contained within.
        The output of this command is intended to be uploaded to the FSInfoCat website.
         
    .PARAMETER RootPath
        The starting subdirectory to crawl (supports wildcards).
      
    .PARAMETER LiteralPath
        The literal path(s) of the starting subdirectory to crawl.
      
    .PARAMETER MaxDepth
        Maximum depth of sub-folders to crawl. This can be used to mitigate possible endless recursion due to the possiblity of a symbolic link referencing a parent folder.
        A value less than 1 only crawls the root path. The default is 32.
      
    .PARAMETER MaxItems
        Maximum number of files and subdirectories to process. The default is 4,294,967,295. This applies to the entire crawl job (as opposed to each starting subdirectory).
        This can be used to help limit the size of the package to be uploaded to the FsInfoCat website.
      
    .PARAMETER Ttl
        The maximum number of minutes to allow the background crawl job to run. The default value is 4,320 (3 days) unless the StopAt parameter or NoExpire switch is used.
      
    .PARAMETER StopAt
        Date and Time when the background crawl job will be stopped if it has not already terminated.
      
    .PARAMETER NoExpire
        Job does not expire and continues to crawl files util it is completed, is explicitly stopped, or it encounters a terminal error.
      
    .PARAMETER Name
        Specifies a friendly name for the new job. You can use the name to identify the job to other job cmdlets, such as the Stop-Job cmdlet.
      
    .EXAMPLE
        PS C:\> $id = Get-LocalMachineIdentifier

    .OUTPUTS
        The string value of the SID or UUID that uniquely identifies the current host machine.

    .NOTES
        On Windows Systems, the machine SID is determined through this SID of the local administrator account. The local administrator account is retrieved by using the
        Microsoft.PowerShell.Management module to invoke a CIM query.
        On Linux and OSX systems, the contents of the local machine UUID is determined by reading the contents of the /etc/machine-id file.
    #>
    [CmdletBinding(DefaultParameterSetName = "age:true")]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = "none:true")]
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = "age:true")]
        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = "datetime:true")]
        [string]$RootPath,
        
        [Parameter(Mandatory = $true, ParameterSetName = "none:false")]
        [Parameter(Mandatory = $true, ParameterSetName = "age:false")]
        [Parameter(Mandatory = $true, ParameterSetName = "datetime:false")]
        [string]$LiteralPath,
        
        [int]$MaxDepth,

        [long]$MaxItems,
        
        [Parameter(ParameterSetName = "age:true")]
        [Parameter(Mandatory = $true, ParameterSetName = "age:false")]
        [long]$Ttl,
        
        [Parameter(Mandatory = $true, ParameterSetName = "datetime:true")]
        [Parameter(Mandatory = $true, ParameterSetName = "datetime:false")]
        [ValidateNotNull()]
        [DateTime]$StopAt,
        
        [Parameter(Mandatory = $true, ParameterSetName = "none:true")]
        [Parameter(Mandatory = $true, ParameterSetName = "none:false")]
        [switch]$NoExpire,

        [string]$Name
    )
    
    [FsLogicalVolume[]]$LogicalVolumes = Get-FsLogicalVolume;
}

Function Get-LocalMachineIdentifier {
    <#
    .SYNOPSIS
        Get unique identifier for local machine.

    .DESCRIPTION
        Gets the SID or UUID that will be used as the unique identifier of the current host machine.

    .EXAMPLE
        PS C:\> $id = Get-LocalMachineIdentifier

    .OUTPUTS
        The string value of the SID or UUID that uniquely identifies the current host machine.

    .NOTES
        On Windows Systems, the machine SID is determined through this SID of the local administrator account. The local administrator account is retrieved by using the
        Microsoft.PowerShell.Management module to invoke a CIM query.
        On Linux and OSX systems, the contents of the local machine UUID is determined by reading the contents of the /etc/machine-id file.
    #>
    [CmdletBinding()]
    Param()
    
    if ([string]::IsNullOrWhiteSpace($Script:LocalMachineIdentifier)) {
        Write-Warning -Message 'Module initialization failed - no machine identifier to return';
    } else {
        $Script:LocalMachineIdentifier | Write-Output;
    }
}
