using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public class MatchItem : CaptureItem
    {
        public int ItemNumber { get; }
        public List<GroupResult> Groups { get; }
        public MatchItem(Match match, int itemNumber = 0) : base(match)
        {
            ItemNumber = itemNumber;
            Groups = match.Groups.Cast<Group>().Select(g => new GroupResult(g)).ToList();
        }

        protected override XElement CreateElement() => new XElement("Match");

        protected override void SetElementContent(XElement element)
        {
            if (ItemNumber > 0)
                element.SetAttributeValue(nameof(ItemNumber), ItemNumber);
            element.SetAttributeValue(nameof(Success), Success);
            if (Success)
            {
                element.SetAttributeValue(nameof(Index), Index);
                element.SetAttributeValue(nameof(Length), Length);
                element.Add(new XElement(nameof(Match.Value), new XCData(RawValue)));
                if (!(Groups is null))
                    foreach (GroupResult group in Groups.Where(g => !(g is null)))
                        element.Add(group.ToXElement());
            }
        }
    }
}
