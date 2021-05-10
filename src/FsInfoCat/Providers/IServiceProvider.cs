using System;

namespace FsInfoCat.Providers
{
    public interface IServiceProvider<TArg>
    {
        object GetService(Type serviceType, TArg arg);
    }

    public interface IServiceProvider<TArg1, TArg2>
    {
        object GetService(Type serviceType, TArg1 arg1, TArg2 arg2);
    }

    public interface IServiceProvider<TArg1, TArg2, TArg3>
    {
        object GetService(Type serviceType, TArg1 arg1, TArg2 arg2, TArg3 arg3);
    }
}
