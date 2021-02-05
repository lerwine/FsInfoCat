using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace DevHelper
{
    public class ChoiceValueDescription : IEquatable<ChoiceValueDescription>, IComparable<ChoiceValueDescription>
    {
        private readonly PSObject _value;
        private readonly ChoiceDescription _choiceDescription;
        public string Label { get { return _choiceDescription.Label; } }
        public string HelpMessage { get { return _choiceDescription.HelpMessage; } }
        public ChoiceValueDescription(object value, ChoiceDescription description)
        {
            _choiceDescription = description;
            _value = (value is null || value is PSObject) ? value as PSObject : PSObject.AsPSObject(value);
        }
        public ChoiceValueDescription(object value, string label) : this(value, new ChoiceDescription(label)) { }
        public ChoiceValueDescription(object value, string label, string helpMessage) : this(value, new ChoiceDescription(label, helpMessage)) { }
        public bool Equals(ChoiceValueDescription other)
        {
            return null != other && (ReferenceEquals(this, other) || (((other._value is null) ? _value is null : other._value.Equals(_value)) && base.Equals(this)));
        }
        public PSObject GetValue() { return _value; }
        public ChoiceDescription GetChoiceDescription() { return _choiceDescription; }
        public int CompareTo(ChoiceValueDescription other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            if (other._value is null)
                return (_value is null) ? 0 : 1;
            if (_value is null)
                return -1;
            int result = _value.CompareTo(other._value);
            if (result != 0 || ReferenceEquals(_choiceDescription, other._choiceDescription) || (result = _choiceDescription.Label.CompareTo(other._choiceDescription.Label)) != 0)
                return result;
            return (_choiceDescription.HelpMessage is null) ?
                ((other._choiceDescription.HelpMessage is null) ? 0 : -1) :
                ((other._choiceDescription.HelpMessage is null) ? 1 : _choiceDescription.HelpMessage.CompareTo(other._choiceDescription.HelpMessage));
        }
        public static ChoiceValueDescription Create(object obj)
        {
            if (obj is null)
                return null;
            PSObject psObject;
            if (obj is PSObject)
                obj = (psObject = (PSObject)obj).BaseObject;
            else
                psObject = PSObject.AsPSObject(obj);
            if (obj is ChoiceValueDescription)
                return obj as ChoiceValueDescription;
            if (obj is ChoiceDescription)
            {
                ChoiceDescription choiceDescription = (ChoiceDescription)obj;
                IEnumerable<PSPropertyInfo> properties = psObject.Properties.Match("Value", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance);
                PSPropertyInfo pi = properties.FirstOrDefault(p => null != p.Value);
                return new ChoiceValueDescription((null != pi) ? pi.Value : properties.Select(p => p.Value).DefaultIfEmpty(choiceDescription.Label), choiceDescription);
            }
            string label = null, helpMessage = null;
            object value = null;
            bool valueIsNull = false;
            if (obj is IDictionary)
            {
                IDictionary dictionary = (IDictionary)obj;
                if (dictionary.Contains("Label"))
                {
                    object a = dictionary["Label"];
                    if (null != a)
                    {
                        object o = (a is PSObject) ? ((PSObject)a).BaseObject : a;
                        label = ((o is string) ? (string)o : a.ToString()).Trim();
                    }
                }
                if (dictionary.Contains("HelpMessage"))
                {
                    object a = dictionary["HelpMessage"];
                    if (null != a)
                    {
                        object o = (a is PSObject) ? ((PSObject)a).BaseObject : a;
                        helpMessage = ((o is string) ? (string)o : a.ToString()).Trim();
                    }
                }
                if (dictionary.Contains("Value"))
                    valueIsNull = null == (value = dictionary["Value"]);
            }
            if (string.IsNullOrWhiteSpace(label))
            {
                IEnumerable<PSPropertyInfo> properties = psObject.Properties.Match("Label", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance);
                IEnumerable<string> sv = properties.Select(p =>
                {
                    if (p.Value is null)
                        return null;
                    object o = (p.Value is PSObject) ? ((PSObject)p.Value).BaseObject : p.Value;
                    return (o is string) ? (string)o : o.ToString();
                });
                label = sv.FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            }
            if (value is null && !valueIsNull)
                value = psObject.Properties.Match("Value", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance && null != p.Value).Select(p => p.Value).DefaultIfEmpty(label).First();
            if (string.IsNullOrWhiteSpace(label) && string.IsNullOrWhiteSpace(label = (value is null) ? null : value.ToString()) && string.IsNullOrWhiteSpace(label = obj.ToString()))
                throw new ArgumentException("Could not calculate a label string", "obj");
            if (string.IsNullOrWhiteSpace(helpMessage))
                helpMessage = psObject.Properties.Match("HelpMessage", PSMemberTypes.Properties).Where(p => p.IsGettable && p.IsInstance && null != p.Value)
                    .Select(p => (p.Value is string) ? (string)p.Value : p.Value.ToString()).FirstOrDefault(s => !string.IsNullOrWhiteSpace(s));
            if (value is null && !valueIsNull)
                value = label;
            return new ChoiceValueDescription((null != value || valueIsNull) ? value : label, (string.IsNullOrWhiteSpace(helpMessage)) ? new ChoiceDescription(label) : new ChoiceDescription(label.Trim(), helpMessage.Trim()));
        }
        public static Collection<ChoiceValueDescription> ToCollection(IEnumerable source)
        {
            Collection<ChoiceValueDescription> result = new Collection<ChoiceValueDescription>();
            if (null != source)
            {
                if (source is string)
                    result.Add(Create(source));
                else
                    foreach (object obj in source)
                    {
                        ChoiceValueDescription d = Create(obj);
                        if (null != d && !result.Contains(d))
                            result.Add(d);
                    }
            }
            return result;
        }
    }
}
