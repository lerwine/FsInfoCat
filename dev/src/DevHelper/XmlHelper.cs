using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;

namespace DevHelper
{
    public static class XmlHelper
    {
        public static IEnumerable<XmlNode> FollowingNodes(this XmlNode node, bool includeCurrent = false) =>
            new EnumerableWrapper(() => new FollowingEnumerator(node, includeCurrent));

        public static IEnumerable<XmlNode> PrecedingNodes(this XmlNode node, bool includeCurrent = false) =>
            new EnumerableWrapper(() => new PrecedingEnumerator(node, includeCurrent));

        public static bool TryGetLastChild(this XmlNode node, out XmlNode lastChild)
        {
            if (null == node)
                lastChild = null;
            else if (null != (lastChild = node.LastChild))
                return true;
            return false;
        }

        public static IEnumerable<XmlNode> DescendantNodes(this XmlNode node, bool includeCurrent = false, int maxDepth = -1) =>
            new EnumerableWrapper(() => new DescendantEnumerator(node, includeCurrent, maxDepth));

        public static IEnumerable<XmlNode> AncestorNodes(this XmlNode node, bool includeCurrent = false, bool excludeDocument = false) =>
            new EnumerableWrapper(() => new AncestorEnumerator(node, includeCurrent, excludeDocument));

        public static T FirstOfType<T>(this XmlNode node) where T : XmlNode => (node is null) ? null : node.ChildNodes.OfType<T>().FirstOrDefault();

        public static T LastOfType<T>(this XmlNode node) where T : XmlNode => (node is null) ? null : node.ChildNodes.OfType<T>().LastOrDefault();

        public static T NextOfType<T>(this XmlNode node, bool includeCurrent = false)
            where T : XmlNode
        {
            if (node is null || (!includeCurrent && (node = node.NextSibling) is null))
                return null;
            do
            {
                if (node is T t)
                    return t;
            }
            while (null != (node = node.NextSibling));
            return null;
        }

        public static T PreviousOfType<T>(this XmlNode node, bool includeCurrent = false)
            where T : XmlNode
        {
            if (node is null || (!includeCurrent && (node = node.PreviousSibling) is null))
                return null;
            do
            {
                if (node is T t)
                    return t;
            }
            while (null != (node = node.PreviousSibling));
            return null;
        }

        public static bool Precedes(this XmlNode node, XmlNode refNode)
        {
            if (node is null || (node = node.NextSibling) is null)
                return refNode is null;
            if (refNode is null || refNode.PreviousSibling is null || node.ParentNode is null || !ReferenceEquals(node.ParentNode, refNode.ParentNode))
                return false;
            do
            {
                if (ReferenceEquals(refNode, node))
                    return true;
            }
            while (null != (node = node.PreviousSibling));
            return false;
        }

        public static bool Follows(this XmlNode node, XmlNode refNode)
        {
            if (node is null || (node = node.PreviousSibling) is null)
                return refNode is null;
            if (refNode is null || refNode.NextSibling is null || node.ParentNode is null || !ReferenceEquals(node.ParentNode, refNode.ParentNode))
                return false;
            do
            {
                if (ReferenceEquals(refNode, node))
                    return true;
            }
            while (null != (node = node.NextSibling));
            return false;
        }

        public static bool Contains(this XmlNode node, XmlNode refNode, bool recursive = false)
        {
            if (node is null)
                return refNode is null;
            if (refNode is null || !ReferenceEquals(node.OwnerDocument, refNode.OwnerDocument))
                return false;
            if (!recursive)
                return null != (refNode = refNode.ParentNode) && ReferenceEquals(node, refNode);
            while (null != (refNode = refNode.ParentNode))
            {
                if (ReferenceEquals(node, refNode))
                    return true;
            }
            return false;
        }

        public static bool ContainedBy(this XmlNode node, XmlNode refNode, bool recursive = false)
        {
            if (refNode is null)
                return node is null;
            if (node is null || !ReferenceEquals(refNode.OwnerDocument, node.OwnerDocument))
                return false;
            if (!recursive)
                return null != (node = node.ParentNode) && ReferenceEquals(refNode, node);
            while (null != (node = node.ParentNode))
            {
                if (ReferenceEquals(refNode, node))
                    return true;
            }
            return false;
        }

        public static bool IsEqualTo(this XmlNode node, XmlQualifiedName qName)
        {
            if (node is null)
                return qName is null;
            return !(qName is null || qName.IsEmpty) && node.LocalName.Equals(qName.Name) && node.NamespaceURI.Equals(qName.Namespace);
        }

        public static bool IsEqualTo(this XmlNode node, string localName, string namespaceURI)
        {
            if (node is null)
                return localName is null && namespaceURI is null;
            return node.LocalName.Equals(localName) && node.NamespaceURI.Equals(namespaceURI ?? "");
        }

        public static bool IsEqualTo(this XmlQualifiedName qName, string localName, string namespaceURI)
        {
            if (localName is null)
                return namespaceURI is null && qName is null;
            if (localName.Length == 0)
                return string.IsNullOrEmpty(namespaceURI) && qName.IsEmpty;
            return !qName.IsEmpty && localName == qName.Name && ((string.IsNullOrEmpty(namespaceURI)) ? string.IsNullOrWhiteSpace(qName.Namespace) : namespaceURI.Equals(qName.Namespace));
        }

        public static string AsValidNCName(this string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;
            if (XmlConvert.IsStartNCNameChar(name[0]))
            {
                if (name.Length == 1)
                    return name;
                return name.Substring(0, 1) + string.Join("_", name.Substring(1).Split('_').Select(s => (s.Length > 0) ? XmlConvert.EncodeLocalName(s) : s));
            }
            if (name.Length == 1)
                return XmlConvert.EncodeLocalName(name.Substring(0, 1));
            return XmlConvert.EncodeLocalName(name.Substring(0, 1)) + string.Join("_", name.Substring(1).Split('_').Select(s => (s.Length > 0) ? XmlConvert.EncodeLocalName(s) : s));
        }

        public static bool IsValidXmlNCName(this string value)
        {
            if (value is null)
                return false;
            using (IEnumerator<char> enumerator = value.GetEnumerator())
            {
                if (!enumerator.MoveNext() || !XmlConvert.IsStartNCNameChar(enumerator.Current))
                    return false;
                while (enumerator.MoveNext())
                {
                    if (!XmlConvert.IsStartNCNameChar(enumerator.Current))
                        return false;
                }
            }
            return true;
        }

        public static bool IsValidXmlName(this string name)
        {
            if (name is null)
                return false;
            using (IEnumerator<char> enumerator = name.GetEnumerator())
            {
                if (!enumerator.MoveNext() || !XmlConvert.IsStartNCNameChar(enumerator.Current))
                    return false;
                while (enumerator.MoveNext())
                {
                    char c = enumerator.Current;
                    if (c == ':')
                    {
                        if (!enumerator.MoveNext() || !XmlConvert.IsStartNCNameChar(c = enumerator.Current))
                            return false;
                        while (enumerator.MoveNext())
                        {
                            if (!XmlConvert.IsStartNCNameChar(enumerator.Current))
                                return false;
                        }
                        return true;
                    }
                    if (!XmlConvert.IsNCNameChar(c))
                        return false;
                }
            }
            return true;
        }

        public static bool TryGetPrefixAndLocalName(this string cName, out string prefix, out string localName)
        {
            if (!string.IsNullOrEmpty(cName))
            {
                int index = cName.IndexOf(':');
                if (index < 0)
                {
                    prefix = "";
                    localName = cName;
                }
                else
                {
                    prefix = cName.Substring(0, index);
                    if ((localName = cName.Substring(index + 1)).Length == 0 || index == 0 || !XmlConvert.IsStartNCNameChar(prefix[0]))
                        return false;
                    for (int i = 1; i < prefix.Length; i++)
                    {
                        if (!XmlConvert.IsNCNameChar(prefix[i]))
                            return false;
                    }
                }
                if (XmlConvert.IsStartNCNameChar(localName[0]))
                {
                    for (int i = 1; i < localName.Length; i++)
                    {
                        if (!XmlConvert.IsNCNameChar(localName[i]))
                            return false;
                    }
                    return true;
                }
            }
            else
                prefix = localName = cName;
            return false;
        }

        public static bool IsValidXmlNsUri(this string value) => null != value && (value.Length == 0 || Uri.IsWellFormedUriString(value, UriKind.Absolute));

        public static bool TryGetComponents(this XmlQualifiedName qName, out string localName, out string namespaceURI)
        {
            if (!(qName is null || qName.IsEmpty))
            {
                namespaceURI = qName.Namespace ?? "";
                return (localName = qName.Name).IsValidXmlNCName() && namespaceURI.IsValidXmlNsUri();
            }
            else
                localName = namespaceURI = null;
            return false;
        }

        public static XmlNode EnsureContiguous(IEnumerable<XmlNode> sequential)
        {
            if (sequential is null)
                return null;
            XmlNode firstNode = (sequential = sequential.Where(n => null != n)).FirstOrDefault(n => null != n.ParentNode);
            if (firstNode is null)
                throw new InvalidOperationException();
            XmlNode parentNode = firstNode;
            XmlDocument ownerDocument = firstNode.OwnerDocument;
            XmlNode[] preceding = sequential.TakeWhile(n => n.ParentNode is  null).Reverse().ToArray();
            XmlNode[] following = sequential.SkipWhile(n => n.ParentNode is null).Skip(1).ToArray();
            XmlNode refNode = firstNode;
            foreach (XmlNode node in following)
            {
                if (node.Precedes(refNode))
                    refNode = node;
            }
            if (!ReferenceEquals(refNode, firstNode))
            {
                parentNode.InsertBefore(parentNode.RemoveChild(firstNode), refNode);
                refNode = firstNode;
            }
            foreach (XmlNode node in preceding)
            {
                if (node.ParentNode is null)
                    firstNode = parentNode.InsertBefore((ReferenceEquals(ownerDocument, node.OwnerDocument)) ? node :
                        ownerDocument.ImportNode(node, true), firstNode);
            }
            foreach (XmlNode node in following)
            {
                if (ReferenceEquals(node, refNode) || node.Precedes(refNode))
                    continue;
                if (ReferenceEquals(refNode.NextSibling, node))
                    refNode = node;
                else if (ReferenceEquals(node.OwnerDocument, ownerDocument))
                {
                    if (ReferenceEquals(node.ParentNode, parentNode))
                        refNode = parentNode.InsertAfter(parentNode.RemoveChild(node), refNode);
                    else
                        refNode = parentNode.InsertAfter((node.ParentNode is null) ? node : node.CloneNode(true), refNode);
                }
                else
                    refNode = parentNode.InsertAfter(ownerDocument.ImportNode(node, true), refNode);
            }
            return firstNode;
        }

        public static XmlElement EnsureChildElementInSequence(this XmlElement parent, string localName, string namespaceURI,
            IEnumerable<XmlQualifiedName> sequence)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            if (localName is null)
                throw new ArgumentNullException(nameof(localName));

            if (localName.Length == 0)
                throw new ArgumentException($"'{nameof(localName)}' cannot be empty.", nameof(localName));

            if (namespaceURI is null)
                namespaceURI = "";
            XmlElement element;
            if (parent.HasChildNodes && parent.ChildNodes.OfType<XmlElement>().Any())
            {
                XmlElement refChild = (sequence = sequence.Where(qn => null != qn && !qn.IsEmpty).Distinct(QualifiedNameComparer.Default))
                    .TakeWhile(qn => !qn.IsEqualTo(localName, namespaceURI))
                    .Select(qn => parent.ChildNodes.OfType<XmlElement>()
                    .LastOrDefault(e => e.IsEqualTo(qn))).FirstOrDefault(e => null != e);
                bool refIsFollowing = refChild is null;
                if (refIsFollowing)
                    refChild = sequence.SkipWhile(qn => !qn.IsEqualTo(localName, namespaceURI)).Skip(1).Select(qn => parent.ChildNodes.OfType<XmlElement>()
                    .FirstOrDefault(e => e.IsEqualTo(qn))).FirstOrDefault(e => null != e);
                XmlElement[] existing = parent.ChildNodes.OfType<XmlElement>().Where(e => e.IsEqualTo(localName, namespaceURI)).ToArray();
                if (existing.Length > 0)
                {
                    if (refIsFollowing)
                    {
                        if (!(refChild is null || ReferenceEquals(refChild.PreviousSibling, existing[0])))
                            parent.InsertBefore(parent.RemoveChild(existing[0]), refChild);
                    }
                    else if (!ReferenceEquals(refChild.NextSibling, existing[0]))
                        parent.InsertAfter(parent.RemoveChild(existing[0]), refChild);
                    if (existing.Length > 1)
                        EnsureContiguous(existing);
                    return existing[0];
                }
                else
                {
                    element = parent.OwnerDocument.CreateElement(localName, namespaceURI);
                    if (refIsFollowing)
                        parent.InsertBefore(element, (refChild is null) ? parent.FirstChild : refChild);
                    else
                        parent.InsertAfter(element, refChild);
                }
            }
            else
            {
                element = parent.OwnerDocument.CreateElement(localName, namespaceURI);
                XmlNode refChild = parent.FirstChild;
                if (refChild is null)
                    parent.AppendChild(element);
                else if (refChild.NextSibling is null && refChild is XmlCharacterData && refChild.InnerText.Trim().Length == 0)
                {
                    parent.RemoveChild(refChild);
                    parent.AppendChild(element);
                }
                else
                    parent.InsertBefore(element, refChild);
            }
            return element;
        }

        public static XmlElement EnsureChildElementInSequence(this XmlElement parent, string localName, IEnumerable<XmlQualifiedName> sequence)
            => EnsureChildElementInSequence(parent, localName, parent.NamespaceURI, sequence);

        public static IEnumerable<XmlElement> EnsureChildElementSequence(this XmlElement parent, IEnumerable<XmlQualifiedName> sequence,
            out IEnumerable<XmlElement> otherNamedElements)
        {
            if (parent.IsEmpty)
            {
                otherNamedElements = new XmlElement[0];
                return new XmlElement[0];
            }
            IEnumerable<XmlElement> childElements = parent.ChildNodes.OfType<XmlElement>();
            if (childElements.Any() || !(sequence = sequence.Where(qn => !(qn is null || qn.IsEmpty)).Distinct(QualifiedNameComparer.Default)).Any())
            {
                otherNamedElements = childElements;
                return new XmlElement[0];
            }
            IEnumerable<XmlNode> childNodes = parent.ChildNodes.Cast<XmlNode>().Where(n => !((n is XmlElement e) && sequence.Any(qn => e.IsEqualTo(qn))));
            otherNamedElements = childNodes.OfType<XmlElement>();
            childElements = sequence.SelectMany(qn => childElements.Where(e => e.IsEqualTo(qn)));
            EnsureContiguous(childElements.Concat(otherNamedElements));
            return childElements;
        }

        private sealed class QualifiedNameComparer : IEqualityComparer<XmlQualifiedName>
        {
            public bool Equals(XmlQualifiedName x, XmlQualifiedName y) => (x is null) ? y is null : null != y &&
                (ReferenceEquals(x, y) || ((x.IsEmpty) ? y.IsEmpty : !y.IsEmpty && x.Name.Equals(y.Name) && (x.Namespace ?? "").Equals(y.Namespace ?? "")));
            public int GetHashCode(XmlQualifiedName obj) => (obj is null) ? 0 : HashCode.Combine(obj.Name, obj.Namespace);
            public static readonly QualifiedNameComparer Default = new QualifiedNameComparer();
        }
        private sealed class EnumerableWrapper : IEnumerable<XmlNode>
        {
            private readonly Func<IEnumerator<XmlNode>> _getEnumerator;
            internal EnumerableWrapper(Func<IEnumerator<XmlNode>> getEnumerator) { _getEnumerator = getEnumerator; }
            public IEnumerator<XmlNode> GetEnumerator() => _getEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _getEnumerator();
        }
        private abstract class NodeEnumerator : IEnumerator<XmlNode>
        {
            private readonly bool _includeCurrent;
            private bool _currentEmitted = false;

            private readonly XmlDocument _ownerDocument;
            private XmlNode _current;
            private bool _isDisposed;
            private bool _isModified;

            protected XmlNode Target { get; }

            public XmlNode Current
            {
                get
                {
                    Monitor.Enter(this);
                    try
                    {
                        if (_isDisposed)
                            throw new ObjectDisposedException(GetType().FullName);
                        if (_current is null || ReferenceEquals(_current, Target))
                            throw new InvalidOperationException();
                        return _current;
                    }
                    finally { Monitor.Exit(this); }
                }
            }

            object System.Collections.IEnumerator.Current => Current;

            protected NodeEnumerator(XmlNode target, bool includeCurrent)
            {
                _currentEmitted = !(_includeCurrent = includeCurrent);
                _current = Target = target;
                if (target is null)
                    _ownerDocument = null;
                else
                    (_ownerDocument = target.OwnerDocument).NodeRemoved += RaiseNodeRemoved;
            }

            private void RaiseNodeRemoved(object sender, XmlNodeChangedEventArgs e)
            {
                Monitor.Enter(this);
                try
                {
                    if (!(_current is null || _ownerDocument is null || _isModified) && OnNodeRemoved(e.Node, e.OldParent))
                        _isModified = true;
                }
                finally { Monitor.Exit(this); }
            }

            protected virtual bool OnNodeRemoved(XmlNode node, XmlNode oldParent) => ReferenceEquals(node, _current);

            public bool MoveNext()
            {
                Monitor.Enter(this);
                try
                {
                    if (_isDisposed)
                        throw new ObjectDisposedException(GetType().FullName);
                    if (_current is null)
                        return false;
                    if (_isModified)
                        throw new InvalidOperationException();
                    if (_currentEmitted)
                        return !((_current = GetNext(_current)) is null);
                    _currentEmitted = true;
                }
                finally { Monitor.Exit(this); }
                return true;
            }

            protected abstract XmlNode GetNext(XmlNode current);

            public virtual void Reset()
            {
                Monitor.Enter(this);
                try
                {
                    OnReset();
                    _current = Target;
                    _currentEmitted = !_includeCurrent;
                }
                finally { Monitor.Exit(this); }
            }

            protected virtual void OnReset()
            {
                if (_isDisposed)
                    throw new ObjectDisposedException(GetType().FullName);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!_isDisposed)
                {
                    _isDisposed = true;
                    if (disposing && !(_ownerDocument is null))
                        OnDisposing(_ownerDocument);
                }
            }

            protected virtual void OnDisposing(XmlDocument ownerDocument) => ownerDocument.NodeRemoved -= RaiseNodeRemoved;

            public void Dispose()
            {
                Dispose(disposing: true);
                GC.SuppressFinalize(this);
            }
        }

        private sealed class DescendantEnumerator : NodeEnumerator
        {
            private readonly Stack<XmlNode> _next = new Stack<XmlNode>();
            private readonly int _maxDepth;
            private int _currentDepth = 0;

            public DescendantEnumerator(XmlNode node, bool includeCurrent, int maxDepth) : base(node, includeCurrent)
            {
                _maxDepth = (maxDepth < 0) ? -1 : maxDepth;
            }

            protected override bool OnNodeRemoved(XmlNode node, XmlNode oldParent) => base.OnNodeRemoved(node, oldParent) ||
                (Target.Contains(oldParent) && (oldParent.Contains(Current) ||
                    _next.Any(n => ReferenceEquals(node, n) || oldParent.Contains(n))));

            protected override XmlNode GetNext(XmlNode current)
            {
                XmlNode next = current.NextSibling;
                if (_currentDepth == _maxDepth)
                    return next;
                if ((current = current.FirstChild) is null)
                {
                    if (next is null)
                    {
                        if (!_next.TryPop(out current))
                            return null;
                        _currentDepth--;
                    }
                    else
                        return next;
                }
                else
                {
                    _currentDepth++;
                    if (!(next is null))
                        _next.Push(next);
                }
                return current;
            }

            protected override void OnReset()
            {
                base.OnReset();
                _next.Clear();
            }
        }

        private sealed class AncestorEnumerator : NodeEnumerator
        {
            private readonly bool _excludeDocument;

            public AncestorEnumerator(XmlNode target, bool includeCurrent, bool excludeDocument) : base(target, includeCurrent) =>
                _excludeDocument = excludeDocument;

            protected override bool OnNodeRemoved(XmlNode node, XmlNode oldParent) => base.OnNodeRemoved(node, oldParent) ||
                !Current.Contains(Target);

            protected override XmlNode GetNext(XmlNode current) => (_excludeDocument) ?
                (((current = current.ParentNode) is XmlDocument) ? null : current) :
                current.ParentNode;
        }

        private sealed class PrecedingEnumerator : NodeEnumerator
        {
            internal PrecedingEnumerator(XmlNode target, bool includeCurrent) : base(target, includeCurrent) { }
            protected override XmlNode GetNext(XmlNode current) => current.PreviousSibling;
        }

        private sealed class FollowingEnumerator : NodeEnumerator
        {
            internal FollowingEnumerator(XmlNode target, bool includeCurrent) : base(target, includeCurrent) { }
            protected override XmlNode GetNext(XmlNode current) => current.NextSibling;
        }
    }
}
