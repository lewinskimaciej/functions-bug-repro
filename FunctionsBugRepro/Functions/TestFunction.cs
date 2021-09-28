using System;
using System.IO;
using System.Threading.Tasks;
using FunctionsBugRepro.Dependency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionsBugRepro.Functions
{
    public class TestFunction
    {
        private readonly ILogger<TestFunction> _log;
        private readonly IDependency _dependency;

        public TestFunction(ILoggerFactory loggerFactory, IDependency dependency)
        {
            _dependency = dependency
                          ?? throw new ArgumentNullException("Failed to inject IDependency");

            _log = loggerFactory.CreateLogger<TestFunction>()
                   ?? throw new ArgumentNullException("Failed to create a logger");
        }
        
        [FunctionName("TestFunction")]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequest req)
        {
            _log.LogInformation("C# HTTP trigger function processed a request");

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            return new OkObjectResult($"Result: {_dependency.GetRandomNumber()}");

        }
    }
}
