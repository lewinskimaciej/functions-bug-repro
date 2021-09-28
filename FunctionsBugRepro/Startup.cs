using System;
using FunctionsBugRepro;
using FunctionsBugRepro.Dependency;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

[assembly: FunctionsStartup(typeof(Startup))]
namespace FunctionsBugRepro
{
    public class Startup : FunctionsStartup
    {
        private IConfiguration Configuration { get; set; }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            Configuration = ConfigureAppConfiguration(builder.Services.BuildServiceProvider());

            builder.Services.AddSingleton(Configuration);
            builder.Services.AddSingleton<IDependency, RandomDependency>();
            
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddDebug();
                loggingBuilder.AddConsole();
            });
        }
        
        private static IConfigurationRoot ConfigureAppConfiguration(IServiceProvider serviceProvider)
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
