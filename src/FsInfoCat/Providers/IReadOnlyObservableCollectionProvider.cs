using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Linq;

namespace FsInfoCat.Providers
{

    public interface IReadOnlyObservableCollectionProvider<T>
    {
        ReadOnlyObservableCollection<T> GetReadOnlyObservableCollection();
    }
}
