using EasyTest.Models;
using EasyTest.Models.Results;
using System.Collections.Generic;

namespace EasyTest.Interfaces
{
    public interface ITestResultFormatter
    {
        void ProcessHeader(GroupResults groupResults);

        void Process(ScriptTestResult result);
        
        void ProcessSummary(string groupName, List<TestRunnerResult> groupResults);
    }
}
