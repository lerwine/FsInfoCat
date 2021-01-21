using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class InputSet
    {
        public const string ROOT_ELEMENT_NAME = "InputSet";
        private string _description = "";
        private Collection<RootDefinition> _roots = null;
        private Collection<TestDefinition> _tests = null;

        [XmlAttribute()]
        public string Description
        {
            get => _description;
            set => _description = (null == value) ? "" : value;
        }

        [XmlArray()]
        [XmlArrayItem(RootDefinition.ROOT_ELEMENT_NAME)]
        public Collection<RootDefinition> Roots
        {
            get
            {
                Collection<RootDefinition> items = _roots;
                if (null == items)
                    _roots = items = new Collection<RootDefinition>();
                return items;
            }
            set => _roots = value;
        }

        [XmlArray()]
        [XmlArrayItem(TestDefinition.ROOT_ELEMENT_NAME)]
        public Collection<TestDefinition> Tests
        {
            get
            {
                Collection<TestDefinition> items = _tests;
                if (null == items)
                    _tests = items = new Collection<TestDefinition>();
                return items;
            }
            set => _tests = value;
        }

        // internal void AddTests(Collection<Hashtable> result, Collection<ContentTemplate> templates)
        // {
        //     string description = Description;
        //     Hashtable setHashtable = new Hashtable();
        //     setHashtable.Add(EXPANDED_TEST_DATA_DESCRIPTION, Description);
        //     Collection<Hashtable> testHashtables = new Collection<Hashtable>();
        //     Hashtable rootsHashtable = new Hashtable();
        //     Collection<ContentTemplate> templatesCollection = new Collection<ContentTemplate>();
        //     foreach (RootDefinition root in Roots)
        //     {
        //         if (null == root)
        //             continue;
        //         Hashtable h = new Hashtable();

        //         rootsHashtable.Add(root.ID, new Collection<string>(root.TemplateRefs.Select(t =>
        //         {
        //             ContentTemplate contentTemplate = templates.FirstOrDefault(c => c.ID == t);
        //             if (null == contentTemplate)
        //                 throw new InvalidOperationException("Template ID not found (" + t + " in InputSet['" + Description + "'].Roots[" + r.ID + "])");
        //             if (!templatesCollection.Contains(contentTemplate))
        //                 templatesCollection.Add(contentTemplate);
        //             return contentTemplate;
        //         }).Select(c => c.FileName).Distinct().ToArray()));
        //     }
        //     setHashtable.Add(EXPANDED_TEST_DATA_ROOTS, rootsHashtable);
        //     setHashtable.Add(EXPANDED_TEST_DATA_TEMPLATES, templatesCollection);
        //     // if (description.Contains(",") || description.Contains("+"))
        //     //     description = "(" + description + ")";
        //     // foreach (TestDefinition test in Tests)
        //     // {
        //     //     index++;
        //     //     if (null == test)
        //     //         continue;
        //     //     Hashtable item = new Hashtable();
        //     //     StringBuilder sb = new StringBuilder(description).Append(" | Import-FsCrawlJobTestData");
        //     //     if (test.MaxDepth.HasValue)
        //     //         sb.Append(" -MaxDepth ").Append(test.MaxDepth.Value);
        //     //     if (test.MaxItems.HasValue)
        //     //         sb.Append(" -MaxItems ").Append(test.MaxItems.Value);
        //     //     item.Add(EXPANDED_TEST_DATA_DESCRIPTION, sb.ToString());
        //     //     item.Add(EXPANDED_TEST_DATA_RETURNS, "{ " + (string.Join(", ",
        //     //         test.Expected.Select(e =>
        //     //         ((e.FileCount ==1 ) ? "1 file; " : e.FileCount + " files; ") +
        //     //         ((e.FolderCount ==1 ) ? "1 folder" : e.FolderCount + " folders")
        //     //     ).ToArray()) + " }"));
        //     //     item.Add(EXPANDED_TEST_DATA_MAX_DEPTH, test.MaxDepth);
        //     //     item.Add(EXPANDED_TEST_DATA_MAX_ITEMS, test.MaxItems);
        //     //     testHashtables.Add(item);
        //     // }
        //     setHashtable.Add(EXPANDED_TEST_DATA_TESTDEFINITIONS, Tests);
        //     result.Add(setHashtable);
        // }
        // public const string EXPANDED_TEST_DATA_DESCRIPTION = "Description";
        // // public const string EXPANDED_TEST_DATA_RETURNS = "Returns";
        // // public const string EXPANDED_TEST_DATA_MAX_DEPTH = "MaxDepth";
        // // public const string EXPANDED_TEST_DATA_MAX_ITEMS = "MaxItems";
        // public const string EXPANDED_TEST_DATA_TESTDEFINITIONS = "TestDefinitions";
        // public const string EXPANDED_TEST_DATA_ROOTS = "Roots";
        // public const string EXPANDED_TEST_DATA_TEMPLATES = "Templates";
    }
}
