using Common;
using IPE.SmsIrClient.Models.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Services.Services.SmsSender.SmsIr;

public class SmsSender(
    ILogger<SmsSender> logger,
    IConfiguration configuration) : ISmsSenderService, IScopedDependency
{

    private async Task SendFast(string phoneNumber, VerifySendParameter[] parameters, int templateId)
    {
        try
        {
            var token = configuration["SiteSettings:SmsSetting:ApiKey"];
            IPE.SmsIrClient.SmsIr smsIr = new(token);
            var result = await smsIr.VerifySendAsync(phoneNumber, templateId, parameters);
            logger.LogInformation("Send sms -> PhoneNumber= {PhoneNumber} and  Status = {Status} and Message= {Message}", phoneNumber, result.Status, result.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }

    public async Task SendOTP(string phoneNumber, string otp)
    {
        _ = int.TryParse(configuration["SiteSettings:SmsSetting:OtpLogin"], out int templateId);
        var parameters = new VerifySendParameter[]
           {
                new("OTPCode",otp)
           };

        await SendFast(phoneNumber, parameters, templateId);
    }
        public async Task test(string phoneNumber, string otp)
    {
        _ = int.TryParse(configuration["SiteSettings:SmsSetting:OtpLogin"], out int templateId);
        var parameters = new VerifySendParameter[]
           {
                new("Code",otp)
           };

        await SendFast(phoneNumber, parameters, 123456);
    }


}