using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Management.Automation.Runspaces;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Text;
using Microsoft.PowerShell.Commands;
using System.IO;

namespace TestHelper
{
    [Cmdlet(VerbsData.Expand,"InputSetData")]
    [OutputType(typeof(Hashtable[]))]
    public class ExpandInputSetDataCommand : PSCmdlet
    {
        public const string KEY_NAME_DESCRIPTION = "Description";
        public const string KEY_NAME_ROOTS = "Roots";
        public const string KEY_NAME_INPUT_SET = "InputSet";
        public const string KEY_NAME_ID = "ID";
        public const string KEY_NAME_TEMPLATE_NAMES = "TemplateNames";
        public const string KEY_NAME_TEMP_PATH = "TempPath";

        [Parameter(Mandatory = true, Position = 0)]
        [ValidateNotNull]
        public FsCrawlJobTestData TestData { get; set; }

        protected override void ProcessRecord()
        {
            foreach (InputSet inputSet in TestData.InputSets)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add(
                    KEY_NAME_DESCRIPTION,
                    (inputSet.Description.Contains(",") || inputSet.Description.Contains("+")) ?
                        "(" + inputSet.Description + ")" : inputSet.Description
                );
                hashtable.Add(KEY_NAME_ROOTS, new Collection<Hashtable>(inputSet.Roots.Select(r =>
                {
                    Hashtable rh = new Hashtable();
                    rh.Add(KEY_NAME_ID, r.ID);
                    rh.Add(KEY_NAME_TEMPLATE_NAMES, new Collection<string>(
                        TestData.Templates.Where(t => r.TemplateRefs.Contains(t.ID)).Select(t => t.FileName).ToArray())
                    );
                    rh.Add(KEY_NAME_TEMP_PATH, Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("n")));
                    return rh;
                }).ToArray()));
                hashtable.Add(KEY_NAME_INPUT_SET, inputSet);
                WriteObject(hashtable, false);
            }
        }
    }
}
