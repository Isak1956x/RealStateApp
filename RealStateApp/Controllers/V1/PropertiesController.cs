using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Properties.Queries;
using RealStateApp.Presentation.API.Controllers.V1.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyDto))]
    [SwaggerTag("Matenimiento de propiedades, solo lectura")]
    public class PropertiesController : GenericReadController<PropertyDto, int>
    {
        public PropertiesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet("Code")]
        [Authorize(Roles = "Admin, Developer")]
        public async Task<ActionResult<PropertyDto>> GetByCode([FromQuery] string code, CancellationToken cancellationToken)
        {
            var query = new GetPropertyByCodeQuery { Code = code };
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
