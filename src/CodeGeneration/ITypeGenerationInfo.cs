using System.Collections.Generic;
using System.Xml.Linq;

namespace CodeGeneration
{
    public interface ITypeGenerationInfo
    {
        string Name { get; }
        XElement Source { get; }
        IEnumerable<IMemberGenerationInfo> GetMembers();
    }
}
