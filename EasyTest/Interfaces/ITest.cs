using Microsoft.ClearScript;

namespace EasyTest.Interfaces
{
    public interface ITest
    {
        void Run(string description, ScriptObject callBack);
    }
}
