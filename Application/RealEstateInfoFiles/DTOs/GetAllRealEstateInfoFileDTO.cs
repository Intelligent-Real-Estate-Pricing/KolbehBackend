using Application;
using Entities.RealEstateInfoFiles;

public class GetAllRealEstateInfoFileDTO : GlobalGrid
{
    public string? Mantaghe { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinSquareMeters { get; set; }
    public int? MaxSquareMeters { get; set; }
    public int? Bedrooms { get; set; }
    public bool? HasParking { get; set; }
    public bool? HasStorage { get; set; }
    public bool? HasElevator { get; set; }
    public KitchenType? KitchenType { get; set; }
    public BathroomType? BathroomType { get; set; }
    public FlooringType? FlooringType { get; set; }
    public FacadeType? FacadeType { get; set; }
    public DocumentType? DocumentType { get; set; }
}
