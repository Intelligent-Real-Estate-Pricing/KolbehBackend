using Application;
using Application.Cqrs.Commands;
using Application.Cqrs.Queris;
using Application.SendOTP.Command.Create;
using Application.Users.Command.ResetPasswordByOtp;
using Application.Users.DTOs;
using Asp.Versioning;
using Common;
using Common.Exceptions;
using Common.Roles;
using Common.Utilities;
using Data.Contracts;
using Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Services;
using Services.Services;
using System.Security.Claims;
using Kolbeh.Api.Models;
using Kolbeh.Api.Models.DTOs;
using Kolbeh.Api.Models.Users.Extentions;
using WebFramework.Api;
using WebFramework.Configuration;
using Entities.Shared;
using Data.Repositories;

namespace Kolbeh.Api.Controllers.v1;
/// <summary>
/// کنترلر کاربر
/// </summary>

[ApiVersion("1")]
public class UsersController : BaseController
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UsersController> logger;
    private readonly IJwtService _jwtService;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;
    private readonly IRepository<ValidationCode> _ValidationCodeRepository;
    private readonly SignInManager<User> signInManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsersController(
        IUserRepository userRepository,
        ILogger<UsersController> logger,
        IJwtService jwtService,
        IConfiguration configuration,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IRepository<ValidationCode> ValidationCodeRepository,
        SignInManager<User> signInManager,
        ICommandDispatcher commandDispatcher,
         IQueryDispatcher queryDispatcher,
        IHttpContextAccessor httpContextAccessor)
    {
        //this.userRepository = userRepository;
        this.logger = logger;
        _jwtService = jwtService;
        this.userManager = userManager;
        _ValidationCodeRepository = ValidationCodeRepository;
        this.roleManager = roleManager;
        _configuration = configuration;
        this.signInManager = signInManager;
        _commandDispatcher = commandDispatcher;
        _userRepository = userRepository;
        _queryDispatcher = queryDispatcher;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// اطلاعات یوزر با آیدی
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Roles = $"{UserRoleNames.Admin}")]
    public virtual async Task<ApiResult<GetUserInfoDTO>> Get(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.Table.Where(x => x.Id == id).Select(x => new GetUserInfoDTO
        {
            Id = x.Id,
            UserName = x.UserName,
            IsActive = x.IsActive,
            PhoneNumber = x.PhoneNumber,
            NationalCode = x.NationalCode,
            FullName = x.FullName,
            ImageUrl = x.ImageUrl,
            Roles = x.UserRoles.Select(x => new RoleDTO { Description = x.Role.Description, Name = x.Role.Name, }).ToArray()
        }).FirstOrDefaultAsync();
        var role = await roleManager.FindByNameAsync("Admin");

        if (user == null)
            return NotFound();


        //await userRepository.UpdateSecurityStampAsync(user, cancellationToken);

        return ServiceResult.Ok(user).ToApiResult();
    }

    /// <summary>
    /// This method generate JWT Token
    /// </summary>
    /// <param name="tokenRequest">The information of token request</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public virtual async Task<ApiResult<TokenResponseDTO>> Token([FromBody] TokenRequest tokenRequest, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(tokenRequest.username) ?? throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");
        var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.password);
        if (!isPasswordValid)
            throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

        var jwt = await _jwtService.GenerateAsync(user);
        var userDTo = await _userRepository.Table
                   .Where(x => x.Id == user.Id)
                   .Select(user => new UserDTO
                   {
                       FullName = user.FullName,
                       Id = user.Id,
                       IsActive = user.IsActive,
                       PhoneNumber = user.PhoneNumber,
                       Email = user.Email,
                       EmailConfirmed = user.EmailConfirmed,
                       Roles = user.UserRoles.Select(x => new RoleDTO { Description = x.Role.Description, Name = x.Role.Name, }).ToArray()
                   }).FirstOrDefaultAsync();
        var tokenReponse = new TokenResponseDTO
        {
            AccessToken = jwt,
            User = userDTo
        };
        return new ApiResult<TokenResponseDTO>(true, ApiResultStatusCode.Success, tokenReponse);
    }

    /// <summary>
    /// بررسی نام کاربری کاربر
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public virtual async Task<bool> CheckUserName(string username, CancellationToken cancellationToken) =>
        (await _userRepository.Table.AnyAsync(t => t.UserName.ToLower().Equals(username.ToLower())));

   
     /// <summary>
    /// ثبت نام کاربرعادی
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public virtual async Task<ApiResult<AccessToken>> RegisterUser([FromBody] RegisterUserDTO input, CancellationToken cancellationToken)
    {
        var aa = await _userRepository.TableNoTracking.AnyAsync(u => u.PhoneNumber == input.PhoneNumber || u.NationalCode == input.NationalCode, cancellationToken);
        if (await _userRepository.TableNoTracking.AnyAsync(u => u.PhoneNumber == input.PhoneNumber || u.NationalCode == input.NationalCode, cancellationToken))
            return (ServiceResult.BadRequest<AccessToken>("شماره موبایل یا کدملی وارده  تکراری میباشد")).ToApiResult();


/*        var code = await _ValidationCodeRepository.Table.OrderByDescending(x => x.Id).Where(x => x.PhoneNumber == input.PhoneNumber).FirstOrDefaultAsync(cancellationToken);
        if (code == null||!code.IsValid||code.Code!=input.validateCode)
            return (ServiceResult.BadRequest<AccessToken>("کد پیامکی وارده منقضی شده یا اشتباه است")).ToApiResult();
*/
        User newUser = Entities.Users.User.RegisterUser(input.PhoneNumber, 
            input.NationalCode, input.FullName);
        var user = await userManager.CreateAsync(newUser, input.Password);
        if (!user.Succeeded)
            return (ServiceResult.BadRequest<AccessToken>(string.Join(',', user.Errors.Select(a => a.Description).ToArray())).ToApiResult());

        await userManager.AddToRoleAsync(newUser, UserRoleNames.User);

        var jwt = await _jwtService.GenerateAsync(newUser);
        return (ServiceResult.Ok(jwt)).ToApiResult();
    }

    [HttpPost("[action]")]
    [Authorize]
    public virtual async Task<ApiResult> UpdatePassword(Guid id, string password, CancellationToken cancellationToken)
    {
        // 1. Get user and validate
        var updateUser = await _userRepository.Table.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        if (updateUser == null)
        {
            return BadRequest("User not found");
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(updateUser);

        var result = await userManager.ResetPasswordAsync(updateUser, token, password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }
        await userManager.UpdateSecurityStampAsync(updateUser);

        return Ok();
    }

    /// <summary>
    /// حذف کاربر با ایدی توسط ادمین
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [Authorize(Roles = $"{UserRoleNames.Admin}")]
    public virtual async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(cancellationToken, id);
        if (user is null)
            return (ServiceResult.BadRequest("یافت نشد")).ToApiResult();
        await _userRepository.SoftDeleteAsync(user, cancellationToken);

        return Ok();
    }
    [HttpPost("[action]")]
    [Authorize(Roles = $"{UserRoleNames.User}")]
    public virtual async Task<ApiResult> SelfAccountDelete(CancellationToken cancellationToken)
    {
        var userIdString = _httpContextAccessor.HttpContext.User.Identity.GetUserId();
        var userId = Guid.Parse(userIdString);


        var user = await _userRepository.GetByIdAsync(cancellationToken, userId);
        if (user is null)
            return (ServiceResult.BadRequest("یافت نشد")).ToApiResult();
        await _userRepository.SoftDeleteAsync(user, cancellationToken);

        return Ok();
    }

    /// <summary>
    /// ارسال OTP
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [AllowAnonymous]
    public virtual async Task<ApiResult> SendOtp(string phoneNumber, CancellationToken cancellationToken) =>
      (await _commandDispatcher.SendAsync(new SendOTPCommand(phoneNumber), cancellationToken)).ToApiResult();

    /// <summary>
    /// تعویض رمز عبور با otp
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [EnableRateLimiting(RateLimitPolicies.Fixed)]
    [HttpPost("[action]")]
    [AllowAnonymous]
    public virtual async Task<ApiResult> ResetPasswordByOtp(ResetPasswordByOtp input, CancellationToken cancellationToken) =>
      (await _commandDispatcher.SendAsync(new ResetPasswordByOtpCommand(input.PhoneNumber, input.Otp, input.Password), cancellationToken)).ToApiResult();

    /// <summary>
    /// افزودن نقش ها به کاربر
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("[action]")]
    [Authorize(Roles = $"{UserRoleNames.Admin}")]
    public virtual async Task<ApiResult<string>> AddRoleToUser([FromBody] AddRoleToUserDTO input, CancellationToken cancellationToken)
    {

        List<string> InvalidRoles = new();
        var userInDb = await _userRepository.GetByIdAsync(cancellationToken, input.UserId);
        var identityResult = await userManager.AddToRolesAsync(userInDb, input.Roles);
        if (identityResult.Succeeded)
        {
            await userManager.UpdateSecurityStampAsync(userInDb);
            return (ServiceResult.Ok<string>(null)).ToApiResult();
        }

        var errors = string.Join(',', identityResult.Errors);
        return (ServiceResult.Ok<string>(errors)).ToApiResult();
    }

    /// <summary>
    /// حذف نقش ها از کاربر
    /// </summary>
    /// <param name="input"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("[action]")]
    [Authorize(Roles = $"{UserRoleNames.Admin}")]
    public virtual async Task<ApiResult<string>> RemoveFromRolesUser([FromQuery] AddRoleToUserDTO input, CancellationToken cancellationToken)
    {

        List<string> InvalidRoles = new();
        var userInDb = await _userRepository.GetByIdAsync(cancellationToken, input.UserId);
        var identityResult = await userManager.RemoveFromRolesAsync(userInDb, input.Roles);

        if (identityResult.Succeeded)
        {
            await userManager.UpdateSecurityStampAsync(userInDb);
            return (ServiceResult.Ok<string>(null)).ToApiResult();
        }

        var errors = string.Join(',', identityResult.Errors);
        return (ServiceResult.Ok<string>(errors)).ToApiResult();
    }

}
