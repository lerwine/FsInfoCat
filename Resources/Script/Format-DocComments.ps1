Param(
    [string[]]$Path = (
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Desktop',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Local',
        'C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat.Upstream'
    ),
    [bool]$Recurse = $true
)
<#
Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process;
#>
# https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
# https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtree?view=roslyn-dotnet-4.1.0
Add-Type -AssemblyName 'Microsoft.CodeAnalysis' -ErrorAction Stop;
# https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpsyntaxtree?view=roslyn-dotnet-4.1.0
Add-Type -AssemblyName 'Microsoft.CodeAnalysis.CSharp' -ErrorAction Stop;

Function New-ErrorRecord {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Management.Automation.ErrorRecord]$ErrorRecord,
        [string]$ErrorId,
        [System.Management.Automation.ErrorCategory]$Category,
        [object]$TargetObject,
        [string]$RecommendedAction,
        [string]$CategoryActivity,
        [string]$CategoryReason,
        [string]$CategoryTargetName,
        [string]$CategoryTargetType,
        [string]$Message
    )
    $e = $ErrorRecord.Exception;
    if ($e -is [System.Management.Automation.MethodInvocationException] -and $null -ne $e.InnerException) { $e = $e.InnerException }
    $Result = $null;
    if ($PSBoundParameters.ContainsKey('ErrorId')) {
        if ($PSBoundParameters.ContainsKey('Category')) {
            if ($PSBoundParameters.ContainsKey('TargetObject')) {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorId, $Category, $TargetObject);
            } else {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorId, $Category, $ErrorRecord.TargetObject);
            }
        } else {
            if ($PSBoundParameters.ContainsKey('TargetObject')) {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorId, $ErrorRecord.CategoryInfo.Category, $TargetObject);
            } else {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorId, $ErrorRecord.CategoryInfo.Category, $ErrorRecord.TargetObject);
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('Category')) {
            if ($PSBoundParameters.ContainsKey('TargetObject')) {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorRecord.FullyQualifiedErrorId, $Category, $TargetObject);
            } else {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorRecord.FullyQualifiedErrorId, $Category, $ErrorRecord.TargetObject);
            }
        } else {
            if ($PSBoundParameters.ContainsKey('TargetObject')) {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorRecord.FullyQualifiedErrorId, $ErrorRecord.CategoryInfo.Category, $TargetObject);
            } else {
                $Result = [System.Management.Automation.ErrorRecord]::new($e, $ErrorRecord.FullyQualifiedErrorId, $ErrorRecord.CategoryInfo.Category, $ErrorRecord.TargetObject);
            }
        }
    }
    if ($PSBoundParameters.ContainsKey('CategoryActivity')) { $Result.CategoryInfo.Activity = $CategoryActivity }
    if ($PSBoundParameters.ContainsKey('CategoryReason')) { $Result.CategoryInfo.Reason = $CategoryReason }
    if ($PSBoundParameters.ContainsKey('CategoryTargetName')) { $Result.CategoryInfo.TargetName = $CategoryTargetName }
    if ($PSBoundParameters.ContainsKey('CategoryTargetType')) {
        $Result.CategoryInfo.TargetType = $CategoryTargetType;
    } else {
        if (-not ($PSBoundParameters.ContainsKey('Category') -or$PSBoundParameters.ContainsKey('CategoryActivity') -or $PSBoundParameters.ContainsKey('CategoryReason') -or $PSBoundParameters.ContainsKey('CategoryTargetName'))) {
            $Result.CategoryInfo.Activity = $ErrorRecord.CategoryInfo.Activity;
            $Result.CategoryInfo.Reason = $ErrorRecord.CategoryInfo.Reason;
            $Result.CategoryInfo.TargetName = $ErrorRecord.CategoryInfo.TargetName;
            $Result.CategoryInfo.TargetType = $ErrorRecord.CategoryInfo.TargetType;
        }
    }
    if ($PSBoundParameters.ContainsKey('Message')) {
        $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new($Message);
        if ($PSBoundParameters.ContainsKey('RecommendedAction')) { $Result.ErrorDetails.RecommendedAction = $RecommendedAction }
    } else {
        if ($PSBoundParameters.ContainsKey('RecommendedAction')) {
            $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new('');
            $Result.ErrorDetails.RecommendedAction = $RecommendedAction;
        } else {
            if ($null -ne $ErrorRecord.ErrorDetails) {
                $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new($ErrorRecord.ErrorDetails.Message);
                $Result.ErrorDetails.RecommendedAction = $ErrorRecord.ErrorDetails.RecommendedAction;
            }
        }
    }
    return $Result;
}

Function Get-DocumentationComment {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Declaration
    )
    
    Process {
        if ($Delcaration.HasLeadingTrivia) {
            $Delcaration.GetLeadingTrivia() | ForEach-Object {
                if ($_.HasStructure) { $_.GetStructure() }
            } | Where-Object {
                $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax];
            } | Write-Output;
        }
    }
}

Function Format-DelegateDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Declaration
    )
    
    Process {
        foreach ($DocumentationComment in (Get-DocumentationComment -Declaration $Declaration)) {

        }
    }
}

Function Get-InterfaceMembers {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$Declaration,

        [switch]$Property,

        [switch]$Indexer,

        [switch]$EventField,

        [switch]$EventProperty,

        [switch]$Method
    )

    Process {
        if ($Declaration.Members.Count -gt 0) {
            if ($EventField.IsPresent) {
                if ($Method.IsPresent) {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object { 
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        }
                    }
                } else {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax];
                                } | Write-Output;
                            }
                        }
                    }
                }
            } else {
                if ($Method.IsPresent) {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } | Write-Output;
                            }
                        }
                    }
                } else {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } | Write-Output;
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object {
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax] -or
                                    $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                } | Write-Output;
                            } else {
                                $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } | Write-Output;
                            }
                        } else {
                            if ($EventProperty.IsPresent) {
                                $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]  } | Write-Output;
                            } else {
                                $Declaration.Members | Write-Output;
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Format-InterfaceDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$Declaration
    )

    Process {
        $Declaration | Format-DelegateDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Property) | Format-DelegateDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Indexer) | Format-DelegateDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -EventProperty) | Format-DelegateDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -EventFielderty) | Format-DelegateDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Method) | Format-DelegateDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Constructor) | Format-DelegateDocComments;
    }
}

Function Get-ClassMembers {
    [CmdletBinding(DefaultParameterSetName = 'SpecificTypes')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$Declaration,

        [Parameter(Mandatory = $true, ParameterSetName = 'AllTypes')]
        [switch]$AllTypes,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Interface,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Class,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Struct,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Delegate,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Record,

        [Parameter(ParameterSetName = 'SpecificTypes')]
        [switch]$Enum,

        [Parameter(Mandatory = $true, ParameterSetName = 'NoTypes')]
        [switch]$NoTypes,

        [switch]$Property,

        [switch]$Field,

        [switch]$EventProperty,

        [switch]$EventField,

        [switch]$Operator,

        [switch]$ConversionOperator,

        [switch]$Indexer,

        [switch]$Method,

        [switch]$Constructor,

        [switch]$Destructor
    )

    Process {
        if ($Declaration.Members.Count -gt 0) {
            [System.Type[]]$FilterTypes = @();
            if ($AllTypes.IsPresent) {
                $FilterTypes = @(
                    [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax],
                    [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]
                );
            } else {
                if ($Interface.IsPresent) {
                    if ($Class.IsPresent) {
                        if ($Struct.IsPresent) {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]);
                                } else {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]);
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]
                                    );
                                }
                            }
                        } else {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]
                                    );
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]
                                    );
                                }
                            }  
                        }
                    } else {
                        if ($Struct.IsPresent) {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]
                                    );
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]
                                    );
                                }
                            }
                        } else {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]
                                    );
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]);
                                }
                            }  
                        }    
                    }
                } else {
                    if ($Class.IsPresent) {
                        if ($Struct.IsPresent) {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]
                                    );
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]
                                    );
                                }
                            }
                        } else {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]
                                    );
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]);
                                }
                            }  
                        }
                    } else {
                        if ($Struct.IsPresent) {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]
                                    );
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]);
                                }
                            }
                        } else {
                            if ($Record.IsPresent) {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @(
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax],
                                        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]
                                    );
                                } else {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]);
                                }
                            } else {
                                if ($Enum.IsPresent) {
                                    $FilterTypes = @([Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]);
                                }
                            }  
                        }    
                    }
                }
                if ($Delegate.IsPresent) {
                    $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]);
                }
            }
            if ($Method.IsPresent) {
                if ($Constructor.IsPresent) {
                    if ($Destructor.IsPresent) {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]);
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]
                                );
                            }
                        }
                    } else {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]
                                );
                            }
                        }
                    }
                } else {
                    if ($Destructor.IsPresent) {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]
                                );
                            }
                        }
                    } else {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            }
                        }
                    }
                }
            } else {
                if ($Constructor.IsPresent) {
                    if ($Destructor.IsPresent) {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]
                                );
                            }
                        }
                    } else {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorDeclarationSyntax]);
                            }
                        }
                    }
                } else {
                    if ($Destructor.IsPresent) {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]
                                );
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]);
                            }
                        }
                    } else {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @(
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax],
                                    [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]
                                );
                            } else {
                                $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]);
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            }
                        }
                    }
                }
            }
            if ($Property.IsPresent) {
                if ($EventProperty.IsPresent) {
                    if ($Indexer.IsPresent) {
                        $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]);
                    } else {
                        $FilterTypes += @(
                            [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax],
                            [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]
                        );
                    }
                } else {
                    if ($Indexer.IsPresent) {
                        $FilterTypes += @(
                            [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax],
                            [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]
                        );
                    } else {
                        $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]);
                    }
                }
            } else {
                if ($EventProperty.IsPresent) {
                    if ($Indexer.IsPresent) {
                        $FilterTypes += @(
                            [Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax],
                            [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]
                        );
                    } else {
                        $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]);
                    }
                } else {
                    if ($Indexer.IsPresent) {
                        $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]);
                    }
                }
            }
            if ($Field.IsPresent) {
                if ($EventField.IsPresent) {
                    $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]);
                } else {
                    $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]);
                }
            } else {
                if ($EventField.IsPresent) {
                    $FilterTypes += @([Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]);
                }
            }
            if ($FilterTypes.Count -eq 0) {
                if ($NoTypes.IsPresent) {
                    $Declaration.Members | Where-Object {
                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or `
                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or `
                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax];
                    } | Write-Output;
                } else {
                    $Declaration.Members | Write-Output;
                }
            } else {
                if ($FilterTypes.Count -eq 1) {
                    $t = $FilterTypes[0];
                    $Declaration.Members | Where-Object { $_ -is $t } | Write-Output;
                } else {
                    $Declaration.Members | Where-Object {
                        $IsMatch = $false;
                        foreach ($t in $FilterTypes) {
                            if ($_ -is $t) {
                                $IsMatch = $true;
                                break;
                            }
                        }
                        $IsMatch;
                    }
                }
            }
        }
    }
}

Function Format-ClassDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$Declaration
    )

    Process {
        $Declaration | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -Interface) | Format-InterfaceDocComments;
        (Get-ClassMembers -Declaration $Declaration -Class) | Format-ClassDocComments;
        (Get-ClassMembers -Declaration $Declaration -Struct) | Format-StructDocComments;
        (Get-ClassMembers -Declaration $Declaration -Record) | Format-RecordDocComments;
        (Get-ClassMembers -Declaration $Declaration -Enum) | Format-EnumDocComments;
        (Get-ClassMembers -Declaration $Declaration -Declaration) | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -Property) | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -EventProperty) | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -EventFielderty) | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -Indexer) | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -Method) | Format-DelegateDocComments;
        (Get-ClassMembers -Declaration $Declaration -Constructor) | Format-DelegateDocComments;
    }
}

Function Get-StructMembers {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]$Declaration,

        [switch]$Property,

        [switch]$Indexer,

        [switch]$Field,

        [switch]$Method,

        [switch]$Operator,

        [switch]$ConversionOperator,

        [switch]$Constructor
    )

    Process {
        if ($Declaration.Members.Count -gt 0) {
            if ($Constructor.IsPresent) {
                if ($Field.IsPresent) {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {$_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor]} | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                if ($Field.IsPresent) {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] } | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } | Write-Output;
                                    } else {
                                        $Declaration.Members | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Format-StructDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]$Declaration
    )

    Process {
        $Declaration | Format-DelegateDocComments;
        (Get-StructMembers -Declaration $Declaration -Property) | Format-DelegateDocComments;
        (Get-StructMembers -Declaration $Declaration -Indexer) | Format-DelegateDocComments;
        (Get-StructMembers -Declaration $Declaration -Constructor) | Format-DelegateDocComments;
    }
}

Function Get-RecordMembers {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]$Declaration,

        [switch]$Property,

        [switch]$Indexer,

        [switch]$Field,

        [switch]$Method,

        [switch]$Operator,

        [switch]$ConversionOperator,

        [switch]$Constructor
    )

    Process {
        if ($Declaration.Members.Count -gt 0) {
            if ($Constructor.IsPresent) {
                if ($Field.IsPresent) {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {$_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Constructor]} | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                }
            } else {
                if ($Field.IsPresent) {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.Field] } | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if ($Property.IsPresent) {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Indexer.IsPresent) {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                    
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            }
                        } else {
                            if ($Method.IsPresent) {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax] } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax];
                                        } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax] } | Write-Output;
                                    }
                                }
                            } else {
                                if ($Operator.IsPresent) {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object {
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] -or
                                            $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax];
                                        } | Write-Output;
                                    } else {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax] } | Write-Output;
                                    }
                                } else {
                                    if ($ConversionOperator.IsPresent) {
                                        $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax] } | Write-Output;
                                    } else {
                                        $Declaration.Members | Write-Output;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Format-RecordDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]$Declaration
    )

    Process {
        $Declaration | Format-DelegateDocComments;
        (Get-RecordMembers -Declaration $Declaration -Property) | Format-DelegateDocComments;
        (Get-RecordMembers -Declaration $Declaration -Property) | Format-DelegateDocComments;
        (Get-RecordMembers -Declaration $Declaration -Property) | Format-DelegateDocComments;
        (Get-RecordMembers -Declaration $Constructor -Property) | Format-DelegateDocComments;
    }
}

Function Get-DelegateArguments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Declaration
    )

    Process {

    }
}

Function Format-DelegateDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Declaration
    )

    Process {
        $Declaration | Format-DelegateDocComments;
        (Get-DelegateArguments -Declaration $Declaration) | Format-DelegateDocComments;
    }
}

Function Get-EnumFields {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Declaration
    )

    Process {
    }
}

Function Format-EnumDocComments {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Declaration
    )

    Process {
        $Declaration | Format-DelegateDocComments;
        (Get-EnumFields -Declaration $Declaration) | Format-DelegateDocComments;
    }
}

Function Get-TypeDeclarations {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$Declaration,

        [switch]$Interface,

        [switch]$Class,

        [switch]$Struct,

        [switch]$Delegate,

        [switch]$Record,

        [switch]$Enum
    )
    Process {
        if ($Declaration.Members.Count -gt 0) {
            if ($Delegate.IsPresent) {
                if ($Enum.IsPresent) {
                    if ($Interface.IsPresent) {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        }
                    }
                } else {
                    if ($Interface.IsPresent) {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax] }
                                }
                            }
                        }
                    }
                }
            } else {
                if ($Enum.IsPresent) {
                    if ($Interface.IsPresent) {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            }
                        }
                    } else {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax] }
                                }
                            }
                        }
                    }
                } else {
                    if ($Interface.IsPresent) {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax] }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax];
                                    }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax] }
                                }
                            }
                        }
                    } else {
                        if ($Class.IsPresent) {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax];
                                    }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax];
                                    }
                                } else {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax] }
                                }
                            }
                        } else {
                            if ($Struct.IsPresent) {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] }
                                } else {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax] }
                                }
                            } else {
                                if ($Record.IsPresent) {
                                    $Declaration.Members | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax] }
                                } else {
                                    $Declaration.Members | Where-Object {
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax] -or `
                                        $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax];
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

Function Get-NamespaceDeclaration {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]$SyntaxTree
    )
    
    $CompilationUnitRoot = $null;
    if ($SyntaxTree.HasCompilationUnitRoot) {
        try { $CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot() }
        catch {
            $ErrorRecord = New-ErrorRecord -ErrorRecord $_ -ErrorId 'Format-SourceFileDocumentation:AccessError' -Category PermissionDenied `
                -CategoryActivity 'Get compilation unit root' `
                -CategoryReason 'Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax.GetCompilationUnitRoot throw an exception' -TargetObject $SourceFile;
        }
    }
    if ($null -ne $ErrorRecord) {
        Write-Error -ErrorRecord $ErrorRecord;
    } else {
        if ($null -eq $CompilationUnitRoot) {
            Write-Warning -Message "Source file has no compilation unit root ($($SourceFile.FullName))";
        } else {
            @($CompilationUnitRoot.Members) | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] }
        }
    }
}

Function Read-SyntaxTree {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.IO.FileInfo]$InputFile
    )
    Process {
        $SyntaxTree = $null;
        $StreamReader = $null;
        $ErrorRecord = $null;
        try { $StreamReader = $SourceFile.OpenText();  }
        catch {
            $ErrorRecord = New-ErrorRecord -ErrorRecord $_ -ErrorId 'Format-SourceFileDocumentation:ReadError' -Category OpenError -TargetObject $SourceFile `
                -CategoryActivity 'Open source file' -CategoryReason 'System.IO.FileInfo.OpenText threw an exception';
        }
        if ($null -ne $StreamReader) {
            $SourceCode = $null;
            try { $SourceCode = $StreamReader.ReadToEnd() }
            catch {
                $ErrorRecord = New-ErrorRecord -ErrorRecord $_ -ErrorId 'Format-SourceFileDocumentation:ReadError' -Category OpenError -TargetObject $SourceFile `
                    -CategoryActivity 'Read source file' -CategoryReason 'System.IO.StreamReader.ReadToEnd threw an exception';
            }
            finally { $StreamReader.Close() }
            if ($null -ne $ErrorRecord) {
                if ([string]::IsNullOrWhiteSpace($SourceCode)) {
                    Write-Warning -Message "Source code file is empty ($($SourceFile.FullName))";
                } else {
                    try { $SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($SourceCode, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $SourceFile.FullName) }
                    catch {
                        $ErrorRecord = New-ErrorRecord -ErrorRecord $_ -ErrorId 'Format-SourceFileDocumentation:ParseFailure' -Category ParserError -TargetObject $SourceFile `
                            -CategoryActivity 'Parse source code' -CategoryReason 'Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree.ParseText throw an exception';
                    }
                }
            }
        }
        if ($null -ne $ErrorRecord) {
            Write-Error -ErrorRecord $ErrorRecord;
        } else {
            if ($null -eq $SyntaxTree) {
                Write-Warning -Message "Source file has no code ($($SourceFile.FullName))";
            } else {
                $SyntaxTree | Write-Output;
            }
        }
    }
}

<#
$SourceFile = [System.IO.FileInfo]::new('C:\Users\Lenny\Source\Repositories\FsInfoCat\src\FsInfoCat\Upstream\DataModel\IUpstreamTagDefinitionRow.cs')
$StreamReader = $SourceFile.OpenText();
$SourceCode = $StreamReader.ReadToEnd()
$StreamReader.Close();
$SyntaxTree = [Microsoft.CodeAnalysis.CSharp.CSharpSyntaxTree]::ParseText($SourceCode, [Microsoft.CodeAnalysis.CSharp.CSharpParseOptions]::Default, $SourceFile.FullName)
$CompilationUnitRoot = $SyntaxTree.GetCompilationUnitRoot()
$NamespaceDeclarationSyntax = @($CompilationUnitRoot.Members) | Where-Object { $_ -is [Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax] } | Select-Object -First 1
$Definition = $NamespaceDeclarationSyntax.Members[0]
#>

$DocCommentBlockRegex = [System.Text.RegularExpressions.Regex]::new('([^\S\r\n]*///[^\r\n]*(\r\n?|\n))+', [System.Text.RegularExpressions.RegexOptions]::Compiled);
$DocCommentLineRegex = [System.Text.RegularExpressions.Regex]::new('(\s*///)\s?(.+)?', [System.Text.RegularExpressions.RegexOptions]::Compiled);

$ProgressActivity = [System.IO.Path]::GetFileNameWithoutExtension($MyInvocation.MyCommand.Source).Replace('-', ' ');
Write-Progress -Activity $ProgressActivity -Status 'Getting File Paths' -PercentComplete 0;
[System.IO.FileInfo[]]$FileInfos = @();
if ($Recurse) {
    $FileInfos = @(Get-ChildItem -LiteralPath $Path -Filter '*.cs' -Exclude '*.Designer.cs' -File -Recurse);
} else {
    $FileInfos = @(Get-ChildItem -LiteralPath $Path -Filter '*.cs' -Exclude '*.Designer.cs' -File);
}
[long]$TotalBytes = 0;
$FileInfos | % { $TotalBytes += $_.Length };
$TotalCount = $FileInfos.Count;
$PercentComplete = 0;
$DirectoryPath = '';
$ItemIndex = -1;
[long]$BytesCompleted = 0;
$FileInfos | % {
    $ItemIndex++;
    [int]$pc = ($BytesCompleted * 100) / $TotalBytes;
    if ($DirectoryPath -ne $_.FullName -or $PercentComplete -ne $pc) {
        $PercentComplete = $pc;
        $DirectoryPath = $_.FullName;
        Write-Progress -Activity $ProgressActivity -Status "Checking file $($ItemIndex + 1) of $TotalCount" -PercentComplete $pc -CurrentOperation $DirectoryPath;
    }
    foreach ($SyntaxTree in ($_ | Read-SyntaxTree )) {
        foreach ($ns in (Get-NamespaceDeclaration -SyntaxTree $SyntaxTree)) {
            $ns | Format-DelegateDocComments;
            (Get-TypeDeclarations -Declaration $ns -Interface) | Format-InterfaceDocComments;
            (Get-TypeDeclarations -Declaration $ns -Class) | Format-ClassDocComments;
            (Get-TypeDeclarations -Declaration $ns -Struct) | Format-StructDocComments;
            (Get-TypeDeclarations -Declaration $ns -Record) | Format-RecordDocComments;
            (Get-TypeDeclarations -Declaration $ns -Enum) | Format-EnumDocComments;
            (Get-TypeDeclarations -Declaration $ns -Declaration) | Format-DelegateDocComments;
        }
    }
}
Write-Progress -Activity $ProgressActivity -Status 'Finished' -PercentComplete 100 -Completed;
#>
#Get-Variable
