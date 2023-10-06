using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace FsInfoCat
{
    // TODO: Document FsInfoCatOptions class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class FsInfoCatOptions
    {
        public const string FsInfoCat = nameof(FsInfoCat);

        public const string SHORTHAND_l = "-l";

        public string LocalDbFile { get; set; }

        public const string SHORTHAND_u = "-u";

        public string UpstreamDbConnectionString { get; set; }

        internal static Dictionary<string, string> GetSwitchMappings() => new()
        {
            { SHORTHAND_l, $"{FsInfoCat}:{nameof(LocalDbFile)}" },
            { SHORTHAND_u, $"{FsInfoCat}:{nameof(UpstreamDbConnectionString)}" }
        };

        internal static void Configure(string[] args, IConfigurationBuilder builder, HostBuilderContext context, Assembly[] assemblies)
        {
            Dictionary<string, string> mappings = new()
            {
                { SHORTHAND_l, $"{FsInfoCat}:{nameof(LocalDbFile)}" },
                { SHORTHAND_u, $"{FsInfoCat}:{nameof(UpstreamDbConnectionString)}" }
            };
            CommandLineSwitchMappingsInitializerAttribute.InvokeHandlers(context, mappings, assemblies);
            builder.AddCommandLine(args, mappings);
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
