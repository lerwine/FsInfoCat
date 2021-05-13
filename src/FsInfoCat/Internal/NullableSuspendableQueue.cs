using FsInfoCat.Services;

namespace FsInfoCat.Internal
{
    class NullableSuspendableQueue<T> : ISuspendableQueue<T?>
        where T : struct
    {
        public NullableSuspendableQueue()
        {

        }
    }
}
