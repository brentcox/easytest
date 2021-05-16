using EasyTest.Interfaces;
using EasyTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyTest.Classes.Formatters
{
    public class ConsoleResultFormatter : ITestResultFormatter
    {
        public void Process(TestResult result)
        {
            Console.WriteLine("Executed : " + result.TestName);
            Console.WriteLine("Result   : " + (result.Error == null ? "Passed" : "Failed"));
            Console.WriteLine("Duration : " + result.Duration.ToString());
            var error = result.Error;
            if (error != null)
            {
                Console.WriteLine(error.FileName + " Line " + error.Row + " Column " + error.Column + ")");
                Console.WriteLine(error.Error);
            }
            Console.WriteLine("");
        }

        public void ProcessSummary(List<TestResult> results)
        {
            Console.WriteLine("Test Summary");
            Console.WriteLine("");
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("| Name                                     | Result    | Execution        |");
            Console.WriteLine("---------------------------------------------------------------------------");
            foreach (var result in results)
            {
                var desc = result.TestName.Length > 40 ? result.TestName.Substring(0, 40) : result.TestName.PadRight(40); 
                Console.WriteLine("| "+desc+" | "+(result.Error==null?"Passed":"Failed").ToString().PadRight(10)+"| "+result.Duration.ToString().PadRight(15) + " | ");
            }
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Total Run     : " + results.Count);
            Console.WriteLine("Total Passed  : " + results.Count(a => a.Error == null));
            Console.WriteLine("Total Failed  : " + results.Count(a => a.Error != null));
            Console.WriteLine("Total Runtime : " + new TimeSpan(results.Sum(a => a.Duration.Ticks)));

            var errors = results.Where(a => a.Error != null);
            if (errors.Count() > 0) {
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
