namespace TheCollabSys.Backend.Entity.Models;
public partial class DdAttributeValue
{
    public int ValueId { get; set; }

    public string EntityId { get; set; } = null!;

    public int AttributeId { get; set; }

    public string? AttributeValue { get; set; }

    public virtual DdAttribute Attribute { get; set; } = null!;
}
