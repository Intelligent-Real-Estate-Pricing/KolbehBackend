using Entities.Common;

public class BuildParticipation : BaseEntity<Guid>
{
    public string PhoneNumber { get; set; }
    public OwnerBuilderType Role { get; set; } // Owner or Builder
    public string Zone { get; set; }
    public string Neighborhood { get; set; }
    public int PassageWidth { get; set; }
    public int Frontage { get; set; }
    public int Area { get; set; }
}
public enum OwnerBuilderType
{
    Owner,
    Builder
}
