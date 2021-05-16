using System.Collections.Generic;

namespace FsInfoCat.Services
{

    public interface ISuspendableService
    {
        ISuspendableQueue<T> CreateSuspendableQueue<T>();
        ISuspendableQueue<T> CreateSuspendableQueue<T>(IEqualityComparer<T> itemComparer);
    }
}
