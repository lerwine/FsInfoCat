using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace DevHelperUI.RegexBuilder
{
    public class PatternGroupItem : DependencyObject
    {
        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register("Number", typeof(int), typeof(PatternGroupItem),
            new PropertyMetadata(0));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(nameof(Name), typeof(string), typeof(PatternGroupItem),
            new PropertyMetadata(""));

        public int MatchCount
        {
            get { return (int)GetValue(MatchCountProperty); }
            set { SetValue(MatchCountProperty, value); }
        }

        public static readonly DependencyProperty MatchCountProperty = DependencyProperty.Register("MatchCount", typeof(int), typeof(PatternGroupItem),
            new PropertyMetadata(0));

        public static IEnumerable<PatternGroupItem> Create(Regex regex)
        {
            if (regex is null)
                return Array.Empty<PatternGroupItem>();
            return regex.GetGroupNumbers().Select(n => new PatternGroupItem { Number = n, Name = regex.GroupNameFromNumber(n) });
        }
    }
}
