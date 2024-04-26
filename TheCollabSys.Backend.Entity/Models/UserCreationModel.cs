namespace TheCollabSys.Backend.Entity.Models;

public class UserCreationModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    // Puedes agregar otras propiedades si es necesario
}

public class LoginModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class UpdatePassword
{
    public string Id { get; set; }
    public string NewPassword { get; set; }
}