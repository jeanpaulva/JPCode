using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Exceptions
{
    public class ObsoleteDataException : ApplicationException
    {
        public override string Message => "Obsolete Data Exception!";
    }
}
