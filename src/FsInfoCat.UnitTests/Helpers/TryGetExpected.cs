namespace FsInfoCat.UnitTests.Helpers;

public record TryGetExpected<T>
{
    public bool Returned { get; init; }

    public T Result { get; init; }
}
