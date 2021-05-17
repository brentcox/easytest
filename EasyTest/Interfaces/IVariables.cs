namespace EasyTest.Interfaces
{
    public interface IVariables
    {
        void Set(string name, object value);
        
        public object Get(string name);

        bool Exists(string name);

        string Parse(string value);
    }
}
