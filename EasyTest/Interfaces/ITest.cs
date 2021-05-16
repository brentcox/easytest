using EasyTest.Models;
using Microsoft.ClearScript;
using System.Collections.Generic;

namespace EasyTest.Interfaces
{
    public interface ITest
    {
        List<TestResult> Results { get; }
        void Run(string description, ScriptObject callBack);
    }
}
