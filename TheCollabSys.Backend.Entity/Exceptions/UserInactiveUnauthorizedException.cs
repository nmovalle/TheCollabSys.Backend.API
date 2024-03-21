using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.Exceptions;

public sealed class UserInactiveUnauthorizedException : UnauthorizedException
{
    public UserInactiveUnauthorizedException(string username) : base($"Unauthorized access to user {username} because the user is inactive")
    {
    }
}
