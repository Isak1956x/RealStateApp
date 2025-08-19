using MediatR;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace RealStateApp.Presentation.API.Controllers.V1.Generic
{
    [ApiVersion("1.0")]
    public abstract class GenericReadController<TDto, TId> : BaseApiController
    {
        protected readonly IMediator _mediator;

        public GenericReadController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "List all",
            Description = "Devuelve todos los elementos sin filtros."
        )]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<ActionResult<IEnumerable<TDto>>> List(CancellationToken cancellationToken)
        {
            var query = new GetAllListQuery<TId, TDto>();
            var result = await _mediator.Send(query, cancellationToken);
            if( result == null || !result.Any())
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Get By ID",
            Description = "Permite obtener elemento por el ID."
        )]
        [Authorize(Roles = "Admin,Developer")]
        public async Task<ActionResult<TDto>> GetById(TId id, CancellationToken cancellationToken)
        {
            var query = new GetByIdQuery<TId, TDto> { Id = id};
            var result = await _mediator.Send(query, cancellationToken);
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }
    }
}
