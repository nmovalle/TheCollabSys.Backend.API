using System.ComponentModel.DataAnnotations;

namespace TheCollabSys.Backend.Entity.Request;

public class AuthenticationRequestBody
{
    [Required]
    public string UserName { get; set; }
}

public class AuthenticationDomainRequestBody
{
    [Required]
    public string UserId { get; set; }
}