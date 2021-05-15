using FsInfoCat.Services;
using System.ComponentModel;

namespace FsInfoCat.Internal
{
    class SuspendableComponentQueue<T> : SuspendableQueue<T>
        where T : class
    {
        public SuspendableComponentQueue(PropertyDescriptor[] properties)
        {

        }

        protected override bool AreEqual(T x, T y)
        {
            throw new System.NotImplementedException();
        }

        protected override int GetHashcode(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
