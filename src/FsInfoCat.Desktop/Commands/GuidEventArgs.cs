using System;

namespace FsInfoCat.Desktop.Commands
{
    public class GuidEventArgs : EventArgs
    {
        public Guid? Guid { get; }

        public GuidEventArgs(Guid? id)
        {
            Guid = id;
        }
    }
}
