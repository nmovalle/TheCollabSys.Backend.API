using System.Text.RegularExpressions;

namespace TheCollabSys.Backend.Entity.Exceptions;

public sealed class ActiveDirectoryBadRequestException : BadRequestException
{
    public ActiveDirectoryBadRequestException(string uri, string data) : base($"Get: {uri}. {Regex.Unescape(data)}")
    {
    }
}
