Import-Module -Name './bin/Debug/net5.0/DevHelper' -ErrorAction Stop;

[Type[]]$ConcreteTypes = [DevUtil.EntityHelper]::GetLocalConcreteTypes();
$t = $ConcreteTypes | where-object { $_.Name -eq 'CrawlConfigListItemBase' }
$Fields = [DevUtil.EntityHelper]::GetFields($t);
$Fields;
