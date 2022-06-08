namespace DevUtil.Wrappers
{
    public interface ISyntaxTree
    {
        string FilePath { get; }
        int Length { get; }
        bool HasLeadingTrivia { get; }
        bool HasStructuredTrivia { get; }
        bool HasTrailingTrivia { get; }
    }
}
