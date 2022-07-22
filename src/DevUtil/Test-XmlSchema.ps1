Param(
    [string]$InputPath = 'ModelDefinitions.xml',

    [string[]]$Schemas = 'ModelDefinitions.xsd',

    [System.Xml.ConformanceLevel]$ConformanceLevel,

    [System.Xml.Schema.XmlSchemaValidationFlags[]]$ValidationFlags,

    [switch]$CheckCharacters,

    [switch]$IgnoreComments,

    [switch]$IgnoreWhitespace,

    [switch]$IgnoreProcessingInstructions,

    [switch]$ProcessInlineSchema,

    [switch]$ProcessSchemaLocation,

    [switch]$ReportValidationWarnings,

    [switch]$DoNotProcessIdentityConstraints,

    [switch]$DisAllowXmlAttributes
)

Add-Type -TypeDefinition @'
namespace ScriptCLR
{
    using System;
    using System.Collections.Generic;
    using System.Management.Automation.Host;
    using System.Xml;
    using System.Xml.Schema;

    public class ValidationEventReader
    {
        private readonly XmlReaderSettings _settings;
        private readonly LinkedList<ValidationEventArgs> _events = new LinkedList<ValidationEventArgs>();
        public XmlReaderSettings Settings { get { return _settings; } }
        public ValidationEventCollection(XmlReaderSettings settings)
        {
            _settings = (settings == null) ? new XmlReaderSettings() : settings;
            _settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new ValidationEventHandler(validationEventHandler);
        }
        void validationEventHandler(object sender, ValidationEventArgs e) { _events.AddLast(e); }
        public static void Read(PSHostUserInterface ui, string path)
        {
            using (XmlReader reader = XmlReader.Create(path, _settings))
            {
                while (reader.Read())
                {
                    LinkedListNode<ValidationEventArgs> node = _events.First;
                    if (node != null)
                    {
                        do
                        {
                            ui.Write()
                            yield return node.Value;
                        } while ((node = node.Next) != null);
                        _events.Clear();
                    }
                }
            }
        }
    }
}
'@

$ValidationEventCollection = [ValidationEventCollection]::new([System.Xml.XmlReaderSettings]@{
    CheckCharacters = $CheckCharacters.IsPresent;
    IgnoreComments = $IgnoreComments.IsPresent;
    IgnoreWhitespace = $IgnoreWhitespace.IsPresent;
    IgnoreProcessingInstructions = $IgnoreProcessingInstructions.IsPresent;
});
if ($PSBoundParameters.ContainsKey('ConformanceLevel')) { $ValidationEventCollection.Settings.ConformanceLevel = $ConformanceLevel }
if ($DoNotProcessIdentityConstraints.IsPresent) {
    if ($DisAllowXmlAttributes.IsPresent) {
        $ValidationEventCollection.Settings.ValidationFlags = [System.Xml.Schema.XmlSchemaValidationFlags]::None;
    } else {
        $ValidationEventCollection.Settings.ValidationFlags = [System.Xml.Schema.XmlSchemaValidationFlags]::AllowXmlAttributes;
    }
} else {
    if ($DisAllowXmlAttributes.IsPresent) {
        $ValidationEventCollection.Settings.ValidationFlags = [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessIdentityConstraints;
    } else {
        $ValidationEventCollection.Settings.ValidationFlags = [System.Xml.Schema.XmlSchemaValidationFlags]::AllowXmlAttributes -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessIdentityConstraints;
    }
}
if ($ProcessInlineSchema.IsPresent) {
    $ValidationEventCollection.Settings.ValidationFlags = $ValidationEventCollection.Settings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessInlineSchema;
}
if ($ProcessSchemaLocation.IsPresent) {
    $ValidationEventCollection.Settings.ValidationFlags = $ValidationEventCollection.Settings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ProcessSchemaLocation;
}
if ($ReportValidationWarnings.IsPresent) {
    $ValidationEventCollection.Settings.ValidationFlags = $ValidationEventCollection.Settings.ValidationFlags -bor [System.Xml.Schema.XmlSchemaValidationFlags]::ReportValidationWarnings;
}
