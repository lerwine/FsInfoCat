using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public abstract class DbSettingsAttribute : ValidationAttribute
    {
        protected object GetRawSettingsValue(string key) => DBSettings.Default[key];

        protected T GetSettingsValue<T>(string key) => (T)DBSettings.Default[key];


        protected bool TryGetSettingsValue<T>(string key, out T value)
        {
            object obj = DBSettings.Default[key];
            return Coersion<T>.Default.TryCoerce(obj, out value);
        }

        protected bool TryConvertSettingsValue<T>(string key, out T value)
        {
            object obj = DBSettings.Default[key];
            return Coersion<T>.Default.TryConvert(obj, out value);
        }
    }
}
