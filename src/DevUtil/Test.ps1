Import-Module -Name './bin/Debug/net5.0/DevHelper' -ErrorAction Stop;

Function ConvertTo-SimleTypeName {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        if ($Type.IsArray) {
            $r = $Type.GetArrayRank();
            if ($r -lt 2) {
                "$(ConvertTo-SimleTypeName -Type $Type.GetElementType() -Namespace $Namespace)[]";
            } else {
                "$(ConvertTo-SimleTypeName -Type $Type.GetElementType() -Namespace $Namespace)[$([string]::new(([char]','), $r - 1))]";
            }
        } else {
            $ns = $Type.Namespace;
            if ($Type.IsNested) {
                $ns = "$(ConvertTo-SimleTypeName $Type.DeclaringType -Namespace $Namespace).";
            } else {
                if ([string]::IsNullOrEmpty($ns) -or $ns -eq 'System' -or $Namespace -ccontains $ns) {
                    $ns = '';
                } else {
                    $s = $Namespace |  Where-Object { $ns.StartsWith("$_.") } | Sort-Object -Property @{ Expression = { $_.Length } } | Select-Object -Last 1;
                    if ($null -ne $s) { $ns = "$($ns.Substring($s.Length + 1))." } else { $ns = "$ns." }
                }
            }
            if ($Type.IsGenericType -and -not $Type.IsGenericTypeParameter) {
                $n = $Type.Name;
                $n = $n.Substring(0, $n.IndexOf('`'));
                "$ns$n{$(($Type.GetGenericArguments() | ConvertTo-SimleTypeName -Namespace $Namespace) -join ', ')}";
            } else {
                switch ($Type) {
                    { $_ -eq [System.Byte] } { 'byte' | Write-Output; break; }
                    { $_ -eq [System.SByte] } { 'sbyte' | Write-Output; break; }
                    { $_ -eq [System.Char] } { 'char' | Write-Output; break; }
                    { $_ -eq [System.Int16] } { 'short' | Write-Output; break; }
                    { $_ -eq [System.Int32] } { 'int' | Write-Output; break; }
                    { $_ -eq [System.Int64] } { 'long' | Write-Output; break; }
                    { $_ -eq [System.UInt16] } { 'ushort' | Write-Output; break; }
                    { $_ -eq [System.UInt32] } { 'uint' | Write-Output; break; }
                    { $_ -eq [System.UInt64] } { 'ulong' | Write-Output; break; }
                    { $_ -eq [System.Single] } { 'float' | Write-Output; break; }
                    { $_ -eq [System.Double] } { 'double' | Write-Output; break; }
                    { $_ -eq [System.Decimal] } { 'decimal' | Write-Output; break; }
                    default { "$ns$($Type.Name)" | Write-Output; break; }
                }
            }
        }
    }
}
Function Get-DirectlyImplementingInterfaces {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $Type.Assembly.GetTypes() | ? {
            $_ -ne $Type -and $_.IsInterface -and $Type.IsAssignableFrom($_) -and @($_.GetInterfaces() | ? { $_ -ne $Type -and $Type.IsAssignableFrom($_) }).Count -eq 0;
        } | % { "    /// <seealso cref=`"$($_ | ConvertTo-SimleTypeName -Namespace $Namespace)`" />"} | Sort-Object;
    }
}
Function Get-PropertyReferences {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $e = @([System.Collections.Generic.ICollection`1], [System.Collections.Generic.IEnumerable`1], [System.Collections.Generic.ISet`1],
        [System.Collections.Generic.IReadOnlyCollection`1], [System.Collections.Generic.IReadOnlySet`1]) | % { $_.MakeGenericType($Type) }
        $Type.Assembly.GetTypes() | % { $_.GetProperties() } | ? {
            $_.ReflectedType -eq $_.DeclaringType -and ($_.PropertyType -eq $Type -or ($_.PropertyType.IsGenericType -and @($_.PropertyType.GetGenericArguments() | ? { $_ -eq $Type }).Count -gt 0))
        } | % { "    /// <seealso cref=`"$($_.DeclaringType | ConvertTo-SimleTypeName -Namespace $Namespace).$($_.Name)`" />" } | Sort-Object;
    }
}
Function Get-InterfaceBaseTypes {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $AllInterfaces = $Type.GetInterfaces();
        $AllInterfaces | ? {
            $t = $_;
            @($AllInterfaces | ? {
                $t -ne $_ -and $t.IsAssignableFrom($_)
            }).Count -eq 0;
        } | % { "    /// <seealso cref=`"$($_ | ConvertTo-SimleTypeName -Namespace $Namespace)`" />"} | Sort-Object;
    }
}
Function Get-SeeAlsoElements {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type]$Type,

        [Parameter(Mandatory = $true)]
        [string[]]$Namespace
    )
    Process {
        $Type | Get-InterfaceBaseTypes -Namespace $Namespace;
        $Type | Get-DirectlyImplementingInterfaces -Namespace $Namespace;
        $Type | Get-PropertyReferences -Namespace $Namespace;
    }
}
[FsInfoCat.IAccessError] | Get-SeeAlsoElements -Namespace 'FsInfoCat';
