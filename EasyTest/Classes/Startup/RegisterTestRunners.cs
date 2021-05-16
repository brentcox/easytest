using EasyTest.Classes.JavaScript;
using EasyTest.Classes.TestRunner;
using EasyTest.Factories;
using System.Net.Http;

namespace EasyTest.Classes.Startup
{
    public class RegisterTestRunners
    {
        public void Register() => TestRunnerFactory.RegisterTestRunner<GenericTestRunner>(() => new GenericTestRunner(new Variables(), new Test(), new HttpClient()));
    }
}
