#if DESKTOP
using System.Reflection;
using System.Runtime.CompilerServices;
#endif
using System.Runtime.InteropServices;

#if DESKTOP
// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("FS InfoCat PowerShell Library")]
[assembly: AssemblyDescription("Supporting CLR Library for PowerShell Module FsInfoCat")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Leonard T. Erwine")]
[assembly: AssemblyProduct("PowerShell Module FsInfoCat.PS")]
[assembly: AssemblyCopyright("Copyright © Leonard Thomas Erwine 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("13b95235-fca5-4752-8e8d-82de423d55c2")]
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.1.0.0")]
[assembly: AssemblyFileVersion("0.1.0.0")]
#elif LINUX
[assembly: Guid("42d4c464-85f8-4e80-ab2d-2aa84eabfba3")]
#else
[assembly: Guid("d6a47138-19ef-4860-9c80-64c78eafdeee")]
#endif
