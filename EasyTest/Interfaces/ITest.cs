using EasyTest.Models;
using EasyTest.Models.Results;
using Microsoft.ClearScript;
using System.Collections.Generic;

namespace EasyTest.Interfaces
{
    public interface ITest
    {
        List<ScriptTestResult> Results { get; }
        void Run(string description, ScriptObject callBack);
    }
}
