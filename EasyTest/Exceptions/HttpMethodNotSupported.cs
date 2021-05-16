using System;

namespace EasyTest.Exceptions
{
    public class HttpMethodNotSupported : Exception
    {
        public HttpMethodNotSupported(string method) : base(method + " is not supported")
        {
        }
    }
}
