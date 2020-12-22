$pw = 'kbdf1901*MC';
$HashString = "cbcc0a61e192218d9f395e86648c21c4fb88000a84fb4b752ba823884e35f43ad9d1d55d9644498c";
$PwBytes = [System.Text.Encoding]::ASCII.GetBytes($pw);
$SaltBytes = New-Object -TypeName 'System.Byte[]' -ArgumentList 8;
for ($i = 0; $i -lt 8; $i++) {
    $SaltBytes[$i] = [int]::Parse($HashString.Substring($i -shl 1, 2), [System.Globalization.NumberStyles]::HexNumber);
}
$HashBytes = New-Object -TypeName 'System.Byte[]' -ArgumentList 32;
for ($i = 0; $i -lt 32; $i++) {
    $HashBytes[$i] = [int]::Parse($HashString.Substring(16 + ($i -shl 1), 2), [System.Globalization.NumberStyles]::HexNumber);
}
$SHA256 = [System.Security.Cryptography.SHA256]::Create();
$SHA256.ComputeHash(([byte[]]($SaltBytes + $PwBytes))) | Out-Null;
$CompareBytes = $SHA256.Hash;
$SHA256.Dispose();
$success = $true;
for ($i = 0; $i -lt 32; $i++) {
    if ($CompareBytes[$i] -ne $HashBytes[$i]) {
        $success = $false;
        break;
    }
}
if ($success) {
    "Match succeeded";
} else {
    "Match failed";
}