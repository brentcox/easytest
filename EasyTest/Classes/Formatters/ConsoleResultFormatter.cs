using EasyTest.Interfaces;
using EasyTest.Models;
using EasyTest.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTest.Classes.Formatters
{
    public class ConsoleResultFormatter : ITestResultFormatter
    {
        public void ProcessHeader(GroupResults groupResults)
        {
            Console.WriteLine("Executing Test Config - " + groupResults.Name);
            Console.WriteLine("");
        }

        public void Process(ScriptTestResult result)
        {
            var desc = result.TestName.Length > 40 ? result.TestName.Substring(0, 40) : result.TestName.PadRight(40);
            Console.WriteLine($"{desc} {(result.Error == null ? "Passed" : "Failed")} {result.Duration}" );
            var error = result.Error;
            if (error != null)
            {
                Console.WriteLine(error.FileName + " Line " + error.Row + " Column " + error.Column + ")");
                Console.WriteLine(error.Error);
            }
        }

        public void ProcessSummary(string groupName, List<TestRunnerResult> results)
        {
            Console.WriteLine("");
            var tests = results.SelectMany(a => a.TestResults).ToList();
            Console.WriteLine("Total Run     : " + tests.Count);
            Console.WriteLine("Total Passed  : " + tests.Count(a => a.Error == null));
            Console.WriteLine("Total Failed  : " + tests.Count(a => a.Error != null));
            Console.WriteLine("Total Runtime : " + new TimeSpan(tests.Sum(a => a.Duration.Ticks)));

            var errors = tests.Where(a => a.Error != null);
            if (errors.Count() > 0)
            {
                Console.WriteLine("");
                Console.WriteLine("Errors");
                Console.WriteLine("");
                foreach (var error in errors)
                {
                    Console.WriteLine(error.TestName);
                    Console.WriteLine(error.Error.FileName + " Line " + error.Error.Row + " Column " + error.Error.Column + ")");
                    Console.WriteLine(error.Error.Error);
                    Console.WriteLine("");
                }
            }
        }
    }
}
