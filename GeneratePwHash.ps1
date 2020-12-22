$pw = 'kbdf1901*MC';
$PwBytes = [System.Text.Encoding]::ASCII.GetBytes($pw);
$RNGCryptoServiceProvider = [System.Security.Cryptography.RNGCryptoServiceProvider]::new();
$SaltBytes = New-Object -TypeName 'System.Byte[]' -ArgumentList 8;
$RNGCryptoServiceProvider.GetBytes($SaltBytes);
$RNGCryptoServiceProvider.Dispose();
$SHA256 = [System.Security.Cryptography.SHA256]::Create();
$SHA256.ComputeHash(([byte[]]($SaltBytes + $PwBytes))) | Out-Null;
$HashBytes = $SHA256.Hash;
$SHA256.Dispose();
$sb = [System.Text.StringBuilder]::new();
$SaltBytes | ForEach-Object {
    $sb.Append(([int]$_).ToString('x2')) | Out-Null;
}
$HashBytes | ForEach-Object {
    $sb.Append(([int]$_).ToString('x2')) | Out-Null;
}
$HashString = $sb.ToString();
$HashString;