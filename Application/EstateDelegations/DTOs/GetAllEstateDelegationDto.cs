using Application;

public class GetAllEstateDelegationDto : GlobalGrid 
{
    public string? Zone { get; set; }
    public EstateDelegationType? Type { get; set; }
    public string? FullName { get; set; }
}
