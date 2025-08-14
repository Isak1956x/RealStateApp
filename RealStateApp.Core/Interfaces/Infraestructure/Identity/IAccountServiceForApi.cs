using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Infraestructure.Identity.Service
{
    public interface IAccountServiceForApi : IBaseAccountService
    {
        Task<string> Login(LoginRequestDTO loginRequestDTO);
        Task<Result<Unit>> SendResetPassword(string email, string origin, bool isApi = false);
    }
}