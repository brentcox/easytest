using EasyTest.Classes;
using EasyTest.Classes.Startup;
using EasyTest.Factories;
using EasyTest.Interfaces;
using System.Threading.Tasks;

namespace EasyTest
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var startUp = new Startup();
            await startUp.ConfigureSystemAsync(args);
            
            IProjectRunner runner = new ProjectRunner(ProjectFactory.GetProject(), string.Empty);
            await runner.RunAsync();

            return 0;
        }
    }
}
