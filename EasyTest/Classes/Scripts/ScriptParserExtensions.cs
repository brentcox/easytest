using EasyTest.Enum;
using EasyTest.Models;
using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;
using Serilog;
using System.IO;

namespace EasyTest.Classes.Scripts
{
    public static class ScriptParserExtensions
    {
        public static ScriptError ParseError(ScriptEngineException error, string fileName)
        {
            if (error.InnerException is ScriptEngineException)
            {
                int start = error.ErrorDetails.IndexOf("at Script [") + "at Script [".Length;
                int end = error.ErrorDetails.IndexOf("->") - start;
                var lines = error.ErrorDetails.Substring(start, end).Split(':');
                var line = lines[1];
                var character = lines[2].Trim();

                int row = int.Parse(line);
                int col = int.Parse(character);

                var errorType = error.Message.Contains("Assert.") ? ErrorTypes.Test : ErrorTypes.Script;

                return new ScriptError(error.Message, row, col, fileName, errorType);

                //Log.Error("Error processing script {fileName}({line}-{character}) {error}", fileName, line, character, error.Message);
            }
            return null;

        }

        const string errorMessageFormat = "Error processing script {fileName}({line}-{character}) {error}";

        public static bool ExecuteScript(this V8ScriptEngine engine, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException($"File not found {fileName}");
                }
                var script = File.ReadAllText(fileName);
                try
                {
                    engine.Execute(script);
                }
                catch (ScriptEngineException e)
                {
                    var error = ParseError(e, fileName);
                    Log.Error(errorMessageFormat, error.FileName, error.Row, error.Column, error.Error);
                    return false;
                }
            }
            return true;
        }

        public static bool ExecuteTests(this V8ScriptEngine engine, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                if (!File.Exists(fileName))
                {
                    throw new FileNotFoundException($"File not found {fileName}");
                }
                var script = File.ReadAllText(fileName);
                try
                {
                    engine.Execute(script);
                }
                catch (ScriptEngineException e)
                {
                    var error = ParseError(e, fileName);
                    Log.Error(errorMessageFormat, error.FileName, error.Row, error.Column, error.Error);
                    return false;
                }
            }
            return true;
        }


    }
}
