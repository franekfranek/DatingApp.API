using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace DatingApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using(var scope = host.Services.CreateScope())
            {
                var serivces = scope.ServiceProvider;
                try
                {
                    var context = serivces.GetRequiredService<DataContext>();
                    var userManager = serivces.GetRequiredService<UserManager<User>>();
                    var roleManger = serivces.GetRequiredService<RoleManager<Role>>();
                    context.Database.Migrate(); //it will check every time we run application if there is a pending migration
                    //if yes it will create it for us
                    Seed.SeedUser(userManager, roleManger);
                    //this is usedful because if we mess up db we can just drop it and start again with a clean one
                }
                catch (Exception ex)
                {
                    var logger = serivces.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error accured during migration");
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
