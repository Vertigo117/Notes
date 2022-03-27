using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Notes.Api.Extensions;
using System;

namespace Notes.Api
{
    public class Program
    {
        private const string EnvironmentVariableKey = "ASPNETCORE_ENVIRONMENT";
        private const string DevelopmentEnvironmentVariableValue = "Development";

        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();

            if (!IsDevelopment())
            {
                host.ApplyMigrations();
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static bool IsDevelopment()
        {
            return Environment.GetEnvironmentVariable(EnvironmentVariableKey) == DevelopmentEnvironmentVariableValue;
        }
    }
}
