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
    [Cmdlet(VerbsData.Expand,"TestData")]
    [OutputType(typeof(Hashtable[]))]
    public class ExpandTestDataCommand : PSCmdlet
    {
        public const string KEY_NAME_TEST_DESCRIPTION = "TestDescription";
        public const string KEY_NAME_RETURNS = "Returns";
        public const string KEY_NAME_JOB_NAME = "JobName";
        public const string KEY_NAME_ROOT_PATH = "RootPath";
        public const string KEY_NAME_MAX_DEPTH = "MaxDepth";
        public const string KEY_NAME_MAX_ITEMS = "MaxItems";

        [Parameter(Mandatory = true, Position = 0)]
        public InputSet InputSet { get; set; }

        [Parameter(Mandatory = true, Position = 0)]
        public string[] inputSet { get; set; }

        protected override void ProcessRecord()
        {
            int index = -1;
            foreach (TestDefinition testDefinition in InputSet.Tests)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add(KEY_NAME_JOB_NAME, "(" + InputSet.Description + ") # " + (++index));
                hashtable.Add(KEY_NAME_ROOT_PATH, testDefinition.MaxDepth);
                hashtable.Add(KEY_NAME_MAX_DEPTH, testDefinition.MaxDepth);
                hashtable.Add(KEY_NAME_MAX_ITEMS, testDefinition.MaxItems);
                hashtable.Add(
                    KEY_NAME_TEST_DESCRIPTION,
                    (testDefinition.MaxDepth.HasValue) ?
                        (
                            (testDefinition.MaxItems.HasValue) ?
                                "Start-FsCrawlJob -MaxDepth " + testDefinition.MaxDepth.Value + " -MaxItems " + testDefinition.MaxItems.Value :
                                "Start-FsCrawlJob -MaxDepth " + testDefinition.MaxDepth.Value
                        ) :
                        (
                            (testDefinition.MaxItems.HasValue) ?
                                "Start-FsCrawlJob -MaxItems " + testDefinition.MaxItems.Value : "Start-FsCrawlJob"
                        )
                );
                WriteObject(hashtable, false);
            }
        }
    }
}
