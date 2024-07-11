using System.ComponentModel.DataAnnotations;

namespace TheCollabSys.Backend.Entity.Request;

public class AuhenticationRefreshRequestBody
{
    //[Required]
    //public string username { get; set; }

    [Required]
    public string refreshToken { get; set; }
}
