using Application.Cqrs.Commands;
using Application.Users.DTOs;
using Common.Roles;
using Data.Contracts;
using Entities.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services;

public class RegisterUserWithOtpCommand : ICommand<ServiceResult<AccessToken>>
{
    public string PhoneNumber { get; set; }
    public string FullName { get; set; }
    public string OtpCode { get; set; }
}

public class LoginWithOtpCommand : ICommand<ServiceResult<TokenResponseDTO>>
{
    public string PhoneNumber { get; set; }
    public string OtpCode { get; set; }
}

public class SetUserPasswordCommand : ICommand<ServiceResult>
{
    public Guid UserId { get; set; }
    public string NewPassword { get; set; }
}

public class LoginWithPasswordCommand : ICommand<ServiceResult<TokenResponseDTO>>
{
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
}


public class RegisterUserWithOtpCommandHandler : ICommandHandler<RegisterUserWithOtpCommand, ServiceResult<AccessToken>>
{
    private readonly IRepository<ValidationCode> _validationCodeRepository;
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;

    public RegisterUserWithOtpCommandHandler(
        IRepository<ValidationCode> validationCodeRepository,
        IUserRepository userRepository,
        UserManager<User> userManager,
        IJwtService jwtService)
    {
        _validationCodeRepository = validationCodeRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<ServiceResult<AccessToken>> Handle(RegisterUserWithOtpCommand request, CancellationToken cancellationToken)
    {
        // Validate OTP
        var validationCode = await _validationCodeRepository
            .Table
            .OrderByDescending(x => x.Id)
            .Where(x => x.PhoneNumber == request.PhoneNumber && x.Code == request.OtpCode)
            .FirstOrDefaultAsync(cancellationToken);

        if (validationCode is null || !validationCode.IsValid)
            return ServiceResult.BadRequest<AccessToken>("کد OTP وارد شده صحیح نیست یا منقضی شده است");

        // Check for existing user
        if (await _userRepository.TableNoTracking.AnyAsync(u =>
            u.PhoneNumber == request.PhoneNumber  , cancellationToken))
            return ServiceResult.BadRequest<AccessToken>("شماره موبایل یا کدملی وارده تکراری میباشد");

        // Create user without password
        var newUser = User.RegisterUser(request.PhoneNumber, request.FullName);
        newUser.AuthenticationMethod = AuthenticationMethod.OTP;

        // Create user without password (OTP-only authentication)
        var result = await _userManager.CreateAsync(newUser);
        if (!result.Succeeded)
            return ServiceResult.BadRequest<AccessToken>(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(newUser, UserRoleNames.User);

        // Mark OTP as used
        validationCode.IsValid = false;
        await _validationCodeRepository.UpdateAsync(validationCode, cancellationToken);

        var jwt = await _jwtService.GenerateAsync(newUser);
        return ServiceResult.Ok(jwt);
    }
}

public class LoginWithOtpCommandHandler : ICommandHandler<LoginWithOtpCommand, ServiceResult<TokenResponseDTO>>
{
    private readonly IRepository<ValidationCode> _validationCodeRepository;
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginWithOtpCommandHandler(
        IRepository<ValidationCode> validationCodeRepository,
        IUserRepository userRepository,
        IJwtService jwtService)
    {
        _validationCodeRepository = validationCodeRepository;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<ServiceResult<TokenResponseDTO>> Handle(LoginWithOtpCommand request, CancellationToken cancellationToken)
    {
        // Validate OTP
        var validationCode = await _validationCodeRepository
            .Table
            .OrderByDescending(x => x.Id)
            .Where(x => x.PhoneNumber == request.PhoneNumber && x.Code == request.OtpCode)
            .FirstOrDefaultAsync(cancellationToken);

        if (validationCode is null || !validationCode.IsValid)
            return ServiceResult.BadRequest<TokenResponseDTO>("کد OTP وارد شده صحیح نیست یا منقضی شده است");

        // Find user
        var user = await _userRepository.Table
            .Where(x => x.PhoneNumber == request.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return ServiceResult.BadRequest<TokenResponseDTO>("کاربر یافت نشد");

        if (!user.CanAuthenticateWithOTP())
            return ServiceResult.BadRequest<TokenResponseDTO>("احراز هویت با OTP برای این کاربر فعال نیست");

        // Update last login
        await _userRepository.UpdateLastLoginDateAsync(user, cancellationToken);

        // Mark OTP as use  d
        validationCode.IsValid = false;
        await _validationCodeRepository.UpdateAsync(validationCode, cancellationToken);

        // Generate JWT
        var jwt = await _jwtService.GenerateAsync(user);
        var userDto = await GetUserDto(user);

        var tokenResponse = new TokenResponseDTO
        {
            AccessToken = jwt,
            User = userDto
        };

        return ServiceResult.Ok(tokenResponse);
    }

    private async Task<UserDTO> GetUserDto(User user)
    {
        return await _userRepository.Table
            .Where(x => x.Id == user.Id)
            .Select(u => new UserDTO
            {
                FullName = u.FullName,
                Id = u.Id,
                IsActive = u.IsActive,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                AuthenticationMethod = u.AuthenticationMethod,
                Roles = u.UserRoles.Select(x => new RoleDTO
                {
                    Description = x.Role.Description,
                    Name = x.Role.Name
                }).ToArray()
            }).FirstOrDefaultAsync();
    }
}

public class SetUserPasswordCommandHandler : ICommandHandler<SetUserPasswordCommand, ServiceResult>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;

    public SetUserPasswordCommandHandler(
        IUserRepository userRepository,
        UserManager<User> userManager)
    {
        _userRepository = userRepository;
        _userManager = userManager;
    }

    public async Task<ServiceResult> Handle(SetUserPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Table.FirstOrDefaultAsync(t => t.Id == request.UserId, cancellationToken);
        if (user == null)
            return ServiceResult.BadRequest("کاربر یافت نشد");

        // If user doesn't have a password, add one
        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (!addPasswordResult.Succeeded)
                return ServiceResult.BadRequest(string.Join(", ", addPasswordResult.Errors.Select(e => e.Description)));
        }
        else
        {
            // If user has a password, reset it
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await _userManager.ResetPasswordAsync(user, token, request.NewPassword);
            if (!resetResult.Succeeded)
                return ServiceResult.BadRequest(string.Join(", ", resetResult.Errors.Select(e => e.Description)));
        }

        // Enable password authentication
        user.EnablePasswordAuthentication();
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userManager.UpdateSecurityStampAsync(user);

        return ServiceResult.Ok();
    }
}

public class LoginWithPasswordCommandHandler : ICommandHandler<LoginWithPasswordCommand, ServiceResult<TokenResponseDTO>>
{
    private readonly IUserRepository _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly IJwtService _jwtService;

    public LoginWithPasswordCommandHandler(
        IUserRepository userRepository,
        UserManager<User> userManager,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<ServiceResult<TokenResponseDTO>> Handle(LoginWithPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.PhoneNumber);
        if (user == null)
            return ServiceResult.BadRequest<TokenResponseDTO>("شماره موبایل یا رمز عبور اشتباه است");

        if (!user.CanAuthenticateWithPassword())
            return ServiceResult.BadRequest<TokenResponseDTO>("احراز هویت با رمز عبور برای این کاربر فعال نیست");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!isPasswordValid)
            return ServiceResult.BadRequest<TokenResponseDTO>("شماره موبایل یا رمز عبور اشتباه است");

        // Update last login
        await _userRepository.UpdateLastLoginDateAsync(user, cancellationToken);

        var jwt = await _jwtService.GenerateAsync(user);
        var userDto = await GetUserDto(user);

        var tokenResponse = new TokenResponseDTO
        {
            AccessToken = jwt,
            User = userDto
        };

        return ServiceResult.Ok(tokenResponse);
    }

    private async Task<UserDTO> GetUserDto(User user)
    {
        return await _userRepository.Table
            .Where(x => x.Id == user.Id)
            .Select(u => new UserDTO
            {
                FullName = u.FullName,
                Id = u.Id,
                IsActive = u.IsActive,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                EmailConfirmed = u.EmailConfirmed,
                AuthenticationMethod = u.AuthenticationMethod,
                Roles = u.UserRoles.Select(x => new RoleDTO
                {
                    Description = x.Role.Description,
                    Name = x.Role.Name
                }).ToArray()
            }).FirstOrDefaultAsync();
    }
}
