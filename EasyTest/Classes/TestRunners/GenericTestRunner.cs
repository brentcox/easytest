using EasyTest.Classes.Scripts;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models.Results;
using EasyTest.Models.TestTypes;
using Microsoft.ClearScript.V8;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        private void ExecutePreRequestScripts(V8ScriptEngine engine, GenericTestType testType)
        {
            foreach (var script in testType.PreRequestScripts)
            {
                if (!engine.ExecuteScript(script))
                {
                    continue;
                }
            }
        }

        private List<ScriptTestResult> ExecuteTests(V8ScriptEngine engine, GenericTestType testType)
        {
            List<ScriptTestResult> results = new List<ScriptTestResult>();
            foreach (var script in testType.TestScripts)
            {
                if (engine.ExecuteTests(script))
                {
                    results.AddRange(this.test.Results);
                }
            }
            return results;
        }

        public async Task<TestRunnerResult> RunAsync(string name, BaseTestType test)
        {
            var engine = ScriptEngineFactory.GetEngine();
            SetupEngine(engine);
            GenericTestType testType = test as GenericTestType;
            var result = Timer.Time(
                () =>
                {
                    ExecutePreRequestScripts(engine, testType);
                    return ExecuteTests(engine, testType);
                });
            return await Task.FromResult(new TestRunnerResult(name, result.Duration, result.Result));
        }
    }
}
