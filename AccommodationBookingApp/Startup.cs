using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace AccommodationBookingApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer("Server=DESKTOP-HJRJMK9\\SQLEXPRESS;Database=AccommodationBookingApp;Trusted_Connection=True;MultipleActiveResultSets=True"/*AppConfiguration.sqlConnectionString*/));

            services.AddIdentity<ApplicationUser, IdentityRole>(IdentityOptions =>
            {
                    IdentityOptions.Password.RequireNonAlphanumeric = false;
                    IdentityOptions.Password.RequiredUniqueChars = 0;
                    IdentityOptions.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<DatabaseContext>()
                  .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "AcommodationBookingAppCookie";
                config.LoginPath = "/Login";
                config.AccessDeniedPath = "/AccessDenied";
            });
            services.AddAuthenticationCore();
            services.AddAuthorizationCore();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //this dosen't redirect you to the error page, but instead issues a proper 404/400 error and keeps the original URL
                app.UseStatusCodePagesWithReExecute("/{0}");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
