
# https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/get-started/syntax-analysis
# https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.syntaxtree?view=roslyn-dotnet-4.1.0
Add-Type -AssemblyName 'Microsoft.CodeAnalysis' -ErrorAction Stop;
# https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.csharpsyntaxtree?view=roslyn-dotnet-4.1.0
Add-Type -AssemblyName 'Microsoft.CodeAnalysis.CSharp' -ErrorAction Stop;

Add-Type -TypeDefinition @'
namespace CodeAnalysisUtil
{
    using Microsoft.CodeAnalysis.CSharp;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Text.RegularExpressions;
    public class TypeFilter
    {
        private Collection<Type> _baseTypes = new Collection<Type>();
        public Collection<Type> BaseTypes { get { return _baseTypes; } }
        public TypeFilter(params Type[] types)
        {
            if (types != null)
                foreach (Type t in types)
                    _baseTypes.Add(t);
        }
        public bool TestType(Type type)
        {
            foreach (Type t in _baseTypes)
                if (t.IsAssignableFrom(type) && !t.Equals(type)) return true;
            return false;
        }
        public bool TestObject(object obj)
        {
            if (obj is null) return false;
            foreach (Type t in _baseTypes)
                if (t.IsInstanceOfType(obj)) return true;
            return false;
        }
        public IEnumerable<T> FilterObjects<T>(IEnumerable<T> values)
        {
            foreach (T t in values)
                foreach (Type b in _baseTypes)
                    if (b.IsInstanceOfType(t)) yield return t;
        }
        public IEnumerable<T> FilterObjects<T>(params T[] values)
        {
            foreach (T t in values)
                foreach (Type b in _baseTypes)
                    if (b.IsInstanceOfType(t)) yield return t;
        }
        public IEnumerable<Type> FilterTypes(IEnumerable<Type> types)
        {
            foreach (Type t in types)
                foreach (Type b in _baseTypes)
                    if (b.IsAssignableFrom(t) && !t.Equals(b)) yield return t;
        }
        public IEnumerable<Type> FilterTypes(params Type[] types)
        {
            foreach (Type t in types)
                foreach (Type b in _baseTypes)
                    if (b.IsAssignableFrom(t) && !t.Equals(b)) yield return t;
        }
    }
    public class SourceFile
    {
        private readonly FileInfo _file;
        private CSharpSyntaxTree _syntaxTree;
        private string _code;
        public FileInfo File { get { return _file; } }
        public SourceFile(FileInfo file)
        {
            if (file is null) throw new ArgumentNullException("file");
            _file = file;
        }
        public CSharpSyntaxTree GetSyntaxTree()
        {
            if (_syntaxTree != null) return _syntaxTree;
            if (_code == null)
                using (StreamReader reader = _file.OpenText())
                    _code = reader.ReadToEnd();
            if (string.IsNullOrWhiteSpace(_code)) throw new InvalidOperationException("Code is empty");
            _syntaxTree = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(_code, CSharpParseOptions.Default, _file.FullName);
            if (_syntaxTree is null) throw new InvalidOperationException("Unable to parse source code");
            _code = null;
            return _syntaxTree;
        }
        public string GetCode()
        {
            if (_syntaxTree != null) return _syntaxTree.ToString();
            if (_code == null)
                using (StreamReader reader = _file.OpenText())
                    _code = reader.ReadToEnd();
            return _code;
        }
        public void Reload()
        {
            _file.Refresh();
            _code = null;
            _syntaxTree = null;
        }
    }
    public struct LineLengthInfo : IEquatable<LineLengthInfo>, IComparable<LineLengthInfo>, IComparable
    {
        private readonly int _number;
        private readonly int _length;
        public int Number { get { return _number; } }
        public int Length { get { return _length; } }
        public LineLengthInfo(int number, int length)
        {
            _number = number;
            _length = length;
        }
        public int CompareTo(LineLengthInfo other)
        {
            int result = other._number - _number;
            return (result == 0) ? other._length - _length : result;
        }
        int IComparable.CompareTo(object obj) { return (obj is LineLengthInfo) ? CompareTo((LineLengthInfo)obj) : -1; }
        public bool Equals(LineLengthInfo other) { return _number == other._number && _length == other._length; }
        public override bool Equals(object obj) { return obj is LineLengthInfo && Equals((LineLengthInfo)obj); }
        public override int GetHashCode() { return _number; }
    }
    public class LineComplianceInfo
    {
        public static readonly Regex LineBreakRegex = new Regex(@"\r\n?|\n", RegexOptions.Compiled);
        private readonly int _maxLineLength;
        private SourceFile _file;
        private readonly Collection<LineLengthInfo> _backingCollection = new Collection<LineLengthInfo>();
        private readonly ReadOnlyCollection<LineLengthInfo> _violations;
        public SourceFile File { get { return _file; } }
        public int MaxLineLength { get { return _maxLineLength; } }
        public ReadOnlyCollection<LineLengthInfo> Violations { get { return _violations; } }
        public LineComplianceInfo(SourceFile file) : this(file, 180) { }
        public LineComplianceInfo(SourceFile file, int maxLength)
        {
            if ((_file = file) is null) throw new ArgumentNullException("file");
            _maxLineLength = maxLength;
            int lineNumber = 0;
            foreach (string line in LineBreakRegex.Split(file.GetCode()))
            {
                lineNumber++;
                if (line.Length > maxLength)
                    _backingCollection.Add(new LineLengthInfo(lineNumber, line.Length));
            }
            _violations = new ReadOnlyCollection<LineLengthInfo>(_backingCollection);
        }
        public void Reload()
        {
            _backingCollection.Clear();
            _file.Reload();
            int lineNumber = 0;
            foreach (string line in LineBreakRegex.Split(_file.GetCode()))
            {
                lineNumber++;
                if (line.Length > _maxLineLength)
                    _backingCollection.Add(new LineLengthInfo(lineNumber, line.Length));
            }
        }
    }
}
'@ -ReferencedAssemblies 'System.Runtime', 'Microsoft.CodeAnalysis', 'Microsoft.CodeAnalysis.CSharp', 'System.Text.RegularExpressions' -IgnoreWarnings -WarningAction Ignore -ErrorAction Stop;
