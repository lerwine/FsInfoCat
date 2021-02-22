Function Format-Range {
    [CmdletBinding(DefaultParameterSetName = 'Params')]
    Param(
        [int]$Start = 1,

        [Parameter(Mandatory = $true)]
        [int]$Count,
        
        [string]$Format = '{0}'
    )

    switch ($Start) {
         0 {
            for ($i = 0; $i -lt $Count; $i++) { $Format -f $i }
            break;
         }
         1 {
            for ($i = 1; $i -le $Count; $i++) { $Format -f $i }
            break;
         }
         default {
            for ($i = 0; $i -lt $Count; $i++) { $Format -f ($i + $Start) }
            break;
        }
    }
}

Function Write-GenericArguments {
    [CmdletBinding(DefaultParameterSetName = 'Type')]
    Param(
        [int]$Count = 0,

        [int]$OutputCount = 0,

        [Parameter(ParameterSetName = 'Doc')]
        [Parameter(ParameterSetName = 'Type')]
        [switch]$TResult,
        
        [Parameter(Mandatory = $true, ParameterSetName = 'Doc')]
        [switch]$Doc,

        [Parameter(ParameterSetName = 'Type')]
        [switch]$Type,

        [Parameter(Mandatory = $true, ParameterSetName = 'Params')]
        [switch]$Params
    )

    $Text = '';
    if ($Count -eq 1) {
        if ($Params.IsPresent) { $Text = 'T t' } else { $Text = 'T' }
    } else {
        if ($Count -gt 1) {
            if ($Params.IsPresent) {
                $Text = (Format-Range -Count $Count -Format 'T{0} t{0}') -join ', ';
            } else {
                $Text = (Format-Range -Count $Count -Format 'T{0}') -join ', ';
            }
        }
    }
    if ($OutputCount -eq 1) {
        if ($Count -gt 0) {
            if ($Params.IsPresent) {
                $Text = "$Text, R r";
            } else {
                $Text = "$Text, R";
            }
        } else {
            if ($Params.IsPresent) {
                $Text = "R r";
            } else {
                $Text = "R";
            }
        }
    } else {
        if ($OutputCount -gt 0) {
            if ($Count -gt 0) {
                if ($Params.IsPresent) {
                    $Text = "$Text, $((Format-Range -Count $OutputCount -Format 'R{0} r{0}') -join ', ')";
                } else {
                    $Text = "$Text, $((Format-Range -Count $OutputCount -Format 'R{0}') -join ', ')";
                }
            } else {
                if ($Params.IsPresent) {
                    $Text = (Format-Range -Count $OutputCount -Format 'R{0} r{0}') -join ', ';
                } else {
                    $Text = (Format-Range -Count $OutputCount -Format 'R{0}') -join ', ';
                }
            }
        }
    }

    if ($TResult.IsPresent) {
        if ($Count -gt 0 -or $OutputCount -gt 0) {
            $Text = "$Text, TResult";
        } else {
            $Text = 'TResult';
        }
    }
    if ($Params.IsPresent) {
        "($Text)" | Write-Output;
    } else {
        if ($Count -gt 0 -or $OutputCount -gt 0 -or $TResult.IsPresent) {
            if ($Type.IsPresent) {
                "<$Text>" | Write-Output;
            } else {
                if ($Doc.IsPresent) {
                    "{$Text}" | Write-Output;
                } else {
                    $Text | Write-Output;
                }
            }
        }
    }
}

Function Write-OrdinalString {
    Param(
        [Parameter(Mandatory = $true)]
        [int]$Value
    )

    Switch ($Value) {
        1 {
            'first' | Write-Output
            break;
        }
        2 {
            'second' | Write-Output
            break;
        }
        3 {
            'third' | Write-Output
            break;
        }
        4 {
            'fourth' | Write-Output
            break;
        }
        5 {
            'fifth' | Write-Output
            break;
        }
        6 {
            'sixth' | Write-Output
            break;
        }
        7 {
            'seventh' | Write-Output
            break;
        }
        8 {
            'eighth' | Write-Output
            break;
        }
        9 {
            'ninth' | Write-Output
            break;
        }
        default {
            if ($Value -ne 0) {
                switch ($Value % 100) {
                    11 {
                        '$($Value)th' | Write-Output
                        break;
                    }
                    12 {
                        '$($Value)th' | Write-Output
                        break;
                    }
                    13 {
                        '$($Value)th' | Write-Output
                        break;
                    }
                    default {
                        switch ($Value % 10) {
                            1 {
                                '$($Value)st' | Write-Output
                                break;
                            }
                            2 {
                                '$($Value)nd' | Write-Output
                                break;
                            }
                            3 {
                                '$($Value)rd' | Write-Output
                                break;
                            }
                            default {
                                '$($Value)th' | Write-Output
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            break;
        }
    }
}

Function Write-ParamDocComment {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Name,
        
        [Parameter(Mandatory = $true)]
        [string]$Description,
        
        [switch]$TypeParam
    )
    if ($TypeParam.IsPresent) {
        "    /// <typeparam name=`"$Name`">$Description</typeparam>" | Write-Output;
    } else {
        "    /// <param name=`"$Name`">$Description</param>" | Write-Output;
    }
}

Function Write-SummaryDocComment {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Text
    )

    Begin {
        '    /// <summary>' | Write-Output;
    }
    Process {
        @($Text.Trim() -split '\r\n?|\n') | ForEach-Object {
            "    /// $_" | Write-Output;
        }
    }
    End {
        '    /// </summary>' | Write-Output;
    }
}

Function Write-GenericParamDocComment {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [int]$Count,
        
        [int]$OutputCount = 0,
        
        [switch]$TypeParam
    )

    $InSplat = @{ Name = 't' };
    $OutSplat = @{ Name = 'r' };
    $Lead = 'The';
    if ($TypeParam.IsPresent) {
        $InSplat['TypeParam'] = $TypeParam;
        $InSplat['Name'] = 'T';
        $OutSplat['TypeParam'] = $TypeParam;
        $OutSplat['Name'] = 'R';
        $Lead = 'The type of the';
    }
    if ($Count -eq 1) {
        if ($OutputCount -eq 1) {
            Write-ParamDocComment @InSplat -Description "$Lead first parameter.";
            Write-ParamDocComment @OutSplat -Description "$Lead output parameter.";
        } else {
            if ($OutputCount -gt 0) {
                Write-ParamDocComment @InSplat -Description "$Lead first parameter.";
                $n = $OutSplat['Name'];
                for ($i = 0; $i -lt $OutputCount; $i++) {
                    $OutSplat['Name'] = "$n$($i + 1)"
                    Write-ParamDocComment @OutSplat -Description "$Lead $(Write-OrdinalString -Value ($i + 1)) output parameter.";
                }
            } else {
                Write-ParamDocComment @InSplat -Description "$Lead parameter.";
            }
        }
    } else {
        $n = $InSplat['Name'];
        for ($i = 0; $i -lt $Count; $i++) {
            $InSplat['Name'] = "$n$($i + 1)";
            Write-ParamDocComment @InSplat -Description "$Lead $(Write-OrdinalString -Value ($i + 1)) parameter.";
        }
        if ($OutputCount -eq 1) {
            Write-ParamDocComment @OutSplat -Description "$Lead output parameter.";
        } else {
            if ($OutputCount -gt 0) {
                $n = $OutSplat['Name'];
                for ($i = 0; $i -lt $OutputCount; $i++) {
                    $OutSplat['Name'] = "$n$($i + 1)"
                    Write-ParamDocComment @OutSplat -Description "$Lead $(Write-OrdinalString -Value ($i + 1)) output parameter.";
                }
            }
        }
    }
}

Function Write-GenericDelegateDeclaration {
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateRange(1, 7)]
        [int]$OutputCount,

        [int]$ParamCount = 0,
        
        [switch]$TResult
    )

    $Text = 'Encapsulates a method that has ';
    if ($ParamCount -eq 0) {
        if ($OutputCount -eq 1) {
            $Text = "$($Text)1 output parameter";
        } else {
            $Text = "$Text$OutputCount output parameters";
        }
    } else {
        if ($ParamCount -eq 1) {
            $Text = "$($Text)1 parameter";
        } else {
            $Text = "$Text$ParamCount parameters";
        }
        if ($OutputCount -eq 1) {
            $Text = "$Text, 1 output parameter,";
        } else {
            $Text = "$Text, $OutputCount output parameters,";
        }
    }
    if ($TResult.IsPresent) {
        "$Text and returns a value." | Write-SummaryDocComment;
    } else {
        "$Text and does not return a value." | Write-SummaryDocComment;
    }
    Write-GenericParamDocComment -Count $ParamCount -OutputCount $OutputCount -TypeParam;
    if ($TResult.IsPresent) {
        Write-ParamDocComment -Name 'TResult' -Description 'The type of return value.' -TypeParam;
    }
    Write-GenericParamDocComment -Count $ParamCount -OutputCount $OutputCount;
    if ($TResult.IsPresent) {
        if ($OutputCount -eq 1) {
            "    public delegate TResult FuncWithOutput$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Type -TResult)$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Params);" | Write-Output;
        } else {
            "    public delegate TResult FuncWithOutput$OutputCount$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Type -TResult)$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Params);" | Write-Output;
        }
    } else {
        if ($OutputCount -eq 1) {
            "    public delegate void ActionWithOutput$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Type)$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Params);" | Write-Output;
        } else {
            "    public delegate void ActionWithOutput$OutputCount$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Type)$(Write-GenericArguments -Count $ParamCount -OutputCount $OutputCount -Params);" | Write-Output;
        }
    }

}

Function Write-InvocationInterface {
    Param(
        [int]$ParamCount = 0,
        
        [int]$OutputCount = 0,
        
        [switch]$TResult
    )

    $Text = 'Defines test data to represent results from';
    switch ($ParamCount) {
        0 {
            switch ($OutputCount) {
                0 {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"Func{TResult}";
                    } else {
                        $Text = "$Text an <seealso cref=`"Action";
                    }
                    break;
                }
                1 {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"FuncWithOutput{R, TResult}";
                    } else {
                        $Text = "$Text an <seealso cref=`"ActionWithOutput{R}";
                    }
                    break;
                }
                default {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"FuncWithOutput$OutputCount{R1";
                    } else {
                        $Text = "$Text an <seealso cref=`"ActionWithOutput$OutputCount{R1";
                    }
                    for ($i = 1; $i -lt $OutputCount; $i++) { $Text = "$Text, R$($i + 1)" }
                    if ($TResult.IsPresent) {
                        $Text = "$Text, TResult}";
                    } else {
                        $Text = "$Text}";
                    }
                    break;
                }
            }
            break;
        }
        1 {
            switch ($OutputCount) {
                0 {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"Func{T, TResult}";
                    } else {
                        $Text = "$Text an <seealso cref=`"Action{T}";
                    }
                    break;
                }
                1 {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"FuncWithOutput{T, R, TResult}";
                    } else {
                        $Text = "$Text an <seealso cref=`"ActionWithOutput{T, R}";
                    }
                    break;
                }
                default {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"FuncWithOutput$OutputCount{T";
                    } else {
                        $Text = "$Text an <seealso cref=`"ActionWithOutput$OutputCount{T";
                    }
                    for ($i = 0; $i -lt $OutputCount; $i++) { $Text = "$Text, R$($i + 1)" }
                    if ($TResult.IsPresent) {
                        $Text = "$Text, TResult}";
                    } else {
                        $Text = "$Text}";
                    }
                    break;
                }
            }
            break;
        }
        default {
            switch ($OutputCount) {
                0 {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"Func{T1";
                    } else {
                        $Text = "$Text an <seealso cref=`"Action{T1";
                    }
                    for ($i = 1; $i -lt $ParamCount; $i++) { $Text = "$Text, T$($i + 1)" }
                    break;
                }
                1 {
                    if ($TResult.Write) {
                        $Text = "$Text a <seealso cref=`"FuncWithOutput{T1";
                    } else {
                        $Text = "$Text an <seealso cref=`"ActionWithOutput{T1";
                    }
                    for ($i = 1; $i -lt $ParamCount; $i++) { $Text = "$Text, T$($i + 1)" }
                    $Text = "$Text, R";
                    break;
                }
                default {
                    if ($TResult.IsPresent) {
                        $Text = "$Text a <seealso cref=`"FuncWithOutput$OutputCount{T1";
                    } else {
                        $Text = "$Text an <seealso cref=`"ActionWithOutput$OutputCount{T1";
                    }
                    for ($i = 1; $i -lt $ParamCount; $i++) { $Text = "$Text, T$($i + 1)" }
                    for ($i = 0; $i -lt $OutputCount; $i++) { $Text = "$Text, R$($i + 1)" }
                    break;
                }
            }
            if ($TResult.IsPresent) {
                $Text = "$Text, TResult}";
            } else {
                $Text = "$Text}";
            }
            break;
        }
    }
    "$Text`"/> invocation." | Write-SummaryDocComment;
    Write-GenericParamDocComment -Count $ParamCount -OutputCount $OutputCount -TypeParam;
    if ($TResult.IsPresent) {
        Write-ParamDocComment -Name 'TResult' -Description 'The type of return value.' -TypeParam;
    }
    Write-GenericParamDocComment -Count $ParamCount -OutputCount $OutputCount;

    if ($OutputCount -gt 0) {
        if ($ParamCount -eq 1) {
            if ($OutputCount -eq 1) {
            } else {
            }
        } else {
            if ($ParamCount -gt 1) {
                if ($OutputCount -eq 1) {
                } else {
                }
            } else {
                if ($OutputCount -lt 1) {
                    '    public interface IInvocationOutput { }' | Write-Output;
                } else {
                    if ($OutputCount -eq 1) {
                        if ($TResult.IsPresent) {
                            '    public interface IFuncInvocation1<R, TResult> : IFuncInvocation<TResult>, IInvocationOutput<R> { }' | Write-Output;
                        } else {
                        @"
    public interface IInvocationOutput<R> : IInvocationOutput
    {
        R Output1 { get; }
    }
"@ | Write-Output;
                        }
                    } else {
                        if ($TResult.IsPresent) {
                            "    public interface IFuncInvocation$OutputCount$(Write-GenericArguments -OutputCount $OutputCount -Type -TResult) : IFuncInvocation$OutputCount$(Write-GenericArguments -OutputCount ($OutputCount - 1) -Type -TResult), IInvocationOutput$(Write-GenericArguments -Count $OutputCount -Type) { }" | Write-Output;
                        } else {
                            @"
    public interface IInvocationOutput$(Write-GenericArguments -Count $OutputCount -Type) : IInvocationOutput$(Write-GenericArguments -Count ($OutputCount - 1) -Type)
    {
        T$OutputCount Input$OutputCount { get; }
    }
"@ | Write-Output;
                        }
                    }
                }
            }
        }
    } else {
        if ($PSBoundParameters.ContainsKey('ParamCount')) {
            if ($ParamCount -lt 1) {
                if ($TResult.IsPresent) {
                    @"
    public interface IFuncInvocation<TResult> : IFuncInvocation
    {
        new TResult ReturnValue { get; }
    }
"@ | Write-Output;
                } else {
                    '    public interface IInvocationInput { }' | Write-Output;
                }
            } else {
                if ($ParamCount -eq 1) {
                    if ($TResult.IsPresent) {
                        '    public interface IFuncInvocation<T, TResult> : IFuncInvocation<TResult>, IInvocationInput<T> { }' | Write-Output;
                    } else {
                        @"
    public interface IInvocationInput<T> : IInvocationInput
    {
        T Input1 { get; }
    }
"@ | Write-Output;
                    }
                } else {
                    if ($TResult.IsPresent) {
                        @"
    public interface IFuncInvocation$(Write-GenericArguments -Count $ParamCount -Type -TResult) : IFuncInvocation$(Write-GenericArguments -Count ($ParamCount - 1) -Type -TResult), IInvocationInput$(Write-GenericArguments -Count $ParamCount -Type)
    {
        T$ParamCount Input$ParamCount { get; }
    }
"@ | Write-Output;
                    } else {
                        @"
    public interface IInvocationInput$(Write-GenericArguments -Count $ParamCount -Type) : IInvocationInput$(Write-GenericArguments -Count ($ParamCount - 1) -Type)
    {
        T$ParamCount Input$ParamCount { get; }
    }
"@ | Write-Output;
                    }
                }
            }
        } else {
            if ($TResult.IsPresent) {
                @"
    public interface IFuncInvocation : IInvocationInput
    {
        object ReturnValue { get; }
    }
"@ | Write-Output;
            } else {
                '    public interface IInvocationInput { }' | Write-Output;
            }
        }
    }
}

<#
for ($o = 1; $o -lt 8; $o++) {
    for ($p = 0; $p -lt 8; $p++) {
        if (($p + $o) -lt 8) {
            ''
            Write-GenericDelegateDeclaration -ParamCount $p -OutputCount $o;
        }
    }
}
for ($i = 0; $i -lt 8; $i++) {
    ''
    Write-InvocationInterface -ParamCount $i;
}

for ($i = 0; $i -lt 8; $i++) {
    ''
    Write-InvocationInterface -OutputCount $i;
}


for ($p = 0; $p -lt 8; $p++) {
    ''
    Write-InvocationInterface -ParamCount $p -TResult;
}
for ($o = 1; $o -lt 8; $o++) {
    for ($p = 0; $p -lt 8; $p++) {
        if (($p + $o) -lt 8) {
            ''
            Write-InvocationInterface -ParamCount $p -OutputCount $o -TResult;
        }
    }
}
#>

''
Write-InvocationInterface -OutputCount 0 -TResult;
''
Write-InvocationInterface -OutputCount 1 -TResult;
''
Write-InvocationInterface -OutputCount 2 -TResult;
''
Write-InvocationInterface -OutputCount 3 -TResult;
