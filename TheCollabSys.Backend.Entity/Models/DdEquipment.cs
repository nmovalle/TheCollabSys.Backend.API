namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEquipment
{
    public int EquipmentId { get; set; }

    public string EquipmentName { get; set; } = null!;

    public virtual ICollection<DdEngineerEquipment> DdEngineerEquipments { get; set; } = new List<DdEngineerEquipment>();

    public virtual ICollection<DdProjectEquipment> DdProjectEquipments { get; set; } = new List<DdProjectEquipment>();
}
