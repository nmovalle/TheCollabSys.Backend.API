namespace TheCollabSys.Backend.Entity.Models;

public partial class DdUserCompany
{
    public int UserCompayId { get; set; }

    public string? UserId { get; set; }

    public int? CompanyId { get; set; }

    public virtual AspNetUser? User { get; set; }
}
