namespace TheCollabSys.Backend.Entity.DTOs;

public class ClientDTO
{
    public int ClientID { get; set; }
    public string ClientName { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public DateTime? DateCreated { get; set; }
}
