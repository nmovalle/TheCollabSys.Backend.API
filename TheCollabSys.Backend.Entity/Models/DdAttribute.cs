namespace TheCollabSys.Backend.Entity.Models;
public partial class DdAttribute
{
    public int AttributeId { get; set; }

    public string AttributeName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<DdAttributeValue> DdAttributeValues { get; set; } = new List<DdAttributeValue>();
}
