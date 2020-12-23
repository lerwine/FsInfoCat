using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace FsInfoCat.WebApp
{
    public class ConfigureFsInfoCatCookie : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        // You can inject services here
        public ConfigureFsInfoCatCookie()
        {
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            if (name == Startup.CookieScheme)
                options.LoginPath = "/Login.html";
        }

        public void Configure(CookieAuthenticationOptions options)
            => Configure(Options.DefaultName, options);
    }
}
