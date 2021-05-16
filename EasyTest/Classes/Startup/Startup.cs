using EasyTest.Factories;
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
                    description: "Location of the config file to use"),
            };

            rootCommand.Description = "Easy Test";

            rootCommand.Handler = CommandHandler.Create<string>(async (config) =>
            {
                await ProjectFactory.LoadConfigAsync(config);
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
