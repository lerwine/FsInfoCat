Function Test-CsClassName {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Name
    )

    Begin {
        if ($null -eq $Script:__Test_CsClassName) { $Script:__Test_CsClassName = [System.Text.RegularExpressions.Regex]::new('^[A-Z][A-Za-z\d]*$', [System.Text.RegularExpressions.RegexOptions]::Compiled) }
        $Result = $true;
    }
    Process {
        if ($Result -and -not $Script:__Test_CsClassName.IsMatch($Name)) {
            if ($DisplayWarning.IsPresent) {
                Write-Warning -Message "$Name is an invalid class name"
            } else {
                Write-Information -MessageData "$Name is valid";
            }
            $Result = $false;
        }
    }
    End {
        $Result | Write-Output
    }
}

Function Test-FileInfo {
    [CmdletBinding(DefaultParameterSetName = 'Exists')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [string]$Path,
        [Parameter(Mandatory = $true, ParameterSetName = 'Created')]
        [switch]$AsCreated,
        [Parameter(ParameterSetName = 'NotExists')]
        [switch]$AssertNotExists,
        [Parameter(Mandatory = $true, ParameterSetName = 'Exists')]
        [switch]$AssertExists
    )

    Begin {
        $Result = $true;
    }
    Process {
        $FileInfo = $null;
        $FileInfo = [System.IO.FileInfo]::new($Path);
        if ($null -eq $FileInfo) {
            $Result = $false;
            Write-Warning -Message "Failed to validate $Path";
        } else {
            if ($FileInfo.Exists) {
                if ($AsCreated.IsPresent) {
                    Write-Information -MessageData "$($FileInfo.Length) bytes written to $($FileInfo.FullName).";
                } else {
                    if ($AssertExists.IsPresent) {
                        Write-Information -MessageData "$($FileInfo.FullName) contains $($FileInfo.Length) bytes.";
                    } else {
                        if ($AssertNotExists.IsPresent) {
                            Write-Warning -Message "$($FileInfo.FullName) contains $($FileInfo.Length) bytes.";
                        }
                    }
                }
            } else {
                $Result = $false;
                if ($AsCreated.IsPresent) {
                    Write-Warning -Message "Failed to create $($FileInfo.FullName).";
                } else {
                    if ($AssertExists.IsPresent) {
                        Write-Warning -Message "$($FileInfo.FullName) does not exist.";
                    } else {
                        if ($AssertNotExists.IsPresent) {
                            Write-Information -MessageData "$($FileInfo.FullName) does not exist.";
                        }
                    }
                }
            }
        }
    }
    End {
        $Result | Write-Output
    }
}

Function Read-CsClassName {
    [CmdletBinding()]
    Param(
        [string]$Prompt = 'Enter class name'
    )

    $Name = '';
    do {
        $Name = Read-Host -Prompt $Prompt;
        if ($null -eq $Name -or ($Name = $Name.Trim()).Length -eq 0) { return }
    } while (-not ($Name | Test-CsClassName));
    $Name | Write-Output;
}

Function Read-Choice {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [object[]]$Options,
        [Parameter(Mandatory = $true)]
        [string]$Caption,
        [Parameter(Mandatory = $true)]
        [string]$Message,
        [object]$DefaultChoice,
        [switch]$GridView
    )

    Begin {
        $AllOptions = @();
    }

    Process {
        $AllOptions += @($Options);
    }

    End {
        $Collection = [CodeHelper.ChoiceValueDescription]::ToCollection($AllOptions);
        $d = $null;
        $i = 0;
        if ($PSBoundParameters.ContainsKey($DefaultChoice)) {
            $d = [CodeHelper.ChoiceValueDescription]::Create($DefaultChoice);
            if (($i = $Collection.IndexOf($d)) -lt 0) {
                $Collection.Insert(0, $d);
                $i = 0;
            }
        }
        if ($Collection.Count -gt 0) {
            $Result = $null;
            if ($GridView.IsPresent) {
                $Result = $Collection | Out-GridView -Title "$Caption`: $Message" -OutputMode Single;
            } else {
                $i = $Host.UI.PromptForChoice($Caption, $Message, $Collection, $i);
                if ($i -ge 0 -and $i -lt $Collection.Count) { $Result = $Collection[$i] }
            }
            if ($null -ne $Result) { return $Result.GetValue() }
        }
    }
}

Function Read-YesOrNo {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Caption,
        [Parameter(Mandatory = $true)]
        [string]$Message,
        [bool]$Default = $false
    )

    if ($null -eq $Script:__Read_YesOrNo_Choices) {
        $Script:__Read_YesOrNo_Choices = @(
            [System.Management.Automation.Host.ChoiceDescription]::new('Yes'),
            [System.Management.Automation.Host.ChoiceDescription]::new('No')
        );
    }
    $DefaultChoice = 1;
    if ($Default) { $DefaultChoice = 0 }
    $Host.UI.PromptForChoice($Caption, $Message, $Script:__Read_YesOrNo_Choices, $DefaultChoice)
}

Function Convert-ToCommentLines {
    [CmdletBinding()]
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

Function New-WpfWindowScaffold {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [ValidatePattern('^[A-Z][A-Za-z\d]*$')]
        [string]$Name
    )

    $WindowXamlPath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\$($Name)Window.xaml";
    $WindowCsPath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\$($Name)Window.xaml.cs";
    $ViewModelPath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat.Desktop\ViewModels\$($Name)ViewModel.cs";

    if ($WindowXamlPath | Test-FileInfo -InformationAction Ignore -WarningAction Continue) {
        Write-Warning -Message "Aborted.";
        return;
    }

    if ($WindowCsPath | Test-FileInfo -AssertNotExists -InformationAction Ignore -WarningAction Continue) {
        Write-Warning -Message "Aborted.";
        return;
    }

    if ($ViewModelPath | Test-FileInfo -InformationAction Ignore -WarningAction Continue) {
        Write-Warning -Message "Aborted.";
        return;
    }

    $Encoding = [System.Text.UTF8Encoding]::new($false, $false);
    [System.IO.File]::WriteAllText($WindowXamlPath, @"
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:i="http://schemas.microsoft.com/expression/2010/interactivity"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:local="clr-namespace:FsInfoCat.Desktop"
    xmlns:vm="clr-namespace:FsInfoCat.Desktop.ViewModels" mc:Ignorable="d" x:Class="FsInfoCat.Desktop.$($Name)Window"
    Title="$Name" Height="450" Width="800">
    <Window.DataContext>
        <vm:$($Name)ViewModel />
    </Window.DataContext>
</Window>
"@, $Encoding);
    if (-not ($WindowXamlPath | Test-FileInfo -AsCreated)) {
        Write-Warning -Message "Failed to create $Name WPF window scaffold.";
        return;
    }

    [System.IO.File]::WriteAllText($WindowCsPath, @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsInfoCat.Desktop.ViewModels;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for $($Name)Window.xaml
    /// </summary>
    public partial class $($Name)Window : Window
    {
        public $($Name)ViewModel ViewModel { get { return DataContext as $($Name)ViewModel; } }

        public $($Name)Window()
        {
            InitializeComponent();
        }
    }
}
"@, $Encoding);
    if (-not ($WindowCsPath | Test-FileInfo -AsCreated)) {
        Write-Information -MessageData "Deleting $WindowXamlPath";
        Write-Warning -Message "Failed to create $Name WPF window scaffold.";
        return;
    }

    [System.IO.File]::WriteAllText($Path, @"
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FsInfoCat.Desktop.Commands;

namespace FsInfoCat.Desktop.ViewModels
{
    /// <summary>
    /// View model for <see cref="$($Name)Window" />.xaml
    /// </summary>
    public class $($Name)ViewModel : DependencyObject
    {
        public $($Name)ViewModel()
        {
        }
    }
}
"@, $Encoding);
    if ($ViewModelPath | Test-FileInfo -AsCreated) {
        Write-Information -MessageData "$Name WPF window scaffold created.";
    } else {
        Write-Information -MessageData "Deleting $WindowXamlPath";
        Write-Information -MessageData "Deleting $WindowCsPath";
        Write-Warning -Message "Failed to create $Name WPF window scaffold.";
    }
}

Function New-MvcScaffold {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][A-Z\da-z]*([A-Z\d][A-Z\da-z]*)*$')]
        [string]$Name
    )

    $FilePath = $PSScriptRoot | Join-Path -ChildPath "src\FsInfoCat\Models\$Name.cs";
    [System.IO.File]::WriteAllText($FilePath, @"
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class $Name
    {
        private string _displayName = "";
        private string _notes = "";

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid $($Name)ID { get; set; }

        [MaxLength(256)]
        [Display(Name = "Display Name")]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (value is null) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (value is null) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public $($Name)() { }

        public $($Name)(string displayName, Guid createdBy)
        {
            $($Name)ID = Guid.NewGuid();
            DisplayName = displayName;
            CreatedOn = ModifiedOn = DateTime.Now;
            CreatedBy = ModifiedBy = createdBy;
        }

    }
}
"@, [System.Text.UTF8Encoding]::new($false, $false));
    $FilePath = (Get-Command -Name 'dotnet').Path;
    Push-Location;
    try {
        Set-Location -Path ($PSScriptRoot | Join-Path -ChildPath 'src\FsInfoCat.Web');
        . dotnet build
        . dotnet aspnet-codegenerator controller -name "$($Name)Controller" -m $Name -dc FsInfoDataContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
    } finally {
        Pop-Location;
    }
}

Function Read-CsTypeModel {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Prompt
    )

    if ($null -eq $Script:__Read_CsTypeModel_Choices) {
        $Script:__Read_CsTypeModel_Choices = [System.Collections.ObjectModel.Collection[CodeHelper.ChoiceValueDescription]]::new();
        ([string], [bool], [byte], [sbyte], [System.Int16], [System.UInt16], [int], [System.UInt32], [long], [System.UInt64], [float], [double], [decimal], [Guid], [Uri], [DateTime]) | ForEach-Object {
            $t = [DevHelper.CsTypeModel]::new($_);
            $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($t, [System.Management.Automation.Host.ChoiceDescription]::new($t.ShortHand, $t.ToString())));
        }
        $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($false, [System.Management.Automation.Host.ChoiceDescription]::new("(other class)", "Other class type")));
        $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($true, [System.Management.Automation.Host.ChoiceDescription]::new("(other struct)", "Other struct type")));
    }

    $Result = $Script:__Read_CsTypeModel_Choices | Out-GridView -Title $Prompt -OutputMode Single;
    [DevHelper.CsTypeModel]$CsTypeModel = $null;
    if ($null -ne $Result) {
        if ($Result -is [DevHelper.CsTypeModel]) {
            $CsTypeModel = $Result;
        } else {
            $IsValueType = $Result;
            $Name = Read-CsClassName -Prompt 'Class name';
            if ([string]::IsNullOrWhiteSpace($Name)) { return }
            $CsTypeModel = [DevHelper.CsTypeModel]::new("", $Name, $IsValueType, $null);
        }
        if (Read-YesOrNo -Caption 'Collection', 'Make this a collection?') {
            $mc = [System.Collections.ObjectModel.Collection[DevHelper.CsTypeModel]]::new();
            $mc.Add($CsTypeModel);
            $CsTypeModel = [DevHelper.CsTypeModel]::new('System.Collections.ObjectModel', 'Collection', $false, $mc);
        }
        if ($CsTypeModel.IsValueType -and (Read-YesOrNo -Caption 'Nullability', 'Make this nullable?')) {
            $CsTypeModel = $CsTypeModel.AsNullable();
        }
    }
}

Function New-DependencyProperty {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][a-zA-Z\d]*$')]
        [string]$Name,
        [Parameter(Mandatory = $true)]
        [DevHelper.CsTypeModel]$PropertyType,
        [Parameter(Mandatory = $true)]
        [string]$ModelType,
        [switch]$ReadOnly,
        [switch]$HandlePropertyChanged,
        [switch]$HandleCoerceValue,
        [switch]$ExposeChangedEvent
    )

    $sb = [System.Text.StringBuilder]::new();
    $sb.AppendLine().Append('        #region ').Append($Name) | Out-Null;
    [xml]$Xml = '<summary />';
    $Xml.PreserveWhitespace = $true;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nDefines the name for the ")) | Out-Null;
    $Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $Name;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;
    $sb.AppendLine(' Property Members').AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public const string PropertyName_').Append($Name).Append(' = "').Append($Name).AppendLine('";').AppendLine() | Out-Null;
    [xml]$Xml = '<summary />';
    $Xml.PreserveWhitespace = $true;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode("`nIdentifies the ")) | Out-Null;
    $Xml.DocumentElement.AppendChild($Xml.CreateElement('see')).Attributes.Append($Xml.CreateAttribute('cref')).Value = $Name;
    $Xml.DocumentElement.AppendChild($Xml.CreateTextNode(" dependency property.`n")) | Out-Null;

    if ($ReadOnly.IsPresent) {
        $sb.Append('        private static readonly DependencyPropertyKey ').Append($Name).Append('PropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_').Append($Name) | Out-Null;
    } else {
        $sb.AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($Name).Append('Property = DependencyProperty.Register(PropertyName_').Append($Name) | Out-Null;
    }
    $sb.Append(', typeof(').Append($PropertyType.ShortHand).Append('), typeof(').Append($ModelType).AppendLine('),').Append('                new PropertyMetadata(').Append($PropertyType.DefaultValue) | Out-Null;
    if ($HandlePropertyChanged.IsPresent -or $HandleCoerceValue.IsPresent) {
        if ($HandlePropertyChanged.IsPresent) {
            $sb.AppendLine(',').Append('                    (d, e) => (d as ').Append($ModelType).Append(').On').Append($Name).Append('PropertyChanged(e.OldValue as ').Append($PropertyType.AsNullable().ShortHand) | Out-Null;
            if ($PropertyType.IsValueType) {
                $sb.Append(', (').Append($PropertyType.ShortHand).Append(')(e.NewValue)') | Out-Null;
            } else {
                $sb.Append(', e.NewValue as ').Append($PropertyType.ShortHand) | Out-Null;
            }
            $sb.Append(')') | Out-Null;
        } else {
            if ($HandleCoerceValue.IsPresent) { $sb.Append(', null') | Out-Null; }
        }
        if ($HandleCoerceValue.IsPresent) {
            $sb.AppendLine(',').Append('                    (d, baseValue) => Coerce').Append($Name).Append('Value(baseValue as ').Append($PropertyType.ShortHand).Append(')') | Out-Null;
        }
        $sb.AppendLine().AppendLine('            )').AppendLine('        );') | Out-Null;
        if ($ReadOnly.IsPresent) {
            $sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public static readonly DependencyProperty ').Append($Name).Append('Property = ').Append($Name).AppendLine('PropertyKey.DependencyProperty;') | Out-Null;
        }
        if ($HandlePropertyChanged.IsPresent) {
            $sb.AppendLine().Append('        private void On').Append($Name).Append('PropertyChanged(').Append($PropertyType.ShortHand) | Out-Null;
            $sb.Append(' oldValue, ').Append($PropertyType.ShortHand).AppendLine(' newValue)').AppendLine('        {') | Out-Null;
            if ($IsObservableCollection) {
                $sb.AppendLine('            if (null != oldValue)').Append('                oldValue.CollectionChanged -= On').Append($Name).AppendLine('CollectionChanged;') | Out-Null;
                $sb.Append('            newValue.CollectionChanged += On').Append($Name).AppendLine('CollectionChanged;') | Out-Null;
                $sb.Append('            On').Append($Name).AppendLine('CollectionChanged(newValue, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));') | Out-Null;
                $sb.AppendLine('        }').AppendLine().Append('        private void On').Append($Name).AppendLine('CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)').AppendLine('        {') | Out-Null;
                $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('CollectionChanged') | Out-Null;
            } else {
                $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('PropertyChanged') | Out-Null;
            }
            $sb.AppendLine('            throw new NotImplementedException();').AppendLine('        }') | Out-Null;
        }
        if ($HandleCoerceValue.IsPresent) {
            $sb.AppendLine().Append('        public static ').Append($PropertyType.ShortHand).Append(' Coerce').Append($Name).Append('Value(').Append($PropertyType.ShortHand) | Out-Null;
            $sb.AppendLine(' value)').AppendLine('        {') | Out-Null;
            if ($PropertyType.DefaultValue -eq 'null') {
                if ($IsObservableCollection) {
                    $sb.AppendLine('            if (value is null)').Append('                return new ').Append($PropertyType.ShortHand).AppendLine('();').AppendLine('            return value;') | Out-Null;
                } else {
                    $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('PropertyChanged').AppendLine('            throw new NotImplementedException();') | Out-Null;
                }
            } else {
                if ($PropertyType.IsValueType) {
                    $sb.AppendLine('            if (value.HasValue)').AppendLine('                return value.Value;').Append('            return ').Append($PropertyType.DefaultValue).AppendLine(';') | Out-Null;
                } else {
                    $sb.AppendLine('            if (value is null)').Append('                return ').Append($PropertyType.DefaultValue).AppendLine(';').AppendLine('            return value;') | Out-Null;
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
    $sb.AppendLine().AppendLine((Convert-ToCommentLines -Xml $Xml)).Append('        public ').Append($PropertyType.ShortHand).Append(' ').AppendLine($Name).AppendLine('        {') | Out-Null;
    $sb.AppendLine('            get').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    return (').Append($PropertyType.ShortHand).Append(')(GetValue(').Append($Name).AppendLine('Property));') | Out-Null;
    $sb.Append('                return Dispatcher.Invoke(() => (').Append($PropertyType.ShortHand).Append(')(GetValue(').Append($Name).AppendLine('Property)));').AppendLine('            }').Append('            ') | Out-Null;
    if ($ReadOnly.IsPresent) { $sb.Append('private ') | Out-Null }
    $sb.AppendLine('set').AppendLine('            {').AppendLine('                if (CheckAccess())').Append('                    SetValue(').Append($Name).Append('Property') | Out-Null;
    if ($ReadOnly.IsPresent) { $sb.Append('Key') | Out-Null }
    $sb.AppendLine(', value);').AppendLine('                else').Append('                    Dispatcher.Invoke(() => SetValue(').Append($Name).Append('Property') | Out-Null;
    if ($ReadOnly.IsPresent) { $sb.Append('Key') | Out-Null }
    $sb.AppendLine(', value));').AppendLine('            }').AppendLine('        }').AppendLine().AppendLine('        #endregion') | Out-Null;
    $sb.ToString();
}

Function Read-DependencyProperty {
    [CmdletBinding()]
    Param()
    $Name = Read-CsClassName -Prompt 'Property name';
    if ([string]::IsNullOrWhiteSpace($Name)) { return }
    $CsTypeModel = Read-CsTypeModel -Prompt 'Property type';
    if ($null -eq $CsTypeModel) { return }
    $ModelType = Read-Host -Prompt 'View model type';
    if ([string]::IsNullOrWhiteSpace($ModelType)) { return }
    $ReadOnly = Read-YesOrNo -Caption 'Read-Only' -Message 'Is this a read-only property?';
    $HandlePropertyChanged = $ExposeChangedEvent = Read-YesOrNo -Caption 'Property Change' -Message 'Expose property changed event?';
    if (-not $HandlePropertyChanged) {
        $HandlePropertyChanged = Read-YesOrNo -Caption 'Property Change' -Message 'Handle property changed event?';
    }
    if ($CsTypeModel.IsValueType -or (Read-YesOrNo -Caption 'Coerce value' -Message 'Handle coerce value?')) {
        if ($ReadOnly) {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -ExposeChangedEvent -HandlePropertyChanged -HandleCoerceValue;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -HandlePropertyChanged -HandleCoerceValue;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -HandleCoerceValue;
                }
            }
        } else {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ExposeChangedEvent -HandlePropertyChanged -HandleCoerceValue;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -HandlePropertyChanged -HandleCoerceValue;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -HandleCoerceValue;
                }
            }
        }
    } else {
        if ($ReadOnly) {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -ExposeChangedEvent -HandlePropertyChanged;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly -HandlePropertyChanged;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ReadOnly;
                }
            }
        } else {
            if ($ExposeChangedEvent) {
                New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -ExposeChangedEvent -HandlePropertyChanged;
            } else {
                if ($HandlePropertyChanged) {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType -HandlePropertyChanged;
                } else {
                    New-DependencyProperty -Name $Name -PropertyType $CsTypeModel -ModelType $ModelType;
                }
            }
        }
    }
}

Function New-XmlWriterSettings {
    [CmdletBinding(DefaultParameterSetName = 'Pipeline')]
    Param(
        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$DoNotCheckCharacters = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$CloseOutput = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [System.Xml.ConformanceLevel]$ConformanceLevel = [System.Xml.ConformanceLevel]::Auto,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$DoNotEscapeUriAttributes = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [System.Text.UTF8Encoding]$Encoding = (New-Object -TypeName 'System.Text.UTF8Encoding' -ArgumentList $false, $true),

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$Indent = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [string]$IndentChars = '  ',

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$OmitDuplicateNamespaces = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [string]$NewLineChars = [System.Environment]::NewLine,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [System.Xml.NewLineHandling]$NewLineHandling = [System.Xml.NewLineHandling]::Replace,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$NewLineOnAttributes = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$OmitXmlDeclaration = $false,

        [Parameter(ValueFromPipelineByPropertyName = $true)]
        [bool]$NoAutoCloseTag = $false,

        [Parameter(ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true)]
        [Alias('XmlWriterSettings')]
        # XmlWriterSettings to clone.
        [System.Xml.XmlWriterSettings]$Settings
    )

    Process {
        $XmlWriterSettings = $null;
        if ($PSBoundParameters.ContainsKey('Settings')) {
            $XmlWriterSettings = $Settings.Clone();
            if ($PSBoundParameters.ContainsKey('DoNotCheckCharacters')) { $XmlWriterSettings.CheckCharacters = -not $DoNotCheckCharacters }
            if ($PSBoundParameters.ContainsKey('CloseOutput')) { $XmlWriterSettings.CloseOutput = $CloseOutput }
            if ($PSBoundParameters.ContainsKey('ConformanceLevel')) { $XmlWriterSettings.ConformanceLevel = $ConformanceLevel }
            if ($PSBoundParameters.ContainsKey('DoNotEscapeUriAttributes')) { $XmlWriterSettings.DoNotEscapeUriAttributes = $DoNotEscapeUriAttributes }
            if ($PSBoundParameters.ContainsKey('Encoding')) { $XmlWriterSettings.Encoding = $Encoding }
            if ($PSBoundParameters.ContainsKey('Indent')) { $XmlWriterSettings.Indent = $Indent }
            if ($PSBoundParameters.ContainsKey('IndentChars')) { $XmlWriterSettings.IndentChars = $IndentChars }
            if ($PSBoundParameters.ContainsKey('OmitDuplicateNamespaces')) {
                if ($OmitDuplicateNamespaces) {
                    $XmlWriterSettings.NamespaceHandling = [System.Xml.NamespaceHandling]::OmitDuplicates;
                } else {
                    $XmlWriterSettings.NamespaceHandling = [System.Xml.NamespaceHandling]::Default;
                }
            }
            if ($PSBoundParameters.ContainsKey('NewLineChars')) { $XmlWriterSettings.NewLineChars = $NewLineChars }
            if ($PSBoundParameters.ContainsKey('NewLineHandling')) { $XmlWriterSettings.NewLineHandling = $NewLineHandling }
            if ($PSBoundParameters.ContainsKey('NewLineOnAttributes')) { $XmlWriterSettings.NewLineOnAttributes = $NewLineOnAttributes }
            if ($PSBoundParameters.ContainsKey('OmitXmlDeclaration')) { $XmlWriterSettings.OmitXmlDeclaration = $OmitXmlDeclaration }
            if ($PSBoundParameters.ContainsKey('NoAutoCloseTag')) { $XmlWriterSettings.WriteEndDocumentOnClose = -not $NoAutoCloseTag }
        } else {
            $XmlWriterSettings = New-Object -TypeName 'System.Xml.XmlWriterSettings' -Property @{
                Async = false;
                CheckCharacters = -not $DoNotCheckCharacters;
                CloseOutput = $CloseOutput;
                ConformanceLevel = $ConformanceLevel;
                DoNotEscapeUriAttributes = $DoNotEscapeUriAttributes;
                Encoding = $Encoding;
                Indent = $Indent;
                IndentChars = $IndentChars;
                NamespaceHandling = $NamespaceHandling;
                NewLineChars = $NewLineChars;
                NewLineHandling = $NewLineHandling;
                NewLineOnAttributes = $NewLineOnAttributes;
                OmitXmlDeclaration = $OmitXmlDeclaration;
                WriteEndDocumentOnClose = -not $NoAutoCloseTag;
            };
            if ($OmitDuplicateNamespaces) {
                $XmlWriterSettings.NamespaceHandling = [System.Xml.NamespaceHandling]::OmitDuplicates;
            } else {
                $XmlWriterSettings.NamespaceHandling = [System.Xml.NamespaceHandling]::Default;
            }
        }
        $XmlWriterSettings | Write-Output;
    }
}

enum PsHelpNsPrefix {
    msh;
    maml;
    command;
    dev;
    MSHelp;
};

enum PsHelpNames {
    helpItems;
    command;
    para;
}

$Script:PsHelpNamespaces = [System.Collections.ObjectModel.ReadOnlyDictionary[PsHelpNsPrefix, string]]::new((&{
    $dict = [System.Collections.Generic.Dictionary[PsHelpNsPrefix, string]]::new();
    @(@{
        msh = "http://msh";
        maml = "http://schemas.microsoft.com/maml/2004/10";
        command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
        dev = "http://schemas.microsoft.com/maml/dev/2004/10";
        MSHelp = "http://msdn.microsoft.com/mshelp";
    }.GetEnumerator()) | ForEach-Object { $dict.Add($_.Key, $_.Value) }
    return ,$dict;
}));
$Script:SchemaLocations = [System.Collections.ObjectModel.ReadOnlyDictionary[PsHelpNsPrefix, string]]::new((&{
    $dict = [System.Collections.Generic.Dictionary[PsHelpNsPrefix, string]]::new();
    @(@{
        msh = 'Msh.xsd';
        maml = 'PSMaml/Maml.xsd';
        command = 'PSMaml/developerCommand.xsd';
        dev = 'PSMaml/developer.xsd';
    }.GetEnumerator()) | ForEach-Object { $dict.Add($_.Key, $_.Value) }
    return ,$dict;
}));

Function Test-PsHelpXml {
    [CmdletBinding(DefaultParameterSetName = 'ByName')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [System.Xml.XmlNode]$Value,

        [Parameter(ParameterSetName = 'ByName')]
        [PsHelpNsPrefix[]]$NS,

        [Parameter(ParameterSetName = 'ByName')]
        [PsHelpNames[]]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'Document')]
        [switch]$Document,

        [Parameter(Mandatory = $true, ParameterSetName = 'Command')]
        [switch]$Command
    )

    Begin {
        $LocalNames = @();
        $Namespaces = @();
        if ($Document.IsPresent) {
            $LocalNames = @([PsHelpNames]::helpItems.ToString('F'));
            $Namespaces = @($Script:PsHelpNamespaces[[PsHelpNsPrefix]::msh]);
        } else {
            if ($Document.IsPresent) {
                $LocalNames = @([PsHelpNames]::command.ToString('F'));
                $Namespaces = @($Script:PsHelpNamespaces[[PsHelpNsPrefix]::msh]);
            } else {
                if ($PSBoundParameters.ContainsKey('Name')) {
                    $LocalNames = @($Name | ForEach-Object { $_.ToString('F') });
                }
                if ($PSBoundParameters.ContainsKey('NS')) {
                    $LocalNames = @($NS | ForEach-Object { $Script:PsHelpNamespaces[$_] });
                } else {
                    $LocalNames = @($Script:PsHelpNamespaces.Values);
                }
            }
        }
        $Success = $true;
    }
    Process {
        if ($Success) {
            $Node = $null;
            if ($null -ne $Value -and $Value -is [Sytem.Xml.XmlDocument]) {
                $Node = $Value.DocumentElement;
            } else {
                $Node = $Value;
            }
            $Success = ($null -ne $Node -and $Node -is [System.Xml.XmlElement] -and ($LocalNames.Count -eq 0 -or $LocalNames -ccontains $_.LocalName) -and `
                $Namespaces -ccontains $_.NamespaceURI);
        }
    }

    End { $success | Write-Output }
}

Function Test-NCName {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$Value
    )

    Process {
        if ($null -eq $Value) {
            Write-Debug -Message 'Value was null';
            $false | Write-Output;
        } else {
            $s = '';
            if ($Value -is [string]) {
                $s = $Value;
            } else {
                if ($null -ne $Value.Text) {
                    $s = '' + $Value.Text;
                } else {
                    $s = '' + $Value;
                }
            }
            if ($s.Trim().Length -eq 0) {
                Write-Debug -Message 'Empty value';
                $false | Write-Output;
            } else {
                $a = $s.ToCharArray();
                $index = 0;
                if ([System.Xml.XmlConvert]::IsStartNCNameChar($a[0])) {
                    while (++$index -lt $a.Length) {
                        if (-not [System.Xml.XmlConvert]::IsNCNameChar($a[$index])) { break }
                    }
                }
                if ($index -lt $a.Length) {
                    Write-Debug -Message "Invalid NCName char at index $index";
                    $false | Write-Output;
                } else {
                    $true | Write-Output;
                }
            }
        }
    }
}

Function Test-PsTypeName {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [string]$Value
    )

    Begin {
        $NameTokenRegex = New-Object -TypeName 'System.Text.RegularExpressions.Regex' -ArgumentList '(?<n>[a-z_][a-z_\d]*(\.[a-z_][a-z\d]*)*)|(?<o>\[ *(?<a>(,[ ,]*)?\](\[[, ]*\])*)?)|(?<s> *, *)|(?<c> *\])', ([System.Text.RegularExpressions.RegexOptions]([System.Text.RegularExpressions.RegexOptions]::IgnoreCase -bor [System.Text.RegularExpressions.RegexOptions]::Compiled));
    }

    Process {
        if ($null -eq $Value -or ($Value = $Value.Trim()).Length -eq 0) {
            $false | Write-Output;
        } else {
            $OriginalValue = $Value;
            $mode = 0; # match expected: 0=n; 1=a|o|s|c; 2=s|c; 3=a|s|c
            $m = $NameTokenRegex.Match($Value);
            $Passed = $m.Success -and $m.Index -eq 0;
            $Level = 0;
            $Position = 0;
            $Iteration = 0;
            while ($Passed) {
                Write-Information -Message "Matched $($m.Length) characters";
                $Iteration++;
                Write-Information -Message "Checking mode $mode";
                switch ($mode) {
                    0 {
                        $Passed = $m.Groups['n'].Success;
                        break;
                    }
                    1 {
                        $Passed = -not $m.Groups['n'].Success;
                        break;
                    }
                    2 {
                        $Passed = $m.Groups['s'].Success -or $m.Groups['c'].Success;
                        break;
                    }
                    default {
                        $Passed = -not ($m.Groups['n'].Success -or $m.Groups['o'].Success);
                        break;
                    }
                }
                if (-not $Passed) { break; }
                Write-Information -Message "Mode $mode passed";
                if ($m.Groups['n'].Success) {
                    Write-Information -Message "Name matched change to mode 1";
                    $Mode = 1;
                } else {
                    if ($m.Groups['a'].Success) {
                        Write-Information -Message "Array matched change to mode 2";
                        $Mode = 2;
                    } else {
                        if ($m.Groups['c'].Success) {
                            Write-Information -Message "Close matched checking level";
                            $Passed = $Level -gt 0;
                            if (-not $Passed) { break }
                            Write-Information -Message "Close Level $Level passed; decrementing";
                            $Level--;
                            Write-Information -Message "Change to mode 3";
                            $Mode = 3;
                        } else {
                            if ($m.Groups['o'].Success) {  Write-Information -Message "Incrementing from level $Level"; $Level++ }
                            Write-Information -Message "Change to mode 0";
                            $Mode = 0;
                        }
                    }
                }
                $Position += $m.Length;
                Write-Information -Message "Moved to position $Position";
                if ($m.Length -eq $Value.Length) { break }
                $Value = $Value.Substring($m.Length);
                $m = $NameTokenRegex.Match($Value);
                $Passed = $m.Success -and $m.Index -eq 0;
            }

            if ($Passed -and $Level -eq 0) {
                $true | Write-Output;
            } else {
                Write-Warning -Message "Failed at Iteration $Iteration; Position $Position; Level $Level; Mode $Mode (`"$($OriginalValue.Substring($Position))`" of `"$OriginalValue`"";
                $false | Write-Output;
            }
        }
    }
}

Function ConvertTo-XmlValue {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [object]$Value
    )

    if ($Value -is [string]) { return $Value }
    if ($Value -is [bool] -or $Value -is [int] -or $Value -is [byte] -or $Value -is [char] -or $Value -is [decimal] -or $Value -is [double] -or `
            $Value -is [Guid] -or $Value -is [long] -or $Value -is [sbyte] -or $Value -is [Int16] -or $Value -is [single] -or $Value -is [TimeSpan] -or `
            $Value -is [UInt16] -or $Value -is [UInt32] -or $Value -is [UInt64]) {
        return [System.Xml.XmlConvert]::ToString($Value);
    }
    return '' + $Value;
}

Function Add-Attribute {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-NCName})]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'String')]
        [AllowEmptyString()]
        [object]$Value
    )

    $a = $ParentElement.PSBase.SelectSingleNode("@$Name");
    if ($null -ne $a) {
        $a = $ParentElement.Attributes.Append($ParentElement.OwnerDocument.CreateAttribute($Name));
    }
    if ($Value -is [string]) {
        $a.Value = $Value;
    } else {
        if ($null -eq $Value.Text) {
            $a.Value = ConvertTo-XmlValue -Value $Value;
        } else {
            $a.Value = '' + $Value.Text;
        }
    }
}

Function Add-TextElement {
    [CmdletBinding(DefaultParameterSetName = 'NoEmpty')]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsHelpXml })]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true, ParameterSetName = 'String')]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$Value,

        [Parameter(Mandatory = $true)]
        [PsHelpNsPrefix]$NS,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'CommentIfEmpty')]
        [string]$CommentIfEmpty,

        [Parameter(Mandatory = $true, ParameterSetName = 'TextIfEmpty')]
        [string]$TextIfEmpty,

        [Parameter(ParameterSetName = 'NoEmpty')]
        [switch]$NoEmpty,

        [switch]$PassThru
    )

    if ($null -ne $Value) {
        $s = $null;
        if ($Value -is [string]) {
            $s = $Value;
        } else {
            if ($null -eq $Value.Text) {
                $s = ConvertTo-XmlValue -Value $Value;
            } else {
                $s = '' + $Value.Text;
            }
        }
        if (-not [string]::IsNullOrWhiteSpace($s)) {
            $XmlElement = $ParentElement.PSBase.AppendChild(
                $ParentElement.PSBase.OwnerDocument.CreateElement($Name, $Script:PsHelpNamespaces[$NS])
            );
            $XmlElement.PSBase.InnerText = $s;
            if ($PassThru) { return $XmlElement }
            return;
        }
    }
    if ($NoEmpty.IsPresent) { return }
    $XmlElement = $ParentElement.PSBase.AppendChild(
        $ParentElement.PSBase.OwnerDocument.CreateElement($Name, $Script:PsHelpNamespaces[$NS])
    );
    if ($PSBoundParameters.ContainsKey('TextIfEmpty')) {
        $ParentElement.InnerText = $TextIfEmpty;
    } else {
        if ($PSBoundParameters.ContainsKey('CommentIfEmpty')) {
            $ParentElement.AppendChild($ParentElement.PSBase.OwnerDocument.CreateComment($CommentIfEmpty)) | Out-Null;
        }
    }
    if ($PassThru) { return $XmlElement }
}

Function Add-XmlElement {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-NCName })]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [PsHelpNsPrefix]$NS
    )

    Write-Output -InputObject ($ParentElement.PSBase.AppendChild(
        $ParentElement.PSBase.OwnerDocument.CreateElement($Name, $Script:PsHelpNamespaces[$NS])
    )) -NoEnumerate;
}

Function Add-MamlParagraphs {
    [CmdletBinding(DefaultParameterSetName = 'CommentIfEmpty')]
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [AllowNull()]
        [AllowEmptyString()]
        [object]$ParaObj,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsHelpXml })]
        [System.Xml.XmlElement]$ParentElement,

        [Parameter(ParameterSetName = 'CommentIfEmpty')]
        [string]$CommentIfEmpty,

        [Parameter(Mandatory = $true, ParameterSetName = 'TextIfEmpty')]
        [string[]]$TextIfEmpty
    )

    Begin { $NoContent = $true }

    Process {
        if ($null -ne (Add-TextElement -Value $ParaObj -ParentElement $ParentElement -NS maml -Name ([PsHelpNames]::para) -NoEmpty -PassThru)) {
            $NoContent = $false;
        }
    }

    End {
        if ($NoContent) {
            if ($PSBoundParameters.ContainsKey('CommentIfEmpty')) {
                (Add-XmlElement -ParentElement $ParentElement -Name para -NS maml).AppendChild($ParentElement.OwnerDocument.CreateComment($CommentIfEmpty)) | Out-Null;
            } else {
                if ($PSBoundParameters.ContainsKey('TextIfEmpty')) {
                    $TextIfEmpty | ForEach-Object {
                        (Add-XmlElement -ParentElement $ParentElement -Name para -NS maml).InnerText = $_;
                    }
                }
            }
        }
    }
}

Function New-PsHelpNamespaceManager {
    Param(
        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNode]$Target
    )

    $Nsmgr = [System.Xml.XmlNamespaceManager]::new($Xml.NameTable);
    $Script:PsHelpNamespaces.Keys | ForEach-Object { $Nsmgr.AddNamespace($_, $Script:PsHelpNamespaces[$_]) }
    Write-Output -InputObject $Nsmgr -NoEnumerate;
}

Function New-PsHelpXml {
    [CmdletBinding()]
    Param([switch]$IncludeSchemaLocation)

    $XmlDocument = New-Object -TypeName 'System.Xml.XmlDocument';
    $XmlElement = $XmlDocument.AppendChild($XmlDocument.CreateElement('', 'helpItems', $Script:PsHelpNamespaces['msh']));
    if ($IncludeSchemaLocation) {
        $XmlElement.Attributes.Append($XmlDocument.CreateAttribute('xsi', 'schemaLocation', 'http://www.w3.org/2001/XMLSchema-instance')).Value = `
            @($Script:SchemaLocations.Keys | ForEach-Object { $Script:PsHelpNamespaces[$_]; $Script:SchemaLocations[$_] }) -join ' ';
    }
    Write-Output -InputObject $XmlDocument -NoEnumerate;
}

Function Add-PsCommandHelp {
    [CmdletBinding(DefaultParameterSetName = 'Common')]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsHelpXml -Document })]
        [System.Xml.XmlDocument]$PsHelpXml,

        [Parameter(Mandatory = $true, ParameterSetName = 'Common')]
        [System.Management.Automation.VerbsCommon]$CommonVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Communications')]
        [System.Management.Automation.VerbsCommunications]$CommunicationsVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Data')]
        [System.Management.Automation.VerbsData]$DataVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Diagnostic')]
        [System.Management.Automation.VerbsDiagnostic]$DiagnosticVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Lifecycle')]
        [System.Management.Automation.VerbsLifecycle]$LifecycleVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Security')]
        [System.Management.Automation.VerbsSecurity]$SecurityVerb,

        [Parameter(Mandatory = $true, ParameterSetName = 'Other')]
        [System.Management.Automation.VerbsOther]$OtherVerb,

        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][a-z\d]*$')]
        [string]$Noun,

        [string]$Synopsis = '',

        [Version]$Version = [version]::new(0, 1),

        [string[]]$Description
    )

    $commandElement = Add-XmlElement -ParentElement $ParentElement -Name 'command' -NS command;
    $PsHelpXml.DocumentElement.AppendChild($PsHelpXml.CreateElement('command', 'command', $Script:PsHelpNamespaces['command']));
    $commandElement.Attributes.Append($PsHelpXml.CreateAttribute('xmlns', 'maml', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['maml'];
    $commandElement.Attributes.Append($PsHelpXml.CreateAttribute('xmlns', 'dev', 'http://www.w3.org/2000/xmlns/')).Value = $Script:PsHelpNamespaces['dev'];

    $detailsElement = Add-XmlElement -ParentElement $commandElement -Name 'details' -NS command;
    $Verb = '';
    switch ($PSCmdlet.ParameterSetName) {
        'Common' { $Verb = $CommonVerb.ToString('F'); break; }
        'Communications' { $Verb = $CommunicationsVerb.ToString('F'); break; }
        'Data' { $Verb = $DataVerb.ToString('F'); break; }
        'Diagnostic' { $Verb = $DiagnosticVerb.ToString('F'); break; }
        'Lifecycle' { $Verb = $LifecycleVerb.ToString('F'); break; }
        'Security' { $Verb = $SecurityVerb.ToString('F'); break; }
        default { $Verb = $OtherVerb.ToString('F'); break; }
    }
    Add-TextElement -ParentElement $detailsElement -NS 'command' -Name 'name' -Value "$Verb-$Noun";
    $descriptionElement = Add-XmlElement -ParentElement $detailsElement -Name 'description' -NS maml;
    Add-MamlParagraphs -ParentElement $descriptionElement -ParaObj $Summary -CommentIfEmpty 'Summary goes here';
    $copyrightElement = Add-XmlElement -ParentElement $detailsElement -Name 'copyright' -NS maml;
    Add-MamlParagraphs -ParentElement $copyrightElement -ParaObj "Copyright © Leonard Thomas Erwine $([DateTime]::Now.ToString('yyyy'))";
    Add-TextElement -ParentElement $detailsElement -NS 'command' -Name 'verb' -Value $Verb;
    Add-TextElement -ParentElement $detailsElement -NS 'command' -Name 'noun' -Value $Noun;
    Add-TextElement -ParentElement $detailsElement -NS 'dev' -Name 'version' -Value $Version;
    $descriptionElement = Add-XmlElement -ParentElement $commandElement -Name 'description' -NS maml;
    Add-MamlParagraphs -ParentElement $descriptionElement -ParaObj $Description -CommentIfEmpty 'Detailed description goes here';
    Write-Output -InputObject $commandElement -NoEnumerate;
}

Function Get-CommandParameter {
    [CmdletBinding(DefaultParameterSetName = 'All')]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsHelpXml -Command })]
        [System.Xml.XmlElement]$CommandElement,

        [Parameter(Mandatory = $true, ValueFromPipeline = $true, ParameterSetName = 'ByName')]
        [string[]]$Name,

        [Parameter(Mandatory = $true, ParameterSetName = 'ListNames')]
        [switch]$ListNames,

        [Parameter(Mandatory = $true, ParameterSetName = 'All')]
        [switch]$All
    )

    Begin {
        $parametersElement = $CommandElement.SelectSingleNode('command:parameters', $Nsmgr);
        $NamesChecked = @();
    }

    Process {
        if ($null -ne $parametersElement) {
            if ($ListNames.IsPresent) {
                @($parametersElement.SelectNodes("command:parameter[@name='$Name']/maml:name", $Nsmgr)) | Select-Object {
                    if (-not $_.IsEmpty) {
                        $text = $_.InnerText.Trim();
                        if ($text.Length -gt 0) { $text | Write-Output }
                    }
                }
            } else {
                if ($PSBoundParameters.ContainsKey('Name')) {
                    $Name | ForEach-Object {
                        if ($NamesChecked -inotcontains $_) {
                            $NamesChecked += $_;
                            $element = $parametersElement.SelectSingleNode("command:parameter[@name='$Name']", $Nsmgr);
                            if ($null -ne $element) {
                                Write-Output -InputObject $element -NoEnumerate;
                            }
                        }
                    }
                } else {
                    @($parametersElement.SelectNodes("command:parameter[@name='$Name']", $Nsmgr)) | Write-Output;
                }
            }
        }
    }
}

Function Add-SyntaxItem {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsHelpXml -Command })]
        [System.Xml.XmlElement]$CommandElement,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr
    )

    $syntaxElement = $CommandElement.SelectSingleNode('command:syntax', $Nsmgr);
    if ($null -eq $parametesyntaxElementrsElement) {
        $syntaxElement = Add-XmlElement -ParentElement $CommandElement -Name 'syntax' -NS command;
    }
    Write-Output -InputObject (Add-XmlElement -ParentElement $syntaxElement -Name 'syntaxItem' -NS command) -NoEnumerate;
}

Function Add-SyntaxParameter {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsHelpXml -Name 'syntaxItem' -NS command })]
        [System.Xml.XmlElement]$SyntaxItemElement,

        [Parameter(Mandatory = $true)]
        [ValidatePattern('^[A-Z][a-z\d]*$')]
        [string]$Name,

        [Parameter(Mandatory = $true)]
        [ValidateScript({ $_ | Test-PsTypeName })]
        [string]$Type,

        [Parameter(Mandatory = $true)]
        [System.Xml.XmlNamespaceManager]$Nsmgr,

        [string[]]$Description,

        [ValidateRange(1, [int]::MaxValue)]
        [int]$Position,

        [string]$DefaultValue = 'None',

        [switch]$PipelineByValue,

        [switch]$PipelineByPropertyName,

        # Array, collection, etc
        [switch]$VariableLength,

        [switch]$Required,

        # Can include wildcard characters.
        [switch]$Globbing,

        [switch]$PassThru
    )

    if ($null -ne (Get-CommandParameter -CommandElement $CommandElement -Name $Name)) {
        Write-Warning -Message 'A parameter with that name already exists';
        break;
    }
    $parameterElement = Add-XmlElement -ParentElement $parametersElement -Name 'parameter' -NS command;
    Add-Attribute -ParentElement $parameterElement -Name 'required' -Value $Required.IsPresent;
    Add-Attribute -ParentElement $parameterElement -Name 'variableLength' -Value $VariableLength.IsPresent;
    Add-Attribute -ParentElement $parameterElement -Name 'globbing' -Value $Globbing.IsPresent;

    if ($PipelineByValue.IsPresent) {
        if ($PipelineByPropertyName.IsPresent) {
            Add-Attribute -ParentElement $parameterElement -Name 'pipelineInput' -Value 'True (ByValue, ByPropertyName)';
        } else {
            Add-Attribute -ParentElement $parameterElement -Name 'pipelineInput' -Value 'True (ByValue)';
        }
    } else {
        if ($PipelineByPropertyName.IsPresent) {
            Add-Attribute -ParentElement $parameterElement -Name 'pipelineInput' -Value 'True (ByPropertyName)';
        } else {
            Add-Attribute -ParentElement $parameterElement -Name 'pipelineInput' -Value 'False';
        }
    }
    if ($PSBoundParameters.ContainsKey('Position')) {
        Add-Attribute -ParentElement $parameterElement -Name 'position' -Value $Position;
    } else {
        Add-Attribute -ParentElement $parameterElement -Name 'position' -Value 'named';
    }
    Add-TextElement -ParentElement $parameterElement -NS 'maml' -Name 'name' -Value $Name;
    $descriptionElement = Add-XmlElement -ParentElement $parameterElement -Name 'description' -NS maml;
    Add-MamlParagraphs -ParentElement $descriptionElement -ParaObj $Description -CommentIfEmpty 'Detailed description goes here';
    $parameterValueElement = Add-TextElement -ParentElement $parameterElement -NS 'command' -Name 'parameterValue' -Value $Type -PassThru;
    $parameterValueElement.Attributes.Append($PsHelpXml.CreateAttribute('required')).Value = $Required.IsPresent.ToString().ToLower();
    $parameterValueElement.Attributes.Append($PsHelpXml.CreateAttribute('variableLength')).Value = $VariableLength.IsPresent.ToString().ToLower();
    $typeElement = Add-XmlElement -ParentElement $parameterElement -Name 'type' -NS dev;
    Add-TextElement -ParentElement $typeElement -NS 'maml' -Name 'name' -Value $Type;
    (Add-XmlElement -ParentElement $parameterElement -Name 'uri' -NS maml).IsEmpty = $true;
    Add-TextElement -ParentElement $parameterElement -NS 'dev' -Name 'defaultValue' -Value $DefaultValue;
    if ($PassThru.IsPresent) { Write-Output -InputObject $parameterElement -NoEnumerate }
}
