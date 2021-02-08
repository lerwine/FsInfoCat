using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        class SerialNumberOrdinalTestValues
        {
            private readonly ReadOnlyCollection<SerialNumberOrdinalTestValues> _variants;
            internal uint SerialNumber { get; }
            internal byte Ordinal { get; }
            internal string StringParam { get; }
            internal string StringValue { get; }
            internal string Description { get; }
            internal string UrnParam { get; }
            internal Uri Url { get; }
            protected SerialNumberOrdinalTestValues(uint serialNumber, byte ordinal, string description)
            {
                SerialNumber = serialNumber;
                Ordinal = ordinal;
                StringParam = $"{serialNumber.ToString("x8")}-{ordinal.ToString("x2")}";
                StringValue = $"volume:id:{StringParam}";
                Description = description;
                Url = new Uri($"urn:{StringValue}", UriKind.Absolute);
                UrnParam = Url.AbsoluteUri;
                Collection<SerialNumberOrdinalTestValues> variants = new Collection<SerialNumberOrdinalTestValues>();
                _variants = new ReadOnlyCollection<SerialNumberOrdinalTestValues>(variants);
                variants.Add(this);
                if (StringParam.ToUpper() != StringParam)
                {
                    SerialNumberOrdinalTestValues caseVariant = new SerialNumberOrdinalTestValues(this, StringParam.ToUpper(),
                        $"Upper case {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                        $"urn:VOLUME:ID:{StringParam.ToUpper()}");
                    variants.Add(caseVariant);
                    if (ordinal < 16)
                    {
                        variants.Add(new SerialNumberOrdinalTestValues(this, $"{serialNumber.ToString("x8")}-{ordinal.ToString("x")}",
                            $"{Description} without leading zero",
                            $"urn:volume:id:{serialNumber.ToString("x8")}-{ordinal.ToString("x")}"));
                        variants.Add(new SerialNumberOrdinalTestValues(this, $"{serialNumber.ToString("X8")}-{ordinal.ToString("X")}",
                            $"{caseVariant.Description.Substring(1)} without leading zero",
                            $"urn:VOLUME:ID:{serialNumber.ToString("X8")}-{ordinal.ToString("X")}"));
                    }
                }
                else if (ordinal < 16)
                    variants.Add(new SerialNumberOrdinalTestValues(this, $"{serialNumber.ToString("x8")}-{ordinal.ToString("x")}",
                        $"{Description} without leading zero",
                        $"urn:volume:id:{serialNumber.ToString("x8")}-{ordinal.ToString("x")}"));
            }
            protected SerialNumberOrdinalTestValues(SerialNumberOrdinalTestValues masterVariant, string stringParam, string description, string urn)
            {
                _variants = masterVariant._variants;
                SerialNumber = masterVariant.SerialNumber;
                Ordinal = masterVariant.Ordinal;
                StringParam = stringParam;
                StringValue = masterVariant.StringValue;
                Description = description;
                Url = masterVariant.Url;
                UrnParam = urn;
            }

            private static IEnumerable<Tuple<byte, string>> _GetOrdinalValues()
            {
                yield return new Tuple<byte, string>(0, "zero value");
                yield return new Tuple<byte, string>(1, "Value of 1");
                yield return new Tuple<byte, string>(10, "Value of 10");
                yield return new Tuple<byte, string>(16, "Value of 16");
                yield return new Tuple<byte, string>(Byte.MaxValue, "Byte.MaxValue");
            }

            public static IEnumerable<SerialNumberOrdinalTestValues> GetTestValues(bool includeVariants = false)
            {
                if (includeVariants)
                    return GetTestValues(false).SelectMany(o => o._variants);
                return SerialNumberTestValues.GetTestValues(false, false).SelectMany(sn => _GetOrdinalValues().Select(ord =>
                    new SerialNumberOrdinalTestValues(sn.SerialNumber, ord.Item1, $"serialNumber = {sn.Description}; ordinal = {ord.Item2}")
                ));
            }
        }
    }
}
