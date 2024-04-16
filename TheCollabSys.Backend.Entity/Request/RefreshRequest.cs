using System.ComponentModel.DataAnnotations;

namespace TheCollabSys.Backend.Entity.Request;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; }
}
