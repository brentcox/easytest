using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.TestTypes;
using System;
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
            foreach (var group in project.Groups) {
                foreach (var test in group.Tests) {
                    Type type = Type.GetType($"EasyTest.Classes.TestRunner.{test.Type}TestRunner");
                    var testRunner = TestRunnerFactory.GetRunner(type);

                    //TODO: Need to load test here that needs running
                    testRunner.Run(await ProjectFactory.LoadFileAsync<GenericTestType>(test.Path));
                }
            }
        }
    }
}
