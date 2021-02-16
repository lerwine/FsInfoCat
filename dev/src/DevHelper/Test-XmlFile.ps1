<#
.SYNOPSIS
    Validates XML files.

.DESCRIPTION
    Validates XML files against a schema set.

.PARAMETER ConformanceLevel
    Sets the level of conformance checking.
    This can be Document, Fragment or Auto. The default is Document.

.PARAMETER ValidationType
    Determines whether the XmlReader will perform validation or type assignment when reading.
    This can be None, Auto, DTD, XDR, or Schema. The default is None.

.PARAMETER NoCheckCharacters
    Do not do character checking.

.PARAMETER IgnoreDtd
    Ignore DTDs.

.PARAMETER IgnoreComments
    Ignore comments.

.PARAMETER IgnoreProcessingInstructions
    Ignore processing instructions.

.PARAMETER IgnoreWhitespace
    Ignore whitespace.

.PARAMETER AllowDtd
    Allow DTD processing.

.PARAMETER DisallowXmlAttributes
    Do not allow "xml:*" attributes.

.PARAMETER IgnoreIdentityConstraints
    Do not process identity constraints (xs:ID, xs:IDREF, xs:key, xs:keyref, xs:unique).

.PARAMETER ProcessInlineSchema
    Process inline schemas encountered during validation.

.PARAMETER ProcessSchemaLocation
    Process schema location hints (xsi:schemaLocation, xsi:noNamespaceSchemaLocation) encountered during validation.

.PARAMETER ReportValidationWarnings
    Report schema validation warnings encountered during validation.

.PARAMETER Schema
    Specifies the XML schemas to load.
    This refer to paths of single XmlSchema files or to subdirectories containing schema (*.xsd) files.
    This will not recursively search nested subdirectories.

.PARAMETER InputFile
    The path(s) of the XML files to validate.

.INPUTS
    None. You cannot pipe objects to Add-Extension.

.OUTPUTS
    System.String. Test-XmlFile returns validation result messages.

.EXAMPLE
    # Validates XML file, auto-detecting schemas.
    PS> . Test-XmlFile.ps1 'MyData.xml'

.EXAMPLE
    # Validates XML file against a specific XML schema.
    PS> . Test-XmlFile.ps1 -Schema 'MyData.xsd' 'MyData.xml'

.EXAMPLE
    # Validates mutiple XML file against a specific XML schema set.
    PS> . Test-XmlFile.ps1 -Schema 'Common.xsd','MyData.xsd' 'MyData1.xml' 'MyData2.xml'
#>
Param(
    [string[]]$Schema,

    [System.Xml.ConformanceLevel]$ConformanceLevel = [System.Xml.ConformanceLevel]::Document,

    [System.Xml.ValidationType]$ValidationType = [System.Xml.ValidationType]::None,

    [switch]$NoCheckCharacters,

    [switch]$IgnoreDtd,

    [switch]$IgnoreComments,

    [switch]$IgnoreProcessingInstructions,

    [switch]$IgnoreWhitespace,

    [switch]$AllowDtd,

    [switch]$DisallowXmlAttributes,

    [switch]$IgnoreIdentityConstraints,

    [switch]$ProcessInlineSchema,

    [switch]$ProcessSchemaLocation,

    [switch]$ReportValidationWarnings,

    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$InputFile = @('C:\Users\lerwi\Git\FsInfoCat\dev\src\DevHelper\PsHelpIntermediate.xml')
)

if ($null -eq $InputFile -or $InputFile.Length -eq 0) {
    Write-Warning -Message 'No files to process.';
    return;
}

class ValidationListener {
    [int]$ErrorCount;
    [int]$WarningCount;
    hidden [string]$FileName;
    hidden [System.Management.Automation.Host.PSHostUserInterface]$UI;

    ValidationListener([string]$FileName, [System.Management.Automation.Host.PSHostUserInterface]$ui) {
        $this.UI = $ui;
    }

    [void]OnValidationEvent([object]$s, [System.Xml.Schema.ValidationEventArgs]$e)
    {
        if ($e.Severity -eq [System.Xml.Schema.XmlSeverityType]::Error) {
            $this.ErrorCount++;
            $this.UI.WriteErrorLine("$($this.FileName)`: $($e.Message)");
        } else {
            $this.WarningCount++;
            $this.UI.WriteWarningLine("$($this.FileName)`: $($e.Message)");
        }
    }
}

Function Test-XmlFile {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$FileName,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlReaderSettings]$Settings
    )

    Begin {
        $InputFileNumber = 0;
        $Success = $true;
    }

    Process {
        ++$InputFileNumber;
        $XmlReaderSettings = $Settings.Clone();
        $ValidationListener = [ValidationListener]::new($FileName, $Host.UI);
        $XmlReaderSettings.add_ValidationEventHandler($ValidationListener.OnValidationEvent);
        $XmlReader = [System.Xml.XmlReader]::Create($FileName, $XmlReaderSettings);
        try {
            while ($XmlReader.Read()) { }
        } finally { $XmlReader.Close() }
        if ($Success) {
            $Success = ($ValidationListener.ErrorCount -eq 0);
        }
    }

    End { $Success | Write-Output }
}

Function Read-PreloadedXmlSchema {
    [CmdletBinding()]
    Param(
        [Parameter(ValueFromPipeline = $true)]
        [string]$Path
    )

    Begin {
        $XmlPreloadedResolver = [System.Xml.Resolvers.XmlPreloadedResolver]::new([System.Xml.XmlUrlResolver]::new());
    }

    Process {
        $oldErrorAction = $ErrorActionPreference;
        try {
            if ($Path | Test-Path -PathType Leaf) {
                $SilentFail = $false;
                try {
                    $count = $XmlPreloadedResolver.PreloadedUris.Count;
                    $XmlPreloadedResolver.Add([Uri]::new($Path), [System.IO.File]::OpenRead($Path));
                    $SilentFail =  ($count -eq $XmlPreloadedResolver.PreloadedUris.Count);
                } catch {
                    if ($_ -is [System.Management.Automation.ErrorRecord]) {
                        Write-Error -ErrorRecord $_ -CategoryActivity 'Pre-loading XML schemas';
                    } else {
                        if ($_ -is [System.Exception]) {
                            if ([string]::IsNullOrWhiteSpace($_.Message)) {
                                Write-Error -Exception $_ -Message "Error loading schema file '$Path'" -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                            } else {
                                Write-Error -Exception $_ -Message $_.Message -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                            }
                        } else {
                            $Message = '' + $_;
                            if ([string]::IsNullOrWhiteSpace($Message)) {
                                Write-Error -Message "Error loading schema file '$Path'" -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                            } else {
                                Write-Error -Message $Message -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                            }
                        }
                    }
                }
                if ($SilentFail) {
                    Write-Error -Message "Error loading schema file '$Path'" -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                }
            } else {
                if ($Path | Test-Path -PathType Container) {
                    $Files = @();
                    try { $Files = @(Get-ChildItem -Path $Path -Filter '*.xsd' -File -ErrorAction Stop); }
                    catch {
                        if ($_ -is [System.Management.Automation.ErrorRecord]) {
                            Write-Error -ErrorRecord $_ -CategoryActivity 'Pre-loading XML schemas';
                        } else {
                            if ($_ -is [System.Exception]) {
                                if ([string]::IsNullOrWhiteSpace($_.Message)) {
                                    Write-Error -Exception $_ -Message "Error getting file listing" -Category ReadError -ErrorId 'GetChildItemError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                                } else {
                                    Write-Error -Exception $_ -Message $_.Message -Category ReadError -ErrorId 'GetChildItemError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                                }
                            } else {
                                $Message = '' + $_;
                                if ([string]::IsNullOrWhiteSpace($Message)) {
                                    Write-Error -Message "Error getting file listing" -Category ReadError -ErrorId 'GetChildItemError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                                } else {
                                    Write-Error -Message $Message -Category ReadError -ErrorId 'GetChildItemError' -TargetObject $Path -CategoryActivity 'Pre-loading XML schemas';
                                }
                            }
                        }
                    }
                    if ($Files.Count -gt 0) {
                        $Files | ForEach-Object {
                            $FullName = $_.FullName;
                            $SilentFail = $false;
                            try {
                                $count = $XmlPreloadedResolver.PreloadedUris.Count;
                                $XmlPreloadedResolver.Add([Uri]::new($FullName), [System.IO.File]::OpenRead($FullName));
                                $SilentFail =  ($count -eq $XmlPreloadedResolver.PreloadedUris.Count);
                            } catch {
                                if ($_ -is [System.Management.Automation.ErrorRecord]) {
                                    Write-Error -ErrorRecord $_ -CategoryActivity 'Pre-loading XML schemas';
                                } else {
                                    if ($_ -is [System.Exception]) {
                                        if ([string]::IsNullOrWhiteSpace($_.Message)) {
                                            Write-Error -Exception $_ -Message "Error loading schema file '$FullName'" -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $FullName -CategoryActivity 'Pre-loading XML schemas';
                                        } else {
                                            Write-Error -Exception $_ -Message $_.Message -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $FullName -CategoryActivity 'Pre-loading XML schemas';
                                        }
                                    } else {
                                        $Message = '' + $_;
                                        if ([string]::IsNullOrWhiteSpace($Message)) {
                                            Write-Error -Message "Error loading schema file '$FullName'" -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $FullName -CategoryActivity 'Pre-loading XML schemas';
                                        } else {
                                            Write-Error -Message $Message -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $FullName -CategoryActivity 'Pre-loading XML schemas';
                                        }
                                    }
                                }
                            }
                            if ($SilentFail) {
                                Write-Error -Message "Error loading schema file '$FullName'" -Category ReadError -ErrorId 'ResolverAddError' -TargetObject $FullName -CategoryActivity 'Pre-loading XML schemas';
                            }
                        }
                    }
                } else {
                    Write-Error -Message "File '$Path' not found." -Category ObjectNotFound -ErrorId 'pathNotFound' -TargetObject $Path -CategoryActivity 'Pre-load Schemas';
                }
            }
        } finally {
            $ErrorActionPreference = $oldErrorAction;
        }
        $SchemaPaths = @((Get-Location).Path);
        if ($PSBoundParameters.ContainsKey('Schema')) { $SchemaPaths += @($Schema) }
        if ($SchemaPaths.Count -gt 0) {
            $oldErrorAction = $ErrorActionPreference;
            $ErrorActionPreference = [System.Management.Automation.ActionPreference]::SilentlyContinue;
            try {
                $SchemaPaths | Where-Object {
                    try {
                        $XmlPreloadedResolver.Add([Uri]::new($_.FullName), [System.IO.File]::OpenRead($_.FullName));
                    } catch { <# ignored on purpose #> }
                }
            } finally {
                $ErrorActionPreference = $oldErrorAction;
            }
            if ($XmlPreloadedResolver.PreloadedUris.Count -gt 0) {
                $XmlReaderSettings.XmlResolver = $XmlPreloadedResolver;
            } else {
                $XmlReaderSettings.XmlResolver = $XmlUrlResolver;
            }
        } else {
            $XmlReaderSettings.XmlResolver = $XmlUrlResolver;
        }
    }

    End {
        if ($XmlPreloadedResolver.PreloadedUris.Count -gt 0) {
            Write-Output -InputObject $XmlPreloadedResolver -NoEnumerate;
        } else {
            Write-Output -InputObject ([System.Xml.XmlUrlResolver]::new()) -NoEnumerate;
        }
    }
}

$XmlReaderSettings = [System.Xml.XmlReaderSettings]::new();
$XmlReaderSettings.CheckCharacters = -not $NoCheckCharacters.IsPresent;
$XmlReaderSettings.ConformanceLevel = $ConformanceLevel;
if ($IgnoreDtd.IsPresent) {
    $XmlReaderSettings.DtdProcessing = [System.Xml.DtdProcessing]::Ignore;
} else {
    $XmlReaderSettings.DtdProcessing = [System.Xml.DtdProcessing]::Prohibit;
}
$XmlReaderSettings.IgnoreComments = $IgnoreComments.IsPresent;
$XmlReaderSettings.IgnoreProcessingInstructions = $IgnoreComments.IsPresent;
$XmlReaderSettings.IgnoreWhitespace = $IgnoreComments.IsPresent;
$XmlReaderSettings.ProhibitDtd = -not $AllowDtd.IsPresent;
if ($DisallowXmlAttributes.IsPresent) {
    $XmlReaderSettings.ValidationFlags = [System.Xml.Schema.XmlSchemaValidationFlags]::None;
} else {
    $XmlReaderSettings.ValidationFlags = [System.Xml.Schema.XmlSchemaValidationFlags]::AllowXmlAttributes;
}
if (-not $IgnoreIdentityConstraints.IsPresent) {
    $XmlReaderSettings.ValidationFlags = $XmlReaderSettings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessIdentityConstraints;
}
if ($ProcessInlineSchema.IsPresent) {
    $XmlReaderSettings.ValidationFlags = $XmlReaderSettings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessInlineSchema;
}
if ($ProcessSchemaLocation.IsPresent) {
    $XmlReaderSettings.ValidationFlags = $XmlReaderSettings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessSchemaLocation;
}
if ($ReportValidationWarnings.IsPresent) {
    $XmlReaderSettings.ValidationFlags = $XmlReaderSettings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ReportValidationWarnings;
}
$XmlReaderSettings.ValidationType = $ValidationType;
if ($PSBoundParameters.ContainsKey('Schema')) {
    $XmlReaderSettings.Schemas = (@((Get-Location).Path) + $Schema) | Read-PreloadedXmlSchema;
} else {
    $XmlReaderSettings.Schemas = (Get-Location).Path | Read-PreloadedXmlSchema;
}

$InputFile | Test-XmlFile -Settings $XmlReaderSettings;
