using EasyTest.Classes.Scripts;
using EasyTest.Interfaces;
using EasyTest.Models;
using Microsoft.ClearScript;
using System.Collections.Generic;

namespace EasyTest.Classes.JavaScript
{
    public class Test : ITest
    {
        Dictionary<string, ScriptError> results = new Dictionary<string, ScriptError>();

        public void Run(string description, ScriptObject callBack)
        {
            try
            {
                callBack.Invoke(false);
            }catch(ScriptEngineException e)
            {
                var result = ScriptParserExtensions.ParseError(e, string.Empty);
                if (result.ErrorType == Enum.ErrorTypes.Script)
                {
                    throw;
                }
                results.Add(description, result);
            }
        }
    }
}
