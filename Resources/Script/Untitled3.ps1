class FactoryType {
    [string]$GenericName;
    [string]$ClrType
}

class FactoryMethod {
    [bool]$IsTimed;
    [FactoryType]$StateType;
    [bool]$HandlesOnCompleted;
    [FactoryType]$ResultType;
    [bool]$PassesTokens;

    [string] GetTestAsyncMethodName() {
        $MethodName = 'TestAction';
        if ($null -ne $this.ResultType) { $MethodName = 'TestFunc' }
        if ($null -ne $this.StateType) { $MethodName += 'State' }
        if ($this.IsTimed) { return "Timed$MethodName" }
        return $MethodName;
    }

    [string] GetTestAsyncMethodParameters() {
        $AsyncMethodParameters = 'AutoResetEvent syncEvent, Initialize';
        if ($null -ne $this.StateType) { $AsyncMethodParameters += 'State' }
        $AsyncMethodParameters += 'Data initializeData, UpdateData updateData, ';
        if ($null -ne $this.ResultType) {
            $AsyncMethodParameters += 'IResultData';
        } else {
            $AsyncMethodParameters += 'IActionCompleteData';
        }
        $AsyncMethodParameters += ' completeData, ';
        $AsyncMethodParameters += $this.GetProgressType($false);
        return "$AsyncMethodParameters progress";
    }

    [string] GetProgressEventType([bool]$AsGeneric) {
        if ($null -eq $this.StateType) {
            if ($this.IsTimed) {
                return 'ITimedBackgroundProgressEvent';
            }
            return 'IBackgroundProgressEvent';
        }
        if ($AsGeneric) {
            if ($this.IsTimed) {
                return "ITimedBackgroundProgressEvent<$($this.StateType.GenericName)>";
            }
            return "IBackgroundProgressEvent<$($this.StateType.GenericName)>";
        }
        if ($this.IsTimed) {
            return "ITimedBackgroundProgressEvent<$($this.StateType.ClrType)>";
        }
        return "IBackgroundProgressEvent<$($this.StateType.ClrType)>";
    }

    [string] GetProgressType([bool]$AsGeneric) {
        if ($null -eq $this.StateType) {
            if ($this.IsTimed) {
                return "ITimedBackgroundProgress<$($this.GetProgressEventType($AsGeneric))>";
            }
            return "IBackgroundProgress<$($this.GetProgressEventType($AsGeneric))>";
        }
        if ($AsGeneric) {
            if ($this.IsTimed) {
                return "ITimedBackgroundProgress<$($this.StateType.GenericName), $($this.GetProgressEventType($AsGeneric))>";
            }
            return "IBackgroundProgress<$($this.StateType.GenericName), $($this.GetProgressEventType($AsGeneric))>";
        }
        if ($this.IsTimed) {
            return "ITimedBackgroundProgress<$($this.StateType.ClrType), $($this.GetProgressEventType($AsGeneric))>";
        }
        return "IBackgroundProgress<$($this.StateType.ClrType), $($this.GetProgressEventType($AsGeneric))>";
    }

    [string] GetTaskType([bool]$AsGeneric) {
        if ($null -eq $this.ResultType) {
            return 'Task';
        }
        if ($AsGeneric) {
            return "Task<$($this.ResultType.GenericName)>";
        }
        return "Task<$($this.ResultType.ClrType)>";
    }

    [string] GetOperationType([bool]$AsGeneric) {
        if ($null -eq $this.ResultType) {
            if ($null -eq $this.StateType) {
                if ($this.IsTimed) {
                    return 'ITimedBackgroundOperation';
                }
                return 'IBackgroundOperation';
            }
            if ($AsGeneric) {
                if ($this.IsTimed) {
                    return "ITimedBackgroundOperation<$($this.StateType.GenericName)>";
                }
                return "IBackgroundOperation<$($this.StateType.GenericName)>";
            }
            if ($this.IsTimed) {
                return "ITimedBackgroundOperation<$($this.StateType.ClrType)>";
            }
            return "IBackgroundOperation<$($this.StateType.ClrType)>";
        }
        if ($AsGeneric) {
            if ($null -eq $this.StateType) {
                if ($this.IsTimed) {
                    return "ITimedBackgroundFunc<$($this.ResultType.GenericName)>";
                }
                return "IBackgroundFunc<$($this.ResultType.GenericName)>";
            }
            if ($this.IsTimed) {
                return "ITimedBackgroundFunc<$($this.StateType.GenericName), $($this.ResultType.GenericName)>";
            }
            return "IBackgroundFunc<$($this.StateType.GenericName), $($this.ResultType.GenericName)>";
        }
        if ($null -eq $this.StateType) {
            if ($this.IsTimed) {
                return "ITimedBackgroundFunc<$($this.ResultType.ClrType)>";
            }
            return "IBackgroundFunc<$($this.ResultType.ClrType)>";
        }
        if ($this.IsTimed) {
            return "ITimedBackgroundFunc<$($this.StateType.ClrType), $($this.ResultType.ClrType)>";
        }
        return "IBackgroundFunc<$($this.StateType.ClrType), $($this.ResultType.ClrType)>";
    }

    [string] GetAsyncMethodDelegateType([bool]$AsGeneric) {
        return "Func<$($this.GetProgressType($AsGeneric)), $($this.GetTaskType($AsGeneric))>";
    }

    [string] GetCompletedEventType([bool]$AsGeneric) {
        if ($null -eq $this.StateType) {
            if ($this.IsTimed) {
                return 'ITimedBackgroundOperationCompletedEvent';
            }
            return 'IBackgroundOperationCompletedEvent';
        }
        if ($AsGeneric) {
            if ($this.IsTimed) {
                return "ITimedBackgroundOperationCompletedEvent<$($this.StateType.GenericName)>";
            }
            return "IBackgroundOperationCompletedEvent<$($this.StateType.GenericName)>";
        }
        if ($this.IsTimed) {
            return "ITimedBackgroundOperationCompletedEvent<$($this.StateType.ClrType)>";
        }
        return "IBackgroundOperationCompletedEvent<$($this.StateType.ClrType)>";
    }

    [string] GetFinalEventType([bool]$AsGeneric) {
        if ($null -eq $this.ResultType) {
            return $this.GetCompletedEventType($AsGeneric);
        }
        if ($AsGeneric) {
            if ($null -eq $this.StateType) {
                if ($this.IsTimed) {
                    return "ITimedBackgroundOperationResultEvent<$($this.ResultType.GenericName)>";
                }
                return "IBackgroundOperationResultEvent<$($this.ResultType.GenericName)>";
            }
            if ($this.IsTimed) {
                return "ITimedBackgroundOperationResultEvent<$($this.StateType.GenericName), $($this.ResultType.GenericName)>";
            }
            return "IBackgroundOperationResultEvent<$($this.StateType.GenericName), $($this.ResultType.GenericName)>";
        }
        if ($null -eq $this.StateType) {
            if ($this.IsTimed) {
                return "ITimedBackgroundOperationResultEvent<$($this.ResultType.ClrType)>";
            }
            return "IBackgroundOperationResultEvent<$($this.ResultType.ClrType)>";
        }
        if ($this.IsTimed) {
            return "ITimedBackgroundOperationResultEvent<$($this.StateType.ClrType), $($this.ResultType.ClrType)>";
        }
        return "IBackgroundOperationResultEvent<$($this.StateType.ClrType), $($this.ResultType.ClrType)>";
    }

    [string] GetNewFinalEventArgs([string]$StatusVarName) {
        if ($null -eq $this.ResultType) {
            if ($null -eq $this.StateType) {
                if ($this.IsTimed) {
                    return 'new TimedBackgroundProcessCompletedEventArgs(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled);';
                }
                return 'new BackgroundProcessCompletedEventArgs(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled);';
            }
            if ($this.IsTimed) {
                return "new TimedBackgroundProcessCompletedEventArgs<$($this.StateType.ClrType)>(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled, $StatusVarName);";
            }
            return "new BackgroundProcessCompletedEventArgs<$($this.StateType.ClrType)>(backgroundOperation, completeData.Code, null, !backgroundOperation.Task.IsCanceled, $StatusVarName);";
        }
        if ($null -eq $this.StateType) {
            if ($this.IsTimed) {
                return "new TimedBackgroundProcessResultEventArgs<$($this.ResultType.ClrType)>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result);";
            }
            return "new BackgroundProcessResultEventArgs<$($this.ResultType.ClrType)>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result);";
        }
        if ($this.IsTimed) {
            return "new TimedBackgroundProcessResultEventArgs<$($this.StateType.ClrType), $($this.ResultType.ClrType)>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result, $StatusVarName);";
        }
        return "new BackgroundProcessResultEventArgs<$($this.StateType.ClrType), $($this.ResultType.ClrType)>(backgroundOperation, completeData.Code, null, backgroundOperation.Task.Result, $StatusVarName);";
    }

    [string] GetOnCompletedType([bool]$AsGeneric) {
        if ($this.HandlesOnCompleted) {
            return "Func<$($this.GetOperationType($AsGeneric)), $($this.GetFinalEventType($AsGeneric))>";
        }
        return '';
    }

    [string] GetFactoryMethodName([bool]$AsGeneric) {
        if ($null -eq $this.ResultType) {
            if ($null -eq $this.StateType) {
                return 'InvokeAsync';
            }
            if ($AsGeneric) {
                return "InvokeAsync<$($this.StateType.GenericName)>";
            }
            return "InvokeAsync<$($this.StateType.ClrType)>";
        }
        if ($AsGeneric) {
            if ($null -eq $this.StateType) {
                return "InvokeAsync<$($this.ResultType.GenericName)>";
            }
            return "InvokeAsync<$($this.StateType.GenericName), $($this.ResultType.GenericName)>";
        }
        if ($null -eq $this.StateType) {
            return "InvokeAsync<$($this.ResultType.ClrType)>";
        }
        return "InvokeAsync<$($this.StateType.ClrType), $($this.ResultType.ClrType)>";
    }

    [string[]] GetFactoryMethodParameters([bool]$IncludeParameterNames, [bool]$AsGeneric) {
        $Parameters = [System.Collections.ObjectModel.Collection[string]]::new();
        if ($IncludeParameterNames) {
            $Parameters.Add("[DisallowNull] $($this.GetAsyncMethodDelegateType($AsGeneric)) asyncMethodDelegate");
        } else {
            $Parameters.Add($this.GetAsyncMethodDelegateType($AsGeneric));
        }
        if ($this.HandlesOnCompleted) {
            if ($IncludeParameterNames) {
                $Parameters.Add("[DisallowNull] $($this.GetOnCompletedType($AsGeneric)) onCompleted");
            } else {
                $Parameters.Add($this.GetOnCompletedType($AsGeneric));
            }
        }
        if ($IncludeParameterNames) {
            $Parameters.Add('[DisallowNull] string activity');
            $Parameters.Add('[DisallowNull] string statusDescription');
        } else {
            $Parameters.Add('string');
            $Parameters.Add('string');
        }
        if ($null -ne $this.StateType) {
            if ($AsGeneric) {
                if ($IncludeParameterNames) {
                    $Parameters.Add("$($this.StateType.GenericName) state");
                } else {
                    $Parameters.Add($this.StateType.GenericName);
                }
            } else {
                if ($IncludeParameterNames) {
                    $Parameters.Add("$($this.StateType.ClrType) state");
                } else {
                    $Parameters.Add($this.StateType.ClrType);
                }
            }
        }
        if ($this.PassesTokens) {
            if ($IncludeParameterNames) {
                $Parameters.Add('params CancellationToken[] tokens');
            } else {
                $Parameters.Add('CancellationToken[]');
            }
        }
        return $Parameters;
    }

    [string] GetFactoryMethodSignature([bool]$IncludeParameterNames, [int]$WrapColumn, [int]$IndentLevel, [string]$IndentText) {
        $Signature = '';
        for ($i = 0; $i -lt $IndentLevel; $i++) { $Signature += $IndentText }
        $Parameters = $this.GetFactoryMethodParameters($IncludeParameterNames, $true);
        $Signature = "$Signature$($this.GetOperationType($true)) $($this.GetFactoryMethodName($true))(";
        if ($WrapColumn -lt 0) {
            return "$Signature$($Parameters -join ', '))";
        }
        $SubIndent = '';
        $Lines = [System.Collections.ObjectModel.Collection[string]]::new();
        if ($IncludeParameterNames) {
            $Lines.Add('        /// <summary>');
            if ($this.IsTimed) {
                if ($null -eq $this.ResultType) {
                    $Lines.Add('        /// Starts a timed asynchronous operation.');
                } else {
                    $Lines.Add('        /// Starts a timed asynchronous operation to produce a result value.');
                }
            } else {
                if ($null -eq $this.ResultType) {
                    $Lines.Add('        /// Starts an asynchronous operation.');
                } else {
                    $Lines.Add('        /// Starts an asynchronous operation to produce a result value.');
                }
            }
            $Lines.Add('        /// <summary>');
            if ($null -ne $this.StateType) {
                $Lines.Add("        /// <typeparam name=`"$($this.StateType.GenericName)`">The type of object to associate with the asynchronous operation.</typeparam>");
            }
            if ($null -ne $this.ResultType) {
                $Lines.Add("        /// <typeparam name=`"$($this.ResultType.GenericName)`">The type of value produced by the asynchronous operation.</typeparam>");
            }
            $Lines.Add('        /// <param name="asyncMethodDelegate">Delegate that refers to the asynchronous method to invoke.</param>');
            if ($this.HandlesOnCompleted) {
                $Lines.Add("        /// <param name=`"onCompleted`">Gets called to create the final <see cref=`"$($this.GetFinalEventType($true).Replace('<', '{').Replace('>', '}'))`"/> notification object.</param>");
            }
            $Lines.Add('        /// <param name="activity">Short description of the high-level activity that the asynchronous operation performs.</param>'),
            $Lines.Add('        /// <param name="statusDescription">The initial status description for the background operation.</param>')
            if ($null -ne $this.StateType) {
                $Lines.Add('        /// <param name="state">The object to associate with the asynchronous operation.</param>');
            }
            if ($this.PassesTokens) {
                $Lines.Add('        /// <param name="tokens">Additional <see cref="CancellationToken">CancellationTokens</see> that can be used to cancel the asynchronous operation.</param>');
            }
            $Lines.Add("        /// <returns>An <see cref=`"$($this.GetOperationType($true).Replace('<', '{').Replace('>', '}'))`"/> object representing the background operation.</returns>");
            if ($this.HandlesOnCompleted) {
                $Lines.Add('        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> or <paramref name="onCompleted"/> is <see langword="null"/>.</exception>');
            } else {
                $Lines.Add('        /// <exception cref="ArgumentNullException"><paramref name="asyncMethodDelegate"/> is <see langword="null"/>.</exception>');
            }
            $Lines.Add('        /// <exception cref="ArgumentException"><paramref name="activity"/> or <paramref name="statusDescription"/> is <see langword="null"/>');
            $Lines.Add('        /// or contains only white-space characters.</exception>');
            $Lines.Add('        /// <exception cref="InvalidOperationException"><paramref name="asyncMethodDelegate"/> returned <see langword="null"/>.</exception>');
        }
        for ($i = 0; $i -le $IndentLevel; $i++) { $SubIndent += $IndentText }
        $t = "$Signature$($Parameters[0])";
        if ($t.Length -ge $WrapColumn) {
            $Lines.Add($Signature);
            $Signature = $SubIndent + $Parameters[0];
        } else {
            $Signature = $t;
        }
        foreach ($p in @($Parameters | Select-Object -Skip 1)) {
            $t = "$Signature, $p";
            if ($t.Length -ge $WrapColumn) {
                $Lines.Add("$Signature,");
                $Signature = $SubIndent + $p;
            } else {
                $Signature = $t;
            }
        }
        if ($Lines.Count -eq 0) { return "$Signature)" }
        $Lines.Add("$Signature)");
        return ($Lines | Out-String).TrimEnd();
    }

    [string] GetFactoryMethodSignature([bool]$IncludeParameterNames) {
        if ($IncludeParameterNames) {
            return $this.GetFactoryMethodSignature($true, 170, 2, '    ');
        }
        return $this.GetFactoryMethodSignature($false, -1, 0, '');
    }

    [string] GetInvokeFactoryMethod() {
        if ($this.HandlesOnCompleted) {
            if ($null -ne $this.StateType) {
                if ($this.PassesTokens) {
                    return "InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token)";
                }
                return "InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, initializeData.State)";
            }
            if ($this.PassesTokens) {
                return "InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token)";
            }
            return "InvokeAsync(asyncMethodDelegate, onCompleted, initializeData.Activity, initializeData.StatusDescription)";
        }
        if ($null -ne $this.StateType) {
            if ($this.PassesTokens) {
                return "InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State, tokenSource.Token)";
            }
            return "InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, initializeData.State)";
        }
        if ($this.PassesTokens) {
            return "InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription, tokenSource.Token)";
        }
        return "InvokeAsync(asyncMethodDelegate, initializeData.Activity, initializeData.StatusDescription)";
    }

    [string] GetTestName() {
        $TestName = 'InvokeAsync';
        if ($this.IsTimed) { $TestName = 'InvokeTimedAsync'; }
        if ($null -eq $this.ResultType) { $TestName += 'Action' } else { $TestName += 'Func' }
        if ($this.HandlesOnCompleted) { $TestName += 'Completed' }
        if ($null -ne $this.StateType) { $TestName += 'State' }
        if ($this.PassesTokens) { return $TestName + 'TokenTest' }
        return $TestName + "Test";
    }
}

$Random = [Random]::new();
$StringWriter = [System.IO.StringWriter]::new();

foreach ($FactoryMethod in @(
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $true; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' }; Istimed = $false; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $true; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' }; HandlesOnCompleted = $false; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = $null; HandlesOnCompleted = $true; PassesTokens = $false },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $true },
    [FactoryMethod]@{ ResultType = $null; Istimed = $false; StateType = $null; HandlesOnCompleted = $false; PassesTokens = $false }
)) {
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('        [DataTestMethod]');
    $StringWriter.Write('        [DynamicData(nameof(GetAsync');
    if ($null -ne $FactoryMethod.StateType) {
        if ($null -ne $FactoryMethod.ResultType) {
            $StringWriter.Write('FuncResultState');
        } else {
            $StringWriter.Write('ActionCompletedState');
        }
    } else {
        if ($null -ne $FactoryMethod.ResultType) {
            $StringWriter.Write('FuncResult');
        } else {
            $StringWriter.Write('ActionCompleted');
        }
    }
    $StringWriter.WriteLine('Data), DynamicDataSourceType.Method)]');
    $StringWriter.WriteLine('        [Priority(10)]');
    $StringWriter.Write('        public async Task ');
    $StringWriter.Write($FactoryMethod.GetTestName());
    $StringWriter.Write('(Initialize');
    if ($null -ne $FactoryMethod.StateType) { $StringWriter.Write('State') }
    $StringWriter.Write('Data initializeData, UpdateData updateData, ');
    if ($null -ne $FactoryMethod.ResultType) { $StringWriter.Write('IResultData') } else { $StringWriter.Write('IActionCompleteData') }
    $StringWriter.WriteLine(' completeData)');
    $StringWriter.WriteLine('        {');
    $StringWriter.WriteLine('            using AutoResetEvent fgEvent = new(false);');
    $StringWriter.WriteLine('            using AutoResetEvent bgEvent = new(false);');
    $StringWriter.Write('            ');
    $StringWriter.Write($FactoryMethod.GetTaskType($false));
    $StringWriter.Write(' asyncMethodDelegate(');
    $StringWriter.Write($FactoryMethod.GetProgressType($false));
    $StringWriter.WriteLine(' progress) =>');
    $StringWriter.Write('                ');
    $StringWriter.Write($FactoryMethod.GetTestAsyncMethodName());
    $StringWriter.WriteLine('(fgEvent, bgEvent, initializeData, updateData, completeData, progress);');
    if ($FactoryMethod.HandlesOnCompleted) {
        $StringWriter.Write('            ');
        $StringWriter.Write($FactoryMethod.GetFinalEventType($false));
        $StringWriter.Write(' onCompleted(');
        $StringWriter.Write($FactoryMethod.GetOperationType($false));
        $StringWriter.WriteLine(' backgroundOperation) =>');
        $StringWriter.Write('                    ');
        $StringWriter.Write($FactoryMethod.GetNewFinalEventArgs('string.IsNullOrWhiteSpace(completeData.StatusDescription) ? backgroundOperation.StatusDescription : completeData.StatusDescription'));
        $StringWriter.WriteLine(';');
    }
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            IBackgroundProgressService service = Hosting.GetBackgroundProgressService();');
    $StringWriter.WriteLine('            if (service is null)');
    $StringWriter.WriteLine('                throw new AssertInconclusiveException();');
    if ($FactoryMethod.PassesTokens) { $StringWriter.WriteLine('            using CancellationTokenSource tokenSource = new();') }
    $StringWriter.Write('            ObserverHelper<');
    $StringWriter.Write($FactoryMethod.GetProgressEventType($false));
    $StringWriter.WriteLine('> operationObserver = new();');
    $StringWriter.Write('            ');
    $StringWriter.Write($FactoryMethod.GetOperationType($false));
    $StringWriter.Write(' backgroundOperation = service.');
    $StringWriter.Write($FactoryMethod.GetInvokeFactoryMethod());
    $StringWriter.WriteLine(';');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #region Test Initial Progress Properties');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            bgEvent.WaitOne(); // Wait until bg operation being executed');
    $StringWriter.WriteLine('            using IDisposable subscription = backgroundOperation.Subscribe(operationObserver);');
    $StringWriter.WriteLine('            Assert.IsNotNull(backgroundOperation);');
    $StringWriter.WriteLine('            Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);');
    $StringWriter.WriteLine('            Assert.AreEqual(initializeData.StatusDescription, backgroundOperation.StatusDescription);');
    $StringWriter.WriteLine('            Assert.AreEqual(string.Empty, backgroundOperation.CurrentOperation);');
    if ($null -ne $FactoryMethod.StateType) { $StringWriter.WriteLine('            Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);') }
    $StringWriter.WriteLine('            Assert.IsFalse(backgroundOperation.PercentComplete.HasValue);');
    $StringWriter.WriteLine('            fgEvent.Set(); // Signal that we''ve tested initial status properties');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #endregion');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #region Test Progress Update');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            bgEvent.WaitOne(); // Wait until status has been updated');
    $StringWriter.WriteLine('            Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);');
    $StringWriter.WriteLine('            Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,');
    $StringWriter.WriteLine('                backgroundOperation.StatusDescription);');
    $StringWriter.WriteLine('            Assert.AreEqual(updateData.CurrentOperation, backgroundOperation.CurrentOperation);');
    $StringWriter.WriteLine('            Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);');
    $StringWriter.WriteLine('            if (updateData.PercentComplete.HasValue)');
    $StringWriter.WriteLine('                Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);');
    if ($null -ne $FactoryMethod.StateType) { $StringWriter.WriteLine('            Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);') }
    $StringWriter.WriteLine('            Assert.AreEqual(updateData.PercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);');
    $StringWriter.WriteLine('            if (updateData.PercentComplete.HasValue)');
    $StringWriter.WriteLine('                Assert.AreEqual(updateData.PercentComplete.Value, backgroundOperation.PercentComplete.Value);');
    $StringWriter.Write('            Assert.IsTrue(operationObserver.TryDequeue(out Observed<');
    $StringWriter.Write($FactoryMethod.GetProgressEventType($false));
    $StringWriter.WriteLine('> observed));');
    $StringWriter.WriteLine('            Assert.IsFalse(observed.IsComplete);');
    $StringWriter.WriteLine('            Assert.IsNull(observed.Error);');
    $StringWriter.Write('            ');
    $StringWriter.Write($FactoryMethod.GetProgressEventType($false));
    $StringWriter.WriteLine(' progressEvent = observed.Value;');
    $StringWriter.WriteLine('            Assert.IsNotNull(progressEvent);');
    $StringWriter.WriteLine('            Assert.AreEqual(initializeData.Activity, progressEvent.Activity);');
    $StringWriter.WriteLine('            Assert.AreEqual(string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription,');
    $StringWriter.WriteLine('                progressEvent.StatusDescription);');
    $StringWriter.WriteLine('            Assert.AreEqual(updateData.CurrentOperation, progressEvent.CurrentOperation);');
    $StringWriter.WriteLine('            Assert.AreEqual(updateData.PercentComplete.HasValue, progressEvent.PercentComplete.HasValue);');
    $StringWriter.WriteLine('            if (updateData.PercentComplete.HasValue)');
    $StringWriter.WriteLine('                Assert.AreEqual(updateData.PercentComplete.Value, progressEvent.PercentComplete.Value);');
    $StringWriter.WriteLine('            Assert.AreEqual(updateData.Code.HasValue, progressEvent.Code.HasValue);');
    $StringWriter.WriteLine('            if (updateData.Code.HasValue)');
    $StringWriter.WriteLine('                Assert.AreEqual(updateData.Code.Value, progressEvent.Code.Value);');
    if ($null -ne $FactoryMethod.StateType) { $StringWriter.WriteLine('            Assert.AreEqual(initializeData.State, progressEvent.AsyncState);') }
    $StringWriter.WriteLine('            Assert.AreEqual(backgroundOperation.OperationId, progressEvent.OperationId);');
    $StringWriter.WriteLine('            Assert.IsFalse(progressEvent.ParentId.HasValue);');
    $StringWriter.WriteLine('            Assert.IsFalse(operationObserver.TryDequeue(out observed));');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #endregion');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #region Test Operation Completion');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            string expectedStatusDescription;');
    $StringWriter.WriteLine('            string expectedCurrentOperation;');
    $StringWriter.WriteLine('            byte? expectedPercentComplete;');
    $StringWriter.WriteLine('            MessageCode? expectedCode;');
    $StringWriter.WriteLine('            if (completeData.Cancel)');
    $StringWriter.WriteLine('            {');
    $StringWriter.WriteLine('                expectedStatusDescription = string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription;');
    $StringWriter.WriteLine('                expectedCurrentOperation = updateData.CurrentOperation;');
    $StringWriter.WriteLine('                expectedPercentComplete = updateData.PercentComplete;');
    $StringWriter.WriteLine('                expectedCode = updateData.Code;');
    if ($FactoryMethod.PassesTokens) {
        $StringWriter.WriteLine('                tokenSource.Cancel();');
    } else {
        $StringWriter.WriteLine('                backgroundOperation.Cancel();');
    }
    $StringWriter.WriteLine('                fgEvent.Set(); // Signal that we''ve canceled the operation');
    $StringWriter.WriteLine('                await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => backgroundOperation.Task);');
    $StringWriter.WriteLine('            }');
    $StringWriter.Write('            else if (completeData is ');
    if ($null -ne $FactoryMethod.ResultType) {
        $StringWriter.Write('ErrorResultData');
    } else {
        $StringWriter.Write('ErrorCompleteData')
    }
    $StringWriter.WriteLine(' errorData)');
    $StringWriter.WriteLine('            {');
    $StringWriter.WriteLine('                expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?');
    $StringWriter.WriteLine('                    (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :');
    $StringWriter.WriteLine('                    completeData.StatusDescription;');
    $StringWriter.WriteLine('                expectedCurrentOperation = updateData.CurrentOperation;');
    $StringWriter.WriteLine('                expectedPercentComplete = updateData.PercentComplete;');
    $StringWriter.WriteLine('                expectedCode = errorData.Code.ToMessageCode(MessageCode.UnexpectedError);');
    $StringWriter.WriteLine('                fgEvent.Set(); // Signal that we''re ready to complete');
    $StringWriter.WriteLine('                await Assert.ThrowsExceptionAsync<AsyncOperationException>(() => backgroundOperation.Task);');
    $StringWriter.WriteLine('            }');
    $StringWriter.WriteLine('            else');
    $StringWriter.WriteLine('            {');
    $StringWriter.WriteLine('                expectedStatusDescription = string.IsNullOrWhiteSpace(completeData.StatusDescription) ?');
    $StringWriter.WriteLine('                    (string.IsNullOrWhiteSpace(updateData.StatusDescription) ? initializeData.StatusDescription : updateData.StatusDescription) :');
    $StringWriter.WriteLine('                    completeData.StatusDescription;');
    $StringWriter.WriteLine('                expectedCurrentOperation = string.Empty;');
    $StringWriter.WriteLine('                expectedPercentComplete = updateData.PercentComplete.HasValue ? 100 : null;');
    $StringWriter.WriteLine('                expectedCode = completeData.Code;');
    $StringWriter.WriteLine('                fgEvent.Set(); // Signal that we''re ready to complete');
    if ($null -ne $FactoryMethod.ResultType) {
        $StringWriter.Write('                ');
        $StringWriter.Write($FactoryMethod.ResultType.ClrType);
        $StringWriter.WriteLine(' actualResult = await backgroundOperation.Task;');
        $StringWriter.WriteLine('                Assert.AreEqual(completeData.Result, actualResult);');
    } else {
        $StringWriter.WriteLine('                await backgroundOperation.Task;');
    }
    $StringWriter.WriteLine('            }');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            Assert.AreEqual(initializeData.Activity, backgroundOperation.Activity);');
    $StringWriter.WriteLine('            Assert.AreEqual(expectedStatusDescription, backgroundOperation.StatusDescription);');
    $StringWriter.WriteLine('            Assert.AreEqual(expectedCurrentOperation, backgroundOperation.CurrentOperation);');
    $StringWriter.WriteLine('            Assert.AreEqual(expectedPercentComplete.HasValue, backgroundOperation.PercentComplete.HasValue);');
    $StringWriter.WriteLine('            if (expectedPercentComplete.HasValue)');
    $StringWriter.WriteLine('                Assert.AreEqual(expectedPercentComplete.Value, backgroundOperation.PercentComplete.Value);');
    if ($null -ne $FactoryMethod.StateType) { $StringWriter.WriteLine('            Assert.AreEqual(initializeData.State, backgroundOperation.AsyncState);') }
    $StringWriter.WriteLine('            Assert.IsTrue(operationObserver.TryDequeue(out observed));');
    $StringWriter.WriteLine('            Assert.IsFalse(observed.IsComplete);');
    $StringWriter.WriteLine('            Assert.IsNull(observed.Error);');
    $StringWriter.Write('            if (observed.Value is ');
    $StringWriter.Write($FactoryMethod.GetCompletedEventType($false));
    $StringWriter.WriteLine(' completedEvent)');
    $StringWriter.WriteLine('            {');
    $StringWriter.WriteLine('                if (completeData.Cancel)');
    if ($null -ne $FactoryMethod.ResultType) {
        $StringWriter.WriteLine('                {');
        $StringWriter.Write('                    Assert.IsNotInstanceOfType(completedEvent, typeof(');
        $StringWriter.Write($FactoryMethod.GetFinalEventType($false));
        $StringWriter.WriteLine('));');
    }
    $StringWriter.WriteLine('                    Assert.IsFalse(completedEvent.RanToCompletion);');
    if ($null -ne $FactoryMethod.ResultType) { $StringWriter.WriteLine('                }') }
    $StringWriter.WriteLine('                else');
    if ($null -ne $FactoryMethod.ResultType) { $StringWriter.WriteLine('                {'); }
    $StringWriter.WriteLine('                    Assert.IsTrue(completedEvent.RanToCompletion);');
    if ($null -ne $FactoryMethod.ResultType) {
        $StringWriter.Write('                    if (completedEvent is ');
        $StringWriter.Write($FactoryMethod.GetFinalEventType($false));
        $StringWriter.WriteLine(' resultEvent)');
        $StringWriter.WriteLine('                        Assert.AreEqual(completeData.Result, resultEvent.Result);');
        $StringWriter.WriteLine('                    else');
        $StringWriter.Write('                        Assert.Fail("observed.Value is not ');
        $StringWriter.Write($FactoryMethod.GetFinalEventType($false));
        $StringWriter.WriteLine('");');
        $StringWriter.WriteLine('                }');
    }
    $StringWriter.WriteLine('                Assert.IsNull(completedEvent.Error);');
    $StringWriter.WriteLine('                Assert.AreEqual(initializeData.Activity, completedEvent.Activity);');
    $StringWriter.WriteLine('                Assert.AreEqual(expectedStatusDescription, completedEvent.StatusDescription);');
    $StringWriter.WriteLine('                Assert.AreEqual(expectedCurrentOperation, completedEvent.CurrentOperation);');
    $StringWriter.WriteLine('                Assert.AreEqual(expectedPercentComplete.HasValue, completedEvent.PercentComplete.HasValue);');
    $StringWriter.WriteLine('                if (expectedPercentComplete.HasValue)');
    $StringWriter.WriteLine('                    Assert.AreEqual(expectedPercentComplete.Value, completedEvent.PercentComplete.Value);');
    $StringWriter.WriteLine('                Assert.AreEqual(expectedCode.HasValue, completedEvent.Code);');
    $StringWriter.WriteLine('                if (expectedCode.HasValue)');
    $StringWriter.WriteLine('                    Assert.AreEqual(expectedCode.Value, completedEvent.Code.Value);');
    if ($null -ne $FactoryMethod.StateType) { $StringWriter.WriteLine('                Assert.AreEqual(initializeData.State, completedEvent.AsyncState);') }
    $StringWriter.WriteLine('                Assert.AreEqual(backgroundOperation.OperationId, completedEvent.OperationId);');
    $StringWriter.WriteLine('                Assert.IsFalse(completedEvent.ParentId.HasValue);');
    $StringWriter.WriteLine('            }');
    $StringWriter.WriteLine('            else');
    $StringWriter.Write('                Assert.Fail("observed.Value is not ');
    $StringWriter.Write($FactoryMethod.GetCompletedEventType($false));
    $StringWriter.WriteLine('");');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #endregion');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #region Test Observer Completion');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            Thread.Sleep(100);');
    $StringWriter.WriteLine('            Assert.IsTrue(operationObserver.TryDequeue(out observed));');
    $StringWriter.WriteLine('            Assert.IsTrue(observed.IsComplete);');
    $StringWriter.WriteLine('            Assert.IsNull(observed.Error);');
    $StringWriter.WriteLine('            Assert.IsNull(observed.Value);');
    $StringWriter.WriteLine('            Assert.IsFalse(operationObserver.TryDequeue(out observed));');
    $StringWriter.WriteLine('');
    $StringWriter.WriteLine('            #endregion');
    $StringWriter.WriteLine('        }');
}

$Text = $StringWriter.ToString();
$Text | Write-Output;
[System.Windows.Clipboard]::SetText($Text);

Function Get-FactoryInterfaceCode {
    [CmdletBinding()]
    Param(
        [switch]$IsTimed,
        [switch]$HasState,
        [switch]$HandlesOnCompleted,
        [switch]$ReturnsValue,
        [switch]$PassesTokens
    )
    $FactoryMethod = [FactoryMethod]@{
        IsTimed = $IsTimed.IsPresent;
        HandlesOnCompleted = $HandlesOnCompleted.IsPresent;
        PassesTokens = $PassesTokens.IsPresent;
    };
    if ($HasState.IsPresent) { $FactoryMethod.StateType = [FactoryType]@{ GenericName = 'TState'; ClrType = 'int' } }
    if ($ReturnsValue.IsPresent) { $FactoryMethod.ResultType = [FactoryType]@{ GenericName = 'TResult'; ClrType = 'double' } }
    @(
        '',
        "$($FactoryMethod.GetFactoryMethodSignature($true));"
    ) | Write-Output;
}
