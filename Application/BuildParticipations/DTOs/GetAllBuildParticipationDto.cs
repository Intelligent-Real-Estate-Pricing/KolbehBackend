using Application;

public class GetAllBuildParticipationDto  :GlobalGrid
{
    public OwnerBuilderType? Role { get; set; }
    public string? Zone { get; set; }
    public string? Neighborhood { get; set; }
}
