using ContactManager.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactManager
{
    public class Program
    {
        public static IConfiguration Configuration
        {
            get; set;
        }

        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
                // Set password with the Secret Manager tool.
                // dotnet user-secrets set SeedUserPW <pw>

                //var testUserPw = Configuration["SeedUserPW"];
                var testUserPw = "PaSSw0rd123";

                SeedData.Initialize(services, testUserPw).Wait();
            }

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}