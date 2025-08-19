using MediatR;
using RealStateApp.Core.Domain.Base;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Service;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Users.Commands
{
    public class RegisterNewDevComand : IRequest<Result<Domain.Base.Unit>>
    {
        [SwaggerParameter("User Name of new User")]
        public string UserName { get; set; }
        [SwaggerParameter("Email of new User")]
        public string Email { get; set; }
        [SwaggerParameter("First Name of new User")]
        public string FirstName { get; set; }
        [SwaggerParameter("Last Name of new User")]
        public string LastName { get; set; }
        [SwaggerParameter("ID card Number, in Dominican format without '-', of new User")]
        public string IdCardNumber { get; set; }
        [SwaggerParameter("Password of new User")]
        public string Password { get; set; }
    }

    public class RegisterNewDevComandHandler : IRequestHandler<RegisterNewDevComand, Result<Domain.Base.Unit>>
    {
        private readonly IAccountServiceForApi _userService;
        public RegisterNewDevComandHandler(IAccountServiceForApi userService)
        {
            _userService = userService;
        }
        public async Task<Result<Domain.Base.Unit>> Handle(RegisterNewDevComand request, CancellationToken cancellationToken)
        {
            var result = await _userService.RegisterByAdmin(new DTOs.Users.RegisterRequestDTO
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdCardNumber = request.IdCardNumber,
                Password = request.Password,
                UserName = request.UserName,
                RoleId = (int)UserRoles.Developer

            });
            if (result.IsSuccess)
            {
                return Result<Domain.Base.Unit>.Ok(Domain.Base.Unit.Value);
            }
            else
            {
                return Result<Domain.Base.Unit>.Fail(result.Error);
            }
        }
    }
}
