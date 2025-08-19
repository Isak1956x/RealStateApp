using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Features.Users.Commands;
using RealStateApp.Infraestructure.Identity.Service;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Login del API")]
    public class ApiLoginController : ControllerBase
    {
        private readonly IAccountServiceForApi _accountServiceForWebApi;
        private readonly IMediator _mediator;

        public ApiLoginController(IAccountServiceForApi accountServiceForWebApi, IMediator mediator)
        {
            _accountServiceForWebApi = accountServiceForWebApi;
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Login",
            Description = "Permite iniciar sesión y obtener un token de acceso."
        )]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            var token = await _accountServiceForWebApi.Login(loginRequestDTO);  
            if(token == null)
            {
                return BadRequest();
            }
            return Ok(token);
        }

        [HttpPost("RegisterAdmin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(
            Summary = "Register Admin",
            Description = "Permite registrar un nuevo administrador."
        )]
        public async Task<ActionResult<string>> RegisterAdmin([FromBody] RegisterNewAdminComand command)
        {
            var  result = await _mediator.Send(command);    
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

        [HttpPost("RegisterDev")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(
            Summary = "Register Developer",
            Description = "Permite registrar un nuevo desarrollador."
        )]
        public async Task<ActionResult<string>> RegisterDev([FromBody] RegisterNewDevComand command)
        {
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Error);
            }
        }

    }
}
