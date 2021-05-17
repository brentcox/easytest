using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EasyTest.Classes
{
    public static class Timer
    {
        public static async Task<(TimeSpan Duration, T Result)> TimeAsync<T>(Func<Task<T>> function) 
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = await function();
            sw.Stop();
            return (sw.Elapsed, result);
        }

        public static async Task<TimeSpan> TimeAsync(Func<Task> function)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            await function();
            sw.Stop();
            return sw.Elapsed;
        }

        public static (TimeSpan Duration, T Result) Time<T>(Func<T> function)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var result = function();
            sw.Stop();
            return (sw.Elapsed, result);
        }

        public static TimeSpan Time(Action function)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            function();
            sw.Stop();
            return sw.Elapsed;
        }

    }
}
