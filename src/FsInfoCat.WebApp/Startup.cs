using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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
        public const string UrlPath_ExceptionHandler = "/error.html#!/error";
        public const string UrlPath_DeniedHandler = "/error.html#!/denied";
        public const string UrlPath_LoginHandler = "/login.html";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAntiforgery();
            // string cookieScheme = Configuration[CookieScheme];
            services.AddAuthentication(CookieScheme) // Sets the default scheme to cookies
                .AddCookie(CookieScheme, options =>
                {
                    options.AccessDeniedPath = UrlPath_DeniedHandler;
                    options.LoginPath = UrlPath_LoginHandler;
                    // options.LogoutPath = UrlPath_LogoutHandler;
                });


            // services.AddAuthorization(options =>
            // {
            //     // This policy cannot succeed since the claim is never added
            //     options.AddPolicy("Impossible", policy =>
            //     {
            //         policy.AuthenticationSchemes.Add("Api");
            //         policy.RequireClaim("Never");
            //     });
            //     options.AddPolicy("Api", policy =>
            //     {
            //         policy.AuthenticationSchemes.Add("Api");
            //         policy.RequireClaim(ClaimTypes.NameIdentifier);
            //         policy.RequireClaim(ClaimTypes.Name);
            //     });
            //     options.AddPolicy("Api-Manager", policy =>
            //     {
            //         policy.AuthenticationSchemes.Add("Api");
            //         policy.Requirements.Add(new OperationAuthorizationRequirement { Name = "Edit" });
            //     });
            // });

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
