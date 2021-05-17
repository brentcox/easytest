using EasyTest.Classes.JavaScript;
using EasyTest.Classes.TestRunner;
using EasyTest.Factories;
using System.Net.Http;

namespace EasyTest.Classes.Startup
{
    public class RegisterTestRunners
    {
        public void Register()
        {
            TestRunnerFactory.RegisterTestRunner<GenericTestRunner>(
                (string testName) => new GenericTestRunner(
                    new Variables(),
                    new Test(testName),
                    new HttpClient()
                    )
                );
            TestRunnerFactory.RegisterTestRunner<RestApiTestRunner>(
                (string testName) => new RestApiTestRunner(
                    new Variables(),
                    new Test(testName),
                    new HttpClient()
                    )
                );
        }
    }
}
