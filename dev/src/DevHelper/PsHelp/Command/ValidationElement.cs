using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DevHelper.PsHelp.Command
{
    public class ValidationElement : PropertyChangeSupport
    {
        private const string AttributeName_minCount = "minCount";
        private const string AttributeName_maxCount = "maxCount";
        private const string AttributeName_minLength = "minLength";
        private const string AttributeName_maxLength = "maxLength";
        private const string AttributeName_minRange = "minRange";
        private const string AttributeName_maxRange = "maxRange";
        private const string AttributeName_pattern = "pattern";

        private int? _minCount;
        private int? _maxCount;
        private int? _minLength;
        private int? _maxLength;
        private string _minRange;
        private string _maxRange;
        private string _pattern;

        [XmlAttribute(AttributeName_minCount)]
        public int? MinCount
        {
            get => _minCount;
            set => CheckPropertyChange(nameof(MinCount), _minCount, value, n => _minCount = n);
        }

        [XmlAttribute(AttributeName_maxCount)]
        public int? MaxCount
        {
            get => _maxCount;
            set => CheckPropertyChange(nameof(MaxCount), _maxCount, value, n => _maxCount = n);
        }

        [XmlAttribute(AttributeName_minLength)]
        public int? MinLength
        {
            get => _minLength;
            set => CheckPropertyChange(nameof(MinLength), _minLength, value, n => _minLength = n);
        }

        [XmlAttribute(AttributeName_maxLength)]
        public int? MaxLength
        {
            get => _maxLength;
            set => CheckPropertyChange(nameof(MaxLength), _maxLength, value, n => _maxLength = n);
        }

        [XmlAttribute(AttributeName_minRange)]
        public string MinRange
        {
            get => _minRange;
            set => CheckPropertyChange(nameof(MinRange), _minRange, value, n => _minRange = n);
        }

        [XmlAttribute(AttributeName_maxRange)]
        public string MaxRange
        {
            get => _maxRange;
            set => CheckPropertyChange(nameof(MaxRange), _maxRange, value, n => _maxRange = n);
        }

        [XmlAttribute(AttributeName_pattern)]
        public string Pattern
        {
            get => _pattern;
            set => CheckPropertyChange(nameof(Pattern), _pattern, value, n => _pattern = n);
        }
    }
}
