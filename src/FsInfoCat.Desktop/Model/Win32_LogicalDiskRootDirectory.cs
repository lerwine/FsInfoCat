using FsInfoCat.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Management;

namespace FsInfoCat.Desktop.Model
{
    /// <summary>
    /// Wraps the The <c>Win32_LogicalDiskRootDirectory</c> association WMI class which relates a logical disk and its directory structure.
    /// </summary>
    /// <remarks>References
    /// <list type="bullet">
    ///     <item><term>White, S. (2018, May 31)</term>
    ///         <description>Win32_LogicalDiskRootDirectory class - Win32 apps
    ///         <para>Retrieved from Microsoft Docs: https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-logicaldiskrootdirectory</para></description>
    ///     </item>
    /// </list></remarks>
    public class Win32_LogicalDiskRootDirectory : CimGroupPartComponent<Win32_LogicalDisk, Win32_Directory>
    {
        public Win32_LogicalDiskRootDirectory(ManagementObject managementObject)
            : base(new Win32_LogicalDisk(new ManagementObject((string)(managementObject ?? throw new ArgumentNullException(nameof(managementObject)))[nameof(GroupComponent)])),
                  new Win32_Directory(new ManagementObject((string)managementObject[nameof(PartComponent)])))
        { }

        public static List<Win32_LogicalDiskRootDirectory> GetLogicalDiskRootDirectories(ModalOperationStatusViewModel.Controller controller)
        {
            controller.ThrowIfCancellationRequested();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM {nameof(Win32_LogicalDiskRootDirectory)}");
            List<Win32_LogicalDiskRootDirectory> result = new List<Win32_LogicalDiskRootDirectory>();
            foreach (ManagementObject managementObject in searcher.Get())
            {
                controller.ThrowIfCancellationRequested();
                result.Add(new Win32_LogicalDiskRootDirectory(managementObject));
            }
            return result;
        }
    }
}
