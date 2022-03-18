$PrimeNumbers = @(2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997);
class PropertyDesc {
    [string]$Name;
    [bool]$AllowsNull;
    [bool]$IsValueType;
    [bool]$IsGuid;
    [string]$IdProperty;
    PropertyDesc([string]$Name, [bool]$AllowsNull, [bool]$IsValueType, [bool]$IsGuid, [string]$IdProperty) {
        $this.Name = $Name;
        $this.AllowsNull = $AllowsNull;
        $this.IsValueType = $IsValueType;
        $this.IsGuid = $IsGuid;
        $this.IdProperty = $IdProperty;
    }
    static [PropertyDesc] String([string]$Name, [bool]$AllowsNull) {
        return [PropertyDesc]::new($Name, $AllowsNull, $false, $false, $null); 
    }
    static [PropertyDesc] Value([string]$Name, [bool]$IsNullable) {
        return [PropertyDesc]::new($Name, $IsNullable, $true, $false, $null);
    }
    static [PropertyDesc] RelatedEntity([string]$Name, [string]$IdProperty, [bool]$AllowsNull) {
        return [PropertyDesc]::new($Name, $AllowsNull, $false, $false, $IdProperty);
    }
    static [PropertyDesc] Guid([string]$Name) {
        return [PropertyDesc]::new($Name, $false, $true, $true, $null);
    }
}
$Properties = @(
    [PropertyDesc]::String('Name', $false),
    [PropertyDesc]::String('Description', $false),
    #[PropertyDesc]::RelatedEntity('Tagged', 'TaggedId', $false),
    #[PropertyDesc]::RelatedEntity('Definition', 'TaggedId', $false),
    [PropertyDesc]::String('Notes', $false),
    [PropertyDesc]::Value('UpstreamId', $true),
    [PropertyDesc]::Value('LastSynchronizedOn', $true),
    [PropertyDesc]::Value('CreatedOn', $false),
    [PropertyDesc]::Value('ModifiedOn', $false)
);
$StringWriter = [System.IO.StringWriter]::new();
$StringWriter.WriteLine('Guid id = Id;');
$StringWriter.WriteLine('if (id.Equals(Guid.Empty))');
$StringWriter.WriteLine('    unchecked');
$StringWriter.WriteLine('    {');
$StringWriter.Write('        int hash = ');
$StringWriter.Write($PrimeNumbers[$Properties.Count - 1]);
$StringWriter.WriteLine(';');
$p = $PrimeNumbers[$Properties.Count + 1];
$Properties | ForEach-Object {
    $StringWriter.Write('        hash = ');
    if ($_.IsGuid) {
        $StringWriter.Write('EntityExtensions.HashGuid(');
        $StringWriter.Write($_.Name);
        $StringWriter.Write(', hash, ');
        $StringWriter.Write($p);
        $StringWriter.WriteLine(');');
    } else {
        if ([string]::IsNullOrWhiteSpace($_.IdProperty)) {
            if ($_.AllowsNull) {
                $StringWriter.Write('EntityExtensions.');
                if ($_.IsValueType) {
                    $StringWriter.Write('HashNullable(')
                } else {
                    $StringWriter.Write('HashObject(');
                }
                $StringWriter.Write($_.Name);
                $StringWriter.Write(', hash, ');
                $StringWriter.Write($p);
                $StringWriter.WriteLine(');');
            } else {
                $StringWriter.Write('hash * ');
                $StringWriter.Write($p);
                $StringWriter.Write(' + ');
                $StringWriter.Write($_.Name);
                $StringWriter.WriteLine('.GetHashCode();');
            }
        } else {
            $StringWriter.Write('EntityExtensions.HashRelatedEntity(');
            $StringWriter.Write($_.Name);
            $StringWriter.Write(', () => ');
            $StringWriter.Write($_.IdProperty);
            $StringWriter.Write(', hash, ');
            $StringWriter.Write($p);
            $StringWriter.WriteLine(');');
        }
    }
}
$StringWriter.WriteLine('        return hash;');
$StringWriter.WriteLine('    }');
$StringWriter.WriteLine('return id.GetHashCode();');
$StringWriter.Flush();
$Text = $StringWriter.ToString();
$Text;
[System.Windows.Clipboard]::SetText($Text);