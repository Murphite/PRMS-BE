using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;
using System.Web;


namespace PRMS.Core.Services;

public class AuthService : IAuthService
{
    private readonly IJwtService _jwtService;
    private readonly UserManager<User> _userManager;
    private readonly IRepository _repository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(UserManager<User> userManager, IRepository repository, IJwtService jwtService,
        IEmailService emailService, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _repository = repository;
        _jwtService = jwtService;
        _emailService = emailService;
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Register(RegisterUserDto registerUserDto)
    {
        var address = new Address
        {
            Street = registerUserDto.Street,
            City = registerUserDto.City,
            State = registerUserDto.State,
            Country = registerUserDto.Country,
            Latitude = registerUserDto.Latitude,
            Longitude = registerUserDto.Longitude
        };

        var user = new User
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            Email = registerUserDto.Email,
            PhoneNumber = registerUserDto.PhoneNumber,
            UserName = registerUserDto.Email,
            AddressId = address.Id,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };

        await _repository.Add(address);
        var isSaved = await _unitOfWork.SaveChangesAsync() > 0;
        if (!isSaved)
            return new Error[] { new("Address.NotSaved", "Address not saved") };

        var result = await _userManager.CreateAsync(user, registerUserDto.Password);

        if (!result.Succeeded)
            return result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        result = await _userManager.AddToRoleAsync(user, RolesConstant.User);
        if (!result.Succeeded)
            return result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        return Result.Success();
    }

    public async Task<Result> AdminRegister(AdminRegisterDTO registerAdminDto)
    {
        var emailExist = await _userManager.FindByEmailAsync(registerAdminDto.Email);

        if (emailExist != null)
            return new Error[] { new("Registration.Error", "email already exist") };

        var user = new User
        {
            FirstName = registerAdminDto.FirstName,
            MiddleName = registerAdminDto.MiddleName,
            LastName = registerAdminDto.LastName,
            Email = registerAdminDto.Email,
            PhoneNumber = registerAdminDto.PhoneNumber,
            UserName = registerAdminDto.Email,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Address = new Address
            {
                Street = registerAdminDto.Street,
                City = registerAdminDto.City,
                State = registerAdminDto.State,
                Country = registerAdminDto.Country,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            }
        };

        var result = await _userManager.CreateAsync(user, registerAdminDto.Password);
        if (!result.Succeeded)
            return (result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray());

        result = await _userManager.AddToRoleAsync(user, RolesConstant.Admin);
        if (!result.Succeeded)
            return result.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        var confirmEmailUrl = _configuration["ConfirmEmailUrl"];
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedEmail = HttpUtility.UrlEncode(user.Email);
        var encodedToken = HttpUtility.UrlEncode(token);
        var confirmationLink = $"{confirmEmailUrl}?email={encodedEmail}&token={encodedToken}";
        var body =
            @$"Hi {user.FirstName}, Please click the link <a href='{confirmationLink}'>here</a> to confirm your account's email";
        var emailResult = await _emailService.SendEmailAsync(user.Email, "Confirm Email", body);

        if (!emailResult)
            return new Error[]
            {
                new("Registration.Error",
                    "Account has been created successfully but error occured while sending verification email")
            };

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

        string? patientId = null;
        if (roles.Contains(RolesConstant.User))
        {
            patientId = _repository.GetAll<Patient>().FirstOrDefault(p => p.UserId == user.Id)?.Id;
        }
        
        return new LoginResponseDto(token, roles.First(), patientId);
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user is null)
            return new Error[] { new("Auth.Error", "No user found with the provided email") };

        var resetPasswordResult =
            await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

        if (!resetPasswordResult.Succeeded)
            return resetPasswordResult.Errors.Select(error => new Error(error.Code, error.Description)).ToArray();

        return Result.Success();
    }

    public async Task<Result> ForgotPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        if (user == null)
            return new Error[] { new("Auth.Error", "No user found with the provided email") };

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink =
            $"{_configuration["ResetPasswordUrl"]}?email={HttpUtility.UrlEncode(user.Email)}&token={HttpUtility.UrlEncode(token)}";

        const string emailSubject = "Your New Password";

        var emailBody = $"Hello {user.FirstName}, click this link to reset your password: {resetLink}.";

        var isSuccessful = await _emailService.SendEmailAsync(resetPasswordDto.Email, emailSubject, emailBody);
        if (!isSuccessful)
            return new Error[] { new("Auth.Error", "Error occured while sending reset password email") };

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