using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RealStateApp.Core.Application.DTOs.Email;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Interfaces.Infraestructure.Shared;
using RealStateApp.Core.Application.Wrappers;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;
using System.Text;

namespace RealStateApp.Infraestructure.Identity.Services
{
    public class AccountServiceForWebApp : BaseAccountService, IAccountServiceForWebApp
    {
        public AccountServiceForWebApp(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IMapper mapper) 
            : base(userManager, signInManager, emailService, mapper)
        {
        }


        public async Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null)
            {
                return Result<LoginResponseDTO>.Fail("Invalid email or password.");
            }
            if (!user.EmailConfirmed)
            {
                return Result<LoginResponseDTO>.Fail("Email not confirmed. Please check your inbox.");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);
            if (!result.Succeeded)
            {
                return Result<LoginResponseDTO>.Fail("Invalid email or password.");
            }
            return new LoginResponseDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task SignOut()
            => await _signInManager.SignOutAsync();


        public override async Task<Result<Unit>> SendResetPassword(string email, string origin, bool isApi = false)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ResetPassword";
            var completeUrl = new Uri($"{origin}/{route}");
            var resetPasswordUrl = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id.ToString());
            resetPasswordUrl = QueryHelpers.AddQueryString(resetPasswordUrl, "token", token);
            await _emailService.SendAsync(new EmailRequestDTO
            {
                To = user.Email,
                Subject = "Reset your password",
                BodyHtml = $"Reset your password: <a>{resetPasswordUrl}</a>"
            });
            return Unit.Value;
        }




        protected override async Task<Result<Unit>> SendEmailVerifaction(AppUser user, string origin)
        {
            var emailVerificationUrl = await GetEmailVerificationUrl(user, origin);
            await _emailService.SendAsync(new EmailRequestDTO
            {
                To = user.Email,
                Subject = "Confirm your email",
                BodyHtml = $"Confirm your email: <a>{emailVerificationUrl}</a>"
            });
            return Result<Unit>.Ok(Unit.Value);
        }


        private async Task<string> GetEmailVerificationUrl(AppUser user, string origin)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var route = "User/ConfirmEmail";
            var completeUrl = new Uri($"{origin}/{route}");
            var verificationUrl = QueryHelpers.AddQueryString(completeUrl.ToString(), "userId", user.Id.ToString());
            verificationUrl = QueryHelpers.AddQueryString(verificationUrl, "token", token);
            return verificationUrl;
        }
    }
}
