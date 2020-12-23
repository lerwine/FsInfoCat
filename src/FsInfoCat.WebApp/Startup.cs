using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

namespace FsInfoCat.WebApp
{
    public class Startup
    {
        public const string CookieScheme = "FsInfoCat";
        public const string SettingsKey_ConnectionString = "FsInfoCat";
        public const string SettingsKey_DBPassword = "DBPassword";
        public const string UrlPath_ExceptionHandler = "/index.html#!/error";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            string cookieScheme = Configuration[CookieScheme];
            services.AddAuthentication(cookieScheme) // Sets the default scheme to cookies
                .AddCookie(cookieScheme, options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/login";
                });

            // Example of how to customize a particular instance of cookie options and
            // is able to also use other services.
            services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureFsInfoCatCookie>();

            string connectionString = Configuration.GetConnectionString(SettingsKey_ConnectionString);
            string pwd = Configuration[SettingsKey_DBPassword];
            if (!String.IsNullOrEmpty(pwd))
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                builder.Password = pwd;
                connectionString = builder.ConnectionString;
            }

            // Add framework services.
            services.AddDbContext<Data.FsInfoDataContext>(options =>
                options.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler(UrlPath_ExceptionHandler);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
