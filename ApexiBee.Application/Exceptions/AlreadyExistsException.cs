using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApexiBee.Application.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException(string entity, string field)
            : base($"{char.ToUpper(entity[0])}{entity.Substring(1)} with the same {field} field(s) value already exists")
        { }
    }
}
