using EasyTest.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        public bool Exists(string name)
        {
            return variables.ContainsKey(name);
        }

        public string Parse(string value)
        {
            Regex regex = new Regex("(?<={{)(.*?)(?=}})");
            foreach (Match match in regex.Matches(value))
            {
                if (Exists(match.Value))
                {
                    value = value.Replace("{{" + match.Value + "}}", Get(match.Value)?.ToString());
                }
            }
            return value;
        }
    }
}
