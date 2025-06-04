using Application.Cqrs.Commands;
using Data.Contracts;
using Entities.Shared;
using Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services.SmsSender;

namespace Application.SendOTP.Command.Create;

public class SendOTPCommandHandler(IRepository<ValidationCode> repository, UserManager<User> userManager, ISmsSenderService smsSender) : ICommandHandler<SendOTPCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(SendOTPCommand request, CancellationToken cancellationToken)
    {

        var user = await userManager.FindByNameAsync(request.PhoneNumber);
        if (user is not null)
            return ServiceResult.BadRequest("کاربری با این شماره موبایل ثبت نام کرده است");


        var code = await repository.Table.OrderByDescending(x => x.Id).Where(x => x.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync(cancellationToken);
        if (code is not null && code.IsValid)
        {
            var timeaLeft = TimeSpan.FromSeconds(120) - (DateTime.Now - code.CreatedAt);
            var secondsLeft = Math.Max(0, (int)timeaLeft.TotalSeconds);
            var result = secondsLeft.ToString();
            return ServiceResult.ExpiredCode(result);
        }


        code = new ValidationCode
        {
            Code = Random.Shared.Next(10000, 99999).ToString(),
            PhoneNumber = request.PhoneNumber
        };

        await repository.AddAsync(code, cancellationToken, true);
        await smsSender.SendOTP(request.PhoneNumber, code.Code);
        return ServiceResult.Ok();
    }
}
