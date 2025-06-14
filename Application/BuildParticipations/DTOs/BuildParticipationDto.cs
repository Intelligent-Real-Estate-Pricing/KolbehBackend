public class BuildParticipationDto
{
    public Guid Id { get; set; }
    public string PhoneNumber { get; set; }
    public OwnerBuilderType Role { get; set; }
    public string Zone { get; set; }
    public string Neighborhood { get; set; }
    public int PassageWidth { get; set; }
    public int Frontage { get; set; }
    public int Area { get; set; }
}
