using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class FsCrawlJobTestData
    {
        private const string ROOT_ELEMENT_NAME = "Contents";
        private Collection<ContentTemplate> _templates = null;
        private Collection<InputSet> _inputSets = null;

        [XmlArray()]
        [XmlArrayItem(ContentTemplate.ROOT_ELEMENT_NAME)]
        public Collection<ContentTemplate> Templates
        {
            get
            {
                Collection<ContentTemplate> items = _templates;
                if (items is null)
                    _templates = items = new Collection<ContentTemplate>();
                return items;
            }
            set => _templates = value;
        }

        [XmlArray()]
        [XmlArrayItem(InputSet.ROOT_ELEMENT_NAME)]
        public Collection<InputSet> InputSets
        {
            get
            {
                Collection<InputSet> items = _inputSets;
                if (items is null)
                    _inputSets = items = new Collection<InputSet>();
                return items;
            }
            set => _inputSets = value;
        }

        // /// <summary>
        // /// Expands <see cref="InputSet" /> to a hashtable for test parameters.
        // /// </summary>
        // /// <returns>@(
        // ///     @{
        // ///         Description = [string];
        // ///         Roots = @{ [ID] = [filename...] };
        // ///         TestDefinitions = @([TestDefinition], ...);
        // ///         Templates = @([ContentTemplate], ...);
        // ///     },
        // ///     ...
        // /// )</returns>
        // public Collection<Hashtable> ExpandInputSets()
        // {
        //     Collection<Hashtable> result = new Collection<Hashtable>();
        //     foreach (InputSet inputSet in InputSets)
        //     {
        //         if (null != inputSet)
        //             inputSet.AddTests(result, Templates);
        //     }
        //     return result;
        // }

        // public Collection<Hashtable> ExpandTests(Collection<TestDefinition> tests, Collection<ContentTemplate> templates, Hashtable roots)
        // {
        //     Collection<Hashtable> result = new Collection<Hashtable>();
        //     foreach (int rootId in roots.Keys.OfType<int>())
        //     {
        //         if (roots[rootId] is string)
        //         {
        //             string t = Path.GetTempFileName();
        //             File.Delete(t);
        //             roots[rootId] = Directory.CreateDirectory(t);
        //         }
        //     }

        //     return result;
        // }
    }
}
