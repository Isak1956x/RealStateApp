using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces.Infraestructure.Shared;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Settings;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Infraestructure.Identity.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealStateApp.Infraestructure.Identity.Services
{
    public class AccountServiceForWebApi : BaseAccountService, IAccountServiceForApi
    {
        private readonly JwtSettings _jwtSettings;

        public AccountServiceForWebApi(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IEmailService emailService, IOptions<JwtSettings> jwtSetting) : base(userManager, signInManager, emailService)
        {
            _jwtSettings = jwtSetting.Value;
        }

        public async Task<string> Login(LoginRequestDTO loginRequestDTO)
        {
            AppUser user;
            user = await _userManager.FindByEmailAsync(loginRequestDTO.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            if (!user.EmailConfirmed)
            {
                throw new UnauthorizedAccessException("Email not confirmed. Please check your inbox.");
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginRequestDTO.Password, false, false);
            if (!result.Succeeded)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            var jwt = await GenerateJWT(user);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public async Task<JwtSecurityToken> GenerateJWT(AppUser user)
        {
            var usaerClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var rolesClaims = roles.Select(r => new Claim("roles", r));
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email?? ""),
                new Claim("UId", user.Id)
            }.Union(usaerClaims).Union(rolesClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );
            return jwt;
        }



        public override Task<Result<Unit>> SendResetPassword(string email, string origin, bool isApi = false)
        {
            throw new NotImplementedException();
        }

        protected override Task<Result<Unit>> SendEmailVerifaction(AppUser user, string origin)
        {
            throw new NotImplementedException();
        }
    }
}
