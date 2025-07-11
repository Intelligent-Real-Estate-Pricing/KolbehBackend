namespace Entities.Otp
{
    public class OtpData
    {
        public string Code { get; set; } = default!;
        public string CreatedAt { get; set; } = default!;
        public string ExpireAt { get; set; } = default!;
        public int TryCount { get; set; }
        public bool IsValid { get; set; }
        public string IP { get; set; } = default!;
    }


}