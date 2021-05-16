using EasyTest.Classes.Formatters;
using EasyTest.Factories;

namespace EasyTest.Classes.Startup
{
    public class RegisterTestResultFormatters
    {
        public void Register()
        {
            TestResultFormatterFactory.Add(new ConsoleResultFormatter());
        }
    }
}
