using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class WindowOwner(IntPtr handle) : System.Windows.Forms.IWin32Window
    {
        private readonly IntPtr _handle = handle;
        public WindowOwner() : this(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle) { }

        public IntPtr Handle { get { return _handle; } }
    }
}
