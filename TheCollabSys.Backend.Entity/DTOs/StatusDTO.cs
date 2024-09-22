namespace TheCollabSys.Backend.Entity.DTOs;

public class StatusDTO
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string Type { get; set; } = null!;
}
