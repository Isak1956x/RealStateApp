using RealStateApp.Core.Application.DTOs.Email;
using RealStateApp.Core.Domain.Base;

namespace RealStateApp.Core.Application.Interfaces.Infraestructure.Shared
{
    public interface IEmailService
    {
        Task<Result<Unit>> SendAsync(EmailRequestDTO emailRequest);
    }
}
