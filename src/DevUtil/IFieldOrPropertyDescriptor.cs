namespace DevUtil
{
    public interface IFieldOrPropertyDescriptor
    {
        string Name { get; }

        bool IsKey { get; }

        bool NotNull { get; }

        EnhancedTypeDescriptor Type { get; }

        EnhancedTypeDescriptor Owner { get; }

        EnhancedPropertyDescriptor Property { get; }
    }
}
