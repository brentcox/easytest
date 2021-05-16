using EasyTest.Interfaces;
using System.Collections.Generic;

namespace EasyTest.Factories
{
    public static class TestResultFormatterFactory
    {
        private static List<ITestResultFormatter> registered = new List<ITestResultFormatter>();

        public static void Add(ITestResultFormatter formatter)
        {
            registered.Add(formatter);
        }

        public static IEnumerable<ITestResultFormatter> GetFormatters()
        {
            return registered.AsReadOnly();
        }
    }
}
