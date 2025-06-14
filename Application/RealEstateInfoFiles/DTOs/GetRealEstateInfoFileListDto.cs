public class GetRealEstateInfoFileListDto
{
    public Guid Id { get; set; }
    public string Mantaghe { get; set; }
    public decimal TotalPrice { get; set; }
    public int SquareMeters { get; set; }
    public int Bedrooms { get; set; }
    public bool HasParking { get; set; }
    public bool HasStorage { get; set; }
    public bool HasElevator { get; set; }
    public string BuildingAge { get; set; }
}