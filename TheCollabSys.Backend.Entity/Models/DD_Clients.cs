namespace TheCollabSys.Backend.Entity.Models;

public class DD_Clients
{
    public int ClientID { get; set; }

    public string ClientName { get; set; }

    public string Address { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public byte[]? Logo { get; set; }

    public DateTime? DateCreated { get; set; }
}
