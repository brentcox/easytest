using EasyTest.Models;
using EasyTest.Models.TestTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyTest.Interfaces
{
    public interface ITestRunner<T> where T : BaseTestType
    {
        Task<List<TestResult>> RunAsync(T test);
    }
}
