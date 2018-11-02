using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Exceptions
{
    public class WebTimeoutException : ApplicationException
    {
        public override string Message => "Web timeout Exception!";
    }
}
