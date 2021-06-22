namespace FsInfoCat
{
    public interface IProducer<TResult>
    {
        TResult Produce();
    }

    public interface IProducer<TArg, TResult>
    {
        TResult Produce(TArg arg);
    }

    public interface IProducer<TArg1, TArg2, TResult>
    {
        TResult Produce(TArg1 arg1, TArg2 arg2);
    }

    public interface IProducer<TArg1, TArg2, TArg3, TResult>
    {
        TResult Produce(TArg1 arg1, TArg2 arg2, TArg3 arg3);
    }
}
