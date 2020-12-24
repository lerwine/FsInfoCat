Function ConvertTo-PasswordHash {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Password,
        [byte[]]$Salt
    )
    $SaltBytes = $Salt;
    if (-not $PSBoundParameters.ContainsKey('Salt')) {
        $SaltBytes = New-Object -TypeName 'System.Byte[]' -ArgumentList 8;
        $RNGCryptoServiceProvider = [System.Security.Cryptography.RNGCryptoServiceProvider]::new();
        $RNGCryptoServiceProvider.GetBytes($SaltBytes);
        $RNGCryptoServiceProvider.Dispose();
    }
    $SHA512 = [System.Security.Cryptography.SHA512]::Create();
    $SHA512.ComputeHash(([byte[]]([System.Text.Encoding]::ASCII.GetBytes($Password) + $SaltBytes))) | Out-Null;
    [Convert]::ToBase64String(([byte[]]($SHA512.Hash + $SaltBytes))) | Write-Output;
    $SHA512.Dispose();
}

Function Get-SaltBytes {
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateLength(96, 96)]
        [ValidatePattern('[A-Za-z\d+/]{52}')]
        [string]$Base64EncodedHash
    )
    [Convert]::FromBase64String($Base64EncodedHash) | Select-Object -Skip 64;
}

Function Test-PasswordHash {
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Password,
        [Parameter(Mandatory = $true)]
        [ValidateLength(96, 96)]
        [ValidatePattern('[A-Za-z\d+/]{52}')]
        [string]$ExpectedHash
    )

    ($ExpectedHash -eq (ConvertTo-PasswordHash -Password $Password -Salt (Get-SaltBytes -Base64EncodedHash $ExpectedHash))) | Write-Output;
}

Function Read-FunctionChoice {
    Param(
        [AllowEmptyString()]
        [string]$Message = ""
    )
    $ChoiceCollection = [System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]]::new();
    $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Enter Password', 'Enter password to hash'));
    $HashFromClipboard = -1;
    $EnterHash = 1;
    $TestFromClipboard = -1;
    $Quit = -1;
    if ([System.Windows.Clipboard]::ContainsText([System.Windows.TextDataFormat]::Text)) {
        $Text = [System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text);
        if ($Text.Length -gt 0) {
            $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Hash From clipboard', 'Convert clipboard text to password hash'));
            $HashFromClipboard = 1;
            $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test PW and Hash', 'Enter password and base4-encoded hash to compare'));
            $EnterHash = 2;
            if ($null -eq $Script:GetFunctionChoices_Regex) {
                $Script:GetFunctionChoices_Regex = [System.Text.RegularExpressions.Regex]::new('^\s*([A-Za-z\d+/]{96})\s*$', [System.Text.RegularExpressions.RegexOptions]::Compiled);
            }
            $m = $Script:GetFunctionChoices_Regex.Match($Text);
            if ($m.Success) {
                $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test from Clipboard', 'Test base64-encoded hash in clipboard with entered password'));
                $TestFromClipboard = 3;
            }
        } else {
            $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test PW and Hash', 'Enter password and base4-encoded hash to compare'));
        }
    } else {
        $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Test PW and Hash', 'Enter password and base4-encoded hash to compare'));
    }
    $ChoiceCollection.Add([System.Management.Automation.Host.ChoiceDescription]::new('Quit'));
    $msg = 'Select desired function';
    if ($Message.Length -gt 0) { $msg = "$Message`n`n$msg" }
    $i = $Host.UI.PromptForChoice('Function', $msg, $ChoiceCollection, 0);
    if ($null -ne $i -and $i -ge 0 -and $i -lt $ChoiceCollection.Count - 1) {
        switch ($i) {
            { $_ -eq $HashFromClipboard } {
                'HashFromClipboard' | Write-Output;
                break;
            }
            { $_ -eq $EnterHash } {
                'EnterHash' | Write-Output;
                break;
            }
            { $_ -eq $TestFromClipboard } {
                'TestFromClipboard' | Write-Output;
                break;
            }
            default {
                'EnterPassword' | Write-Output;
                break;
            }
        }
    } else {
        'Quit' | Write-Output;
    }
}

$OldWarningPreference = $WarningPreference;
$OldInformationPreference = $InformationPreference;
$WarningPreference = [System.Management.Automation.ActionPreference]::Continue;
$InformationPreference = [System.Management.Automation.ActionPreference]::Continue;

try {
    $FunctionChoice = Read-FunctionChoice;
    while ($FunctionChoice -ne 'Quit') {
        $LastMessage = '';
        switch ($FunctionChoice) {
            'HashFromClipboard' {
                $Base64Hash = ConvertTo-PasswordHash -Password ([System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text));
                [System.Windows.Clipboard]::SetText($Base64Hash);
                $LastMessage = "Base64-Encoded hash (copied to clipboard):`n`t$Base64Hash";
                break;
            }
            'EnterHash' {
                $Password = Read-Host -Prompt 'Enter password';
                if ([string]::IsNullOrEmpty($Password)) {
                    $LastMessage = 'Warning: No password to test';
                } else {
                    do {
                        $ExpectedHash = Read-Host -Prompt 'Enter base64-encoded hash string';
                        if ([string]::IsNullOrEmpty($ExpectedHash)) {
                            $LastMessage = 'Warning: No hash to test';
                            break;
                        }
                        if (($ExpectedHash = $ExpectedHash.Trim()).Length -ne 96) {
                            Write-Warning -Message 'Hash string should contain 96 characters';
                        } else {
                            if ($ExpectedHash -notmatch '^[A-Za-z\d+/]*$') {
                                Write-Warning -Message 'Invalid base64 string';
                            } else {
                                if (Test-PasswordHash -Password $Password -ExpectedHash $ExpectedHash) {
                                    $LastMessage = "Success!`n`tBase64-encoded hash string matches the hashed password";
                                } else {
                                    $LastMessage = "Failed!`n`tBase64-encoded hash string does not match the hashed password";
                                }
                                break;
                            }
                        }
                    } while (1);
                }
                break;
            }
            'TestFromClipboard' {
                $Password = Read-Host -Prompt 'Enter password';
                if ([string]::IsNullOrEmpty($Password)) {
                    'No password to test' | Write-Warning;
                } else {
                    if (Test-PasswordHash -Password $Password -ExpectedHash ([System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text))) {
                        $LastMessage = "Success!`n`tBase64-encoded hash string matches the hashed password";
                    } else {
                        $LastMessage = "Failed!`n`tBase64-encoded hash string does not match the hashed password";
                    }
                }
                break;
            }
            default {
                $Password = Read-Host -Prompt 'Enter password';
                if ([string]::IsNullOrEmpty($Password)) {
                    $LastMessage = 'Warning: No password to hash';
                } else {
                    $Base64Hash = ConvertTo-PasswordHash -Password $Password;
                    [System.Windows.Clipboard]::SetText($Base64Hash);
                    $LastMessage = "Base64-Encoded hash (copied to clipboard):`n`t$Base64Hash";
                }
                break;
            }
        }
        $FunctionChoice = Read-FunctionChoice -Message $LastMessage;
    }
} finally {
    $WarningPreference = $OldWarningPreference;
    $InformationPreference = $OldInformationPreference;
}
