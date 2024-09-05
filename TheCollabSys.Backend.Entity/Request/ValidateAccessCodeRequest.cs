namespace TheCollabSys.Backend.Entity.Request;

public class ValidateAccessCodeRequest
{
    public string Email { get; set; }
    public string Code { get; set; }
}