using Microsoft.ClearScript.V8;
using Serilog;
using System.IO;
using Xunit;

namespace EasyTest.Factories
{
    public static class ScriptEngineFactory
    {
        private static int debugPort;
        private static bool enableDebugging;
        private static void SetupEngine(V8ScriptEngine engine)
        {
            engine.AddHostType("log", typeof(Log));
            engine.AddHostType("file", typeof(File));
            engine.AddHostType("assert", typeof(Assert));
            engine.AddHostObject("debug", new Classes.JavaScript.Debug());
        }

        public static void Initialise(bool enableDebugging, int debugPort)
        {
            ScriptEngineFactory.debugPort = debugPort;
            ScriptEngineFactory.enableDebugging = enableDebugging;
        }

        public static V8ScriptEngine GetEngine()
        {
            V8ScriptEngineFlags flags = V8ScriptEngineFlags.None;
            if(enableDebugging)
            {
                flags = flags | V8ScriptEngineFlags.EnableDebugging | V8ScriptEngineFlags.AwaitDebuggerAndPauseOnStart;
            }
            V8ScriptEngine engine = new V8ScriptEngine(flags, debugPort);
            SetupEngine(engine);
            return engine;
        }
    }
}
