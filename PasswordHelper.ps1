if ($null -ne (Get-Module -Name 'FsInfoCat')) { Remove-Module -Name 'FsInfoCat' }
Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath 'src\FsInfoCat.PsDesktop\bin\Debug\FsInfoCat') -ErrorAction Stop;

[FsInfoCat.PsDesktop.CsTypeModel].FullName;

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

<#
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
                $PwHash = [FsInfoCat.PwHash]::Create(([System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text)));
                $Base64Hash = $PwHash.ToString();
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
                        $PwHash = [FsInfoCat.PwHash]::Import($ExpectedHash);
                        if ($null -eq $PwHash) {
                            $LastMessage = 'Warning: No hash to test';
                            break;
                        }
                        if ($PwHash.Test($Password)) {
                            $LastMessage = "Success!`n`tBase64-encoded hash string matches the hashed password";
                        } else {
                            $LastMessage = "Failed!`n`tBase64-encoded hash string does not match the hashed password";
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
                    $PwHash = [FsInfoCat.PwHash]::Import(([System.Windows.Clipboard]::GetText([System.Windows.TextDataFormat]::Text)));
                    if ($PwHash.Test($Password)) {
                        $LastMessage = "Success!`n`tBase64-encoded hash string matches the hashed password";
                    } else {
                        $LastMessage = "Failed!`n`tBase64-encoded hash string does not match the hashed password";
                    }
                }
                break;
            }
            default {
                $Password = Read-Host -Prompt 'Enter password';
                $PwHash = [FsInfoCat.PwHash]::Create($Password);
                if ($null -eq $PwHash) {
                    $LastMessage = 'Warning: No password to hash';
                } else {
                    $Base64Hash = $PwHash.ToString();
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

#>