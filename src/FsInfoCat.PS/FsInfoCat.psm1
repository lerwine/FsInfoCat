$Path = $PSScriptRoot | Join-Path -ChildPath 'FsInfoCat.PS.dll';
if ($Path | Test-Path -PathType Leaf) {
    Add-Type -Path $Path -ErrorAction Stop;
} else {
    $Path = $PSScriptRoot | Join-Path -ChildPath 'bin\Debug\FsInfoCat.PS.dll';
    if ($Path | Test-Path -PathType Leaf) {
        Add-Type -Path $Path -ErrorAction Stop;
    } else {
        Add-Type -Path ($PSScriptRoot | Join-Path -ChildPath 'bin\Release\FsInfoCat.PS.dll') -ErrorAction Stop;
    }
}

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
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
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
            $t = [CodeHelper.CsTypeModel]::new($_);
            $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($t, [System.Management.Automation.Host.ChoiceDescription]::new($t.ShortHand, $t.ToString())));
        }
        $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($false, [System.Management.Automation.Host.ChoiceDescription]::new("(other class)", "Other class type")));
        $Script:__Read_CsTypeModel_Choices.Add([CodeHelper.ChoiceValueDescription]::new($true, [System.Management.Automation.Host.ChoiceDescription]::new("(other struct)", "Other struct type")));
    }

    $Result = $Script:__Read_CsTypeModel_Choices | Out-GridView -Title $Prompt -OutputMode Single;
    [CodeHelper.CsTypeModel]$CsTypeModel = $null;
    if ($null -ne $Result) {
        if ($Result -is [CodeHelper.CsTypeModel]) {
            $CsTypeModel = $Result;
        } else {
            $IsValueType = $Result;
            $Name = Read-CsClassName -Prompt 'Class name';
            if ([string]::IsNullOrWhiteSpace($Name)) { return }
            $CsTypeModel = [CodeHelper.CsTypeModel]::new("", $Name, $IsValueType, $null);
        }
        if (Read-YesOrNo -Caption 'Collection', 'Make this a collection?') {
            $mc = [System.Collections.ObjectModel.Collection[CodeHelper.CsTypeModel]]::new();
            $mc.Add($CsTypeModel);
            $CsTypeModel = [CodeHelper.CsTypeModel]::new('System.Collections.ObjectModel', 'Collection', $false, $mc);
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
        [CodeHelper.CsTypeModel]$PropertyType,
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
                    $sb.AppendLine('            if (null == value)').Append('                return new ').Append($PropertyType.ShortHand).AppendLine('();').AppendLine('            return value;') | Out-Null;
                } else {
                    $sb.Append('            // TODO: Implement On').Append($Name).AppendLine('PropertyChanged').AppendLine('            throw new NotImplementedException();') | Out-Null;
                }
            } else {
                if ($PropertyType.IsValueType) {
                    $sb.AppendLine('            if (value.HasValue)').AppendLine('                return value.Value;').Append('            return ').Append($PropertyType.DefaultValue).AppendLine(';') | Out-Null;
                } else {
                    $sb.AppendLine('            if (null == value)').Append('                return ').Append($PropertyType.DefaultValue).AppendLine(';').AppendLine('            return value;') | Out-Null;
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
Function ConvertTo-PasswordHash {
    [CmdletBinding()]
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
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [ValidateLength(96, 96)]
        [ValidatePattern('[A-Za-z\d+/]{96}')]
        [string]$Base64EncodedHash
    )
    [Convert]::FromBase64String($Base64EncodedHash) | Select-Object -Skip 64;
}

Function Test-PasswordHash {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$Password,
        [Parameter(Mandatory = $true)]
        [ValidateLength(96, 96)]
        [ValidatePattern('[A-Za-z\d+/]{96}')]
        [string]$ExpectedHash
    )

    ($ExpectedHash -eq (ConvertTo-PasswordHash -Password $Password -Salt (Get-SaltBytes -Base64EncodedHash $ExpectedHash))) | Write-Output;
}

Function Get-InitializationQueries {
    [CmdletBinding()]
    Param(
        [Parameter(Mandatory = $true)]
        [string]$AdministrativePassword
    )
    $PwHash = ConvertTo-PasswordHash -Password $AdministrativePassword;
    $HostDeviceID = [Guid]::NewGuid().ToString('d');
    $MachineIdentifier = [FsInfoCat.PS.FsInfoCatUtil]::GetLocalMachineSID();
    $MachineName = [System.Environment]::MachineName;
    @"
-- The next query should be executed immediately after the 'SET @CreatedOn = GETDATE();' statement.
INSERT INTO dbo.Account (AccountID, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy,
        DisplayName, LoginName, PwHash, [Role], Notes)
    Values ('00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000',
        'FS InfoCat Administrator', 'admin', '$PwHash', 4, '');

-- The next query should be executed at the end of the setup script.
INSERT INTO dbo.HostDevice (HostDeviceID, DisplayName, MachineIdentifer, MachineName, IsWindows, IsInactive, Notes,
        CreatedOn, CreatedBy, ModifiedOn, ModifiedBy)
    VALUES('$HostDeviceID', NULL, '$MachineIdentifier', '$MachineName', 1, 0, '',
        @CreatedOn, '00000000-0000-0000-0000-000000000000', @CreatedOn, '00000000-0000-0000-0000-000000000000');
"@ | Write-Host;
}
