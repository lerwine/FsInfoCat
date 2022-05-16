Function Get-NormalizedException {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Exception]$Exception
    )

    Process {
        if ($null -eq $e.InnerException) {
            $Exception | Write-Output -NoEnumerate;
        } else {
            $e = $Exception;
            if ($e -is [System.AggregateException] -and ($e = $e.Flatten()).InnerExceptions.Count -eq 1) { $e = $e.InnerExceptions[0] }
            if ($e -is [System.Management.Automation.MethodInvocationException] -or $e -is [System.Management.Automation.GetValueInvocationException] -or $e -is [System.Management.Automation.SetValueInvocationException]) {
                $Exception.InnerException | Write-Output -NoEnumerate;
            } else {
                $Exception | Write-Output -NoEnumerate;
            }
        }
    }
}

class CodeAnalysisUtilException : System.Management.Automation.RuntimeException {
    CodeAnalysisUtilException([string]$Message, [System.Exception]$InnerException) : base($Message, $InnerException) {

    }
}
Function New-ErrorRecord {
    <#
    .SYNOPSIS
        Creates a new ErrorRecord object.
    .DESCRIPTION
        Creates a new System.Management.Automation.ErrorRecord object, but does not write the error.
    .NOTES
        Information or caveats about the function e.g. 'This function is not supported in Linux'
    .OUTPUTS
        System.Management.Automation.ErrorRecord - An object that represents the error.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Message')]
    Param(
        # The original error record.
        [Parameter(Mandatory = $true, ParameterSetName = 'ErrorRecord')]
        [System.Management.Automation.ErrorRecord]$ErrorRecord,

        # Specifies an exception object that represents the error.
        [Parameter(Mandatory = $true, ParameterSetName = 'Exception')]
        [Exception]$Exception,

        # Specifies the message text of the error.
        [Parameter(Mandatory = $true, ParameterSetName = 'Message')]
        [Parameter(ParameterSetName = 'ErrorRecord')]
        [Parameter(ParameterSetName = 'Exception')]
        [string]$Message,

        # Specifies an ID string to identify the error. The string should be unique to the error.
        [string]$ErrorId,

        # Specifies the category of the error.
        [System.Management.Automation.ErrorCategory]$Category = [System.Management.Automation.ErrorCategory]::NotSpecified,

        # Specifies the object that was being processed when the error occurred. Enter the object, a variable that contains the object, or a command that gets the object.
        [object]$TargetObject,

        # Specifies the action that the user should take to resolve or prevent the error.
        [string]$RecommendedAction,

        # Specifies the action that caused the error.
        [string]$CategoryActivity,

        # Specifies how or why the activity caused the error.
        [string]$CategoryReason,

        # Specifies the name of the object that was being processed when the error occurred.
        [string]$CategoryTargetName,

        # Specifies the type of the object that was being processed when the error occurred.
        [string]$CategoryTargetType,

        # For exceptions of type System.Management.Automation.MethodInvocationException, System.Management.Automation.GetValueInvocationException, or System.Management.Automation.SetValueInvocationException,
        # do not use the inner exception.
        [Parameter(ParameterSetName = 'ErrorRecord')]
        [Parameter(ParameterSetName = 'Exception')]
        [switch]$ExceptionAsIs
    )

    $Result = $null;
    switch ($PSCmdlet.ParameterSetName) {
        'ErrorRecord' {
            $EffectiveException = $ErrorRecord.Exception;
            if (-not $ExceptionAsIs.IsPresent) { $EffectiveException = $EffectiveException | Get-NormalizedException }
            if ($PSBoundParameters.ContainsKey('ErrorId')) {
                if ($PSBoundParameters.ContainsKey('Category')) {
                    if ($PSBoundParameters.ContainsKey('TargetObject')) {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $TargetObject);
                    } else {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $ErrorRecord.TargetObject);
                    }
                } else {
                    if ($PSBoundParameters.ContainsKey('TargetObject')) {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $ErrorRecord.CategoryInfo.Category, $TargetObject);
                    } else {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $ErrorRecord.CategoryInfo.Category, $ErrorRecord.TargetObject);
                    }
                }
            } else {
                if ($PSBoundParameters.ContainsKey('Category')) {
                    if ($PSBoundParameters.ContainsKey('TargetObject')) {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorRecord.FullyQualifiedErrorId, $Category, $TargetObject);
                    } else {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorRecord.FullyQualifiedErrorId, $Category, $ErrorRecord.TargetObject);
                    }
                } else {
                    if ($PSBoundParameters.ContainsKey('TargetObject')) {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorRecord.FullyQualifiedErrorId, $ErrorRecord.CategoryInfo.Category, $TargetObject);
                    } else {
                        $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorRecord.FullyQualifiedErrorId, $ErrorRecord.CategoryInfo.Category, $ErrorRecord.TargetObject);
                    }
                }
            }
            if (-not ($PSBoundParameters.ContainsKey('CategoryTargetType') -or $PSBoundParameters.ContainsKey('Category') -or $PSBoundParameters.ContainsKey('CategoryActivity') `
                    -or $PSBoundParameters.ContainsKey('CategoryReason') -or $PSBoundParameters.ContainsKey('CategoryTargetName'))) {
                $Result.CategoryInfo.Activity = $ErrorRecord.CategoryInfo.Activity;
                $Result.CategoryInfo.Reason = $ErrorRecord.CategoryInfo.Reason;
                $Result.CategoryInfo.TargetName = $ErrorRecord.CategoryInfo.TargetName;
                $Result.CategoryInfo.TargetType = $ErrorRecord.CategoryInfo.TargetType;
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
            break;
        }
        'Exception' {
            $EffectiveException = $Exception;
            if (-not $ExceptionAsIs.IsPresent) { $EffectiveException = $Exception | Get-NormalizedException }
            if ($Exception -is [System.Management.Automation.IContainsErrorRecord] -and $null -ne $Exception.ErrorRecord) {
                if ($PSBoundParameters.ContainsKey('ErrorId')) {
                    if ($PSBoundParameters.ContainsKey('Category')) {
                        if ($PSBoundParameters.ContainsKey('TargetObject')) {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $TargetObject);
                        } else {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $Exception.ErrorRecord.TargetObject);
                        }
                    } else {
                        if ($PSBoundParameters.ContainsKey('TargetObject')) {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Exception.ErrorRecord.CategoryInfo.Category, $TargetObject);
                        } else {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Exception.ErrorRecord.CategoryInfo.Category, $Exception.ErrorRecord.TargetObject);
                        }
                    }
                } else {
                    if ($PSBoundParameters.ContainsKey('Category')) {
                        if ($PSBoundParameters.ContainsKey('TargetObject')) {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $Exception.ErrorRecord.FullyQualifiedErrorId, $Category, $TargetObject);
                        } else {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $Exception.ErrorRecord.FullyQualifiedErrorId, $Category, $Exception.ErrorRecord.TargetObject);
                        }
                    } else {
                        if ($PSBoundParameters.ContainsKey('TargetObject')) {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $Exception.ErrorRecord.FullyQualifiedErrorId, $Exception.ErrorRecord.CategoryInfo.Category, $TargetObject);
                        } else {
                            $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $Exception.ErrorRecord.FullyQualifiedErrorId, $Exception.ErrorRecord.CategoryInfo.Category, $Exception.ErrorRecord.TargetObject);
                        }
                    }
                }
                if (-not ($PSBoundParameters.ContainsKey('CategoryTargetType') -or $PSBoundParameters.ContainsKey('Category') -or $PSBoundParameters.ContainsKey('CategoryActivity') `
                        -or $PSBoundParameters.ContainsKey('CategoryReason') -or $PSBoundParameters.ContainsKey('CategoryTargetName'))) {
                    $Result.CategoryInfo.Activity = $Exception.ErrorRecord.CategoryInfo.Activity;
                    $Result.CategoryInfo.Reason = $Exception.ErrorRecord.CategoryInfo.Reason;
                    $Result.CategoryInfo.TargetName = $Exception.ErrorRecord.CategoryInfo.TargetName;
                    $Result.CategoryInfo.TargetType = $Exception.ErrorRecord.CategoryInfo.TargetType;
                }
                if ($PSBoundParameters.ContainsKey('Message')) {
                    $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new($Message);
                    if ($PSBoundParameters.ContainsKey('RecommendedAction')) { $Result.ErrorDetails.RecommendedAction = $RecommendedAction }
                } else {
                    if ($PSBoundParameters.ContainsKey('RecommendedAction')) {
                        $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new('');
                        $Result.ErrorDetails.RecommendedAction = $RecommendedAction;
                    } else {
                        if ($null -ne $Exception.ErrorRecord.ErrorDetails) {
                            $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new($Exception.ErrorRecord.ErrorDetails.Message);
                            $Result.ErrorDetails.RecommendedAction = $Exception.ErrorRecord.ErrorDetails.RecommendedAction;
                        }
                    }
                }
            } else {
                if ($PSBoundParameters.ContainsKey('TargetObject')) {
                    $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $TargetObject);
                } else {
                    $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $null);
                }
                if ($PSBoundParameters.ContainsKey('Message')) {
                    $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new($Message);
                    if ($PSBoundParameters.ContainsKey('RecommendedAction')) { $Result.ErrorDetails.RecommendedAction = $RecommendedAction }
                } else {
                    if ($PSBoundParameters.ContainsKey('RecommendedAction')) {
                        $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new('');
                        $Result.ErrorDetails.RecommendedAction = $RecommendedAction;
                    }
                }
            }
            break;
        }
        default {
            if ($PSBoundParameters.ContainsKey('TargetObject')) {
                $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $TargetObject);
            } else {
                $Result = [System.Management.Automation.ErrorRecord]::new($EffectiveException, $ErrorId, $Category, $null);
            }
            if ($PSBoundParameters.ContainsKey('RecommendedAction')) {
                $Result.ErrorDetails = [System.Management.Automation.ErrorDetails]::new('');
                $Result.ErrorDetails.RecommendedAction = $RecommendedAction;
            }
            break;
        }
    }
    if ($PSBoundParameters.ContainsKey('CategoryActivity')) { $Result.CategoryInfo.Activity = $CategoryActivity }
    if ($PSBoundParameters.ContainsKey('CategoryReason')) { $Result.CategoryInfo.Reason = $CategoryReason }
    if ($PSBoundParameters.ContainsKey('CategoryTargetName')) { $Result.CategoryInfo.TargetName = $CategoryTargetName }
    if ($PSBoundParameters.ContainsKey('CategoryTargetType')) { $Result.CategoryInfo.TargetType = $CategoryTargetType }
    return $Result;
}

Function Get-DocumentationComment {
    <#
    .SYNOPSIS
        Gets the documentation comment for a member declaration.
    .DESCRIPTION
        Returns the documentation comment object if a documentation comment exists.
    .OUTPUTS
        Microsoft.CodeAnalysis.CSharp.Syntax.DocumentationCommentTriviaSyntax - Object that represents the documentation comment.
    #>
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

Function Format-MemberDocComments {
    <#
    .SYNOPSIS
        Reformats documentation comments for a member declaration if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The member declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        foreach ($DocumentationComment in (Get-DocumentationComment -Declaration $Declaration)) {

        }
    }
}

Function Get-InterfaceMembers {
    <#
    .SYNOPSIS
        Gets the members for an interface declaration.
    .DESCRIPTION
        Gets objects representing the members of an interface declaration.
    #>
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
            $TypeFilter = [CodeAnalysisUtil.TypeFilter]::new();
            if ($Property.IsPresent) {
                if ($Indexer.IsPresent) {
                    if ($EventProperty.IsPresent) {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]);
                    } else {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]);
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]);
                    }
                } else {
                    $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]);
                    if ($EventProperty.IsPresent) {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]);
                    }
                }
            } else {
                if ($Indexer.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]) }
                if ($EventProperty.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]) }
            }
            if ($EventField.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]) }
            if ($Method.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]) }
            if ($TypeFilter.BaseTypes.Count -eq 0) {
                $Declaration.Members | Write-Output;
            } else {
                $TypeFilter.FilterObjects($Declaration.Members) | Write-Output;
            }
        }
    }
}

Function Format-InterfaceDocComments {
    <#
    .SYNOPSIS
        Reformats interface declaration and member documentation comments if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax and its members if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The interface declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        $Declaration | Format-MemberDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Property) | Format-MemberDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Indexer) | Format-MemberDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -EventProperty) | Format-MemberDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -EventFielderty) | Format-MemberDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Method) | Format-MemberDocComments;
        (Get-InterfaceMembers -Declaration $Declaration -Constructor) | Format-MemberDocComments;
    }
}

Function Get-ClassMembers {
    <#
    .SYNOPSIS
        Gets the members for a class declaration.
    .DESCRIPTION
        Gets objects representing the members of a class declaration.
    .OUTPUTS
        Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax - Object(s) representing class member declarations.
    .LINK
        Get-TypeDeclarations
    #>
    [CmdletBinding(DefaultParameterSetName = 'SpecificTypes')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$Declaration,

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
            $TypeFilter = [CodeAnalysisUtil.TypeFilter]::new();
            if ($Property.IsPresent) {
                if ($Indexer.IsPresent) {
                    if ($EventProperty.IsPresent) {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BasePropertyDeclarationSyntax]);
                    } else {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]);
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]);
                    }
                } else {
                    $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]);
                    if ($EventProperty.IsPresent) {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]);
                    }
                }
            } else {
                if ($Indexer.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]) }
                if ($EventProperty.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EventDeclarationSyntax]) }
            }
            if ($Field.IsPresent) {
                if ($EventField.IsPresent) {
                    $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax]);
                } else {
                    $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]);
                }
            } else {
                if ($EventField.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax]) }
            }
            if ($Method.IsPresent) {
                if ($Constructor.IsPresent) {
                    if ($Destructor.IsPresent) {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]);
                            } else {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]);
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]);
                            } else {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]);
                            }
                        }
                    } else {
                        if ($Operator.IsPresent) {
                            if ($ConversionOperator.IsPresent) {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]);
                            } else {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]);
                            }
                        } else {
                            if ($ConversionOperator.IsPresent) {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]);
                            } else {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                            }
                        }
                    }
                } else {
                    if ($Constructor.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]) }
                    if ($Destructor.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]) }
                    if ($Operator.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]) }
                    if ($ConversionOperator.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]) }
                }
            }
            if ($TypeFilter.BaseTypes.Count -eq 0) {
                $Declaration.Members | Write-Output;
            } else {
                $TypeFilter.FilterObjects($Declaration.Members) | Write-Output;
            }
        }
    }
}

Function Format-ClassDocComments {
    <#
    .SYNOPSIS
        Reformats class declaration and member documentation comments if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax and its members if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The class declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        $Declaration | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -Interface) | Format-InterfaceDocComments;
        (Get-ClassMembers -Declaration $Declaration -Class) | Format-ClassDocComments;
        (Get-ClassMembers -Declaration $Declaration -Struct) | Format-StructDocComments;
        (Get-ClassMembers -Declaration $Declaration -Record) | Format-RecordDocComments;
        (Get-ClassMembers -Declaration $Declaration -Enum) | Format-EnumDocComments;
        (Get-ClassMembers -Declaration $Declaration -Declaration) | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -Property) | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -EventProperty) | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -EventFielderty) | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -Indexer) | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -Method) | Format-MemberDocComments;
        (Get-ClassMembers -Declaration $Declaration -Constructor) | Format-MemberDocComments;
    }
}

Function Get-StructMembers {
    <#
    .SYNOPSIS
        Gets the members for a struct declaration.
    .DESCRIPTION
        Gets objects representing the members of a struct declaration.
    #>
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
            $TypeFilter = [CodeAnalysisUtil.TypeFilter]::new();
            if ($Property.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]) }
            if ($Indexer.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]) }
            if ($Field.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]) }
            if ($Method.IsPresent) {
                if ($Constructor.IsPresent) {
                    if ($Operator.IsPresent) {
                        if ($ConversionOperator.IsPresent) {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]);
                        } else {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]);
                        }
                    } else {
                        if ($ConversionOperator.IsPresent) {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]);
                        } else {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                        }
                    }
                } else {
                    if ($Constructor.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]) }
                    if ($Destructor.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]) }
                    if ($Operator.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]) }
                    if ($ConversionOperator.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]) }
                }
            }
            if ($TypeFilter.BaseTypes.Count -eq 0) {
                $Declaration.Members | Write-Output;
            } else {
                $TypeFilter.FilterObjects($Declaration.Members) | Write-Output;
            }
        }
    }
}

Function Format-StructDocComments {
    <#
    .SYNOPSIS
        Reformats struct declaration and member documentation comments if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax and its members if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The struct declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        $Declaration | Format-MemberDocComments;
        (Get-StructMembers -Declaration $Declaration -Property) | Format-MemberDocComments;
        (Get-StructMembers -Declaration $Declaration -Indexer) | Format-MemberDocComments;
        (Get-StructMembers -Declaration $Declaration -Constructor) | Format-MemberDocComments;
    }
}

Function Get-RecordMembers {
    <#
    .SYNOPSIS
        Gets the members for a record declaration.
    .DESCRIPTION
        Gets objects representing the members of a record declaration.
    #>
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
            $TypeFilter = [CodeAnalysisUtil.TypeFilter]::new();
            if ($Property.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax]) }
            if ($Indexer.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.IndexerDeclarationSyntax]) }
            if ($Field.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax]) }
            if ($Method.IsPresent) {
                if ($Constructor.IsPresent) {
                    if ($Operator.IsPresent) {
                        if ($ConversionOperator.IsPresent) {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax]);
                        } else {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]);
                        }
                    } else {
                        if ($ConversionOperator.IsPresent) {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]);
                        } else {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]);
                        }
                    }
                } else {
                    if ($Constructor.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax]) }
                    if ($Destructor.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.DestructorDeclarationSyntax]) }
                    if ($Operator.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.OperatorDeclarationSyntax]) }
                    if ($ConversionOperator.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax]) }
                }
            }
            if ($TypeFilter.BaseTypes.Count -eq 0) {
                $Declaration.Members | Write-Output;
            } else {
                $TypeFilter.FilterObjects($Declaration.Members) | Write-Output;
            }
        }
    }
}

Function Format-RecordDocComments {
    <#
    .SYNOPSIS
        Reformats record declaration and member documentation comments if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax and its members if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The record declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        $Declaration | Format-MemberDocComments;
        (Get-RecordMembers -Declaration $Declaration -Property) | Format-MemberDocComments;
        (Get-RecordMembers -Declaration $Declaration -Property) | Format-MemberDocComments;
        (Get-RecordMembers -Declaration $Declaration -Property) | Format-MemberDocComments;
        (Get-RecordMembers -Declaration $Constructor -Property) | Format-MemberDocComments;
    }
}

Function Get-DelegateArguments {
    <#
    .SYNOPSIS
        Gets the arguments for a delegate method.
    .DESCRIPTION
        Gets objects representing the arguments for a delegate declaration.
    #>
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Declaration
    )

    Process {

    }
}

Function Format-DelegateDocComments {
    <#
    .SYNOPSIS
        Reformats delegate declaration and member documentation comments if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax and its members if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The delegate declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        $Declaration | Format-MemberDocComments;
        (Get-DelegateArguments -Declaration $Declaration) | Format-MemberDocComments;
    }
}

Function Get-EnumFields {
    <#
    .SYNOPSIS
        Gets the fields declared on an enum type.
    .DESCRIPTION
        Gets objects representing the fields for an enum declaration.
    #>
    [CmdletBinding()]
    Param(
        # The enum declaration source code.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Declaration
    )

    Process {
    }
}

Function Format-EnumDocComments {
    <#
    .SYNOPSIS
        Reformats enum declaration and member documentation comments if necessary.
    .DESCRIPTION
        Reformats the documentation comments for a Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax and its members if any of the lines are too long.
    #>
    [CmdletBinding()]
    Param(
        # The enum declaration.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]$Declaration,

        # The maximum line length.
        [int]$MaxLineLength = 180
    )

    Process {
        $Declaration | Format-MemberDocComments;
        (Get-EnumFields -Declaration $Declaration) | Format-MemberDocComments;
    }
}

Function Get-TypeDeclarations {
    <#
    .SYNOPSIS
        Gets type declarations from source code.
    .DESCRIPTION
        Returns a member declaration object for each type declaration or delegate declaration found.
    .OUTPUTS
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax or Microsoft.CodeAnalysis.CSharp.Syntax.DelegateDeclarationSyntax - Object(s) representing type or delegate declarations.
    #>
    [CmdletBinding(DefaultParameterSetName = 'Namespace')]
    Param(
        [Parameter(Mandatory = $true, ParameterSetName = 'Namespace')]
        [Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax]$Namespace,

        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'Class')]
        [Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]$OwnerType,

        [switch]$Interface,

        [switch]$Class,

        [switch]$Struct,

        [switch]$Record,

        [switch]$Enum,

        [switch]$Delegate
    )
    Process {
        $Members = $null;
        if ($PSCmdlet.ParameterSetName -eq 'Class') {
            $Members = $OwnerType.Members;
        } else {
            $Members = $Namespace.Members;
        }
        if ($Members.Count -gt 0) {
            $TypeFilter = [CodeAnalysisUtil.TypeFilter]::new();
            if ($Interface.IsPresent) {
                if ($Class.IsPresent) {
                    if ($Struct.IsPresent) {
                        if ($Record.IsPresent) {
                            if ($Enum.IsPresent) {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeDeclarationSyntax]);
                            } else {
                                $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.TypeDeclarationSyntax]);
                            }
                        } else {
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]);
                            $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]);
                            if ($Enum.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) }
                        }
                    } else {
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]);
                        $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]);
                        if ($Record.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]) }
                        if ($Enum.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) }
                    }
                } else {
                    $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.InterfaceDeclarationSyntax]);
                    if ($Struct.IsPresent) {$TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]) }
                    if ($Record.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]) }
                    if ($Enum.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) }
                }
            } else {
                if ($Class.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax]) }
                if ($Struct.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.StructDeclarationSyntax]) }
                if ($Record.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.RecordDeclarationSyntax]) }
                if ($Enum.IsPresent) { $TypeFilter.BaseTypes.Add([Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax]) }
            }
            if ($TypeFilter.BaseTypes.Count -eq 0) {
                $Members | Write-Output;
            } else {
                $TypeFilter.FilterObjects($Members) | Write-Output;
            }
        }
    }
}

Function Get-NamespaceDeclaration {
    <#
    .SYNOPSIS
        Gets the namespace declaration(s) for a source code file.
    .DESCRIPTION
        Returns a namespace declaration object for each namespace declaration found.
    .OUTPUTS
        Microsoft.CodeAnalysis.CSharp.Syntax.BaseNamespaceDeclarationSyntax - Represents a namespace declaration and its members.
    #>
    [CmdletBinding()]
    Param(
        # The source code file.
        [Parameter(Mandatory = $true)]
        [CodeAnalysisUtil.SourceFile]$SourceFile
    )
    $SyntaxTree = $SourceFile.GetSyntaxTree();
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

Function New-SourceFile {
    <#
    .SYNOPSIS
        Creates a new source file object.
    .DESCRIPTION
        Creates a source file object from a FileInfo object.
    .OUTPUTS
        CodeAnalysisUtil.SourceFile - Represents a source code file.
    #>
    [CmdletBinding()]
    Param(
        # The FileInfo object that points to a source code file.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [System.IO.FileInfo]$InputFile
    )

    Process {
        [CodeAnalysisUtil.SourceFile]::new($InputFile) | Write-Output;
    }
}

Function Get-SourceFile {
    <#
    .SYNOPSIS
        Gets source code files.
    .DESCRIPTION
        Creates a CodeAnalysisUtil.SourceFile object for each source code file found.
    .OUTPUTS
        CodeAnalysisUtil.SourceFile - Objects representing source code files.
    #>
    [CmdletBinding()]
    Param(
        # The subdirectory path to search.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$SearchPath,

        # If present, subdirectories will be recursed into.
        [switch]$Recurse
    )

    Process {
        if ($Recurse.IsPresent) {
            (Get-ChildItem -LiteralPath $Path -Filter '*.cs' -File -Recurse) | Where-Object {
                $_.Extension -ine '.Designer.cs'
            } | New-SourceFile | Write-Output;
        } else {
            (Get-ChildItem -LiteralPath $Path -Filter '*.cs' -File) | Where-Object {
                $_.Extension -ine '.Designer.cs'
            } | New-SourceFile | Write-Output;
        }
    }
}

Function Get-LineLengthCompliance {
    <#
    .SYNOPSIS
        Checks source code file for line length compliance.
    .DESCRIPTION
        Gets a compliance result object to indicate line length compliance.
    .OUTPUTS
        CodeAnalysisUtil.LineComplianceInfo - Object indicating line length compliance.
    #>
    [CmdletBinding()]
    Param(
        # The source file to check.
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [CodeAnalysisUtil.SourceFile]$SourceFile,

        # The maximum line length.
        [int]$MaxLineLength = 180,

        # If not present, a CodeAnalysisUtil.LineComplianceInfo will only be returned if any line exceeds the limit.
        [switch]$Force
    )

    Process {
        if ($Force.IsPresent) {
            [CodeAnalysisUtil.LineComplianceInfo]::new($SourceFile, $MaxLineLength) | Write-Output;
        } else {
            $LineComplianceInfo = [CodeAnalysisUtil.LineComplianceInfo]::new($SourceFile, $MaxLineLength);
            if ($LineComplianceInfo.Violations.Count -gt 0) { $LineComplianceInfo | Write-Output }
        }
    }
}
