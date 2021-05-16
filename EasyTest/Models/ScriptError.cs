using EasyTest.Enum;

namespace EasyTest.Models
{
    public record ScriptError(string Error, int Row, int Column, string FileName, ErrorTypes ErrorType);
}
