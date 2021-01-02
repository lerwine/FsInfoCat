using System;
using System.Linq;
using System.DirectoryServices;
using System.Security.Principal;

namespace FsInfoCat.PsDesktop
{
    public static class FsInfoCatUtil
    {
        public static SecurityIdentifier GetLocalMachineSID()
        {
            try
            {
                using (DirectoryEntry directoryEntry = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer"))
                {
                    DirectoryEntry d = directoryEntry.Children.OfType<DirectoryEntry>().FirstOrDefault();
                    if (null == d)
                        throw new Exception("Computer directory entry not found");
                    return new SecurityIdentifier((byte[])d.InvokeGet("objectSID"), 0).AccountDomainSid;
                }
            }
            catch (Exception exc)
            {
                if (string.IsNullOrWhiteSpace(exc.Message))
                    throw;
                throw new Exception("Encountered an exception while trying to retrieve computer SID - " + exc.Message, exc);
            }
        }
    }
}
