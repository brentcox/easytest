using EasyTest.Classes.Formatters;
using EasyTest.Classes.Startup;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.TestTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyTest.Classes
{
    public class ProjectRunner : IProjectRunner
    {
        private readonly Project project;
        private readonly string targetTest;

        public ProjectRunner(Project project, string targetTest)
        {
            this.project = project;
            this.targetTest = targetTest;
        }

        List<TestResult> testResults = new List<TestResult>();

        public async Task RunAsync()
        {
            foreach (var group in project.Groups) {
                foreach (var test in group.Tests.Where(a => targetTest == string.Empty || (a.Name.ToLower() == targetTest.ToLower())))
                {
                    Type type = Type.GetType($"EasyTest.Classes.TestRunner.{test.Type}TestRunner");
                    var testRunner = TestRunnerFactory.GetRunner(type);
                    testResults.AddRange(await testRunner.RunAsync(await ProjectFactory.LoadFileAsync<RestApiTestType>(test.Path)));
                }
            }
            foreach (var formatter in TestResultFormatterFactory.GetFormatters())
            {
                formatter.ProcessSummary(testResults);
            }
        }
    }
}
