using System;
using System.Net;
using System.Threading.Tasks;
using FunctionsBugRepro.Dependency;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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
        
        [Function("TestFunction")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "test")] HttpRequestData req)
        {
            _log.LogInformation("C# HTTP trigger function processed a request");

            await Task.Delay(TimeSpan.FromMilliseconds(100));

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync($"Result: {_dependency.GetRandomNumber()}");
            return response;

        }
    }
}
