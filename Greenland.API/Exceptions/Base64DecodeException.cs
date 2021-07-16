using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Greenland.API.Exceptions
{
    public class Base64DecodeException :Exception
    {
        public Base64DecodeException()
        {

        }

        public Base64DecodeException(string name)
        : base(name)
        {

        }
    }
}
