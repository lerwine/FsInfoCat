using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;
using FsInfoCat.Util;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        class UrlTestValues
        {
            private readonly ReadOnlyCollection<UrlTestValues> _variants;
            internal Uri NormalizedUrl { get; }
            internal string UrlParam { get; }
            internal string Description { get; }
            internal bool HasTrailingSlash { get; }
            public UrlTestValues(Uri url, string description)
            {
                string originalString = url.OriginalString;
                NormalizedUrl = url.AsNormalized();
                Assert.That(NormalizedUrl.IsAbsoluteUri, Is.True, $"Invalid test data: {url.OriginalString}");
                Assert.That(NormalizedUrl.Query, Is.Empty, $"Invalid test data: {url.OriginalString}");
                Assert.That(NormalizedUrl.Fragment, Is.Empty, $"Invalid test data: {url.OriginalString}");
                if (originalString.EndsWith("#"))
                    originalString = originalString[0..^1];
                if (originalString.EndsWith("?"))
                    originalString = originalString[0..^1];
                UrlParam = originalString;
                Collection<UrlTestValues> variants = new Collection<UrlTestValues>();
                _variants = new ReadOnlyCollection<UrlTestValues>(variants);
                UrlTestValues pathVariant;
                HasTrailingSlash = NormalizedUrl.AbsolutePath.EndsWith('/');
                string noSlashUrl = (HasTrailingSlash) ? originalString[0..^1] : $"{originalString}/";
                if (originalString != NormalizedUrl.AbsoluteUri)
                {
                    Description = $"Improper URL {description}";
                    variants.Add(this);
                    variants.Add(new UrlTestValues(this, $"{noSlashUrl}?", $"{Description} with empty query", false));
                    variants.Add(new UrlTestValues(this, $"{noSlashUrl}#", $"{Description} with empty fragment", false));
                    if (HasTrailingSlash)
                        pathVariant = new UrlTestValues(this, noSlashUrl, $"{Description} without trailing slash", false);
                    else
                        pathVariant = new UrlTestValues(this, $"{UrlParam}/", $"{Description} with trailing slash", true);
                    variants.Add(pathVariant);
                    variants.Add(new UrlTestValues(this, $"{pathVariant.UrlParam}?#", $"{pathVariant.Description} and empty query and fragment", true));
                    variants.Add(new UrlTestValues(this, NormalizedUrl.AbsoluteUri, description, HasTrailingSlash));
                }
                else
                {
                    Description = description;
                    variants.Add(this);
                }
                noSlashUrl = (HasTrailingSlash) ? NormalizedUrl.AbsolutePath[0..^1] : $"{NormalizedUrl.AbsolutePath}/";
                variants.Add(new UrlTestValues(this, $"{noSlashUrl}?", $"{description} with empty query", false));
                variants.Add(new UrlTestValues(this, $"{noSlashUrl}#", $"{description} with empty fragment", false));
                if (HasTrailingSlash)
                    pathVariant = new UrlTestValues(this, noSlashUrl, $"{description} without trailing slash", false);
                else
                    pathVariant = new UrlTestValues(this, $"{NormalizedUrl.AbsolutePath}/", $"{description} with trailing slash", true);
                variants.Add(pathVariant);
                variants.Add(new UrlTestValues(this, $"{pathVariant.UrlParam}?#", $"{pathVariant.Description} and empty query and fragment", true));
            }

            private UrlTestValues(UrlTestValues source, string urlParam, string description, bool hasTrailingSlash)
            {
                NormalizedUrl = source.NormalizedUrl;
                _variants = source._variants;
                UrlParam = urlParam;
                Description = description;
                HasTrailingSlash = hasTrailingSlash;
            }

            private static IEnumerable<UrlTestValues> _GetUrlTestValues()
            {
                yield return new UrlTestValues(new Uri(@"\\servicenowdiag479.file.core.windows.net\testazureshare\", UriKind.Absolute), "File UNC with fqdn");
                yield return new UrlTestValues(new Uri($@"\\{_hostName}\$Admin\", UriKind.Absolute), "File UNC");
                yield return new UrlTestValues(new Uri($@"\\{_ipV4Address}\Us&Them\", UriKind.Absolute), "File with IPV4");
                yield return new UrlTestValues(new Uri($@"\\[{_ipV6Address}]\100% Done", UriKind.Absolute), "File with IPV6");
                yield return new UrlTestValues(new Uri($"urn:uuid:{Guid.NewGuid().ToString("b").ToLower()}", UriKind.Absolute), "UUID urn");
            }

            public static IEnumerable<UrlTestValues> GetUrlTestValues(bool includeVariants = false)
            {
                if (includeVariants)
                    return _GetUrlTestValues().SelectMany(u => u._variants);
                return _GetUrlTestValues();
            }
        }
    }
}
