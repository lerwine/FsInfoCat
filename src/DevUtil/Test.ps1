Import-Module -Name './bin/Debug/net5.0/DevHelper' -ErrorAction Stop;

[DevUtil.EnhancedTypeDescriptor[]]$ConcreteTypes = [DevUtil.EnhancedTypeDescriptor]::GetFsInfoCatTypes();
$t = $ConcreteTypes | where-object { $_.BaseName -eq 'ILocalRedundantSetListItem' }
$StringBuilder = [DevUtil.ReflectionExtensions]::WriteCodeTemplate([System.Text.StringBuilder]::new(), $t);
$StringBuilder.ToString();
