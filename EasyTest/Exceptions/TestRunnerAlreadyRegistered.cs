using System;

namespace EasyTest.Exceptions
{
    public class TestRunnerAlreadyRegistered : Exception
    {
        public TestRunnerAlreadyRegistered(string message) : base(message)
        {
        }
    }
}
