using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Features.Agents.Commands;
using RealStateApp.Core.Application.Features.Agents.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [SwaggerTag("Matenimiento de agentes")]
    public class AgentController : BaseApiController
    {
        private readonly IMediator _mediator;
        public AgentController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get Agent by ID",
            Description = "Permite obtener un agente por su ID."
        )]
        public async Task<ActionResult<UserDto>> GetByID(string id, CancellationToken cancellationToken)
        {
            var query = new Core.Application.Features.Agents.Queries.GetAgentByIdQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List all Agents",
            Description = "Devuelve todos los agentes sin filtros."
        )]
        public async Task<ActionResult<IEnumerable<UserDto>>> List(CancellationToken cancellationToken)
        {
            var query = new GetAllAgentQuery();
            var result = await _mediator.Send(query, cancellationToken);
            if (result.Any())
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("/{id:int}/properties")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get Properties by Agent ID",
            Description = "Permite obtener las propiedades asociadas a un agente por su ID."
        )]
        public async Task<ActionResult<IEnumerable<PropertyDto>>> GetPropertiesByAgentId(string id, CancellationToken cancellationToken)
        {
            var query = new GetAgentsPropertiesQuery { AgentId = id };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null || !result.Any())
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Change Agent Status",
            Description = "Permite cambiar el estado de un agente (activo/inactivo)."
        )]
        public async Task<IActionResult> ChangueAgentStatus(string id,[FromQuery] bool isActive)
        {
            var command = new ChangueAgentStatusCommand { AgentId = id, IsActive = isActive };
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
