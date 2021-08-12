using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class WindowOwner : System.Windows.Forms.IWin32Window
    {
        private readonly IntPtr _handle;
        public WindowOwner() : this(System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle) { }
        public WindowOwner(IntPtr handle) { _handle = handle; }
        public IntPtr Handle { get { return _handle; } }
    }
}
