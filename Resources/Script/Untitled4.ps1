
Function Convert-RgbToHsv {
    [CmdletBinding()]
    Param(
      [Parameter(Mandatory = $true, Position = 0)]
      [ValidateRange(0, 255)]
      [int]$R,
      
      [Parameter(Mandatory = $true, Position = 1)]
      [ValidateRange(0, 255)]
      [int]$G,
      
      [Parameter(Mandatory = $true, Position = 2)]
      [ValidateRange(0, 255)]
      [int]$B
    )
    
    $RF = ([float]$R) / 255.0;
    $GF = ([float]$G) / 255.0;
    $BF = ([float]$B) / 255.0;
    $Cmax = $RF;
    $Cmin = $BF;
    if ($GF -gt $RF) {
        if ($BF -gt $GF) {
            $Cmax = $BF;
            $Cmin = $RF;
        } else {
            $Cmax = $GF;
            if ($BF -gt $RF) { $Cmin = $RF }
        }
    } else {
        if ($BF -gt $RF) {
            $Cmax = $BF;
            $CMin = $GF;
        } else {
            if ($BF -gt $GF) { $Cmin = $GF }
        }
    }
    $Delta = $Cmax - $CMin;
    $Hue = 0.0;
    if ($Delta -gt 0) {
        if ($Cmax -eq $RF) {
            $Hue = ($GF - $BF) / $Delta;
        } else {
            if ($Cmax -eq $GF) {
                $Hue = 2.0 + ($BF - $RF) / $Delta;
            } else {
                $Hue = 4.0 + ($RF - $GF) / $Delta;
            }
        }
        $Hue *= 60.0;
        if ($Hue -lt 0.0) {
            $Hue += 360.0;
        }
        $Saturation = $Cmax;
        if ($Saturation -ne 0) { $Saturation = $Delta / $Saturation }
    }
    return @($Hue, $Saturation, $Cmax);
}

Function Convert-HsvToRgb {
    [CmdletBinding()]
    Param(
      [Parameter(Mandatory = $true, Position = 0)]
      [ValidateRange(0.0, 360.0)]
      [double]$H,
      
      [Parameter(Mandatory = $true, Position = 1)]
      [ValidateRange(0.0, 1.0)]
      [double]$S,
      
      [Parameter(Mandatory = $true, Position = 2)]
      [ValidateRange(0.0, 1.0)]
      [double]$V
    )
    $C = $V * $S;
    $Sextant = (($h / 60.0) % 2) - 1.0;
    if ($Sextant -lt 0) { $Sextant *= -1 }
    $X = $C * (1.0 - $Sextant);
    $m = $V - $C;
    $RF = 0.0;
    $GF = 0.0;
    $BF = 0.0;
    if ($H -lt 60.0) {
        ($RF, $GF, $BF) = @($C, $X, 0.0);
    } else {
        if ($H -lt 120.0) {
            ($RF, $GF, $BF) = @($X, $C, 0.0);
        } else {
            if ($H -lt 180.0) {
                ($RF, $GF, $BF) = @(0.0, $C, $X);
            } else {
                if ($H -lt 240.0) {
                    ($RF, $GF, $BF) = @(0.0, $X, $C);
                } else {
                    if ($H -lt 300.0) {
                        ($RF, $GF, $BF) = @($X, 0.0, $C);
                    } else {
                        ($RF, $GF, $BF) = @($C, 0.0, $X);
                    }
                }
            }
        }
    }

    return @([Convert]::ToInt32(($RF + $m) * 255.0), [Convert]::ToInt32(($GF + $m) * 255.0), [Convert]::ToInt32(($BF + $m) * 255.0));
}

class SnColorSetting {
    [string]$Description;
    [string]$RgbHex;
    [double]$Hue;
    [double]$Saturation;
    [double]$Value;
    SnColorSetting([string]$Description, [string]$RgbHex) {
        $this.Description = $Description;
        $this.RgbHex = $RgbHex;
        ($this.Hue, $this.Saturation, $this.Value) = Convert-RgbToHsv -R ([int]::Parse($RgbHex.Substring(0, 2), [System.Globalization.NumberStyles]::HexNumber)) -G ([int]::Parse($RgbHex.Substring(2, 2), [System.Globalization.NumberStyles]::HexNumber)) -B ([int]::Parse($RgbHex.Substring(4), [System.Globalization.NumberStyles]::HexNumber));
    }
}

$SettingValues = @([SnColorSetting]::new('Header background color', '293e40'),
        [SnColorSetting]::new('Banner text color', 'ffffff'),
        [SnColorSetting]::new('Header divider stripe color', '5a7f71'),
        [SnColorSetting]::new('Navigation header/footer', '293e40'),
        [SnColorSetting]::new('Navigation background expanded items', '213234'),
        [SnColorSetting]::new('Module text color for UI16', 'ffffff'),
        [SnColorSetting]::new('Navigation selected tab background color', '2f4f4e'),
        [SnColorSetting]::new('Navigation selected tab divider bar color', '82c9b8'),
        [SnColorSetting]::new('Navigation unselected tab divider bar color', '213234'),
        [SnColorSetting]::new('Navigation separator color', '293e40'),
        [SnColorSetting]::new('Background for navigator and sidebars', '293e40'),
        [SnColorSetting]::new('Currently selected Navigation tab icon color for UI16', '82c9b8'),
        [SnColorSetting]::new('Unselected navigation tab icon and favorite icons color', 'd1d6d8'),
        [SnColorSetting]::new('Border color for UI16', '7a828a'));
$HT = 0.0;
$AllHues = @($SettingValues | Where-Object { $_.Saturation -gt 0 } | ForEach-Object { $HT += $_.Hue; $_.Hue });
$TargetHue = 120.0;
$TargetHue = 180.0;
$TargetHue = 240.0;
$TargetHue = 330.0;
[double]$Count = $AllHues.Count;
#$TargetHue = 0.0;
$AvgHue = ($HT / $Count);
$HueDelta = $AvgHue - $TargetHue;
if ($HueDelta -lt 0) { $HueDelta += 360.0 }
"Target Hue: $TargetHue; Avg Hue: $AvgHue; Hue Delta: $HueDelta"
foreach ($ColorSetting in $SettingValues) {
    $HH = $ColorSetting.Hue - $HueDelta;
    if ($HH -lt 0.0) { $HH += 360.0 }
    #$SS = ($ColorSetting.Saturation + $ColorSetting.Value + $ColorSetting.Value) / 3.0;
    $SS = $ColorSetting.Saturation * 1.75;
    $VV = (($ColorSetting.Value * 7.0) + 1.0) / 8.0;
    #$VV = $ColorSetting.Value;
    ($RR, $GG, $BB) = Convert-HsvToRgb -H $HH -S $ss -V $VV;

    "$($ColorSetting.Description): $($RR.ToString('x2'))$($GG.ToString('x2'))$($BB.ToString('x2')) <= $HH, $($ss * 100), $($VV * 100)";
}
<#

#>