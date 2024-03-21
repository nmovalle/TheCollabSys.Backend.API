using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.Exceptions;

public class BadRequestGenericException : BadRequestException
{
    public BadRequestGenericException(string message) : base(message) { }
}
