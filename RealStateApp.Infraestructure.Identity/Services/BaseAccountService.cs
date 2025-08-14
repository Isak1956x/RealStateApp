using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RealStateApp.Core.Application.DTOs.Email;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Helpers.Enums;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Interfaces.Infraestructure.Shared;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;
using System.Text;

namespace RealStateApp.Infraestructure.Identity.Services
{
    public abstract class BaseAccountService : IBaseAccountService
    {
        protected readonly UserManager<AppUser> _userManager;
        protected readonly IMapper _mapper;
        protected readonly SignInManager<AppUser> _signInManager;
        protected readonly IEmailService _emailService;


        protected BaseAccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, IMapper mapper)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _emailService = emailService;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Result<UserDto>.Fail("User not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var userDto = _mapper.Map<UserDto>(user);
            userDto.Role = roles.FirstOrDefault();
            return userDto;
        }


        public async Task<Result<string>> RegisterAsync(RegisterRequestDTO registerRequest, string origin)
        {
            var userWithEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (userWithEmail != null)
            {
                return Result<string>.Fail("Email already exists.");
            }
            var userWithUserName = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (userWithUserName != null)
            {
                return Result<string>.Fail("Username already exists.");
            }
            if (!EnumHelper.TryParseEnum<UserRoles>(registerRequest.RoleId, out var role))
            {
                return Result<string>.Fail("Invalid role ID.");
            }

            var user = new AppUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhotoPath = registerRequest.PhotoPath,
                IdNumber = registerRequest.IdNumber,
                PhoneNumberConfirmed = true,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                return Result<string>.Fail("Registration failed. Please try again.");
            }
            await _userManager.AddToRoleAsync(user, role.ToString());

            var res = await SendEmailVerifaction(user, origin);
            if (!res.IsSuccess)
            {
                return Result<string>.Fail("Email verification failed.");
            }
            /* Move to proper method implementation in webapp
            var emailVerificationUrl = await GetEmailVerificationUrl(user, origin);
            await _emailService.SendAsync(new EmailRequestDTO
            {
                To = user.Email,
                Subject = "Confirm your email",
                BodyHtml = $"Confirm your email: <a>{emailVerificationUrl}</a>"
            });
            */
            return user.Id;

        }

        public virtual async Task<Result<UserDto>> RegisterByAdmin(RegisterRequestDTO registerRequest)
        {
            var userWithEmail = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (userWithEmail != null)
            {
                return Result<UserDto>.Fail("Email already exists.");
            }
            var userWithUserName = await _userManager.FindByNameAsync(registerRequest.UserName);
            if (userWithUserName != null)
            {
                return Result<UserDto>.Fail("Username already exists.");
            }
            if(!EnumHelper.TryParseEnum<UserRoles>(registerRequest.RoleId, out var role))
            {
                return Result<UserDto>.Fail("Invalid role ID.");
            }

            var user = new AppUser
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhotoPath = registerRequest.PhotoPath,
                //Role = (UserRole)registerRequest.RoleId,
                IdNumber = registerRequest.IdNumber,
                PhoneNumberConfirmed = true,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
            {
                return Result<UserDto>.Fail("Registration failed. Please try again.");
            }
            await _userManager.AddToRoleAsync(user, role.ToString());
            return _mapper.Map<UserDto>(user);

        }

        public async Task<Result<Unit>> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Email confirmation failed.");
            }
            return Unit.Value;
        }

        public async Task<Result<Unit>> EditUser(UserUpdateDTO dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhotoPath = dto.PhotoPath;
            //user.PhotoPath = string.IsNullOrEmpty(dto.PhotoPath) ? user.PhotoPath : dto.PhotoPath;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("User update failed.");
            }
            // If password is provided, change it

            if (!string.IsNullOrEmpty(dto.Password))
            {
                var resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetPasswordToken, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    return Result<Unit>.Fail("Password change failed.");
                }
            }
            return Unit.Value;
        }
        /*
        public async Task<Result<Unit>> UpdateProfilePhoto(string userId, string photoPath)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Profile photo update failed.");
            }
            return Unit.Value;
        }
        */
        public async Task<Result<Unit>> DeleteUserAsyn(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("User deletion failed.");
            }

            else
            {
                return Result<Unit>.Fail("Domain user deletion failed.");
            }
            return Unit.Value;
        }
        public async Task<IEnumerable<UserDto>> GetByRole(UserRoles role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role.ToString());
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IdCardNumber = user.IdNumber,
                IsActive = user.IsActive,
                Role = role.ToString()
            });

        }

        public async Task<IEnumerable<UserDto>> GetAdmins()
            => await GetByRole(UserRoles.Admin);

        public async Task<IEnumerable<UserDto>> GetDevs()
            => await GetByRole(UserRoles.Developer);
        public async Task<Result<Unit>> SendResetPasswordEmail(string email, string origin)
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

        public async Task<Result<Unit>> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Password reset failed.");
            }
            return Unit.Value;
        }

        public async Task<Dictionary<string, UserHeaderDTO>> GetHeadersOfUsers(IEnumerable<string> ids)
            => await _userManager.Users.Where(u => ids.Contains(u.Id))
                     .ToDictionaryAsync(u => u.Id, U => new UserHeaderDTO
                     {
                         UserName = U.UserName,
                         UserId = U.Id,
                         FirstName = U.FirstName,
                         LastName = U.LastName,
                         Email = U.Email
                     });


        public async Task<UserHeaderDTO> GetHeaderOfUsers(string id)
            => await _userManager.Users.Where(u => u.Id == id)
                     .Select(U => new UserHeaderDTO
                     {
                         UserName = U.UserName,
                         UserId = U.Id,
                         FirstName = U.FirstName,
                         Email = U.Email,
                         LastName = U.LastName,
                     }).FirstOrDefaultAsync();

        public async Task<Result<Unit>> UpdateProfilePhoto(string userId, string photoPath)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<Unit>.Fail("User not found.");
            }
            user.PhotoPath = photoPath;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<Unit>.Fail("Profile photo update failed.");
            }
            return Unit.Value;
        }
        protected abstract Task<Result<Unit>> SendEmailVerifaction(AppUser user, string origin);
        public abstract Task<Result<Unit>> SendResetPassword(string email, string origin, bool isApi = false);

    }
}
