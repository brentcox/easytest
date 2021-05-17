using EasyTest.Classes.JavaScript;
using EasyTest.Classes.Scripts;
using EasyTest.Exceptions;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models.Results;
using EasyTest.Models.TestTypes;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace EasyTest.Classes.TestRunner
{
    public class RestApiTestRunner : ITestRunner<BaseTestType>
    {
        private IVariables variables { get; }
        private ITest test { get; }
        private HttpClient httpClient { get; }

        public RestApiTestRunner(IVariables variables, ITest test, HttpClient httpClient)
        {
            this.variables = variables;
            this.test = test;
            this.httpClient = httpClient;
        }

        private void SetupEngine(V8ScriptEngine engine)
        {
            engine.AddHostObject("variables", variables);
            engine.AddHostObject("test", test);
            engine.AddHostObject("httpClient", httpClient);
        }

        private async Task<HttpResponseMessage> ExecuteGetAsync(HttpClient client, string url)
        {
            return await client.GetAsync(url);
        }

        private async Task<HttpResponseMessage> ExecutePostAsync(HttpClient client, string url, string body)
        {
            return await client.PostAsync(url, new StringContent(body));
        }

        private async Task<HttpResponseMessage> ExecutPutAsync(HttpClient client, string url, string body)
        {
            return await client.PutAsync(url, new StringContent(body));
        }

        private async Task<HttpResponseMessage> ExecutePatchAsync(HttpClient client, string url, string body)
        {
            return await client.PatchAsync(url, new StringContent(body));
        }

        private async Task<HttpResponseMessage> ExecuteAsync(HttpClient client,string method, string url, string body)
        {
            switch (method.ToUpper())
            {
                case "GET":
                    return await ExecuteGetAsync(client, url);
                case "POST":
                    return await ExecutePostAsync(client, url, body);
                case "PATCH":
                    return await ExecutePatchAsync(client, url, body);
                case "PUT":
                    return await ExecutPutAsync(client, url, body);
                default:
                    throw new HttpMethodNotSupported(method);
            }

        }

        private void ProcessHeaders(HttpClient client, string[] headers)
        {
            foreach (var header in headers)
            {
                var h = header.Split("=");
                string hName = variables.Parse(h[0]);
                string hValue = variables.Parse(h[1]);
                client.DefaultRequestHeaders.Add(hName, hValue);
            }
        }

        private void ExecutePreRequestScripts(V8ScriptEngine engine, string[] scripts)
        {
            foreach (var script in scripts)
            {
                if (!engine.ExecuteScript(script))
                {
                    continue;
                }
            }
        }

        private async Task<RestApiResponse> ExecuteRequest(HttpClient client, RestApiTestType restApiTest)
        {
            HttpResponseMessage result = await ExecuteAsync(client, restApiTest.Method, restApiTest.Url, restApiTest.Body);
            var content = await result.Content.ReadAsStringAsync();
            return new RestApiResponse((int)result.StatusCode, content);
        }

        private List<ScriptTestResult> ExecuteTests(V8ScriptEngine engine, RestApiTestType restApiTest)
        {
            List<ScriptTestResult> results = new List<ScriptTestResult>();
            foreach (var script in restApiTest.TestScripts)
            {
                if (engine.ExecuteTests(script))
                {
                    results.AddRange(this.test.Results);
                }
            }
            return results;

        }

        private async Task<(TimeSpan Duration, List<ScriptTestResult> Results)> ExecuteRequestAsync(V8ScriptEngine engine, RestApiTestType restApiTest)
        {
            HttpClient client = new HttpClient();
            ProcessHeaders(client, restApiTest.Headers);
            return await Timer.TimeAsync<List<ScriptTestResult>>(
               async () => {
                   var response = await ExecuteRequest(client, restApiTest);
                   engine.AddHostObject("restApiResponse", response);

                   return ExecuteTests(engine, restApiTest);
               }
           );
        }

        public async Task<TestRunnerResult> RunAsync(string name, BaseTestType test)
        {
            var engine = ScriptEngineFactory.GetEngine();
            SetupEngine(engine);
            var restApiTest = test as RestApiTestType;

            ExecutePreRequestScripts(engine, restApiTest.PreRequestScripts);
            
            var result = await ExecuteRequestAsync(engine, restApiTest);

            return await Task.FromResult(new TestRunnerResult(name, result.Duration, result.Results));
        }
    }
}
