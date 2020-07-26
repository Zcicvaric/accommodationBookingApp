using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccommodationBookingApp.DataAccess.DataContext;
using AccommodationBookingApp.DataAccess.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            options.UseSqlServer(AppConfiguration.sqlConnectionString));
            
            services.AddIdentityCore<ApplicationUser>()
                    .AddEntityFrameworkStores<DatabaseContext>()
                    .AddDefaultTokenProviders();

            //services.AddAuthenticationCore();
            //services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();
            //app.UseAuthorization();
            //app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
