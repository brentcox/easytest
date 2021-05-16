using EasyTest.Models;
using System.Collections.Generic;

namespace EasyTest.Interfaces
{
    public interface ITestResultFormatter
    {
        void Process(TestResult result);
        
        void ProcessSummary(List<TestResult> results);
    }
}
