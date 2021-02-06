using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Util
{
    public class AnyUriBuilder
    {
        private class SchemePropertyRestriction
        {
            public bool HasValue(AnyUriBuilder builder) => _hasValue(builder);
            internal SchemePropertyRestriction(string name, Predicate<AnyUriBuilder> hasValue)
            {
                Name = name;
                _hasValue = hasValue;
            }
            internal string Name { get; }

            private readonly Predicate<AnyUriBuilder> _hasValue;
        }
        private static readonly StringComparer _caseInsensitiveComparer = StringComparer.InvariantCultureIgnoreCase;
        private static readonly ReadOnlyDictionary<string, ReadOnlyCollection<SchemePropertyRestriction>> _schemeRestrictionMap;
        static AnyUriBuilder()
        {
            Dictionary<string, ReadOnlyCollection<SchemePropertyRestriction>> schemeRestrictionMap = new Dictionary<string, ReadOnlyCollection<SchemePropertyRestriction>>();
            schemeRestrictionMap.Add(Uri.UriSchemeFile, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
             {
                new SchemePropertyRestriction(nameof(UserName), b => !string.IsNullOrEmpty(b.UserName)),
                new SchemePropertyRestriction(nameof(Password), b => !string.IsNullOrEmpty(b.Password)),
                new SchemePropertyRestriction(nameof(Port), b => b.Port.HasValue)
             }));
            schemeRestrictionMap.Add(Uri.UriSchemeNetPipe, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
            {
                new SchemePropertyRestriction(nameof(UserName), b => !string.IsNullOrEmpty(b.UserName)),
                new SchemePropertyRestriction(nameof(Password), b => !string.IsNullOrEmpty(b.Password))
            }));
            schemeRestrictionMap.Add(Uri.UriSchemeNetTcp, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
            {
                new SchemePropertyRestriction(nameof(UserName), b => !string.IsNullOrEmpty(b.UserName)),
                new SchemePropertyRestriction(nameof(Password), b => !string.IsNullOrEmpty(b.Password))
            }));
            schemeRestrictionMap.Add(Uri.UriSchemeFtp, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
            {
                new SchemePropertyRestriction(nameof(Query), b => !string.IsNullOrEmpty(b.Query))
            }));
            schemeRestrictionMap.Add(Uri.UriSchemeGopher, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
            {
                new SchemePropertyRestriction(nameof(Query), b => !string.IsNullOrEmpty(b.Query))
            }));
            schemeRestrictionMap.Add(Uri.UriSchemeNntp, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
            {
                new SchemePropertyRestriction(nameof(Query), b => !string.IsNullOrEmpty(b.Query))
            }));
            schemeRestrictionMap.Add(Uri.UriSchemeNews, new ReadOnlyCollection<SchemePropertyRestriction>(new SchemePropertyRestriction[]
            {
                new SchemePropertyRestriction(nameof(Query), b => !string.IsNullOrEmpty(b.Query)),
                new SchemePropertyRestriction(nameof(UserName), b => !string.IsNullOrEmpty(b.UserName)),
                new SchemePropertyRestriction(nameof(Password), b => !string.IsNullOrEmpty(b.Password))
            }));
            _schemeRestrictionMap = new ReadOnlyDictionary<string, ReadOnlyCollection<SchemePropertyRestriction>>(schemeRestrictionMap);
        }
        private object _syncRoot = new object();
        private UriBuilder _builder = null;
        private string _scheme = "";
        private string _userName = null;
        private string _password = null;
        private string _host = "";
        private int? _port = null;
        private string _path = "";
        private string _query = null;
        private string _fragment = null;
        public Uri Uri { get; private set; }

        public string Scheme
        {
            get => _scheme;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    string s = value ?? "";
                    if ((s = s.ToLower()) == _scheme)
                        return;
                    if (s.Length > 0 && !Uri.CheckSchemeName(s))
                        throw new ArgumentOutOfRangeException(nameof(value), "Invalid scheme name");

                    if (_schemeRestrictionMap.ContainsKey(s))
                    {
                        string[] nonEmpty = _schemeRestrictionMap[s].Where(i => i.HasValue(this)).Select(i => i.Name).ToArray();
                        if (nonEmpty.Length > 1)
                            throw new InvalidOperationException($"{s} schema cannot be applied when user {string.Join(",", nonEmpty.Skip(1))} or {nonEmpty[0]} is not empty");
                    }
                    if (s.Length > 0)
                    {
                        if (Uri.IsAbsoluteUri)
                            try
                            {
                                UriBuilder uriBuilder = new UriBuilder(Uri);
                                uriBuilder.Scheme = s;
                                Uri = uriBuilder.Uri;
                                _scheme = uriBuilder.Scheme;
                                return;
                            }
                            catch (Exception exc)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), (string.IsNullOrWhiteSpace(exc.Message)) ?
                                    $"{exc.GetType().Name} occurred while trying to change scheme" :
                                    $"Error while trying changing scheme: {exc.Message}");
                            }
                        else
                            TryChangeToAbsolute(s, _host, _port);
                    }
                    else
                        UpdateRelativeUri();
                    _scheme = value;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public string UserName
        {
            get => _userName;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (value == _userName)
                        return;
                    if (!string.IsNullOrEmpty(value))
                    {
                        AssertSchemeComponentSupport(nameof(UserName));
                        value = value.AsUserNameComponentEncoded();
                    }
                    if (Uri.IsAbsoluteUri && _host.Length > 0)
                    {
                        if (null == value)
                        {
                            if (Uri.TrySetUserInfoComponent(null, null, out Uri result))
                                Uri = result;
                        }
                        else if ((value.Length > 0 || !string.IsNullOrEmpty(Password)) && Uri.TrySetUserInfoComponent(value, _password, out Uri result))
                        {
                            Uri = result;
                            _userName = result.GetUserNameAndPassword(out string pw);
                            _password = pw;
                            return;
                        }
                    }
                    _userName = value;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (value == _password)
                        return;
                    if (!string.IsNullOrEmpty(value))
                    {
                        AssertSchemeComponentSupport(nameof(Password));
                        value = value.AsPasswordComponentEncoded();
                    }
                    if (Uri.IsAbsoluteUri && _host.Length > 0 && null != _userName)
                    {
                        if (null == value)
                        {
                            if (null == _password)
                                return;
                            if (string.IsNullOrEmpty(_userName) && Uri.TrySetUserInfoComponent(null, null, out Uri result))
                                Uri = result;
                        }
                        else
                        {
                            if ((!string.IsNullOrEmpty(_userName) || value.Length > 0 || _password.Length > 0) && Uri.TrySetUserInfoComponent(_userName, _userName, out Uri result))
                            {
                                Uri = result;
                                _userName = result.GetUserNameAndPassword(out string pw);
                                _password = pw;
                                return;
                            }
                        }
                    }
                    _userName = value;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public string Host
        {
            get => _host;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    string h = value ?? "";
                    if ((h = h.ToLower()) == _scheme)
                        return;
                    if (h == _host)
                        return;
                    if (h.Length > 0)
                    {
                        if (Uri.CheckHostName(h) == UriHostNameType.Unknown)
                            throw new ArgumentOutOfRangeException(nameof(value), "Invalid host name");
                        AssertSchemeComponentSupport(nameof(Host));
                    }
                    if (Uri.IsAbsoluteUri)
                    {
                        if (h.Length == 0)
                        {
                            if (Uri.TrySetHostComponent(null, null, out Uri result))
                            {
                                Uri = result;
                                _port = null;
                            }
                        }
                        else
                            try
                            {
                                UriBuilder uriBuilder = new UriBuilder(Uri);
                                uriBuilder.Host = h;
                                Uri = uriBuilder.Uri;
                                _host = uriBuilder.Host;
                                return;
                            }
                            catch (Exception exc)
                            {
                                throw new ArgumentOutOfRangeException(nameof(value), (string.IsNullOrWhiteSpace(exc.Message)) ?
                                    $"{exc.GetType().Name} occurred while trying to change scheme" :
                                    $"Error while trying changing scheme: {exc.Message}");
                            }
                    }
                    else if (h.Length > 0 && TryChangeToAbsolute(_scheme, h, _port))
                    {
                        _host = Uri.Host;
                        return;
                    }
                    _host = h;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public int? Port
        {
            get => _port;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (value == _port)
                        return;
                    if (value.HasValue)
                    {
                        AssertSchemeComponentSupport(nameof(Port));
                        if (TryChangeToAbsolute(_scheme, _host, value))
                        {
                            if (Uri.IsDefaultPort)
                                _port = null;
                            else
                                _port = Uri.Port;
                            return;
                        }
                    }
                    else if (Uri.IsAbsoluteUri)
                    {
                        UriBuilder builder = new UriBuilder(Uri);
                        builder.Port = -1;
                        Uri = builder.Uri;
                    }
                    _port = value;
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public string Path
        {
            get => _path;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    string p = value ?? "";
                    if (p == _path)
                        return;
                    if (p.Length > 0)
                        AssertSchemeComponentSupport(nameof(Path));
                    if (Uri.TrySetPathComponent(p, out Uri result))
                    {
                        Uri = result;
                        _path = result.GetPathComponent();
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public string Query
        {
            get => _query;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (value == _query)
                        return;
                    if (!string.IsNullOrEmpty(value))
                        AssertSchemeComponentSupport(nameof(Query));
                    if (Uri.TrySetQueryComponent(value, out Uri result))
                    {
                        Uri = result;
                        _query = result.GetQueryComponent();
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        public string Fragment
        {
            get => _fragment;
            set
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (value == _fragment)
                        return;
                    if (!string.IsNullOrEmpty(value))
                        AssertSchemeComponentSupport(nameof(Fragment));
                    if (Uri.TrySetFragmentComponent(value, out Uri result))
                    {
                        Uri = result;
                        _fragment = result.GetFragmentComponent();
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        private void AssertSchemeComponentSupport(string name)
        {
            if (_schemeRestrictionMap.ContainsKey(_scheme) && _schemeRestrictionMap[_scheme].Any(i => i.Name.Equals(name)))
                throw new NotSupportedException($"The {_scheme} scheme does not support the {name} component");
        }

        public AnyUriBuilder(Uri uri)
        {
            if (uri is null)
                Uri = new Uri("", UriKind.Relative);
            else if (uri.IsAbsoluteUri)
                Uri = uri;
            else
            {
                string pq = uri.OriginalString;
                if (pq.Length == 0 || Uri.IsWellFormedUriString(pq, UriKind.Relative))
                    Uri = uri;
                else if (Uri.TryCreate(Uri.EscapeUriString(pq), UriKind.Relative, out Uri u))
                    Uri = u;
                else
                    Uri = new Uri(Uri.EscapeDataString(pq), UriKind.Relative);
            }
            ReinitializeFromUri();
        }

        public AnyUriBuilder(string uri)
        {
            if (string.IsNullOrEmpty(uri))
                Uri = new Uri("", UriKind.Relative);
            else if (Uri.TryCreate(uri, UriKind.Absolute, out Uri u))
                Uri = u;
            else if (Uri.IsWellFormedUriString(uri, UriKind.Relative))
                Uri = new Uri(uri, UriKind.Relative);
            else if (Uri.TryCreate(Uri.EscapeUriString(uri), UriKind.Relative, out u))
                Uri = u;
            else
                Uri = new Uri(Uri.EscapeDataString(uri), UriKind.Relative);
            ReinitializeFromUri();
        }

        private void ReinitializeFromUri()
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (Uri.IsAbsoluteUri)
                {
                    _scheme = Uri.Scheme;
                    _userName = Uri.GetUserNameAndPassword(out string pw);
                    _password = pw;
                    _host = Uri.Host;
                    if (Uri.IsDefaultPort)
                        _port = null;
                    else
                        _port = Uri.Port;
                }
                else
                {
                    _scheme = _host = "";
                    _userName = _password = null;
                    _port = null;
                }
                _path = Uri.GetPathComponent();
                _query = Uri.GetQueryComponent();
                _fragment = Uri.GetFragmentComponent();
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        private void UpdateRelativeUri()
        {
            if (null == _query)
            {
                if (null == _fragment)
                    Uri = new Uri(_path, UriKind.Relative);
                else
                    Uri = new Uri($"{_path}#{_fragment}");
            }
            else if (null == _fragment)
                Uri = new Uri($"{_path}?{_query}");
            else
                Uri = new Uri($"{_path}?{_query}#{_fragment}");
        }

        private bool TryChangeToAbsolute(string scheme, string host, int? port)
        {
            if (string.IsNullOrEmpty(scheme))
                return false;
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = scheme;
            try
            {
                if (!string.IsNullOrEmpty(host))
                    uriBuilder.Host = host;
                uriBuilder.Path = _path;
                Uri uri = uriBuilder.Uri;
                if (null == uri)
                    return false;
                if (port.HasValue)
                    uriBuilder.Port = port.Value;
                if (null == (uri = uriBuilder.Uri))
                    return false;
                Uri = uri;
                if (uri.TrySetQueryComponent(_query, out uri))
                    Uri = uri;
                if (uri.TrySetFragmentComponent(_fragment, out uri))
                    Uri = uri;
            }
            catch { return false; }
            _host = Uri.Host;
            if (Uri.IsDefaultPort)
                _port = null;
            else
                _port = Uri.Port;
            _userName = Uri.GetUserNameAndPassword(out string pw);
            _password = pw;
            _path = Uri.GetPathComponent();
            _query = Uri.GetQueryComponent();
            _fragment = Uri.GetFragmentComponent();
            return true;
        }
    }
}
