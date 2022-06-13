
$Process = [System.Diagnostics.Process]::GetCurrentProcess();
(Read-Host -Prompt "Press enter after debugger is attached to $($Process.ProcessName) (PID: $($Process.Id))") | Out-Null;

Import-Module -Name ($PSScriptRoot | Join-Path -ChildPath 'bin/Debug/net6.0-windows/DevHelper.psd1') -ErrorAction Stop;
