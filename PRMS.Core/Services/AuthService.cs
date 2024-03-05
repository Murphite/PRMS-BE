using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;


namespace PRMS.Core.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<User> _userManager;
    private readonly IRepository _repository;
    private readonly IEmailService _emailService;

    public AuthService(UserManager<User> userManager, IRepository repository, IJwtService jwtService, IEmailService emailService)
    {
        _userManager = userManager;
        _repository = repository;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    public async Task<Result> Register(RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            Email = registerUserDto.Email,
            PhoneNumber = registerUserDto.PhoneNumber,
            UserName = registerUserDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerUserDto.Password);

        if (!result.Succeeded)
            return result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        result = await _userManager.AddToRoleAsync(user, RolesConstant.User);
        if (!result.Succeeded)
            return result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        return Result.Success();
    }

    public async Task<Result<LoginResponseDto>> Login(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email);

        if (user is null)
            return new Error[] { new("Auth.Error", "email or password not correct") };

        var isValidUser = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!isValidUser)
            return new Error[] { new("Auth.Error", "email or password not correct") };

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        return new LoginResponseDto(token);
    }
    
    public async Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

        if (!resetPasswordResult.Succeeded)
            return resetPasswordResult.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        return Result.Success();
    }

    public async Task<Result> ForgotPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        if (user == null)
            return new Error[] { new Error("Auth.Error", "No user found with the provided email") };

        var resetLink = ResetPasswordAsync(resetPasswordDto);

        var emailSubject = "Your New Password";

        var emailBody = $"Hello {user.FirstName}, click this link to reset your password: {resetLink}.";

        var emailForgotPassword = await _emailService.SendEmailAsync(resetPasswordDto.Email, emailSubject, emailBody);
        
        return Result.Success();
    }

    public async Task<Result> ConfirmEmail(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return new Error[] { new("Auth.Error", "User not found") };

        var confirmEmailResult = await _userManager.ConfirmEmailAsync(user, token);

        if (!confirmEmailResult.Succeeded)
        {
            return Result.Failure(confirmEmailResult.Errors.Select(e => new Error(e.Code, e.Description)).ToArray());
        }

        user.EmailConfirmed = true;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
        {
            return Result.Failure(updateResult.Errors.Select(e => new Error(e.Code, e.Description)).ToArray());
        }

        return Result.Success();
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null)
            return new Error[] { new("Auth.Error", "email not correct") };

        if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
            return new Error[] { new("Auth.Error", "password not correct") };

        if (model.NewPassword != model.ConfirmPassword)
            return new Error[] { new("Auth.Error", "Newpassword and Confirmpassword must match") };

        var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        if (!result.Succeeded)
            return result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        return Result.Success();

    }
}