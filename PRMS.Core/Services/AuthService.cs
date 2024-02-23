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

    public AuthService(UserManager<User> userManager, IRepository repository, IJwtService jwtService)
    {
        _userManager = userManager;
        _repository = repository;
        _jwtService = jwtService;
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

        var Token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetPasswordResult = await _userManager.ResetPasswordAsync(user, Token, resetPasswordDto.NewPassword);

        if (!resetPasswordResult.Succeeded)
            return resetPasswordResult.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        return Result.Success();
    }
}