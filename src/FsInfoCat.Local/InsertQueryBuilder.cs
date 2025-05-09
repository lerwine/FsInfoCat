using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    // TODO: Document InsertQueryBuilder class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class InsertQueryBuilder
    {
        private bool _finalized;
        private readonly StringBuilder _sql = new();
        private readonly ArrayList _values = [];
        private XElement _source { get; }

        public InsertQueryBuilder(string tableName, XElement source, string identity, params string[] compoundIdentity)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"'{nameof(tableName)}' cannot be null or whitespace.", nameof(tableName));
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException($"'{nameof(identity)}' cannot be null or whitespace.", nameof(identity));
            if (compoundIdentity is null || compoundIdentity.Any(t => t is null))
                throw new ArgumentNullException(nameof(compoundIdentity));

            _source = source ?? throw new ArgumentNullException(nameof(source));
            _ = _values.Add(source.GetAttributeGuid(identity) ?? throw new ArgumentOutOfRangeException(nameof(identity)));
            _sql = new StringBuilder("INSERT INTO \"").Append(tableName).Append("\", (\"").Append(identity).Append('"');
            foreach (string t in compoundIdentity)
            {
                _ = _values.Add(source.GetAttributeGuid(t) ?? throw new ArgumentOutOfRangeException(nameof(compoundIdentity), $"Attribute named \"{t}\" not found"));
                _ = _sql.Append(", \"").Append(t).Append('"');
            }
        }

        public InsertQueryBuilder AppendBoolean(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeBoolean(name, out bool? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendString(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeValue(name, out string value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendElementString(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            XElement element = _source.Element(name);
            if (!(element is null || element.IsEmpty))
            {
                _ = _values.Add(element.Value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendInnerText(string colName)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(colName))
                throw new ArgumentException($"'{nameof(colName)}' cannot be null or whitespace.", nameof(colName));
            if (!_source.IsEmpty)
            {
                _ = _values.Add((_source.Value.Trim().Length > 0) ? _source.Value : "");
                _ = _sql.Append(", \"").Append(colName).Append('"');
            }
            return this;
        }

        public InsertQueryBuilder AppendGuid(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeGuid(name, out Guid? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendGuid(string name, Guid? value)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            _ = _values.Add(value);
            _ = _sql.Append(", \"").Append(name).Append('"');
            return this;
        }

        public InsertQueryBuilder AppendInt16(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeInt16(name, out short? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendInt32(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeInt32(name, out int? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendInt64(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeInt64(name, out long? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendEnum<T>(string name, bool required = false) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeEnumValue(name, out T? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendDateTime(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeDateTime(name, out DateTime? value))
            {
                _ = _values.Add(value);
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendBinary(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeValue(name, out string result))
            {
                _ = _values.Add(ByteArrayCoersion.Parse(result));
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendMd5Hash(string name, bool required = false)
        {
            if (_finalized)
                throw new InvalidOperationException();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeValue(name, out string result))
            {
                _ = _values.Add(FsInfoCat.Model.MD5Hash.Parse(result).GetBuffer());
                _ = _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        internal async Task<int> ExecuteSqlAsync(DatabaseFacade database)
        {
            if (!_finalized)
            {
                _finalized = true;
                _ = _sql.Append(") Values({0}");

                for (int i = 1; i < _values.Count; i++)
                {
                    if (_values[i] is null)
                    {
                        _ = _sql.Append(", NULL");
                        _values.RemoveAt(i--);
                    }
                    else
                        _ = _sql.Append(", {").Append(i).Append('}');
                }
                _ = _sql.Append(')');
            }

            return await database.ExecuteSqlRawAsync(_sql.ToString(), _values.ToArray());
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
