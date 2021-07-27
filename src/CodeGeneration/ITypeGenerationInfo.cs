using System.Collections.Generic;
using System.Xml.Linq;

namespace CodeGeneration
{
    public interface ITypeGenerationInfo
    {
        string Name { get; }
        string CsName { get; }
        XElement Source { get; }
        IEnumerable<IMemberGenerationInfo> GetMembers();
    }
}
