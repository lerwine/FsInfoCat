Add-Type -Path 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat\bin\Debug\net5.0\FsInfoCat.dll' -ErrorAction Stop;
Add-Type -Path 'C:\Users\lerwi\Git\FsInfoCat\src\FsInfoCat.Local\bin\Debug\net5.0\FsInfoCat.Local.dll' -ErrorAction Stop;
Add-Type -AssemblyName 'PresentationCore' -ErrorAction Stop;
Add-Type -TypeDefinition @'
namespace Helpers
{
    using System;
    using System.Linq;
    public class TypeExtensions
    {
        public static string ToCsTypeName(Type type, bool omitNamespaces = false)
        {
            if (type is null)
                return "null";
            if (type.IsGenericParameter)
                return type.Name;
            if (type.IsPointer)
                return ToCsTypeName(type.GetElementType(), omitNamespaces) + "*";
            if (type.IsByRef)
                return ToCsTypeName(type.GetElementType(), omitNamespaces) + "&";
            if (type.IsArray)
            {
                int rank = type.GetArrayRank();
                if (rank < 2)
                    return ToCsTypeName(type.GetElementType(), omitNamespaces) + "[]";
                if (rank == 2)
                    return ToCsTypeName(type.GetElementType(), omitNamespaces) + "[,]";
                return ToCsTypeName(type.GetElementType(), omitNamespaces) + "[" + new string(',', rank - 1) + "]";
            }

            if (type.IsValueType)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return ToCsTypeName(Nullable.GetUnderlyingType(type), omitNamespaces) + "?";
                if (type.Equals(typeof(void)))
                    return "void";
                if (type.Equals(typeof(char)))
                    return "char";
                if (type.Equals(typeof(bool)))
                    return "bool";
                if (type.Equals(typeof(byte)))
                    return "byte";
                if (type.Equals(typeof(sbyte)))
                    return "sbyte";
                if (type.Equals(typeof(short)))
                    return "short";
                if (type.Equals(typeof(ushort)))
                    return "ushort";
                if (type.Equals(typeof(int)))
                    return "int";
                if (type.Equals(typeof(uint)))
                    return "uint";
                if (type.Equals(typeof(long)))
                    return "long";
                if (type.Equals(typeof(ulong)))
                    return "ulong";
                if (type.Equals(typeof(float)))
                    return "float";
                if (type.Equals(typeof(double)))
                    return "double";
                if (type.Equals(typeof(decimal)))
                    return "decimal";
            }
            else
            {
                if (type.Equals(typeof(string)))
                    return "string";
                if (type.Equals(typeof(object)))
                    return "object";
            }
            string n = type.Name;
            string ns;
            if (type.IsNested)
                ns = ToCsTypeName(type.DeclaringType, omitNamespaces);
            else if (omitNamespaces || (ns = type.Namespace) is null || ns == "System")
                ns = "";

            if (type.IsGenericType)
            {
                int i = n.IndexOf("`");
                if (i > 0)
                    n = n.Substring(0, i);
                if (ns.Length > 0)
                    return ns + "." + n + "<" + string.Join(",", type.GetGenericArguments().Select(a => ToCsTypeName(a, omitNamespaces))) + ">";
                return  n + "<" + string.Join(",", type.GetGenericArguments().Select(a => ToCsTypeName(a, omitNamespaces))) + ">";
            }
            return (ns.Length > 0) ? ns + "." + n : n;
        }
    }
}
'@ -ErrorAction Stop;

Function Get-PropertyDefinitions {
    Param(
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [Type[]]$Types,

        [AllowEmptyCollection()]
        [System.Collections.ObjectModel.Collection[Type]]$Ignore
    )

    Begin {
        $Emitted = $null;
        if ($PSBoundParameters.ContainsKey('Ignore')) {
            $Emitted = $Ignore;
        } else  {
            $Emitted = [System.Collections.ObjectModel.Collection[Type]]::new();
        }
    }

    Process {
        foreach ($t in $Types) {
            if (-not $Emitted.Contains($t)) {
                $Emitted.Add($t);
                $t.GetProperties() | Write-Output;
                if ($t.IsInterface) {
                    $Interfaces = $t.GetInterfaces();
                    if ($null -ne $Interfaces -and $Interfaces.Length -gt 0) {
                        foreach ($i in $Interfaces) {
                            Get-PropertyDefinitions -Types $i -Ignore $Emitted;
                        }
                    }
                }
            }
        }
    }
}

Function Get-ColumnVisibilityOptionsProperties {
    Param(
        [Parameter(Mandatory = $true)]
        [Type]$Interface,

        [Parameter(Mandatory = $true)]
        [string]$ClassName
    )

    $Ignore = @('IsChanged', 'Id', 'ExistingFileCount', 'CreatedOn', 'ModifiedOn');
    
    ((Get-PropertyDefinitions -Types ([FsInfoCat.IAudioPropertiesListItem])) | ForEach-Object {
        if ($Ignore -inotcontains $_.Name) {
            @"
        #region $($_.Name) Property Members

        /// <summary>
        /// Identifies the <see cref="$($_.Name)"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty $($_.Name)Property = DependencyPropertyBuilder<$ClassName, bool>
            .Register(nameof($($_.Name)))
            .DefaultValue(false)
            .OnChanged((d, e) => (d as $ClassName)?.RaiseColumnVisibilityPropertyChanged(e))
            .AsReadWrite();

        public bool $($_.Name) { get => (bool)GetValue($($_.Name)Property); set => SetValue($($_.Name)Property, value); }

        #endregion
"@
        }
    }) | Out-String;
}

#$Code = Get-ColumnVisibilityOptionsProperties -Interface ([FsInfoCat.ICrawlConfigurationListItem]) -ClassName 'CrawlConfigPropertiesColumnVisibilityOptions<TEntity, TViewModel>';

$Code = ((Get-PropertyDefinitions -Types ([FsInfoCat.Local.ILocalAudioPropertySet])) | ForEach-Object {
    if ($_.Name -ne 'IsChanged') {
        $DisplayAttribute = [System.Reflection.CustomAttributeExtensions]::GetCustomAttribute($_, [System.ComponentModel.DataAnnotations.DisplayAttribute]);
        $ShortName = $null;
        $DisplayName = $null;
        $Description = $null;
        if ($null -ne $DisplayAttribute) {
            $Description = $DisplayAttribute.GetDescription();
            $DisplayAttribute.GroupName;
            $ShortName = $DisplayAttribute.GetShortName();
            $DisplayName = $DisplayAttribute.GetName();
        }
        if ([string]::IsNullOrWhiteSpace($DisplayName)) {
            [System.ComponentModel.DisplayNameAttribute]$DisplayNameAttribute = [System.Reflection.CustomAttributeExtensions]::GetCustomAttribute($_, [System.ComponentModel.DisplayNameAttribute]);
            if ($null -ne $DisplayNameAttribute) {
                $DisplayName = $DisplayNameAttribute.DisplayName;
            }
        }
        if ([string]::IsNullOrWhiteSpace($Description)) {
            [System.ComponentModel.DescriptionAttribute]$DescriptionAttribute = [System.Reflection.CustomAttributeExtensions]::GetCustomAttribute($_, [System.ComponentModel.DescriptionAttribute]);
            if ($null -ne $DescriptionAttribute) {
                $Description = $DescriptionAttribute.Description;
            }
        }
        
        $UnderlyingType = $_.PropertyType;
        $TypeName = [Helpers.TypeExtensions]::ToCsTypeName($_.PropertyType, $true);
        if ($_.PropertyType.IsValueType -and $_.PropertyType.IsGenericType -and [Nullable`1].Equals($_.PropertyType.GetGenericTypeDefinition())) {
            $UnderlyingType = [Nullable]::GetUnderlyingType($_.PropertyType);
        }
        "                <DataGridTextColumn Binding=""{Binding $($_.Name), Mode=OneWay}"" Header=""$DisplayName""/>"
    }
})
[System.Windows.Clipboard]::SetText($Code);
