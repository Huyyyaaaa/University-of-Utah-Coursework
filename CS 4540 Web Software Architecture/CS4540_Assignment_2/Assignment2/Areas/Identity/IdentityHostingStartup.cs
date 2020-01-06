using System;
using Assignment2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Assignment2.Areas.Identity.IdentityHostingStartup))]
namespace Assignment2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserRolesDB>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserRolesDBConnection")));

            services.AddDefaultIdentity<IdentityUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<UserRolesDB>();

                services.AddTransient<IEmailSender, EmailSender>();
            });

        }
    }
}