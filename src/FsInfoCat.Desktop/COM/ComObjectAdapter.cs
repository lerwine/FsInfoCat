using System;
using System.Runtime.InteropServices;

namespace FsInfoCat.Desktop.COM
{
    public class ComObjectAdapter : IDisposable
    {
        private readonly Type _type;
        private readonly object _instance;
        private bool _disposed;

        protected ComObjectAdapter(string progID)
        {
            _type = Type.GetTypeFromProgID(progID);
            _instance = Activator.CreateInstance(_type);
        }

        protected object InvokeMethod(string memberName, params object[] args)
        {
            return _type.InvokeMember(memberName, System.Reflection.BindingFlags.InvokeMethod, null, _instance, args);
        }

        protected object GetProperty(string memberName, params object[] args)
        {
            return _type.InvokeMember(memberName, System.Reflection.BindingFlags.GetProperty, null, _instance, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _ = Marshal.ReleaseComObject(_instance);
                _disposed = true;
            }
        }

        ~ComObjectAdapter()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
