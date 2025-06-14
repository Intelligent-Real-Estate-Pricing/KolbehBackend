using Entities.Common;

public class EstateDelegation : BaseEntity<Guid>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Title { get; set; }
    public string Zone { get; set; }
    public string Address { get; set; }
    public EstateDelegationType Type { get; set; }
}
public enum EstateDelegationType
{
    Delegation, // ملک‌سپاری
    Rent        // اجاره
}
