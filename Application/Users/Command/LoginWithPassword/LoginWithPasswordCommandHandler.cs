using Application.Cqrs.Commands;
using Application.Users.DTOs;
using Data.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services;

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
