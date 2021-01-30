using System;
using CourseProjectItr.Areas.Identity.Data;
using CourseProjectItr.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CourseProjectItr.Areas.Identity.IdentityHostingStartup))]
namespace CourseProjectItr.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CourseDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CourseProjectItrDbProd")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<CourseDbContext>();
            });
        }
    }
}