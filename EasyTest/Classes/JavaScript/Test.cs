using EasyTest.Classes.Scripts;
using EasyTest.Factories;
using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.Results;
using Microsoft.ClearScript;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EasyTest.Classes.JavaScript
{
    public class Test : ITest
    {
        private readonly string name;

        public Test(string name)
        {
            this.name = name;
        }

        public Group TestGroup { get; set; }

        public List<ScriptTestResult> Results { get; } = new List<ScriptTestResult>();

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
            var result = new ScriptTestResult(name + "/" + description, error, DateTime.Now, sw.Elapsed);
            Results.Add(result);
            foreach (var formatter in TestResultFormatterFactory.GetFormatters())
            {
                formatter.Process(result);
            }

        }
    }
}
