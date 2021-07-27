using System.Data;
using System.Xml.Linq;

namespace CodeGeneration
{
    public interface IMemberGenerationInfo
    {
        string Name { get; }
        DbType Type { get; }
        string CsTypeName { get; }
        string SqlTypeName { get; }
        XElement Source { get; }
        ITypeGenerationInfo DeclaringType { get; }
    }
}
