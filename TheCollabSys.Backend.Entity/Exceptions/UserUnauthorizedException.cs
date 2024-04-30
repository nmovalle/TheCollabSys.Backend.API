using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.Exceptions;

public sealed class UserUnauthorizedException : UnauthorizedException
{
    public UserUnauthorizedException(string username) : base($"Unauthorized access to user {username}")
    {
    }
}
