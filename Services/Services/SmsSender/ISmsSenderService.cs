namespace Services.Services.SmsSender;

public interface ISmsSenderService
{
    Task SendOTP(string phoneNumber, string otp);
    Task test(string phoneNumber, string otp);

}
