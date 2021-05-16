using System;

namespace EasyTest.Models
{
    public record TestResult(string TestName, ScriptError Error, DateTime StartedAt, TimeSpan Duration);
}
