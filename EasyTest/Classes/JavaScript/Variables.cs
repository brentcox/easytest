using EasyTest.Interfaces;
using System.Collections.Generic;

namespace EasyTest.Classes.JavaScript
{
    public class Variables : IVariables
    {
        private Dictionary<string, object> variables = new Dictionary<string, object>();

        public void Set(string name, object value)
        {
            if (variables.ContainsKey(name))
            {
                variables[name]= value;
            }
            else
                variables.Add(name, value);
        }
        public object Get(string name)
        {
            return variables[name];
        }
    }
}
