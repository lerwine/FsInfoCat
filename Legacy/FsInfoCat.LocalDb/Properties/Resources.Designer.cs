﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.LocalDb.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FsInfoCat.LocalDb.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to -- Creating table &apos;FileSystems&apos;
        ///CREATE TABLE [FileSystems] (
        ///    [Id] uniqueidentifier  NOT NULL,
        ///    [DisplayName] nvarchar(128)  NOT NULL,
        ///    [CaseSensitiveSearch] bit  NOT NULL,
        ///    [ReadOnly] bit  NOT NULL,
        ///    [MaxNameLength] bigint  NOT NULL,
        ///    [DefaultDriveType] tinyint  NULL,
        ///    [DefaultSymbolicNameId] uniqueidentifier  NOT NULL,
        ///    [Notes] ntext  NOT NULL,
        ///    [IsInactive] bit  NOT NULL,
        ///    [UpstreamId] uniqueidentifier NULL,
        ///    [LastSynchronized] datetime  NULL,
        ///    [CreatedOn] [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DbInitialization {
            get {
                return ResourceManager.GetString("DbInitialization", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;SqlCommands&gt;
        ///    &lt;CreateTables&gt;
        ///        &lt;Text Message=&quot;Creating table &apos;FileSystems&apos;&quot;&gt;
        ///            &lt;![CDATA[CREATE TABLE [FileSystems] (
        ///    [Id] uniqueidentifier  NOT NULL,
        ///    [DisplayName] nvarchar(128)  NOT NULL,
        ///    [CaseSensitiveSearch] bit  NOT NULL,
        ///    [ReadOnly] bit  NOT NULL,
        ///    [MaxNameLength] bigint  NOT NULL,
        ///    [DefaultDriveType] tinyint  NULL,
        ///    [DefaultSymbolicNameId] uniqueidentifier  NOT NULL,
        ///    [Notes] ntext  NOT NULL,
        ///    [IsIna [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SqlCommands {
            get {
                return ResourceManager.GetString("SqlCommands", resourceCulture);
            }
        }
    }
}
