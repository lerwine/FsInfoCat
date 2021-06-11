using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Shell
{
    public class ComObjectWrapper : IDisposable
    {
        internal static readonly Type ShellType = Type.GetTypeFromProgID("Shell.Application");
        private readonly object _instance;
        private bool disposedValue;

        protected ComObjectWrapper(object instance)
        {
            _instance = instance;
        }

        protected object InvokeMethod(string memberName, params object[] args)
        {
            return ShellType.InvokeMember(memberName, System.Reflection.BindingFlags.InvokeMethod, null, _instance, args);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ComObjectWrapper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class ItemsEnumerable : ComObjectWrapper
    {
        public ItemsEnumerable(object instance) : base(instance) { }
    }

    public class FolderItem : ComObjectWrapper
    {
        public FolderItem(object instance) : base(instance) { }
    }

    public class Folder : ComObjectWrapper
    {
        public Folder(object comInstance) : base(comInstance) { }

        public object GetDetailsOf(object item, int column) => InvokeMethod(nameof(GetDetailsOf), item, column);

        public ItemsEnumerable Items(object item, int column)
        {
            object obj = InvokeMethod(nameof(Items));
            return (obj is null) ? null : new ItemsEnumerable(obj);
        }

        public FolderItem ParseName(string name)
        {
            object obj = InvokeMethod(nameof(ParseName), name);
            return (obj is null) ? null : new FolderItem(obj);
        }
    }

    public class ExplorerShell : ComObjectWrapper
    {
        public ExplorerShell() : base(Activator.CreateInstance(ShellType)) { }

        public Folder NameSpace(string path)
        {
            return new Folder(InvokeMethod(nameof(NameSpace), path));
        }
    }
}
