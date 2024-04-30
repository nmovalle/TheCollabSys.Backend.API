using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheCollabSys.Backend.Entity.Exceptions;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string username) : base($"The account with the username {username} was not found.")
    {
    }
}
