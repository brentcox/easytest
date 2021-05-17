using EasyTest.Models;
using EasyTest.Models.Results;
using EasyTest.Models.TestTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTest.Interfaces
{
    public interface ITestRunner<T> where T : BaseTestType
    {
        Task<TestRunnerResult> RunAsync(string name, T test);
    }
}
