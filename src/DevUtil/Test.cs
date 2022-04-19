using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace DevUtil
{
    public class MyClassA1 : IEquatable<MyClassA1>
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public long? Key4 { get; set; }
        public int Key5 { get; set; }
        public int Key6 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }

        public override bool Equals(object obj) => Equals(obj as MyClassA1);

        public bool Equals(MyClassA1 other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3x = Key3;
            long? key3y = other.Key3;
            long? key4x = Key4;
            long? key4y = other.Key4;
            if (key3x.HasValue && key4x.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty)))
                return key1.Equals(other.Key1) &&
                    key2.Equals(other.Key2) &&
                    key3y.HasValue && key3x.Value.Equals(key3y.Value) &&
                    key4y.HasValue && key4x.Value.Equals(key4y.Value) &&
                    Key5.Equals(other.Key5) &&
                    Key6.Equals(other.Key6);
            return other.Key1.Equals(Guid.Empty) &&
                other.Key2.Equals(Guid.Empty) &&
                !(key3y.HasValue || key4y.HasValue) &&
                Value1 == other.Value1 &&
                EqualityComparer<DateTime?>.Default.Equals(Value2, other.Value2);
        }

        public override int GetHashCode()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            return (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty))) ? HashCode.Combine(Key1, Key2, Key3.Value, Key4.Value, Key5, Key6) : HashCode.Combine(Value1, Value2);
        }

        public int GetHashCode2()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            if (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty))) return HashCode.Combine(Key1, Key2, Key3.Value, Key4.Value, Key5, Key6);
            HashCode hashCode = new();
            hashCode.Add(Value1);
            hashCode.Add(Value2);
            return hashCode.ToHashCode();
        }

        public int GetHashCode3()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            if (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty)))
            {
                HashCode hashCode = new();
                hashCode.Add(Key1);
                hashCode.Add(Key2);
                hashCode.Add(Key3.Value);
                hashCode.Add(Key4.Value);
                hashCode.Add(Key5);
                hashCode.Add(Key6);
                return hashCode.ToHashCode();
            }
            return HashCode.Combine(Value1, Value2);
        }

        public int GetHashCode4()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            HashCode hashCode = new();
            if (key3.HasValue && key4.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty)))
            {
                hashCode.Add(Key1);
                hashCode.Add(Key2);
                hashCode.Add(Key3.Value);
                hashCode.Add(Key4.Value);
                hashCode.Add(Key5);
                hashCode.Add(Key6);
            }
            else
            {
                hashCode.Add(Value1);
                hashCode.Add(Value2);
            }
            return hashCode.ToHashCode();
        }
    }
    public class MyClassB1 : IEquatable<MyClassB1>
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public int Key5 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }

        public override bool Equals(object obj) => Equals(obj as MyClassB1);

        public bool Equals(MyClassB1 other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Guid key1 = Key1;
            long? key2x = Key2;
            long? key2y = other.Key2;
            long? key3x = Key3;
            long? key3y = other.Key3;
            if (key2x.HasValue && key3x.HasValue && !key1.Equals(Guid.Empty))
                return key1.Equals(other.Key1) &&
                    key2y.HasValue && key2x.Value.Equals(key2y.Value) &&
                    key3y.HasValue && key3x.Value.Equals(key3y.Value) &&
                    Key5.Equals(other.Key5);
            return other.Key1.Equals(Guid.Empty) &&
                !(key2y.HasValue || key3y.HasValue) &&
                Value1 == other.Value1 &&
                EqualityComparer<DateTime?>.Default.Equals(Value2, other.Value2);
        }

        public override int GetHashCode()
        {
            Guid key1 = Key1;
            long? key2 = Key2;
            long? key3 = Key3;
            return (key2.HasValue && key3.HasValue && !key1.Equals(Guid.Empty)) ? HashCode.Combine(Key1, Key2.Value, Key3.Value, Key4, Key5) : HashCode.Combine(Value1, Value2);
        }

        public int GetHashCode2()
        {
            Guid key1 = Key1;
            long? key2 = Key3;
            long? key3 = Key4;
            if (key2.HasValue && key3.HasValue && !(key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty))) return HashCode.Combine(Key1, Key2.Value, Key3.Value, Key4, Key5);
            HashCode hashCode = new();
            hashCode.Add(Value1);
            hashCode.Add(Value2);
            return hashCode.ToHashCode();
        }

        public int GetHashCode3()
        {
            Guid key1 = Key1;
            long? key2 = Key3;
            long? key3 = Key4;
            if (key2.HasValue && key3.HasValue && !key1.Equals(Guid.Empty))
            {
                HashCode hashCode = new();
                hashCode.Add(Key1);
                hashCode.Add(Key2.Value);
                hashCode.Add(Key3.Value);
                hashCode.Add(Key4);
                hashCode.Add(Key5);
                return hashCode.ToHashCode();
            }
            return HashCode.Combine(Value1, Value2);
        }

        public int GetHashCode4()
        {
            Guid key1 = Key1;
            long? key2 = Key3;
            long? key3 = Key4;
            HashCode hashCode = new();
            if (key2.HasValue && key3.HasValue && !key1.Equals(Guid.Empty))
            {
                hashCode.Add(Key1);
                hashCode.Add(Key2.Value);
                hashCode.Add(Key3.Value);
                hashCode.Add(Key4);
                hashCode.Add(Key5);
            }
            else
            {
                hashCode.Add(Value1);
                hashCode.Add(Value2);
            }
            return hashCode.ToHashCode();
        }
    }
    public class MyClassC1 : IEquatable<MyClassC1>
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public int Key5 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }

        public override bool Equals(object obj) => Equals(obj as MyClassC1);

        public bool Equals(MyClassC1 other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3x = Key3;
            long? key3y = other.Key3;
            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) || !key3x.HasValue)
                return other.Key1.Equals(Guid.Empty) &&
                    other.Key2.Equals(Guid.Empty) &&
                    !key3y.HasValue &&
                    Value1 == other.Value1 &&
                    EqualityComparer<DateTime?>.Default.Equals(Value2, other.Value2);
            return key1.Equals(other.Key1) &&
                key2.Equals(other.Key2) &&
                key3y.HasValue && key3x.Value.Equals(key3y.Value) &&
                Key4.Equals(other.Key4) &&
                Key5.Equals(other.Key5);
        }

        public override int GetHashCode()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            return (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) || !key3.HasValue) ? HashCode.Combine(Value1, Value2) : HashCode.Combine(Key1, Key2, Key3.Value, Key4, Key5);
        }

        public int GetHashCode2()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) || !key3.HasValue)
            {
                HashCode hashCode = new();
                hashCode.Add(Value1);
                hashCode.Add(Value2);
                return hashCode.ToHashCode();
            }
            return HashCode.Combine(Key1, Key2, Key3.Value, Key4, Key5);
        }

        public int GetHashCode3()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) || !key3.HasValue) HashCode.Combine(Value1, Value2);
            HashCode hashCode = new();
            hashCode.Add(Key1);
            hashCode.Add(Key2);
            hashCode.Add(Key3.Value);
            hashCode.Add(Key4);
            hashCode.Add(Key5);
            return hashCode.ToHashCode();
        }
          
        public int GetHashCode4()
        {
            Guid key1 = Key1;
            Guid key2 = Key2;
            long? key3 = Key3;
            long? key4 = Key4;
            HashCode hashCode = new();
            if (key1.Equals(Guid.Empty) || key2.Equals(Guid.Empty) || !key3.HasValue)
            {
                hashCode.Add(Value1);
                hashCode.Add(Value2);
            }
            else
            {
                hashCode.Add(Key1);
                hashCode.Add(Key2);
                hashCode.Add(Key3.Value);
                hashCode.Add(Key4);
                hashCode.Add(Key5);
            }
            return hashCode.ToHashCode();
        }
    }
    public class MyClassD1
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassE1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public long? Key4 { get; set; }
        public int Key5 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassF1
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassG1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassH1
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassI1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public long? Key4 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassJ1
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public long? Key3 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassK1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassL1
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassM1
    {
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassN1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public int Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassO1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassP1
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassQ1
    {
        public long? Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassR1
    {
        public long? Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassS1
    {
        public long? Key1 { get; set; }
        public long? Key2 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassT1
    {
        public Guid Key1 { get; set; }
        public int Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassU1
    {
        public Guid Key1 { get; set; }
        public int Key2 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassV1
    {
        public Guid Key1 { get; set; }
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassA2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public long? Key4 { get; set; }
        public int Key5 { get; set; }
        public int Key6 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassB2
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public int Key5 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassC2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public int Key5 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassD2
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassE2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public long? Key4 { get; set; }
        public int Key5 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassF2
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassG2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassH2
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassI2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public long? Key4 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassJ2
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public long? Key3 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassK2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public long? Key3 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassL2
    {
        public Guid Key1 { get; set; }
        public long? Key2 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassM2
    {
        public string Value1 { get; set; }
        public DateTime? Value2 { get; set; }
    }
    public class MyClassN2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public int Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassO2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassP2
    {
        public Guid Key1 { get; set; }
        public Guid Key2 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassQ2
    {
        public long? Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public int Key4 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassR2
    {
        public long? Key1 { get; set; }
        public long? Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassS2
    {
        public long? Key1 { get; set; }
        public long? Key2 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassT2
    {
        public Guid Key1 { get; set; }
        public int Key2 { get; set; }
        public int Key3 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassU2
    {
        public Guid Key1 { get; set; }
        public int Key2 { get; set; }
        public string Value1 { get; set; }
    }
    public class MyClassV2
    {
        public Guid Key1 { get; set; }
        public string Value1 { get; set; }
    }
}
