using EasyTest.Classes.Scripts;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.Results;
using EasyTest.Models.TestTypes;
using Microsoft.ClearScript.V8;
using Serilog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public async Task<TestRunnerResult> RunAsync(string name, BaseTestType test)
        {
            var engine = ScriptEngineFactory.GetEngine();
            SetupEngine(engine);
            GenericTestType testType = test as GenericTestType;
            List<ScriptTestResult> results = new List<ScriptTestResult>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
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
            sw.Stop();
            return await Task.FromResult(new TestRunnerResult(name, sw.Elapsed, results));
        }
    }
}
