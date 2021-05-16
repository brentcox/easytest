using EasyTest.Classes.Scripts;
using EasyTest.Interfaces;
using EasyTest.Models.TestTypes;
using Microsoft.ClearScript.V8;
using Serilog;
using System.IO;
using System.Net.Http;
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
            engine.AddHostType("log", typeof(Log));
            engine.AddHostType("file", typeof(File));
            engine.AddHostType("assert", typeof(Assert));
            engine.AddHostObject("test", test);
            engine.AddHostObject("httpClient", httpClient);
        }


        public void Run(BaseTestType test)
        {
            using (var engine = new V8ScriptEngine())
            {
                SetupEngine(engine);
                GenericTestType testType = test as GenericTestType;
                if (!engine.ExecuteScript(testType.PreRequestScript))
                {
                    return;
                }
                if (!engine.ExecuteTests(testType.TestScript))
                {
                    return;
                }
            }
        }
    }
}
