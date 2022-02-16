$PrimeNumbers = @(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997);
$Properties = @(
    @{ Name = 'FileSystemId'; AllowsNull = $false; IsValueType = $true; IdProperty = $null },
    @{ Name = 'RootPath'; AllowsNull = $false; IsValueType = $false; IdProperty = $null },
    @{ Name = 'DisplayName'; AllowsNull = $false; IsValueType = $false; IdProperty = $null },
    @{ Name = 'VolumeName'; AllowsNull = $false; IsValueType = $false; IdProperty = $null },
    @{ Name = 'Identifier'; AllowsNull = $false; IsValueType = $true; IdProperty = $null },
    @{ Name = 'Status'; AllowsNull = $false; IsValueType = $true; IdProperty = $null },
    @{ Name = 'Type'; AllowsNull = $false; IsValueType = $true; IdProperty = $null },
    @{ Name = 'MaxNameLength'; AllowsNull = $true; IsValueType = $true; IdProperty = $null },
    @{ Name = 'Notes'; AllowsNull = $false; IsValueType = $false; IdProperty = $null },
    @{ Name = 'UpstreamId'; AllowsNull = $true; IsValueType = $true; IdProperty = $null },
    @{ Name = 'LastSynchronizedOn'; AllowsNull = $true; IsValueType = $true; IdProperty = $null },
    @{ Name = 'CreatedOn'; AllowsNull = $false; IsValueType = $true; IdProperty = $null },
    @{ Name = 'ModifiedOn'; AllowsNull = $false; IsValueType = $true; IdProperty = $null }
);
'if (Id.Equals(Guid.Empty))'
'    unchecked';
'    {'
"        int hash = $($PrimeNumbers[$Properties.Count - 1]);";
$p = $PrimeNumbers[$Properties.Count + 1];
$Properties | ForEach-Object {
    if ($null -ne $_.IdProperty) {
        "        hash = ($($_.Name) is null) ? ($($_.IdProperty).Equals(Guid.Empty) ? hash * 109 : hash * 109 + $($_.IdProperty).GetHashCode()) : hash * 109 + ($($_.Name)?.GetHashCode() ?? 0);";
    } else {
        if ($_.AllowsNull) {
            if ($_.IsValueType) {
                "        hash = $($_.Name).HasValue ? hash * $p + ($($_.Name) ?? default).GetHashCode() : hash * $p;";
            } else {
                "        hash = ($($_.Name) is null) ? hash * $p : hash * $p + ($($_.Name)?.GetHashCode() ?? 0);";
            }    
        } else {
            "        hash = hash * $p + $($_.Name).GetHashCode();";
        }
    }
}
'        return hash;'
'    }'
'return Id.GetHashCode();'
