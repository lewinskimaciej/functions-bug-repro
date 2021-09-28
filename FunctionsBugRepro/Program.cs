using System;
using FunctionsBugRepro.Dependency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FunctionsBugRepro
{
    public static class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(services =>
                {
                    var configuration = ConfigureAppConfiguration(services.BuildServiceProvider());

                    services.AddSingleton(configuration);
                    services.AddSingleton<IDependency, RandomDependency>();
            
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddDebug();
                        loggingBuilder.AddConsole();
                    });
                })
                .Build();
 
            host.Run();
        }
        
        private static IConfiguration ConfigureAppConfiguration(IServiceProvider serviceProvider)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true)
                .AddEnvironmentVariables();

            var existingConfig = serviceProvider.GetService<IConfiguration>();
            if (existingConfig != null)
            {
                configBuilder.AddConfiguration(existingConfig);
            }

            return configBuilder.Build();
        }
    }
}
