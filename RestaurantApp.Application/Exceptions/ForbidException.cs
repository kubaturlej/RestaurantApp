using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Application.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException()
        {

        }

        override public string Message => "Akcja zabroniona !";
    }
}
