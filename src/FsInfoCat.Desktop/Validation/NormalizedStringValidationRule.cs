using System.Globalization;
using System.Windows.Controls;

namespace FsInfoCat.Desktop.Validation
{
    public class NormalizedStringValidationRule : ValidationRuleBase
    {
        private int _maxLengthValue = DbConstants.DbColMaxLen_LongName;
        private string _maxLengthErrorMessage = FsInfoCat.Properties.Resources.ErrorMessage_DisplayNameLength;
        private bool _isRequired;
        private string _requiredErrorMessage = $"* {FsInfoCat.Properties.Resources.DisplayName_Required}";

        public int MaxLengthValue
        {
            get => _maxLengthValue;
            set
            {
                if (_maxLengthValue == value)
                    return;
                _maxLengthValue = value;
                RaisePropertyChanged();
            }
        }

        public string MaxLengthErrorMessage
        {
            get => _maxLengthErrorMessage;
            set
            {
                string text = value.AsWsNormalizedOrEmpty();
                if (_maxLengthErrorMessage == text)
                    return;
                _maxLengthErrorMessage = text;
                RaisePropertyChanged();
            }
        }

        public bool IsRequired
        {
            get => _isRequired;
            set
            {
                if (_isRequired == value)
                    return;
                _isRequired = value;
                RaisePropertyChanged();
            }
        }

        public string RequiredErrorMessage
        {
            get => _requiredErrorMessage;
            set
            {
                string text = value.AsWsNormalizedOrEmpty();
                if (_requiredErrorMessage == text)
                    return;
                _requiredErrorMessage = text;
                RaisePropertyChanged();
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string text = (value as string).AsWsNormalizedOrEmpty();
            if (text.Length == 0)
            {
                if (IsRequired)
                    return new ValidationResult(false, RequiredErrorMessage);
            }
            else if (text.Length > MaxLengthValue)
                return new ValidationResult(false, MaxLengthErrorMessage);
            return ValidationResult.ValidResult;
        }
    }
}
