using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meditrash4_Midpoint
{
    class UnmatchingTypeListException : Exception
    {
        public UnmatchingTypeListException()
        {

        }
        public UnmatchingTypeListException(String message) :
            base(message)
        {
           
        }
        public UnmatchingTypeListException(String message,Exception inner) :
            base(message, inner)
        {

        }
    }
}
