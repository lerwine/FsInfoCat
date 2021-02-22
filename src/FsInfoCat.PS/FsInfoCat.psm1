Set-Variable -Name 'IsWindows' -Option Constant -Scope 'Script' -Value ($PSVersionTable.PSEdition -eq 'Desktop' -or $PSVersionTable.Platform -eq 'Win32NT');

Function Get-ExceptionObject {
    [CmdletBinding()]
    [OutputType([System.Exception])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptycollection]
        [AllowEmptyString]
        [AllowNull]
        [object]$InputObject,

        [switch]$Immediate
    )

    Process {
        $Exception = $null;
        if ($InputObject -is [System.Exception]) {
            $Exception = $InputObject;
        } else {
            $ErrorRecord = $null;
            if ($InputObject -is [System.Management.Automation.ErrorRecord]) {
                $ErrorRecord = $InputObject;
            } else {
                if ($InputObject -is [System.Management.Automation.IContainsErrorRecord]) {
                    $ErrorRecord = ([System.Management.Automation.IContainsErrorRecord]$InputObject).ErrorRecord;
                }
            }
            if ($null -ne $ErrorRecord) { $Exception = $ErrorRecord.Exception }
        }
        if ($null -ne $Exception) {
            if (-not $Immediate.IsPresent) {
                while ($Exception -is [System.Management.Automation.RuntimeException] -and $null -ne $Exception.InnerException) {
                    $Exception = $Exception.InnerException;
                }
            }
            $Exeption | Write-Output;
        }
    }
}

Function Get-ErrorRecord {
    [CmdletBinding()]
    [OutputType([System.Management.Automation.ErrorRecord])]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowEmptycollection]
        [AllowEmptyString]
        [AllowNull]
        [object]$InputObject,
        
        [string]$Message,

        [string]$ErrorId = 'UnexpectedException',

        [System.Management.Automation.ErrorCategory]$Category = [System.Management.Automation.ErrorCategory]::NotSpecified,
        
        [AllowEmptycollection]
        [AllowEmptyString]
        [AllowNull]
        [object]$TargetObject
    )

    Process {
        [System.Management.Automation.ErrorRecord]$ErrorRecord = $null;
        $Exception = $null;
        if ($InputObject -is [System.Management.Automation.ErrorRecord]) {
            $ErrorRecord = $InputObject;
            $Exception = $ErrorRecord.Exception;
        } else {
            if ($InputObject -is [System.Management.Automation.IContainsErrorRecord]) {
                $ErrorRecord = $InputObject.ErrorRecord;
            }
            if ($null -eq $ErrorRecord) {
                if ($InputObject -is [System.Exception]) {
                    $Exception = $InputObject;
                } else {
                    $m = '' + $InputObject;
                    if ([string]::IsNullOrWhiteSpace($m)) { $m = 'Unexpected error' }
                    $Exception = [System.Exception]::new($m);
                }
            } else {
                $Exception = $ErrorRecord.Exception;
            }
        }
        
        $CreateNewErrorRecord = $null -eq $ErrorRecord;
        $m = '';
        $i = $ErrorId;
        $c = $Category;
        $t = $null;
        if ($CreateNewErrorRecord) {
            if ($PSBoundParameters.ContainsKey('TargetObject')) { $t = $TargetObject }
            if ($PSBoundParameters.ContainsKey('Message')) { $m = $Message }
        } else {
            if ($PSBoundParameters.ContainsKey('TargetObject')) {
                $t = $TargetObject;
                $CreateNewErrorRecord = $t -ne $ErrorRecord.TargetObject;
            } else {
                $t = $ErrorRecord.TargetObject;
            }
            if ($PSBoundParameters.ContainsKey('ErrorId')) {
                $CreateNewErrorRecord = $CreateNewErrorRecord -or $i -ne $ErrorRecord.FullyQualifiedErrorId;
            } else {
                $i = $ErrorRecord.FullyQualifiedErrorId;
            }
            if ($PSBoundParameters.ContainsKey('Category')) {
                $CreateNewErrorRecord = $CreateNewErrorRecord -or $c -ne $ErrorRecord.CategoryInfo.Category;
            } else {
                $Category = $ErrorRecord.CategoryInfo.Category;
            }
            if ($PSBoundParameters.ContainsKey('Message')) {
                $m = $Message
                $CreateNewErrorRecord = $CreateNewErrorRecord -or $m -ne $ErrorRecord.ToString();
            } else {
                $m = $ErrorRecord.ToString();
            }
        }

        if ($CreateNewErrorRecord) {
            if ([string]::IsNullOrWhiteSpace($m)) {
                if ([string]::IsNullOrWhiteSpace($Exception.Message)) {
                    $m = '' + $InputObject;
                    if ([string]::IsNullOrWhiteSpace($m)) { $m = 'Unexpected error' }
                } else {
                    $m = $Exception.Message;
                }
            }
            if ($null -ne $t -or $null -eq $InputObject) {
                [System.Management.Automation.ErrorRecord]::new($Exception, $i, $c, $t) | Write-Output;
            } else {
                if ($null -ne $InputObject.ItemName) {
                    [System.Management.Automation.ErrorRecord]::new($Exception, $i, $c, $InputObject.ItemName) | Write-Output;
                } else {
                    if ($null -ne $InputObject.FileName) {
                        [System.Management.Automation.ErrorRecord]::new($Exception, $i, $c, $InputObject.FileName) | Write-Output;
                    } else {
                        if ($null -ne $InputObject.CommandName) {
                            [System.Management.Automation.ErrorRecord]::new($Exception, $i, $c, $InputObject.CommandName) | Write-Output;
                        } else {
                            if ($null -ne $InputObject.ParamName) {
                                [System.Management.Automation.ErrorRecord]::new($Exception, $i, $c, $InputObject.ParamName) | Write-Output;
                            } else {
                                [System.Management.Automation.ErrorRecord]::new($Exception, $i, $c, $null) | Write-Output;
                            }
                        }
                    }
                }
            }
        } else {
            $ErrorRecord | Write-Output;
        }
    }
}

Write-Verbose -Message "Performing $(if ($PSVersionTable.PSEdition -eq 'Desktop') { 'Desktop edition' } else { "$($PSVersionTable.Platform) platform" })-specific initialization";
&{
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
        $ErrorRecord = Get-ErrorRecord -InputObject $_ -ErrorId $ErrorId -Category $Category -TargetObject $TargetObject -Reason $Reason;
        Write-Error -ErrorRecord $ErrorRecord -CategoryReason $Reason;
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
}

Function Get-FsLogicalVolume {
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
                Write-Verbose -Message $($PSCmdlet.MyInvocation.InvocationName): "Creating new FsLogicalVolume from LogicalDisk $TargetObject and Directory $($_.Directory)";
                $LogicalDisk = $TargetObject;
                $FsLogicalVolume = [FsLogicalVolume]@{
                    RootPath = $_.Directory.Name;
                    VolumeName = $LogicalDisk.VolumeName;
                    VolumeId = $LogicalDisk.VolumeSerialNumber;
                    FsName = $LogicalDisk.FileSystem;
                    IsReadOnly = ($LogicalDisk.Access -ne 0 -and ($LogicalDisk.Access -band 2) -eq 0);
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
                        $FsLogicalVolume.RemotePath = $LogicalDisk.ProviderName;
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
                                        RootPath = $_.target;
                                        VolumeName = $_.label;
                                        FsName = $_.fstype;
                                        IsReadOnly = $ExplicitRO -and -not $ExplicitRW;
                                        IsFixed = -not ($_.hotplug -ne '0' -or $characteristics.isRemovable);
                                    };
                                    if ($characteristics.isNetwork) {
                                        $FsLogicalVolume.RemotePath = $_.source;
                                    } else {
                                        if (-not $characteristics.isRemovable) {
                                            $FsLogicalVolume.VolumeId = $_.uuid;
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
        $ErrorRecord = Get-ErrorRecord -InputObject $_ -ErrorId $ErrorId -Category $Category -TargetObject $TargetObject -Reason $Reason;
        Write-Error -ErrorRecord $ErrorRecord -CategoryReason $Reason;
    } finally {
        Write-Debug -Message "Setting '$Identifier' as LocalMachineIdentifier";
        if (-not [string]::IsNullOrWhiteSpace($Reason)) {
            Write-Error -Message "Failed to detect current machine identifier: $Reason" -Category $Category -ErrorId $ErrorId -CategoryReason $Reason -TargetObject $TargetObject;
        }
    }
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
    #>
    [CmdletBinding()]
    Param()
    
    if ([string]::IsNullOrWhiteSpace($Script:LocalMachineIdentifier)) {
        Write-Warning -Message 'Module initialization failed - no machine identifier to return';
    } else {
        $Script:LocalMachineIdentifier | Write-Output;
    }
}

Function ConvertTo-FsVolumeInfo {
    [CmdletBinding()]
    [OutputType([FsInfoCat.Models.Crawl.FsRoot])]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [FsLogicalVolume]$LogicalVolume,

        [switch]$Force
    )

    Process {
        $IsValid = $true;
        $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
        $FsRoot.DriveFormat = $LogicalVolume.FsName;
        if ([string]::IsNullOrWhiteSpace($FsRoot.DriveFormat)) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlWarning]::new("Drive format was not specified",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Drive format was not specified" -Category InvalidResult -ErrorId 'NoDriveFormat' -TargetObject $LogicalVolume;
            }
        }
        $FsRoot.VolumeName = $LogicalVolume.VolumeName;
        if ([string]::IsNullOrWhiteSpace($FsRoot.VolumeName)) { $FsRoot.VolumeName = ''; }
        $VolumeIdentifier = [FsInfoCat.Models.Volumes.VolumeIdentifier]::new([Guid]::Empty);
        $idObj = $LogicalVolume.VolumeId;
        if ([string]::IsNullOrWhiteSpace($idObj)) { $idObj = $LogicalVolume.RemotePath }
        if (-not [FsInfoCat.Models.Volumes.VolumeIdentifier]::TryCreate($idObj, [ref]$VolumeIdentifier)) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Unable to parse volume serial number",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Unable to parse volume serial number" -Category InvalidResult -ErrorId 'InvalidSerialNumber' -TargetObject $LogicalVolume;
            }
        }
        $FsRoot.Identifier = $VolumeIdentifier;
        try {
            [System.IO.DriveType]$FsRoot.DriveType = $LogicalVolume.DriveType;
        } catch {
            if ($Force.IsPresent) {
                if ($_ -is [System.Exception]) {
                    $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                } else {
                    if ($_.Exception -is [System.Exception]) {
                        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_.Exception, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                    } else {
                        $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new('' + $_, [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                    }
                }
            } else {
                $IsValid = $false;
                if ($_ -is [System.Exception]) {
                    Write-Error -Exception $_ -Message "Unable to discern drive type" -Category InvalidResult -ErrorId 'InvalidDriveType' -TargetObject $LogicalVolume;
                } else {
                    if ($_ -is [System.Management.Automation.ErrorRecord]) {
                        Write-Error -ErrorRecord $_ -CategoryReason "Invalid Drive Type";
                    } else {
                        Write-Error -Message "Unable to discern drive type: $_" -Category InvalidResult -ErrorId 'InvalidDriveType' -TargetObject $LogicalVolume;
                    }
                }
            }
        }
        if ($FsRoot.DriveType -eq [System.IO.DriveType]::NoRootDirectory) {
            if ($Force.IsPresent) {
                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Incompatible drive type (no root directory)",
                    [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
            } else {
                $IsValid = $false;
                Write-Error -Message "Incompatible drive type (no root directory)" -Category InvalidResult -ErrorId 'NoRootDirectory' -TargetObject $LogicalVolume;
            }
        } else {
            if ([string]::IsNullOrWhiteSpace($LogicalVolume.RootPath)) {
                if ($Force.IsPresent) {
                    $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("Could not determine root directory",
                        [FsInfoCat.Models.Crawl.MessageId]::AttributesAccessError));
                } else {
                    $IsValid = $false;
                    Write-Error -Message "Could not determine root directory" -Category InvalidResult -ErrorId 'NoRootDirectory' -TargetObject $LogicalVolume;
                }
            } else {
                $FsRoot.RootPathName = $LogicalVolume.RootPath;
            }
        }
        if ($IsValid) { $FsRoot | Write-Output }
    }
}

Function Get-FsVolumeInfo {
    <#
    .SYNOPSIS
        Get filesystem volume information.

    .DESCRIPTION
        Gets information about system volumes.
        
    .PARAMETER Path
        The root path name(s) of the volumes to search for.
        
    .PARAMETER Force
        Returns a FsInfoCat.Models.Crawl.FsRoot for each path, even if a match is not found. The messages property will contain information about any errors encountered.

    .PARAMETER All
        Returns information for all volumes. This is the default behavior if the Path parameter is not used.

    .EXAMPLE
        PS C:\> $VolumeInfo = Get-FsVolumeInfo -Path 'D:\';

    .OUTPUTS
        VolumeInfo object(s) that contain information about file system volumes
    #>
    [CmdletBinding(DefaultParameterSetName = 'All')]
    [OutputType([FsInfoCat.Models.Crawl.FsRoot])]
    Param(
        [Parameter(ValueFromPipeline = $true, ParameterSetName = 'Explicit')]
        [string[]]$Path,

        [Parameter(ParameterSetName = 'Explicit')]
        [switch]$Force,

        [Parameter(ParameterSetName = 'All')]
        [switch]$All
    )

    if ($PSBoundParameters.ContainsKey('Path')) {
        [FsLogicalVolume[]]$AllVolumes = Get-FsLogicalVolume;
        if ($Force.IsPresent) {
        } else {
        }
    } else {
        Get-FsLogicalVolume | ConvertTo-FsVolumeInfo;
    }
    if ($Script:IsWindows) {
        $LogicalDiskAndRoot = @($LogicalDiskCollection | ForEach-Object {
            $Directory = $_ | Get-CimAssociatedInstance -ResultClassName 'CIM_Directory';
            if ($null -ne $Directory -and -not [string]::IsNullOrWhiteSpace($Directory.Name)) {
                [PsCustomObject]@{
                    RootPath = $Directory.Name;
                    LogicalDisk = $_;
                };
            }
        });
        if ($PSBoundParameters.ContainsKey('Path')) {
            if ($Force.IsPresent) {
                $Path | ForEach-Object {
                    $dr = $PathRoot = $null;
                    $p = $_;
                    try {
                        $PathRoot = [System.IO.Path]::GetPathRoot([System.IO.Path]::GetFullPath($p));
                        $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ceq $_.RootPath } | Select-Object -First 1;
                        if ($null -eq $dr) {
                            $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ieq $_.RootPath } | Select-Object -First 1;
                        }
                    } catch {
                        $dr = $PathRoot = $null;
                        $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
                        $FsRoot.RootPathName = $p;
                        if ($_ -is [System.Exception]) {
                            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                        } else {
                            if ($_.Exception -is [System.Exception]) {
                                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_.Exception, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                            } else {
                                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new('' + $_, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                            }
                        }
                        $FsRoot | Write-Output;
                    }
                    if ($null -ne $dr) {
                        ($dr | ConvertTo-FsVolumeInfo -Force) | Write-Output;
                    } else {
                        if ($null -ne $PathRoot) {
                            $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
                            $FsRoot.RootPathName = $PathRoot;
                            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("No matching volume found",
                                [FsInfoCat.Models.Crawl.MessageId]::PathNotFound));
                            $FsRoot | Write-Output;
                        }
                    }
                }
            } else {
                $Path | ForEach-Object {
                    $dr = $null;
                    $p = $_;
                    try {
                        $PathRoot = [System.IO.Path]::GetPathRoot([System.IO.Path]::GetFullPath($p));
                        $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ceq $_.RootPath } | Select-Object -First 1;
                        if ($null -eq $dr) {
                            $dr = $LogicalDiskAndRoot | Where-Object { $PathRoot -ieq $_.RootPath } | Select-Object -First 1;
                        }
                    } catch {
                        if ($_ -is [System.Exception]) {
                            Write-Error -Exception $_ -Message "Unable to determine root path" -Category InvalidArgument -ErrorId 'InvalidRootPath' -TargetObject $p;
                        } else {
                            if ($_ -is [System.Management.Automation.ErrorRecord]) {
                                Write-Error -ErrorRecord $_ -CategoryReason "Invalid Root Path";
                            } else {
                                Write-Error -Message "Unable to discern drive type: $_" -Category InvalidArgument -ErrorId 'InvalidRootPath' -TargetObject $p;
                            }
                        }
                        $dr = $null;
                    }
                    if ($null -ne $dr) {
                        ($dr | ConvertTo-FsVolumeInfo) | Write-Output;
                    }
                }
            }
        } else {
            if ($Force.IsPresent) {
                ((Get-CimInstance -ClassName 'CIM_LogicalDisk') | ConvertTo-FsVolumeInfo -Force) | Write-Output;
            } else {
                ((Get-CimInstance -ClassName 'CIM_LogicalDisk') | ConvertTo-FsVolumeInfo) | Write-Output;
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('Path')) {
            if ($Force.IsPresent) {
                $Path | ForEach-Object {
                    $dr = $PathRoot = $null;
                    $p = $_;
                    try {
                        # TODO: Create Get-FsPathRoot function
                        $PathRoot = [System.IO.Path]::GetFullPath($p) | Get-FsPathRoot;
                        $dr = $BlockDevices | Where-Object { $PathRoot -ceq $_.mountpoint } | Select-Object -First 1;
                    } catch {
                        $dr = $PathRoot = $null;
                        $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
                        $FsRoot.RootPathName = $p;
                        if ($_ -is [System.Exception]) {
                            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                        } else {
                            if ($_.Exception -is [System.Exception]) {
                                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new($_.Exception, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                            } else {
                                $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new('' + $_, [FsInfoCat.Models.Crawl.MessageId]::InvalidPath));
                            }
                        }
                        $FsRoot | Write-Output;
                    }
                    if ($null -ne $dr) {
                        # TODO: Modify ConvertTo-FsVolumeInfo for linux to accept JSON ojbect. Maybe create a custom Select-Object?
                        ($dr | ConvertTo-FsVolumeInfo -Force) | Write-Output;
                    } else {
                        if ($null -ne $PathRoot) {
                            $FsRoot = [FsInfoCat.Models.Crawl.FsRoot]::new();
                            $FsRoot.RootPathName = $PathRoot;
                            $FsRoot.Messages.Add([FsInfoCat.Models.Crawl.CrawlError]::new("No matching volume found",
                                [FsInfoCat.Models.Crawl.MessageId]::PathNotFound));
                            $FsRoot | Write-Output;
                        }
                    }
                }
            } else {
                $Path | ForEach-Object {
                    $dr = $null;
                    $p = $_;
                    try {
                        $PathRoot = [System.IO.Path]::GetFullPath($p) | Get-FsPathRoot;
                        $dr = $BlockDevices | Where-Object { $PathRoot -ceq $_.mountpoint } | Select-Object -First 1;
                    } catch {
                        if ($_ -is [System.Exception]) {
                            Write-Error -Exception $_ -Message "Unable to determine root path" -Category InvalidArgument -ErrorId 'InvalidRootPath' -TargetObject $p;
                        } else {
                            if ($_ -is [System.Management.Automation.ErrorRecord]) {
                                Write-Error -ErrorRecord $_ -CategoryReason "Invalid Root Path";
                            } else {
                                Write-Error -Message "Unable to discern drive type: $_" -Category InvalidArgument -ErrorId 'InvalidRootPath' -TargetObject $p;
                            }
                        }
                        $dr = $null;
                    }
                    if ($null -ne $dr) {
                        ($dr | ConvertTo-FsVolumeInfo) | Write-Output;
                    }
                }
            }
        } else {
            if ($Force.IsPresent) {
                ($BlockDevices | ConvertTo-FsVolumeInfo -Force) | Write-Output;
            } else {
                ($BlockDevices | ConvertTo-FsVolumeInfo) | Write-Output;
            }
        }
    }
}
