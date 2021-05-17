using EasyTest.Factories;
using Microsoft.ClearScript.V8;
using Serilog;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace EasyTest.Classes.Startup
{
    public class Startup
    {
        private async Task ApplyParametersAsync(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument<string>(
                    "config",
                    description: "Location of the config file to use"
                    ),
                new Option<bool>(
                    new string[]{ "--enable-debugging", "-e"},
                    description:"Enables debugging of scripts. Defaults to port 9222"
                    ),
                new Option<int>(
                    new string[]{ "--debug-port", "-d"},
                    description:"Port for the debugger to listen on. Defaults to 9222"
                    )

            };

            rootCommand.Description = "Easy Test";

            rootCommand.Handler = CommandHandler.Create<string, bool,  int>(async (config, enableDebugging, debugPort) =>
            {
                await ProjectFactory.LoadConfigAsync(config);
                ScriptEngineFactory.Initialise(enableDebugging, debugPort);
            });

            await rootCommand.InvokeAsync(args);
        }

        private void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public async Task ConfigureSystemAsync(string[] args)
        {
            ConfigureLogging();
            await ApplyParametersAsync(args);
            new RegisterTestRunners().Register();
        }
    }
}
