using EasyTest.Classes.Formatters;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.Results;
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
     
        public async Task RunAsync()
        {
            ITestResultFormatter formatter = new ConsoleResultFormatter();
            ProjectResults results = new ProjectResults(project.ProjectName, DateTime.Now, new List<GroupResults>());
            foreach (var testGroup in project.Groups) {
                var group = new GroupResults(testGroup.Name, new List<TestRunnerResult>());
                results.GroupResults.Add(group);
                formatter.ProcessHeader(group);
                foreach (var testConfig in testGroup.Tests.Where(a => targetTest == string.Empty || (a.Name.ToLower() == targetTest.ToLower())))
                {
                    Type type = Type.GetType($"EasyTest.Classes.TestRunner.{testConfig.Type}TestRunner");
                    var testRunner = TestRunnerFactory.GetRunner(type, testConfig.Name);
                    group.TestRunnerResults.Add(await testRunner.RunAsync(testConfig.Name, await ProjectFactory.LoadFileAsync<RestApiTestType>(testConfig.Path)));
                }
                formatter.ProcessSummary(group.Name, group.TestRunnerResults);
            }
            results.FinishedAt = DateTime.Now;
        }
    }
}
