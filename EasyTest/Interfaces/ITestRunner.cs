using EasyTest.Models.TestTypes;
using System.Threading.Tasks;

namespace EasyTest.Interfaces
{
    public interface ITestRunner<T> where T : BaseTestType
    {
        void Run(T test);
    }
}
