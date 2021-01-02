#if DESKTOP
using System.Reflection;
using System.Runtime.CompilerServices;
#endif
using System.Runtime.InteropServices;

#if DESKTOP
// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("FS InfoCat Common Library")]
[assembly: AssemblyDescription("Supporting CLR Library for FS InfoCat")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Leonard T. Erwine")]
[assembly: AssemblyProduct("FS InfoCat Common Library")]
[assembly: AssemblyCopyright("Copyright © Leonard Thomas Erwine 2021")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("89701734-c4a8-450b-9adf-a24f351c6599")]
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
[assembly: Guid("cd042851-2228-4b29-ae1f-3ed1e6ac1a1e")]
#else
[assembly: Guid("f624d84f-6e2d-4656-bd1b-060586988ebc")]
#endif
