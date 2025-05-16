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
        public ValidationEventReader(XmlReaderSettings settings)
        {
            _settings = settings ?? new XmlReaderSettings();
            _settings.ValidationType = ValidationType.Schema;
            _settings.ValidationEventHandler += new ValidationEventHandler(ValidationEventHandler);
        }
        void ValidationEventHandler(object sender, ValidationEventArgs e) { _events.AddLast(e); }

        public void Read(PSHostUserInterface ui, string path)
        {
            using XmlReader reader = XmlReader.Create(path, _settings);
            while (reader.Read())
            {
                LinkedListNode<ValidationEventArgs> node = _events.First;
                if (node != null)
                {
                    do
                    {
                        ValidationEventArgs args = node.Value;
                        ui.Write(args.Severity.ToString("F"));
                        ui.Write(" : ");
                        // if (args.Severity == XmlSeverityType.Error)
                        // {
                        //     args.Exception.LineNumber;
                        //     args.Exception.LinePosition;
                        //     args.Exception.Source;
                        //     args.Exception.SourceUri;
                        //     args.Exception.SourceSchemaObject.LineNumber;
                        //     args.Exception.SourceSchemaObject.LinePosition;
                        // }
                        // else
                        // {

                        // }
                        // TODO: Finish implementation
                    } while ((node = node.Next) != null);
                    _events.Clear();
                }
            }
        }
    }
}
