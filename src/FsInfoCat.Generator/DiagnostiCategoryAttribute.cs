using System;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace FsInfoCat.Generator
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DiagnostiCategoryAttribute : Attribute
    {
        public const DiagnosticSeverity Default_DiagnosticSeverity = DiagnosticSeverity.Error;

        public string Category { get; }

        public DiagnosticSeverity DefaultSeverity { get; set; } = Default_DiagnosticSeverity;

        public DiagnostiCategoryAttribute(string category) => Category = category;

        public static string GetCategory<T>(T value) where T : Enum
        {
            string category = value.GetType().GetField(value.ToString("F")).GetCustomAttribute<DiagnostiCategoryAttribute>(false)?.Category;
            return string.IsNullOrWhiteSpace(category) ? "FsInfoCat.Generator" : category;
        }

        public static DiagnosticSeverity GetDefaultSeverity<T>(T value) where T : Enum => value.GetType().GetField(value.ToString("F")).GetCustomAttribute<DiagnostiCategoryAttribute>(false)?.DefaultSeverity ?? Default_DiagnosticSeverity;

        public static string GetCategory<T>(T value, out DiagnosticSeverity defaultSeverity) where T : Enum
        {
            DiagnostiCategoryAttribute attribute = value.GetType().GetField(value.ToString("F")).GetCustomAttribute<DiagnostiCategoryAttribute>(false);
            if (attribute is null)
                defaultSeverity = Default_DiagnosticSeverity;
            else
            {
                defaultSeverity = attribute.DefaultSeverity;
                if (!string.IsNullOrWhiteSpace(attribute.Category)) return attribute.Category;
            }
            return "FsInfoCat.Generator";
        }
    }
}
