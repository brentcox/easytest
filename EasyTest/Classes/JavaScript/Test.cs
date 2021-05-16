using EasyTest.Classes.Scripts;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EasyTest.Classes.JavaScript
{
    public class Test : ITest
    {
        public List<TestResult> Results { get; } = new List<TestResult>();

        public void Run(string description, ScriptObject callBack)
        {
            Stopwatch sw = new Stopwatch();
            ScriptError error = null;
            try
            {
                sw.Start();
                callBack.Invoke(false);
                sw.Stop();
            }
            catch (ScriptEngineException e)
            {
                error = ScriptParserExtensions.ParseError(e, string.Empty);
                if (error.ErrorType == Enum.ErrorTypes.Script)
                {
                    throw;
                }
            }
            var result = new TestResult(description, error, DateTime.Now, sw.Elapsed);
            Results.Add(result);
            foreach (var formatter in TestResultFormatterFactory.GetFormatters())
            {
                formatter.Process(result);
            }

        }
    }
}
