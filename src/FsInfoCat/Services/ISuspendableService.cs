using System.Collections.Generic;

namespace FsInfoCat.Services
{

    public interface ISuspendableService
    {
        ISuspendableQueue<T> GetSuspendableQueue<T>();
        ISuspendableQueue<T> GetSuspendableQueue<T>(IEqualityComparer<T> itemComparer);
    }
}
