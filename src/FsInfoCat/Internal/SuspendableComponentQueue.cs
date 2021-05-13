using FsInfoCat.Services;
using System.ComponentModel;

namespace FsInfoCat.Internal
{
    class SuspendableComponentQueue<T> : ISuspendableQueue<T>
        where T : class
    {
        public SuspendableComponentQueue(PropertyDescriptor[] properties)
        {

        }
    }
}
