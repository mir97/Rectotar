using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Rectotarat.Models;
using Microsoft.EntityFrameworkCore;
using Rectotarat.Data;
using Microsoft.AspNetCore.Mvc;

namespace Rectotarat
{
    public class Startup
    {
        public Startup(IHostingEnvironment configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(configuration.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("config.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connection = Configuration.GetConnectionString("DefaultConnection");

            services.AddMvc();


            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<RectoratContext>()
            .AddDefaultTokenProviders();

            services.AddDbContext<RectoratContext>(options =>
               options.UseMySQL(Configuration.GetConnectionString("CouncilConnectionMysql")));


        

            services.AddMvc();
        }



        public void Configure(IApplicationBuilder app, RectoratContext context)
        {
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                   name: "default1",
                   template: "Indicator",
                   defaults: new { controller = "Indicator", action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            DbInitializer.Initialize(context);
        }
    }
}
