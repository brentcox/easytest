using System;

namespace EasyTest.Exceptions
{
    public class TestRunnerNotRegistered : Exception
    {
        public TestRunnerNotRegistered(string message) : base(message)
        {
        }
    }
}
