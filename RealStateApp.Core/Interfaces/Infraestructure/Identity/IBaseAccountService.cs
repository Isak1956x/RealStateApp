
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.Interfaces
{
    public interface IBaseAccountService
    {
        Task<Result<Unit>> ConfirmEmailAsync(string userId, string token);
        
        Task<Result<Unit>> DeleteUserAsyn(string id);
        Task<Result<Unit>> EditUser(UserUpdateDTO dto);
        Task<UserHeaderDTO> GetHeaderOfUsers(string id);
        Task<Dictionary<string, UserHeaderDTO>> GetHeadersOfUsers(IEnumerable<string> ids);
        Task<Result<string>> RegisterAsync(RegisterRequestDTO registerRequest, string origin);
        Task<Result<Unit>> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<Result<Unit>> SendResetPassword(string email, string origin, bool isApi = false);
        Task<Result<Unit>> SendResetPasswordEmail(string email, string origin);
        Task<Result<Unit>> UpdateProfilePhoto(string userId, string photoPath);
    }
}