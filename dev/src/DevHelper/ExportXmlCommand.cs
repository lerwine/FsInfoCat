using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using Microsoft.PowerShell.Commands;
using System.IO;

namespace DevHelper
{
    [Cmdlet(VerbsData.Export, "Xml", DefaultParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM)]
    [OutputType(typeof(Hashtable[]))]
    public class ExportXmlCommand : PSCmdlet, IDynamicParameters
    {
        public const string PARAMETER_SET_NAME_PREFIX_SINGLE = "Single";
        public const string PARAMETER_SET_NAME_PREFIX_MULTI = "Multi";
        public const string PARAMETER_SET_NAME_SUFFIX_PARAM = "Object";
        public const string PARAMETER_SET_NAME_SUFFIX_FACTORY = "Factory";
        public const string PARAMETER_SET_NAME_TARGET_PATH = "Path";
        public const string PARAMETER_SET_NAME_TARGET_FILE_INFO = "FileInfo";
        public const string PARAMETER_SET_NAME_TARGET_STRING = "String";
        public const string PARAMETER_SET_NAME_TARGET_STREAM = "Stream";
        public const string PARAMETER_SET_NAME_TARGET_TEXT_WRITER = "TextWriter";
        public const string PARAMETER_SET_NAME_TARGET_XML_WRITER = "XmlWriter";
        public const string PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM = PARAMETER_SET_NAME_PREFIX_SINGLE + ":" + PARAMETER_SET_NAME_TARGET_PATH + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_PATH + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_PATH + ":" + PARAMETER_SET_NAME_SUFFIX_FACTORY;
        public const string PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM = PARAMETER_SET_NAME_PREFIX_SINGLE + ":" + PARAMETER_SET_NAME_TARGET_FILE_INFO + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_FILE_INFO + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM = PARAMETER_SET_NAME_PREFIX_SINGLE + ":" + PARAMETER_SET_NAME_TARGET_STRING + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_STRING + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_STRING + ":" + PARAMETER_SET_NAME_SUFFIX_FACTORY;
        public const string PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM = PARAMETER_SET_NAME_PREFIX_SINGLE + ":" + PARAMETER_SET_NAME_TARGET_STREAM + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_STREAM + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM = PARAMETER_SET_NAME_PREFIX_SINGLE + ":" + PARAMETER_SET_NAME_TARGET_TEXT_WRITER + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM = PARAMETER_SET_NAME_PREFIX_MULTI + ":" + PARAMETER_SET_NAME_TARGET_TEXT_WRITER + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string PARAMETER_SET_NAME_SINGLE_TO_XML_WRITER_PARAM = PARAMETER_SET_NAME_PREFIX_SINGLE + ":" + PARAMETER_SET_NAME_TARGET_XML_WRITER + ":" + PARAMETER_SET_NAME_SUFFIX_PARAM;
        public const string HELP_MESSAGE_XML = "XML Node to be exported.";
        public const string HELP_MESSAGE_PATH = "Export the XML to a file at the specified path. If the parent directory does not exist, it will not be created and teh command will fail.";
        public const string HELP_MESSAGE_PATH_FACTORY = "ScriptBlock to invoke which will return a path for each XML node to export. The XML node to be exported will be passed as the first parameter, the zero-based sequential number of the node to be exported will be the second parameter, and the XmlWriterSettings object being used to control output will be the third parameter.";
        public const string HELP_MESSAGE_FILE_INFO = "Export the XML to a file represented by a FileInfo object. If the parent directory does not exist, it will not be created and teh command will fail.";
        public const string HELP_MESSAGE_FORCE = "Overwrite the existing XML file if it already exists. If this is not present, and the file exist, the command will fail.";
        public const string HELP_MESSAGE_SINGLE_STRING = "Return the exported XML as a string. If multiple XML nodes are provided, the resulting string will be an xml fragment containing the exported nodes.";
        public const string HELP_MESSAGE_MULTI_STRING = "Return the exported XML as a string. If multiple XML nodes are provided, they will be exported as a separate strings for each node.";
        public const string HELP_MESSAGE_STREAM = "Export the XML to a writable Stream object. If multiple XML nodes are provided, each node will be appended to the stream.";
        public const string HELP_MESSAGE_TEXT_WRITER = "Export the XML to a TextWriter object. If multiple XML nodes are provided, each node will be appended to the writer.";
        public const string HELP_MESSAGE_XML_WRITER = "Export the XML to an XmlWriter object.";
        public const string HELP_MESSAGE_SETTINGS = "Specifies a set of features to support on the XmlWriter object. If other feature parameters are combined with this, properties of this object won't be changed, since a modified clone of this object is what will be used when creating the XmlWriter.";
        public const string HELP_MESSAGE_OMIT_XML_DECLARATION = "Do not write an XML declaration.";
        public const string HELP_MESSAGE_NEWLINE_HANDLING = "How to handle newline characters. The default is Replace.";
        public const string HELP_MESSAGE_NEWLINE_CHARS = "Character string to use for line breaks. The default is to use [System.Environment]::NewLine.";
        public const string HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES = "Remove duplicate namespace declarations when writing XML content. The default behavior is for the writer to output all namespace declarations that are present in the writer's namespace resolver.";
        public const string HELP_MESSAGE_INDENT = "Indent elements.";
        public const string HELP_MESSAGE_ENCODING = "Text encoding to use. This is ignored if Writing to a TextWriter and the TextWriter.Encoding property is set. Otherwise, the default is utf-8 without BOM.";
        public const string HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES = "Do not escape URI attributes.";
        public const string HELP_MESSAGE_CLOSE_OUTPUT = "Close the underlying Stream or TextWriter when the XmlWriter.Close() method is called.";
        public const string HELP_MESSAGE_NO_CHECK_CHARACTERS = "Do not check that characters are in the legal XML character set, as defined by W3C.";
        public const string HELP_MESSAGE_CONFORMANCE_LEVEL = "Whether to check that output is a well-formed XML 1.0 document or fragment. The default is Auto;";
        public const string HELP_MESSAGE_NO_AUTO_CLOSE_TAG = "Do not add closing tags to unclosed elements when the XmlWriter.Close() method is called.";
        public const string HELP_MESSAGE_INDENT_CHARS = "Character string to use when indenting. The default is two spaces.";
        public const string HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES = "Write attributes on individual lines.";
        public const NewLineHandling DEFAULT_NEWLINE_HANDLING = NewLineHandling.Replace;
        public static readonly string DEFAULT_NEWLINE_CHARS = Environment.NewLine;
        public static readonly Encoding DEFAULT_ENCODING = new UTF8Encoding(false, true);
        public const ConformanceLevel DEFAULT_CONFORMANCE_LEVEL = ConformanceLevel.Auto;
        public const string DEFAULT_INDENT_CHARS = "  ";

        private readonly IndentDynamicProperties _dynamicProperties = new IndentDynamicProperties();
        private XmlWriterSettings _settings;

        [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_XML_WRITER_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        public XmlNode Xml { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_XML)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_XML)]
        [Alias("XmlNode", "XmlElement")]
        public XmlNode[] InputNode { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_PATH)]
        [Parameter(Mandatory = true, Position = 0, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_PATH)]
        public string Path { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_PATH_FACTORY)]
        public ScriptBlock PathFactory { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_FILE_INFO)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_FILE_INFO)]
        public FileInfo FileInfo { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_SINGLE_STRING)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_SINGLE_STRING)]
        public SwitchParameter AsSingleString { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_MULTI_STRING)]
        public SwitchParameter AsStrings { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_STREAM)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_STREAM)]
        public Stream Stream { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_TEXT_WRITER)]
        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_TEXT_WRITER)]
        public TextWriter TextWriter { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_XML_WRITER_PARAM, HelpMessage = HELP_MESSAGE_XML_WRITER)]
        [Alias("Writer")]
        public XmlWriter XmlWriter { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_SETTINGS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_SETTINGS)]
        public XmlWriterSettings Settings { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_FORCE)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_FORCE)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_FORCE)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_FORCE)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_FORCE)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_FORCE)]
        public SwitchParameter Force { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_OMIT_XML_DECLARATION)]
        public SwitchParameter OmitXmlDeclaration { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_NEWLINE_HANDLING)]
        public NewLineHandling NewLineHandling { get; set; } = DEFAULT_NEWLINE_HANDLING;

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_NEWLINE_CHARS)]
        public string NewLineChars { get; set; } = DEFAULT_NEWLINE_CHARS;

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_OMIT_DUPLICATE_NAMESPACES)]
        public SwitchParameter OmitDuplicateNamespaces { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_ENCODING)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_ENCODING)]
        public Encoding Encoding { get; set; } = DEFAULT_ENCODING;

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_DO_NOT_ESCAPE_URI_ATTRIBUTES)]
        public SwitchParameter DoNotEscapeUriAttributes { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_CONFORMANCE_LEVEL)]
        public ConformanceLevel ConformanceLevel { get; set; } = DEFAULT_CONFORMANCE_LEVEL;

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_CLOSE_OUTPUT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_CLOSE_OUTPUT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_CLOSE_OUTPUT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_CLOSE_OUTPUT)]
        public SwitchParameter CloseOutput { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_NO_CHECK_CHARACTERS)]
        public SwitchParameter NoCheckCharacters { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_NO_AUTO_CLOSE_TAG)]
        public SwitchParameter NoAutoCloseTag { get; set; }

        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_INDENT)]
        [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_INDENT)]
        public SwitchParameter Indent { get; set; }

        public object GetDynamicParameters()
        {
            if (Indent.IsPresent)
                return _dynamicProperties;
            _dynamicProperties.Reset();
            return null;
        }

        protected override void BeginProcessing()
        {
            Dictionary<string, object> boundParameters = MyInvocation.BoundParameters;
            if (boundParameters.ContainsKey(nameof(Settings)))
            {
                _settings = Settings.Clone();
                if (boundParameters.ContainsKey(nameof(OmitXmlDeclaration)))
                    _settings.OmitXmlDeclaration = OmitXmlDeclaration.IsPresent;
                if (boundParameters.ContainsKey(nameof(_dynamicProperties.NewLineOnAttributes)))
                    _settings.NewLineOnAttributes = _dynamicProperties.NewLineOnAttributes.IsPresent;
                if (boundParameters.ContainsKey(nameof(NewLineHandling)))
                    _settings.NewLineHandling = NewLineHandling;
                if (boundParameters.ContainsKey(nameof(NewLineChars)))
                    _settings.NewLineChars = (boundParameters.ContainsKey(nameof(TextWriter))) ? TextWriter.NewLine : NewLineChars;
                if (boundParameters.ContainsKey(nameof(OmitDuplicateNamespaces)))
                    _settings.NamespaceHandling = (OmitDuplicateNamespaces.IsPresent) ? NamespaceHandling.OmitDuplicates : NamespaceHandling.Default;
                if (boundParameters.ContainsKey(nameof(_dynamicProperties.IndentChars)))
                    _settings.IndentChars = _dynamicProperties.IndentChars;
                if (boundParameters.ContainsKey(nameof(Indent)))
                    _settings.Indent = Indent.IsPresent;
                if (boundParameters.ContainsKey(nameof(Encoding)) && !(boundParameters.ContainsKey(nameof(TextWriter)) && null != TextWriter.Encoding))
                    _settings.Encoding = Encoding;
                if (boundParameters.ContainsKey(nameof(DoNotEscapeUriAttributes)))
                    _settings.DoNotEscapeUriAttributes = DoNotEscapeUriAttributes.IsPresent;
                if (boundParameters.ContainsKey(nameof(ConformanceLevel)))
                    _settings.ConformanceLevel = ConformanceLevel;
                if (boundParameters.ContainsKey(nameof(CloseOutput)))
                    _settings.CloseOutput = CloseOutput.IsPresent;
                if (boundParameters.ContainsKey(nameof(NoCheckCharacters)))
                    _settings.CheckCharacters = !NoCheckCharacters.IsPresent;
                if (boundParameters.ContainsKey(nameof(NoAutoCloseTag)))
                    _settings.WriteEndDocumentOnClose = !NoAutoCloseTag.IsPresent;
            }
            else if (ParameterSetName != PARAMETER_SET_NAME_SINGLE_TO_XML_WRITER_PARAM)
                _settings = new XmlWriterSettings
                {
                    Async = false,
                    OmitXmlDeclaration = OmitXmlDeclaration.IsPresent,
                    NewLineOnAttributes = _dynamicProperties.NewLineOnAttributes.IsPresent,
                    NewLineHandling = NewLineHandling,
                    NewLineChars = (boundParameters.ContainsKey(nameof(TextWriter))) ? TextWriter.NewLine : NewLineChars,
                    NamespaceHandling = (OmitDuplicateNamespaces.IsPresent) ? NamespaceHandling.OmitDuplicates : NamespaceHandling.Default,
                    IndentChars = _dynamicProperties.IndentChars,
                    Indent = Indent.IsPresent,
                    Encoding = (boundParameters.ContainsKey(nameof(TextWriter)) && null != TextWriter.Encoding) ? TextWriter.Encoding : Encoding,
                    DoNotEscapeUriAttributes = DoNotEscapeUriAttributes.IsPresent,
                    ConformanceLevel = ConformanceLevel,
                    CloseOutput = CloseOutput.IsPresent,
                    CheckCharacters = !NoCheckCharacters.IsPresent,
                    WriteEndDocumentOnClose = !NoAutoCloseTag.IsPresent
                };
            else
                _settings = XmlWriter.Settings;
        }

        protected override void ProcessRecord()
        {
        }

        public class IndentDynamicProperties
        {
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_INDENT_CHARS)]
            public string IndentChars { get; set; } = DEFAULT_INDENT_CHARS;

            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_SINGLE_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STREAM_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_TEXT_WRITER_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_PATH_FACTORY, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_FILE_INFO_PARAM, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            [Parameter(ParameterSetName = PARAMETER_SET_NAME_MULTI_TO_STRING_FACTORY, HelpMessage = HELP_MESSAGE_NEW_LINE_ON_ATTRIBUTES)]
            public SwitchParameter NewLineOnAttributes { get; set; }

            internal void Reset()
            {
                IndentChars = DEFAULT_INDENT_CHARS;
                NewLineOnAttributes = new SwitchParameter(false);
            }
        }
    }
}
