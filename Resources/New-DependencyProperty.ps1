Function Convert-ToCommentLines {
    Param(
        [Parameter(Mandatory = $true)]
        [xml]$Xml,
        [switch]$Contents
    )
    $Sw = [System.IO.StringWriter]::new();
    $XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
    $XmlWriterSettings.OmitXmlDeclaration = $true;
    if ($Contents.IsPresent) {
        $XmlWriterSettings.ConformanceLevel = [System.Xml.ConformanceLevel]::Fragment;
    } else {
        $XmlWriterSettings.ConformanceLevel = [System.Xml.ConformanceLevel]::Document;
    }
    $XmlWriter = [System.Xml.XmlWriter]::Create($Sw, $XmlWriterSettings);
    if ($Contents.IsPresent) {
        $Elements = @($Xml.DocumentElement.SelectNodes('*'));
        $Elements[0].WriteTo($XmlWriter);
        ($Elements | Select-Object -Skip 1) | ForEach-Object {
            $XmlWriter.Flush();
            $Sw.WriteLine();
            $_.WriteTo($XmlWriter);
        }
    } else {
        $Xml.DocumentElement.WriteTo($XmlWriter);
    }
    $XmlWriter.Flush();
    (($Sw.ToString() -split '\r\n?|\n') | ForEach-Object { '        /// ' + $_.Trim() }) -join ([System.Environment]::NewLine);
    $XmlWriter.Close();
}

$TypeChoices = @(
    [System.Management.Automation.Host.ChoiceDescription]::new('string'),
    [System.Management.Automation.Host.ChoiceDescription]::new('int'),
    [System.Management.Automation.Host.ChoiceDescription]::new('uint'),
    [System.Management.Automation.Host.ChoiceDescription]::new('bool'),
    [System.Management.Automation.Host.ChoiceDescription]::new('long'),
    [System.Management.Automation.Host.ChoiceDescription]::new('other class'),
    [System.Management.Automation.Host.ChoiceDescription]::new('other struct')
);
$YesNoChoices = @(
    [System.Management.Automation.Host.ChoiceDescription]::new('Yes'),
    [System.Management.Automation.Host.ChoiceDescription]::new('No')
);

$PropertyLabel = Read-Host -Prompt 'Property Name';
if ($null -eq $PropertyLabel -or ($PropertyLabel = $PropertyLabel.Trim()).Length -eq 0) { return }

$Elements = @($PropertyLabel -split '\s+');
$PropertyName = $FieldName = $null;
if ($Elements.Count -eq 1) {
    $PropertyName = [System.Xml.XmlConvert]::EncodeLocalName($PropertyLabel);
    if ($PropertyName.Length -eq 1) {
        $PropertyName = $PropertyName.ToUpper();
    } else {
        $PropertyName = $PropertyName.Substring(0, 1).ToUpper() + $PropertyName.Substring(1).ToLower();
    }
} else {
    $PropertyName = -join ($Elements | ForEach-Object {
        $n = [System.Xml.XmlConvert]::EncodeLocalName($_);
        if ($n.Length -eq 1) {
            $n.ToUpper();
        } else {
            $n.Substring(0, 1).ToUpper() + $n.Substring(1).ToLower();
        }
    });
}
$i = $Host.UI.PromptForChoice('Property Type', 'Select property type', $TypeChoices, 0);
if ($i -lt 0 -or $i -ge $TypeChoices.Count) { return }
$TypeName = $TypeChoices[$i].Label;
$Nullable = $i -eq 0;
$IsEnum = $false;
if ($TypeName -eq 'other class') {
    $TypeName = Read-Host -Prompt 'Class Name';
    if ($null -eq $TypeName -or ($TypeName = $TypeName.Trim()).Length -eq 0) { return }
    $Nullable = $true;
} else {
    if ($TypeName -eq 'other struct') {
        $TypeName = Read-Host -Prompt 'Struct Name';
        if ($null -eq $TypeName -or ($TypeName = $TypeName.Trim()).Length -eq 0) { return }
        $i = $Host.UI.PromptForChoice('Enum', 'Is this an enum?', $YesNoChoices, 0);
        $IsEnum = $false;
        if ($i -eq 0) {
            $IsEnum = $true;
        } else {
            if ($i -ne 1) { return }
        }
    }
}
$ItemType = $TypeName;
$ItemNullable = $Nullable;
$ItemIsEnum = $IsEnum;
$i = $Host.UI.PromptForChoice('Collection', 'Is this an observable collection?', $YesNoChoices, 0);
$IsObservableCollection = $false;
if ($i -eq 0) {
    $IsObservableCollection = $true;
    $Nullable = $true;
    $IsEnum = $false;
    $TypeName = "ObservableCollection<$TypeName>";
} else {
    if ($i -ne 1) { return }
}
$i = $Host.UI.PromptForChoice('Read-Only', 'Is this read-only?', $YesNoChoices, 0);
$IsReadOnly = $false;
if ($i -eq 0) {
    $IsReadOnly = $true;
} else {
    if ($i -ne 1) { return }
}
$i = $Host.UI.PromptForChoice('Property Change', 'Handle Property Changed event?', $YesNoChoices, 0);
$HandlePropertyChanged = $false;
if ($i -eq 0) {
    $HandlePropertyChanged = $true;
} else {
    if ($i -ne 1) { return }
}

$DefaultValue = 'null';
switch ($TypeName) {
    'string' {
        $DefaultValue = '""';
        $HandleCoerceValue = $true;
        break;
    }
    'int' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'uint' {
        $DefaultValue = '0';
        $HandleCoerceValue = $true;
        break;
    }
    'bool' {
        $DefaultValue = 'false';
        $HandleCoerceValue = $true;
        break;
    }
    'long' {
        $DefaultValue = '0L';
        $HandleCoerceValue = $true;
        break;
    }
    default {
        if ($IsObservableCollection) {
            $HandleCoerceValue = $true;
        } else {
            if (-not $Nullable) {
                $DefaultValue = "default($TypeName)";
                $HandleCoerceValue = $true;
            }
        }
        break;
    }
}
if (-not $HandleCoerceValue) {
    $i = $Host.UI.PromptForChoice('Coerce Value', 'Handle Coerce Value event?', $YesNoChoices, 0);
    $HandleCoerceValue = $false;
    if ($i -eq 0) {
        $HandleCoerceValue = $true;
    } else {
        if ($i -ne 1) { return }
    }
}
$VmType = Read-Host -Prompt 'View Model Type Name';
if ($null -eq $VmType -or ($VmType = $VmType.Trim()).Length -eq 0) { return }

$sb = [System.Text.StringBuilder]::new();
$sb.AppendLine().Append('        #region ');
if ($PropertyLabel -eq $PropertyName) {
    $sb.Append($PropertyName) | Out-Null;
} else {
    $sb.Append('"').Append($PropertyLabel).Append('" (').Append($PropertyName).Append(')') | Out-Null;
}
[xml]$Xml = '<summary />';
$Xml.PreserveWhitespace = $true;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nDefines the name for the ")) | Out-Null;
$Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $PropertyName;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;
$sb.AppendLine(' Property Members').AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public const string PropertyName_').Append($PropertyName).Append(' = "').Append($PropertyName).AppendLine('";').AppendLine() | Out-Null;
[xml]$Xml = '<summary />';
$Xml.PreserveWhitespace = $true;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nIdentifies the ")) | Out-Null;
$Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $PropertyName;
$Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;

if ($IsReadOnly) {
    $sb.Append('        private static readonly DependencyPropertyKey ').Append($PropertyName).Append('PropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_').Append($PropertyName) | Out-Null;
} else {
    $sb.AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($PropertyName).Append('Property = DependencyProperty.Register(PropertyName_').Append($PropertyName) | Out-Null;
}
$sb.Append(', typeof(').Append($TypeName).Append('), typeof(').Append($VmType).AppendLine('),').Append('                new PropertyMetadata(').Append($DefaultValue) | Out-Null;
if ($HandlePropertyChanged -or $HandleCoerceValue) {
    if ($HandlePropertyChanged) {
        $sb.AppendLine(',').Append('                    (d, e) => (d as ').Append($VmType).Append(').On').Append($PropertyName).Append('PropertyChanged(e.OldValue as ').Append($TypeName) | Out-Null;
        if (-not $Nullable) {
            $sb.Append('?') | Out-Null;
        }
        if ($Nullable) {
            $sb.Append(', e.NewValue as ').Append($TypeName) | Out-Null;
        } else {
            $sb.Append(', (').Append($TypeName).Append(')(e.NewValue)') | Out-Null;
        }
        $sb.Append(')') | Out-Null;
    } else {
        if ($HandleCoerceValue) { $sb.Append(', null') | Out-Null; }
    }
    if ($HandleCoerceValue) {
        $sb.AppendLine(',').Append('                    (d, baseValue) => Coerce').Append($PropertyName).Append('Value(baseValue as ').Append($TypeName) | Out-Null;
        if (-not $Nullable) {
            $sb.Append('?') | Out-Null;
        }
        $sb.Append(')') | Out-Null;
    }
    $sb.AppendLine().AppendLine('            )').AppendLine('        );') | Out-Null;
    if ($IsReadOnly) {
        $sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($PropertyName).Append('Property = ').Append($PropertyName).AppendLine('PropertyKey.DependencyProperty;') | Out-Null;
    }
    if ($HandlePropertyChanged) {
        $sb.AppendLine().Append('        private void On').Append($PropertyName).Append('PropertyChanged(').Append($TypeName) | Out-Null;
        if (-not $Nullable) { $sb.Append('?') | Out-Null }
        $sb.Append(' oldValue, ').Append($TypeName).AppendLine(' newValue)').AppendLine('        {') | Out-Null;
        if ($IsObservableCollection) {
            $sb.AppendLine('            if (null != oldValue)').Append('                oldValue.CollectionChanged -= On').Append($PropertyName).AppendLine('CollectionChanged;') | Out-Null;
            $sb.Append('            newValue.CollectionChanged += On').Append($PropertyName).AppendLine('CollectionChanged;') | Out-Null;
            $sb.Append('            On').Append($PropertyName).AppendLine('CollectionChanged(newValue, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));') | Out-Null;
            $sb.AppendLine('        }').AppendLine().Append('        private void On').Append($PropertyName).AppendLine('CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)').AppendLine('        {') | Out-Null;
            $sb.Append('            // TODO: Implement On').Append($PropertyName).AppendLine('CollectionChanged') | Out-Null;
        } else {
            $sb.Append('            // TODO: Implement On').Append($PropertyName).AppendLine('PropertyChanged') | Out-Null;
        }
        $sb.AppendLine('            throw new NotImplementedException();').AppendLine('        }') | Out-Null;
    }
    if ($HandleCoerceValue) {
        $sb.AppendLine().Append('        public static ').Append($TypeName).Append(' Coerce').Append($PropertyName).Append('Value(').Append($TypeName) | Out-Null;
        if (-not $Nullable) { $sb.Append('?') | Out-Null }
        $sb.AppendLine(' value)').AppendLine('        {');
        if ($DefaultValue -eq 'null') {
            if ($IsObservableCollection) {
                $sb.AppendLine('            if (null == value)').Append('                return new ').Append($TypeName).AppendLine('();').AppendLine('            return value;') | Out-Null;
            } else {
                $sb.Append('            // TODO: Implement On').Append($PropertyName).AppendLine('PropertyChanged').AppendLine('            throw new NotImplementedException();') | Out-Null;
            }
        } else {
            if ($Nullable) {
                $sb.AppendLine('            if (null == value)').Append('                return ').Append($DefaultValue).AppendLine(';').AppendLine('            return value;') | Out-Null;
            } else {
                $sb.AppendLine('            if (value.HasValue)').AppendLine('                return value.Value;').Append('            return ').Append($DefaultValue).AppendLine(';') | Out-Null;
            }
        }
        $sb.AppendLine('        }') | Out-Null;
    }
} else {
    $sb.AppendLine('));') | Out-Null;
}
[xml]$Xml = '<summary />';
$Xml.PreserveWhitespace = $true;
$Xml.DocumentElement.InnerText = "`n$PropertyLabel`n";
$sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public ').Append($TypeName).Append(' ').AppendLine($PropertyName).AppendLine('        {') | Out-Null;
$sb.AppendLine('            get').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    return (').Append($TypeName).Append(')(GetValue(').Append($PropertyName).AppendLine('Property));') | Out-Null;
$sb.Append('                return Dispatcher.Invoke(() => (').Append($TypeName).Append(')(GetValue(').Append($PropertyName).AppendLine('Property)));').AppendLine('            }').Append('            ') | Out-Null ;
if ($IsReadOnly) { $sb.Append('private ') | Out-Null }
$sb.AppendLine('set').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    SetValue(').Append($PropertyName).Append('Property') | Out-Null;
if ($IsReadOnly) { $sb.Append('Key') | Out-Null }
$sb.AppendLine(', value);').AppendLine('                else').Append('                    Dispatcher.Invoke(() => SetValue(').Append($PropertyName).Append('Property') | Out-Null;
if ($IsReadOnly) { $sb.Append('Key') | Out-Null }
$sb.AppendLine(', value));').AppendLine('            }').AppendLine('        }').AppendLine().AppendLine('        #endregion') | Out-Null;
[System.Windows.Clipboard]::SetText($sb.ToString());