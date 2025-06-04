using Common.Utilities;
using FluentValidation;

namespace Application.SendOTP.Command.Create;

public class SendOTPCommandValidator : AbstractValidator<SendOTPCommand>
{
    public SendOTPCommandValidator()
    {
        RuleFor(x => x.PhoneNumber)
            .Must(x => x.IsMobile())
            .WithMessage("شماره موبایل وارد شده قابل قبول نیست");
    }
}
