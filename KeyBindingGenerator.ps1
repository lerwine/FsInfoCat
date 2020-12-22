Param(
    [ValidateSet('KeyBindings', 'CommandBindings')]
    [string]$Get = 'CommandBindings'
)
[xml]$InputBindings = @'
<Window.InputBindings>
    <KeyBinding Key="F5" Command="{Binding SpeakAllTextCommand, Mode=OneWay}" Gesture="Alt+F4" />
    <KeyBinding Modifiers="Alt" Key="F4" Command="ApplicationCommands.Close" Gesture="Alt+F4" />
    <KeyBinding Modifiers="Ctrl" Key="Space" Command="ApplicationCommands.ContextMenu" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="C" Command="ApplicationCommands.Copy" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="Enter" Command="ApplicationCommands.CorrectionList" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="X" Command="ApplicationCommands.Cut" Gesture="Ctrl+Space" />
    <KeyBinding Key="Delete" Command="ApplicationCommands.Delete" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="F" Command="ApplicationCommands.Find" Gesture="Ctrl+Space" />
    <KeyBinding Key="F1" Command="ApplicationCommands.Help" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="N" Command="ApplicationCommands.New" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="O" Command="ApplicationCommands.Open" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="V" Command="ApplicationCommands.Paste" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="P" Command="ApplicationCommands.Print" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl+Shift" Key="P" Command="ApplicationCommands.PrintPreview" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Alt" Key="Enter" Command="ApplicationCommands.Properties" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="Y" Command="ApplicationCommands.Redo" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="H" Command="ApplicationCommands.Replace" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="S" Command="ApplicationCommands.Save" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl+Shift" Key="S" Command="ApplicationCommands.SaveAs" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="A" Command="ApplicationCommands.SelectAll" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Shift" Key="F5" Command="ApplicationCommands.Stop" Gesture="Ctrl+Space" />
    <KeyBinding Modifiers="Ctrl" Key="Z" Command="ApplicationCommands.Undo" Gesture="Ctrl+Space" />
</Window.InputBindings>
'@
[xml]$CommandBindings = '<Window.CommandBindings />';
$XmlWriterSettings = [System.Xml.XmlWriterSettings]::new();
$XmlWriterSettings.Indent = $true;
$XmlWriterSettings.OmitXmlDeclaration = $true;
$Sb = [System.Text.StringBuilder]::new();
@($InputBindings.DocumentElement.SelectNodes('KeyBinding')) | ForEach-Object {
    if ($null -eq $_.Modifiers) {
        $_.Gesture = $_.Key;
    } else {
        $_.Gesture = $_.Modifiers + '+' + $_.Key;
    }
    
    $Element = $_;
    switch ($Get) {
        'CommandBindings' {
            $c = $Element.Command;
            $i = $c.IndexOf('.');
            if ($i -gt 0) {
                $Element = $InputBindings.CreateElement('CommandBinding');
                $Element.Attributes.Append($InputBindings.CreateAttribute('Command')).Value = $c;
                $c = $c.Substring($i + 1);
                $Element.Attributes.Append($InputBindings.CreateAttribute('CanExecute')).Value = $c + 'Command.OnCanExecute';
                $Element.Attributes.Append($InputBindings.CreateAttribute('Executed')).Value = $c + 'Command.OnExecuted';
            } else {
                $Element = $null;
            }
            break;
        }
        default {
            break;
        }
    }
    if ($null -ne $Element) {
        $Sb.AppendLine().Append("        ") | Out-Null;
        $Sw = [System.IO.StringWriter]::new($Sb);
        $XmlWriter = [System.Xml.XmlWriter]::Create($Sw, $XmlWriterSettings);
        $Element.WriteTo($XmlWriter);
        $XmlWriter.Flush();
        $Sw.Flush();
        $XmlWriter.Close();
    }
}

#$Sb.ToString()
[System.Windows.Clipboard]::SetText($Sb.ToString());
