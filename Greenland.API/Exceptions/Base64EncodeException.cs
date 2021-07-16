using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Exceptions
{
    public class Base64EncodeException :Exception
    {
        public Base64EncodeException()
        {

        }

        public Base64EncodeException(string name) :base(name)
        {

        }
    }
}
