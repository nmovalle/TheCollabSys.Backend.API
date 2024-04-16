using System.ComponentModel.DataAnnotations;

namespace TheCollabSys.Backend.Entity.Request;

public class AuthenticationRequestBody
{
    [Required]
    public string UserName { get; set; }
}
