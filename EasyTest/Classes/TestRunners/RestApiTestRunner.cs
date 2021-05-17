using EasyTest.Classes.JavaScript;
using EasyTest.Classes.Scripts;
using EasyTest.Exceptions;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models.Results;
using EasyTest.Models.TestTypes;
using Microsoft.ClearScript.V8;
using System.Collections.Generic;
using System.Diagnostics;
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

        public async Task<TestRunnerResult> RunAsync(string name, BaseTestType test)
        {
            var engine = ScriptEngineFactory.GetEngine();
            SetupEngine(engine);
            RestApiTestType respApiTest = test as RestApiTestType;
            List<ScriptTestResult> results = new List<ScriptTestResult>();
            foreach (var script in respApiTest.PreRequestScripts)
            {
                if (!engine.ExecuteScript(script))
                {
                    continue;
                }
            }

            HttpClient client = new HttpClient();
            foreach (var header in respApiTest.Headers)
            {
                var h = header.Split("=");
                client.DefaultRequestHeaders.Add(h[0], h[1]);
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            HttpResponseMessage result = await ExecuteAsync(client, respApiTest.Method, respApiTest.Url, respApiTest.Body);

            var content = await result.Content.ReadAsStringAsync();
            var response = new RestApiResponse((int)result.StatusCode, content);
            sw.Stop();
            engine.AddHostObject("restApiResponse", response);

            foreach (var script in respApiTest.TestScripts)
            {
                if (engine.ExecuteTests(script))
                {
                    results.AddRange(this.test.Results);
                }
            }
            sw.Stop();
            return await Task.FromResult(new TestRunnerResult(name, sw.Elapsed, results));
        }
    }
}
