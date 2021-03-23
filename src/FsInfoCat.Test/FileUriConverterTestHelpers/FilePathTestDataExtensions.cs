using FsInfoCat.Test.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public static class FilePathTestDataExtensions
    {
        public const string TestDataElementName = "TestData";
        public const string WindowsElementName = "Windows";
        public const string LinuxElementName = "Linux";
        public const string AbsoluteUrlElementName = "AbsoluteUrl";
        public const string RelativeUrlElementName = "RelativeUrl";
        public const string PathElementName = "Path";
        public const string DirectoryElementName = "Directory";
        public const string FileNameElementName = "FileName";
        public const string LocalPathElementName = "LocalPath";
        public const string FileSystemElementName = "FileSystem";
        public const string URIElementName = "URI";
        public const string TranslatedElementName = "Translated";
        public const string WellFormedElementName = "WellFormed";
        public const string AttributeNameIsWellFormed = "IsWellFormed";
        public const string AttributeNameInputString = "InputString";
        public const string AttributeNameMatch = "Match";
        public const string AttributeNameValue = "Value";
        public const string AttributeNameName = "Name";
        public const string AttributeNameDelimiter = "Delimiter";
        public const string AttributeNameIsAbsolute = "IsAbsolute";
        public const string AuthorityElementName = "Authority";
        public const string QueryElementName = "Query";
        public const string FragmentElementName = "Fragment";
        public const string HostElementName = "Host";
        public const string SchemeElementName = "Scheme";

        public static bool IsTestDataElement(XElement source)
        {
            if (source is null || source.Name.LocalName != TestDataElementName)
                return false;
            XDocument doc = source.Document;
            return !(doc is null) && ReferenceEquals(source.Parent, doc.Root);
        }

        public static bool IsWindowsElement(XElement source) => !(source is null) && source.Name.LocalName == WindowsElementName && IsTestDataElement(source.Parent);

        public static bool IsWindowsChildElement(XElement source) => !(source is null) && source.Ancestors(WindowsElementName).Any(e => IsTestDataElement(e.Parent));

        public static bool IsLinuxElement(XElement source) => !(source is null) && source.Name.LocalName == LinuxElementName && IsTestDataElement(source.Parent);

        public static bool IsLinuxChildElement(XElement source) => !(source is null) && source.Ancestors(LinuxElementName).Any(e => IsTestDataElement(e.Parent));

        public static bool IsPlatformElement(XElement source) =>
            !(source is null) && (source.Name.LocalName == WindowsElementName || source.Name.LocalName == LinuxElementName) && IsTestDataElement(source.Parent);

        public static bool IsPlatformChildElement(XElement source) => !(source is null) && source.Ancestors().Any(e => IsPlatformElement(e));

        public static bool IsUrlElement(XElement element)
        {
            if (element is null)
                return false;
            switch (element.Name.LocalName)
            {
                case AbsoluteUrlElementName:
                case RelativeUrlElementName:
                    return IsPlatformElement(element.Parent);
                case TranslatedElementName:
                    return IsUrlElement(element.Parent);
                default:
                    return false;
            }
        }

        public static bool IsAbsoluteUrlElement(XElement element)
        {
            if (element is null)
                return false;
            return element.Name.LocalName switch
            {
                AbsoluteUrlElementName => IsPlatformElement(element.Parent),
                TranslatedElementName => IsAbsoluteUrlElement(element.Parent),
                _ => false,
            };
        }

        public static bool IsRelativeUrlElement(XElement element)
        {
            if (element is null)
                return false;
            return element.Name.LocalName switch
            {
                AbsoluteUrlElementName => IsPlatformElement(element.Parent),
                TranslatedElementName => IsRelativeUrlElement(element.Parent),
                _ => false,
            };
        }

        public static bool IsLocalPathElement(XElement element) => !(element is null) && element.Name == LocalPathElementName && IsUrlElement(element.Parent);

        public static bool IsPathElement(XElement element) => !(element is null) && element.Name == PathElementName && IsUrlElement(element.Parent);

        public static bool IsAuthorityElement(XElement element) => !(element is null) && element.Name == AuthorityElementName && IsUrlElement(element.Parent);

        public static bool IsQueryElement(XElement element) => !(element is null) && element.Name == QueryElementName && IsUrlElement(element.Parent);

        public static bool IsFragmentElement(XElement element) => !(element is null) && element.Name == FragmentElementName && IsUrlElement(element.Parent);

        public static bool IsHostElement(XElement element)
        {
            if (element is null || element.Name != HostElementName || (element = element.Parent) is null)
                return false;
            return element.Name.LocalName switch
            {
                FileSystemElementName => IsPlatformElement(element.Parent),
                AuthorityElementName => IsUrlElement(element.Parent),
                TranslatedElementName => IsFileSystemElement(element.Parent),
                _ => false,
            };
        }

        public static bool IsSchemeElement(XElement element)
        {
            return !(element is null || element.Name != SchemeElementName || (element = element.Parent) is null || element.Name != AuthorityElementName) &&
                IsUrlElement(element.Parent);
        }

        public static bool IsFileSystemElement(XElement element)
        {
            if (element is null)
                return false;
            return element.Name.LocalName switch
            {
                FileSystemElementName => IsPlatformElement(element.Parent),
                TranslatedElementName => IsFileSystemElement(element.Parent),
                _ => false,
            };
        }

        public static XElement WindowsElement(this XNode node)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if (IsTestDataElement(element))
                    return element.Element(WindowsElementName);
                XElement result = element.AncestorsAndSelf(WindowsElementName).FirstOrDefault(e => IsWindowsElement(e));
                if (result is null)
                {
                    if ((result = result.TestDataElement()) is null)
                        return null;
                    return result.Element(WindowsElementName);
                }
                return result;
            }
            return node.Parent.WindowsElement();
        }

        public static XElement LinuxElement(this XNode node)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if (IsTestDataElement(element))
                    return element.Element(WindowsElementName);
                XElement result = element.AncestorsAndSelf(WindowsElementName).FirstOrDefault(e => IsWindowsElement(e));
                if (result is null)
                {
                    if ((result = result.TestDataElement()) is null)
                        return null;
                    return result.Element(WindowsElementName);
                }
                return result;
            }
            return node.Parent.LinuxElement();
        }

        public static XElement PlatformElement(this XNode node)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if (IsTestDataElement(element))
                    return element.Element(WindowsElementName);
                XElement result = element.AncestorsAndSelf(WindowsElementName).FirstOrDefault(e => IsWindowsElement(e));
                if (result is null)
                {
                    if ((result = result.TestDataElement()) is null)
                        return null;
                    return result.Element(WindowsElementName);
                }
                return result;
            }
            return node.Parent.PlatformElement();
        }

        public static string InputString(this XNode node)
        {
            XElement element = node.TestDataElement();
            return element?.StringAttributeValue("InputString");
        }

        public static XElement TestDataElement(this XNode source) => (source is null) ? null : (
            (source is XElement element) ?
            element.AncestorsAndSelf(TestDataElementName).FirstOrDefault(e =>
            {
                XDocument doc = e.Document;
                return !(doc is null) && ReferenceEquals(e.Parent, doc.Root);
            })
            : source.Parent.TestDataElement()
        );

        public static IEnumerable<XElement> UrlElements(this XObject node, bool preferTranslated = false)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if ((element = element.PlatformElement()) is null)
                    return null;
                if (preferTranslated)
                    return element.Elements(AbsoluteUrlElementName).Elements(RelativeUrlElementName).Select(e =>
                    {
                        XElement t = element.Element(TranslatedElementName);
                        return (t is null) ? e : t;
                    }).OrderByDescending(e => e.IsWellFormed());
                return element.Elements(AbsoluteUrlElementName).Elements(RelativeUrlElementName).OrderByDescending(e => e.IsWellFormed());
            }
            return node.Parent.UrlElements(preferTranslated);

        }

        public static IEnumerable<XElement> TestDataElements(this XDocument document) => document.Root.Elements(TestDataElementName);

        public static IEnumerable<XElement> TestDataElements(this XDocument document, Func<XElement, bool> predicate) =>
            document.Root.Elements(TestDataElementName).Where(predicate);

        public static IEnumerable<XElement> WindowsElements(this XDocument document) => document.TestDataElements().Elements(WindowsElementName);

        public static IEnumerable<XElement> WindowsElements(this XDocument document, Func<XElement, bool> predicate) =>
            document.TestDataElements().Elements(WindowsElementName).Where(predicate);

        public static IEnumerable<XElement> LinuxElements(this XDocument document) => document.TestDataElements().Elements(WindowsElementName);

        public static IEnumerable<XElement> LinuxElements(this XDocument document, Func<XElement, bool> predicate) =>
            document.TestDataElements().Elements(WindowsElementName).Where(predicate);

        public static IEnumerable<XElement> AbsoluteUrlElements(this IEnumerable<XElement> platformElements, Func<XElement, bool> predicate) =>
            platformElements.Elements(AbsoluteUrlElementName).Where(predicate);

        public static IEnumerable<XElement> UrlElements(this IEnumerable<XElement> platformElements, Func<XElement, bool> predicate) => platformElements.Select(pe =>
        {
            XElement e1 = pe.AbsoluteUrlElement();
            if (e1 is null)
                return pe.RelativeUrlElement();
            XElement e2 = pe.RelativeUrlElement();
            return (e2 is null || !e2.IsWellFormed() || e1.IsWellFormed()) ? e1 : e2;
        }).Where(predicate);

        public static IEnumerable<XElement> UrlElements(this IEnumerable<XElement> platformElements) => platformElements.Select(pe =>
        {
            XElement e1 = pe.AbsoluteUrlElement();
            if (e1 is null)
                return pe.RelativeUrlElement();
            XElement e2 = pe.RelativeUrlElement();
            return (e2 is null || !e2.IsWellFormed() || e1.IsWellFormed()) ? e1 : e2;
        });

        public static IEnumerable<XElement> AbsoluteUrlElements(this IEnumerable<XElement> platformElements) => platformElements.Elements(AbsoluteUrlElementName);

        public static IEnumerable<XElement> RelativeUrlElements(this IEnumerable<XElement> platformElements, Func<XElement, bool> predicate) =>
            platformElements.Elements(RelativeUrlElementName).Where(predicate);

        public static IEnumerable<XElement> RelativeUrlElements(this IEnumerable<XElement> platformElements) => platformElements.Elements(RelativeUrlElementName);

        public static XElement UrlElement(this XElement element)
        {
            if (element is null)
                return null;
            XElement result = element.AncestorsAndSelf(AbsoluteUrlElementName).Concat(element.AncestorsAndSelf(RelativeUrlElementName))
                .FirstOrDefault(e => IsUrlElement(e));
            if (result is null)
                return ((element = element.PlatformElement()) is null) ? null : element.UrlElements().FirstOrDefault();
            return result;
        }

        public static XElement AuthorityElement(this XElement element)
        {
            if (element is null)
                return null;
            XElement result = element.AncestorsAndSelf(AuthorityElementName).FirstOrDefault(e => IsAuthorityElement(e));
            if (result is null)
                return ((element = element.UrlElement()) is null || !IsAbsoluteUrlElement(element)) ? null : element.Elements(AuthorityElementName).FirstOrDefault();
            return result;
        }

        public static XElement SchemeElement(this XElement element)
        {
            if (element is null)
                return null;
            XElement result = element.AncestorsAndSelf(SchemeElementName).FirstOrDefault(e => IsSchemeElement(e));
            if (result is null)
                return ((element = element.AuthorityElement()) is null) ? null : element.Elements(SchemeElementName).FirstOrDefault();
            return result;
        }

        public static XElement HostElement(this XElement element)
        {
            if (element is null)
                return null;
            XElement result = element.AncestorsAndSelf(HostElementName).FirstOrDefault(e => IsHostElement(e));
            if (result is null)
                return ((element = element.AuthorityElement()) is null && (element = element.FileSystemElement()) is null) ? null :
                    element.Elements(HostElementName).FirstOrDefault();
            return result;
        }

        public static XElement PathElement(this XElement element)
        {
            if (element is null)
                return null;
            XElement result = element.AncestorsAndSelf(PathElementName).FirstOrDefault(e => IsPathElement(e));
            if (result is null)
                if (result is null)
                    return ((element = element.UrlElement()) is null && (element = element.FileSystemElement()) is null) ? null :
                        element.Elements(PathElementName).FirstOrDefault();
            return result;
        }

        public static XElement URIElement(this XElement element)
        {
            if (element is null)
                return null;
            XElement result = element.AncestorsAndSelf(URIElementName).FirstOrDefault(e => IsUrlElement(e.Parent) || IsFileSystemElement(e.Parent));
            if (result is null)
                if (result is null)
                    return ((element = element.UrlElement()) is null && (element = element.FileSystemElement()) is null) ? null :
                        element.Elements(URIElementName).FirstOrDefault();
            return result;
        }

        public static bool IsWellFormed(this XElement element) => IsUrlElement(element) &&
            element.TryGetAttributeBooleanValue(AttributeNameIsWellFormed, out bool result) && result;

        public static bool IsFileScheme(this XElement element)
        {
            if ((element = element.SchemeElement()) is null)
                return false;
            return element.TryGetAttributeStringValue(AttributeNameName, out string result) && result == "file";
        }

        public static bool IsAbsolute(this XElement element) => !(element is null) && (IsLocalPathElement(element) || IsFileSystemElement(element)) &&
            element.TryGetAttributeBooleanValue(AttributeNameIsAbsolute, out bool result) && result;

        public static XElement PreferWellFormed(this XElement source)
        {
            XElement t;
            return (source is null || (t = source.Element(WellFormedElementName)) is null) ? source : t;
        }

        public static XElement PreferTranslated(this XElement source)
        {
            XElement t;
            return (source is null || (t = source.Element(TranslatedElementName)) is null) ? source : t;
        }

        public static IEnumerable<XElement> PreferTranslated(this IEnumerable<XElement> source) => source?.Where(e => !(e is null)).Select(e =>
        {
            XElement t = e.Element(TranslatedElementName);
            return (t is null) ? e : t;
        });

        public static XElement FileSystemElement(this XObject node)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if (IsFileSystemElement(element))
                    return element;
                if (IsPlatformElement(element))
                    return element.Elements(FileSystemElementName).FirstOrDefault();
                XElement result = element.Ancestors(FileSystemElementName).FirstOrDefault(e => IsPlatformElement(e.Parent));
                return (result is null && !((element = element.PlatformElement()) is null)) ? element.Elements(FileSystemElementName).FirstOrDefault() : result;
            }
            return node.Parent.FileSystemElement();
        }

        public static XElement AbsoluteUrlElement(this XObject node)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if (IsAbsoluteUrlElement(element))
                    return element;
                if (IsPlatformElement(element))
                    return element.Elements(AbsoluteUrlElementName).FirstOrDefault();
                XElement result = element.Ancestors(AbsoluteUrlElementName).FirstOrDefault(e => IsPlatformElement(e.Parent));
                return (result is null && !((element = element.PlatformElement()) is null)) ? element.Elements(AbsoluteUrlElementName).FirstOrDefault() : result;
            }
            return node.Parent.AbsoluteUrlElement();
        }

        public static XElement RelativeUrlElement(this XObject node)
        {
            if (node is null)
                return null;
            if (node is XElement element)
            {
                if (IsRelativeUrlElement(element))
                    return element;
                if (IsPlatformElement(element))
                    return element.Elements(RelativeUrlElementName).FirstOrDefault();
                XElement result = element.Ancestors(RelativeUrlElementName).FirstOrDefault(e => IsPlatformElement(e.Parent));
                return (result is null && !((element = element.PlatformElement()) is null)) ? element.Elements(RelativeUrlElementName).FirstOrDefault() : result;
            }
            return node.Parent.RelativeUrlElement();
        }
    }
}
