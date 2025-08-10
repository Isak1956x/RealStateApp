using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.Interfaces
{
    public interface IAccountServiceForWebApp
    {
        Task<Result<LoginResponseDTO>> Login(LoginRequestDTO loginRequest);
        Task<Result<Unit>> SendResetPassword(string email, string origin, bool isApi = false);
        Task SignOut();
    }
}