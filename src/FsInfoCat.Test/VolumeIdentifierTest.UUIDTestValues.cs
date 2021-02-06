using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        class UUIDTestValues
        {
            private readonly ReadOnlyCollection<UUIDTestValues> _variants;
            internal Guid UUID { get; }
            internal string StringParam { get; }
            internal string StringValue { get; }
            internal string Description { get; }
            internal string UrnParam { get; }
            internal Uri Url { get; }
            protected UUIDTestValues(Guid uuid, string description)
            {
                UUID = uuid;
                StringParam = uuid.ToString("d").ToLower();
                StringValue = StringParam;
                Description = description;
                Url = new Uri($"urn:uuid:{StringValue}", UriKind.Absolute);
                UrnParam = Url.AbsoluteUri;
                Collection<UUIDTestValues> variants = new Collection<UUIDTestValues>();
                _variants = new ReadOnlyCollection<UUIDTestValues>(variants);
                variants.Add(this);
                variants.Add(new UUIDTestValues(this, uuid.ToString("b").ToLower(),
                    $"Brace format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("b").ToLower()}"));
                variants.Add(new UUIDTestValues(this, uuid.ToString("n").ToLower(),
                    $"No-dash format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("n").ToLower()}"));
                variants.Add(new UUIDTestValues(this, uuid.ToString("p").ToLower(),
                    $"Parenheses format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("p").ToLower()}"));
                variants.Add(new UUIDTestValues(this, uuid.ToString("x").ToLower(),
                    $"Grouped hex format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("x").ToLower()}"));
                if (StringParam.ToUpper() != StringParam)
                {
                    foreach (UUIDTestValues t in variants.ToArray())
                        variants.Add(new UUIDTestValues(this, t.StringParam.ToUpper(),
                            $"Upper case {t.Description.Substring(0, 1).ToLower()}{t.Description.Substring(1)}",
                            $"urn:UUID:{t.StringParam.ToUpper()}"));
                }
            }
            protected UUIDTestValues(UUIDTestValues masterVariant, string stringParam, string description, string urn)
            {
                _variants = masterVariant._variants;
                UUID = masterVariant.UUID;
                StringParam = stringParam;
                StringValue = masterVariant.StringValue;
                Description = description;
                Url = masterVariant.Url;
                UrnParam = urn;
            }

            public static IEnumerable<UUIDTestValues> GetTestValues(bool includeVariants)
            {
                if (includeVariants)
                {
                    Guid guid = Guid.NewGuid();
                    string s = guid.ToString("n").ToLower();
                    while (s.ToUpper() == s)
                        s = (guid = Guid.NewGuid()).ToString("n").ToLower();
                    return (new UUIDTestValues[] {
                        new UUIDTestValues(guid, "Random UUID"), new UUIDTestValues(Guid.Empty, "Empty UUID")
                    }).SelectMany(u => u._variants);
                }
                return new UUIDTestValues[]
                {
                    new UUIDTestValues(Guid.NewGuid(), "Random UUID #1"),
                    new UUIDTestValues(Guid.NewGuid(), "Random UUID #2"),
                    new UUIDTestValues(Guid.Empty, "Empty UUID")
                };
            }
        }
    }
}
