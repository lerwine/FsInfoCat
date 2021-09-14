$Name = 'FolderOpened';
$ImportPath = [System.IO.Path]::Combine('C:\Users\lerwi\Downloads\VS2019 Image Library\vswin2019', $Name);
$Files = [System.IO.Directory]::GetFiles($ImportPath, '*.xaml');
if ($Files.Length -eq 1) {
    $ImportPath = $Files[0];
} else {
    if ($Files.Length -eq 0) {
        Write-Warning -Message 'Not found';
        return;
    }
    $Choices = [System.Collections.ObjectModel.Collection[System.Management.Automation.Host.ChoiceDescription]]::new();
    $Files | ForEach-Object { $Choices.Add([System.Management.Automation.Host.ChoiceDescription]::new([System.IO.Path]::GetFileNameWithoutExtension($_))) }
    $i = $Host.UI.PromptForChoice("Multiple Matches", "Select name of file to import", $Choices, 0);
    if ($null -eq $i -or $i -lt 0 -or $i -ge $Files.Length) { return }
    $ImportPath = $Files[$i];
}
$ImportXmlDocument = [System.Xml.XmlDocument]::new();
$ImportXmlDocument.Load($ImportPath);
$ImportNsmgr = [System.Xml.XmlNamespaceManager]::new($ImportXmlDocument.NameTable);
$ImportNsmgr.AddNamespace('p', $PresentationNamespace);
$DrawingGroupChildren = $ImportXmlDocument.DocumentElement.SelectSingleNode('//p:DrawingGroup.Children', $ImportNsmgr);
if ($null -eq $DrawingGroupChildren) {
    Write-Warning -Message "Could not find DrawingGroup.Children in $ImportPath";
    return;
}
$KeyBase = [System.IO.Path]::GetFileName([System.IO.Path]::GetDirectoryName($ImportPath));
$StylesPath = 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\Themes\Styles.xaml';
$VectorPath = 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\Themes\Vector.xaml';
$ExamplesPath = 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Desktop\View\VectorExamplesUserControl.xaml'
$PresentationNamespace = 'http://schemas.microsoft.com/winfx/2006/xaml/presentation';
$XamlNamespace = 'http://schemas.microsoft.com/winfx/2006/xaml';
$StylesXmlDocument = [System.Xml.XmlDocument]::new();
$VectorXmlDocument = [System.Xml.XmlDocument]::new();
$ExamplesDocument = [System.Xml.XmlDocument]::new();
$StylesXmlDocument.Load($StylesPath);
$VectorXmlDocument.Load($VectorPath);
$ExamplesDocument.Load($ExamplesPath);
$StylesNsmgr = [System.Xml.XmlNamespaceManager]::new($StylesXmlDocument.NameTable);
$StylesNsmgr.AddNamespace('p', $PresentationNamespace);
$StylesNsmgr.AddNamespace('d', 'http://schemas.microsoft.com/expression/blend/2008');
$StylesNsmgr.AddNamespace('mc', 'http://schemas.openxmlformats.org/markup-compatibility/2006');
$StylesNsmgr.AddNamespace('View', 'clr-namespace:FsInfoCat.Desktop.View');
$StylesNsmgr.AddNamespace('System', 'clr-namespace:System;assembly=mscorlib');
$StylesNsmgr.AddNamespace('x', $XamlNamespace);
$ExamplesNsmgr = [System.Xml.XmlNamespaceManager]::new($ExamplesDocument.NameTable);
$ExamplesNsmgr.AddNamespace('p', $PresentationNamespace);
$ExamplesNsmgr.AddNamespace('x', $XamlNamespace);
$ButtonKey = "$($KeyBase)Button";
if ($null -ne $StylesXmlDocument.SelectSingleNode("p:Style[@x:Key='$ButtonKey']", $nsmgr)) {
    Write-Warning -Message "Key $ButtonKey already exists";
    return;
}
$DataTemplateKey = "$($KeyBase)ImageContentTemplate";
if ($null -ne $VectorXmlDocument.SelectSingleNode("p:DataTemplate[@x:Key='$DataTemplateKey']", $nsmgr)) {
    Write-Warning -Message "Key $DataTemplateKey already exists";
    return;
}

$DataTemplate = $VectorXmlDocument.DocumentElement.AppendChild($VectorXmlDocument.CreateElement('DataTemplate', $PresentationNamespace));
$DataTemplate.Attributes.Append($VectorXmlDocument.CreateAttribute('x', 'Key', $XamlNamespace)).Value = $DataTemplateKey;
$XmlElement = $DataTemplate.AppendChild($VectorXmlDocument.CreateElement('Image', $PresentationNamespace));
$XmlElement = $XmlElement.AppendChild($VectorXmlDocument.CreateElement('Image.Source', $PresentationNamespace));
$XmlElement = $XmlElement.AppendChild($VectorXmlDocument.CreateElement('DrawingImage', $PresentationNamespace));
$XmlElement = $XmlElement.AppendChild($VectorXmlDocument.CreateElement('DrawingImage.Drawing', $PresentationNamespace));
$XmlElement = $XmlElement.AppendChild($VectorXmlDocument.CreateElement('DrawingGroup', $PresentationNamespace));
$XmlElement.AppendChild($VectorXmlDocument.ImportNode($DrawingGroupChildren, $true)) | Out-Null;
$DataTemplate.OuterXml;
$ButtonStyle = $StylesXmlDocument.DocumentElement.AppendChild($StylesXmlDocument.CreateElement('Style', $PresentationNamespace));
$ButtonStyle.Attributes.Append($StylesXmlDocument.CreateAttribute('x', 'Key', $XamlNamespace)).Value = $ButtonKey;
$ButtonStyle.Attributes.Append($StylesXmlDocument.CreateAttribute('TargetType')).Value = '{x:Type Button}';
$ButtonStyle.Attributes.Append($StylesXmlDocument.CreateAttribute('BasedOn')).Value = '{StaticResource CommandButton}';
$XmlElement = $ButtonStyle.AppendChild($StylesXmlDocument.CreateElement('Setter', $PresentationNamespace));
$XmlElement.Attributes.Append($StylesXmlDocument.CreateAttribute('Property')).Value = 'ContentTemplate';
$XmlElement.Attributes.Append($StylesXmlDocument.CreateAttribute('Value')).Value = "{DynamicResource $DataTemplateKey}";
$ButtonStyle.OuterXml;
$WrapPanel = $ExamplesDocument.DocumentElement.SelectSingleNode('p:StackPanel/p:WrapPanel', $ExamplesNsmgr);
$ContentControl = $WrapPanel.AppendChild($ExamplesDocument.CreateElement('ContentControl', $PresentationNamespace));
$ContentControl.Attributes.Append($ExamplesDocument.CreateAttribute('ContentTemplate')).Value = "{DynamicResource $DataTemplateKey}";
$ContentControl.Attributes.Append($ExamplesDocument.CreateAttribute('Height')).Value = '32';
$ContentControl.Attributes.Append($ExamplesDocument.CreateAttribute('Width')).Value = '32';

$XmlWriterSettings = [System.Xml.XmlWriterSettings]@{
    Indent = $true;
    Encoding = [System.Text.UTF8Encoding]::new($false);
};
$XmlWriter = [System.Xml.XmlWriter]::Create($StylesPath, $XmlWriterSettings);
try {
    $StylesXmlDocument.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
$XmlWriter = [System.Xml.XmlWriter]::Create($VectorPath, $XmlWriterSettings);
try {
    $VectorXmlDocument.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }
$XmlWriter = [System.Xml.XmlWriter]::Create($ExamplesPath, $XmlWriterSettings);
try {
    $ExamplesDocument.WriteTo($XmlWriter);
    $XmlWriter.Flush();
} finally { $XmlWriter.Close() }

<#
#>