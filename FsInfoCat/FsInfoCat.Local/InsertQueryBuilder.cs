using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public class InsertQueryBuilder
    {
        private StringBuilder _sql = new();
        private ArrayList _values = new();

        public XElement _source { get; }

        public InsertQueryBuilder(string tableName, XElement source, string identity, params string[] compoundIdentity)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"'{nameof(tableName)}' cannot be null or whitespace.", nameof(tableName));
            if (string.IsNullOrWhiteSpace(identity))
                throw new ArgumentException($"'{nameof(identity)}' cannot be null or whitespace.", nameof(identity));
            if (compoundIdentity is null || compoundIdentity.Any(t => t is null))
                throw new ArgumentNullException(nameof(compoundIdentity));

            _source = source ?? throw new ArgumentNullException(nameof(source));
            _values.Add(source.GetAttributeGuid(identity) ?? throw new ArgumentOutOfRangeException(nameof(identity)));
            _sql = new StringBuilder("INSERT INTO \"").Append(tableName).Append("\", (\"").Append(identity).Append('"');
            foreach (string t in compoundIdentity)
            {
                _values.Add(source.GetAttributeGuid(t) ?? throw new ArgumentOutOfRangeException(nameof(compoundIdentity), $"Attribute named \"{t}\" not found"));
                _sql.Append(", \"").Append(t).Append('"');
            }
        }

        public InsertQueryBuilder AppendBoolean(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeBoolean(name, out bool? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendString(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeValue(name, out string value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendGuid(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeGuid(name, out Guid? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendInt16(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeInt16(name, out short? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendInt32(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeInt32(name, out int? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendInt64(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeInt64(name, out long? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendEnum<T>(string name, bool required = false) where T : struct, IComparable, IConvertible, IFormattable
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeEnumValue(name, out T? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendDateTime(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            if (_source.TryGetAttributeDateTime(name, out DateTime? value))
            {
                _values.Add(value);
                _sql.Append(", \"").Append(name).Append('"');
            }
            else if (required)
                throw new ArgumentOutOfRangeException(nameof(name));
            return this;
        }

        public InsertQueryBuilder AppendBinary(string name, bool required = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            throw new NotImplementedException();
        }

        public InsertQueryBuilder AppendInnerText(string colName)
        {
            if (string.IsNullOrWhiteSpace(colName))
                throw new ArgumentException($"'{nameof(colName)}' cannot be null or whitespace.", nameof(colName));
            throw new NotImplementedException();
        }
    }
}
