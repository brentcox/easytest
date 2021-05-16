using EasyTest.Classes.Scripts;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.TestTypes;
using Microsoft.ClearScript.V8;
using Serilog;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace EasyTest.Classes.TestRunner
{
    public class GenericTestRunner : ITestRunner<BaseTestType>
    {
        private IVariables variables { get; }
        private ITest test { get; }
        private HttpClient httpClient { get; }

        public GenericTestRunner(IVariables variables, ITest test, HttpClient httpClient)
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

        public async Task<List<TestResult>> RunAsync(BaseTestType test)
        {
            var engine = ScriptEngineFactory.GetEngine();
            SetupEngine(engine);
            RestApiTestType testType = test as RestApiTestType;
            List<TestResult> results = new List<TestResult>();
            foreach (var script in testType.PreRequestScripts)
            {
                if (!engine.ExecuteScript(script))
                {
                    continue;
                }
            }
            foreach (var script in testType.TestScripts)
            {
                if (engine.ExecuteTests(script))
                {
                    results.AddRange(this.test.Results);
                }
            }
            return await Task.FromResult(results);
        }
    }
}
