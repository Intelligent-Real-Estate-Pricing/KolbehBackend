namespace Application.Users.DTOs;

public class ResetPasswordByOtp
{
    public string PhoneNumber { get; set; }
    public string Otp { get; set; }
    public string Password { get; set; }
}
